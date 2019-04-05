using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace WordCount
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "text.txt";
                //Console.ReadLine();
            WordBase wb = new WordBase(path);
            // wb.ComputeStr();
            wb.ComputeWord();
            //wb.ComputeNullRow();
        }
    }
}
class WordBase
{
    string content;
    int linenum = 0;
    public WordBase(string path)
    {
        StreamReader sr = File.OpenText(path);
        string nextLine;
        while ((nextLine = sr.ReadLine()) != null)
        {
            content += nextLine+"\n";
            linenum++;
        }
        sr.Close();
        Console.WriteLine(content);
    }

    public int ComputeNotNullRow()
    {
        int count = 0;
        string pattern = @"\n\s*\r";
        foreach (Match match in Regex.Matches(content, pattern))
            count++;
        count -= linenum;
        return count;
    }

    public int ComputeWord()
    {
        int count = 0;
        //string pattern = @"^[a-zA-Z]{4,}[0-9]$";
        string pattern = @"\w.*";
        foreach (Match match in Regex.Matches(content, pattern))
        {
            Console.WriteLine(match);
           //if(match)
            count++;
        }
        return count;
    }
 
    public  int ComputeStr()
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
