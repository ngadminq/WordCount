using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace WordCount
{
    public class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\ThinkPad\Desktop\ceshi.txt";
            int limitOut = 10;
            string outPath = "C:/Users/ThinkPad/Desktop/text.txt";
            int step = 1;
            if (args != null)
            {
                for (int i = 1; i < args.Length-1; i += 2)
                {
                    //奇数个参数作为标志参，进行判断
                    if (args[i] == "-m")
                    {
                        path = args[i + 1];
                    }
                    if (args[i] == "-i")
                    {
                        step = Int32.Parse(args[i + 1]);
                    }
                    if (args[i] == "-n")
                    {
                        limitOut = Int32.Parse(args[i + 1]);
                    }
                    if (args[i] == "-o")
                    {
                        outPath = args[i + 1];
                    }
                }
            }
            WordBase wb = new WordBase(path);
            //统计字符串个数
            wb.ComputeStr();
            //统计有效行
            wb.ComputeNotNullRow();
            //统计单词个数
            wb.ComputeWord();
            //输出文档和单词词频
            wb.OutPut(true, limitOut, outPath,step);

        }
    }

    public class WordBase
    {
        string content;
        int linenum = 0;
        List<string> repeatWord = new List<string>();

        //构造函数用于读取输入文件
        public WordBase(string path = @"C:\Users\ThinkPad\Desktop\ceshi.txt")
        {
            /*
             params::    path:input root
             */
            StreamReader sr = File.OpenText(path);
            string nextLine;
            while ((nextLine = sr.ReadLine()) != null)
            {
                content += nextLine + "\n";
                linenum++;
            }
            sr.Close();
            Console.WriteLine(content);
        }

        //统计有效行数
        public int ComputeNotNullRow()
        {
            int count = 0;
            string pattern = @"\n\s*\r";
            foreach (Match match in Regex.Matches(content, pattern))
                count++;
            count = linenum - count;
            Console.Write("vaild row:   ");
            Console.WriteLine(count);
            return count;
        }

        //统计有效单词数
        public int ComputeWord()
        {

            content = content.ToLower();
            int count = 0;
            String[] token = Regex.Split(content, @"\s{1,}");
            Regex myRegex = new Regex(@"^[a-zA-z]{4,}");

            foreach (string match in token)
            {
                if (myRegex.IsMatch(match.ToString()))
                {
                    repeatWord.Add(match);
                    count++;
                }
            }
            return count;
        }

        //将结果输出到控制终端和文本文档
        public void OutPut(
            bool isWrite = false,
            int limitOut = 10,
            string outPath = "C:/Users/ThinkPad/Desktop/text.txt",
            int step = 1
            )
        {
            /*
             params::    
                isWrite:是否输出词频文件
                limitOut:控制台输出词频个数限制
                outPath:词频文件输出路径
                step：控制台输出的词频输出步长
             */
            int writeTop = 0;
            Dictionary<string, int> dic = new Dictionary<string, int>();
            List<int> uniqueValue = new List<int>();
            Dictionary<string, int> mydic = new Dictionary<string, int>();
            foreach (var p in repeatWord)
            {
                if (dic.Keys.Contains(p))
                    dic[p] += 1;
                else
                    dic.Add(p, 1);
            }

            Dictionary<string, int> dic1desc = dic.OrderByDescending(p => p.Value).ToDictionary(o => o.Key, p => p.Value);
            List<int> valueList = new List<int>(dic1desc.Values);
            HashSet<int> valueSet = new HashSet<int>(valueList);
            //Dictionaryx<string,int> dic1desc = valuedesc.OrderByDescending(o => o.Key).ToDictionary(o => o.Key, p => p.Value);


            foreach (var vs in valueSet)
            {
                Dictionary<string, int> _ = new Dictionary<string, int>();
                var key = dic1desc.Where(pair => pair.Value == vs)
                        .Select(pair => pair.Key); ;

                foreach (var i in key)
                {
                    _.Add(i, vs);
                }
                Dictionary<string, int> tempdic = _.OrderByDescending(p => p.Key).ToDictionary(o => o.Key, p => p.Value);
                foreach (var item in tempdic)
                {
                    mydic.Add(item.Key, item.Value);
                }
            }

            foreach (var item in mydic)
            {
                if (isWrite)
                {
                    FileStream fs = new FileStream(outPath, FileMode.Append, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(fs);
                    sr.WriteLine(item.Key + "       frequency:" + item.Value);//开始写入值
                    sr.Close();
                    fs.Close();
                }
            }

            List<string> keyList = new List<string>(mydic.Keys);

            for (int i = 0; i < mydic.Count / step; i++)
            {
                for (int j = i * step; j < (i + 1) * step; j++)
                {
                    Console.WriteLine(keyList[j] + "       frequency:" + mydic[keyList[j]]);
                    writeTop++;
                }
            }
        }

        //统计字符串数
        public int ComputeStr()
        {
            int returncount = 0;
            string pattern1 = @"\r";
            string pattern2 = @"[\w\s].*?";
            int count = 0;

            foreach (Match match in Regex.Matches(content, pattern1))
            {
                returncount++;
            }
            foreach (Match match in Regex.Matches(content, pattern2))
            {
                count++;
            }
            count = count - returncount;
            Console.WriteLine(count);
            return count;
        }
    }
}

