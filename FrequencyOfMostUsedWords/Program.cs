using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FrequencyOfMostUsedWords
{
    class Program
    {
        public static List<String> GetAllWords(List<string> path, string url, char[] c)
        {
            //To store all posible Vpath to extract required data from webpage
            string line;

            // Load an HTML document
            var html = new HtmlDocument();
            html.LoadHtml(new WebClient().DownloadString(url));
            
            //to store the context 
            StringBuilder sb = new StringBuilder();
            foreach (string s in path)
            {
                // Get value with XPath and add to stringbuilder
                sb.Append(html.DocumentNode.SelectNodes(s)[0].InnerText);
            }

            List<string> words = new List<string>();
            char[] wordsToexclude = c;

            string[] contentString = sb.ToString().Split(wordsToexclude, StringSplitOptions.RemoveEmptyEntries);

            //splitting the complete text in words/string.
            foreach (string s in contentString)
            {
                words.Add(s);
            }

            return words;
        }

        public static void MostUsedWordsFrequency(string url, int k, char[] c)
        {
            List<string> path = new List<string>();
            for (int i = 8; i < 37; i++)
            {
                path.Add($"//*[@id=\"mw-content-text\"]/div[1]/p[{i}]");
            }

            //To store the words and its frequency in Key value pair
            Dictionary<string, int> frequency = new Dictionary<string, int>();
            var words = GetAllWords(path, url, c);

            frequency.Add(words[0], 0);
            foreach (string s in words)
            {
                if (frequency.ContainsKey(s))
                {
                    frequency[s]++;
                }
                else
                    frequency.Add(s, 1);
            }

            //select the top 10 most repeated words
            var dic = frequency.OrderByDescending(key => key.Value).Take(10);

            //input the data in tabular format
            DataTable table = new DataTable();
            table.Columns.Add("Words", typeof(string));
            table.Columns.Add("Frequency", typeof(int));
            foreach (var item in dic)
            {
                table.Rows.Add((item.Key).PadRight(10), item.Value);

            }
            Console.WriteLine("         " + "# of occurances");
            foreach (DataRow item in table.Rows)
            {
                Console.Write(item.Field<String>(0));
                Console.Write(item.Field<int>(1));
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            //Page url to crawl to
            string url = "https://en.wikipedia.org/wiki/Microsoft#History";

            //The number of words to return
            int TopWords = 10;

            //Words to exclude from the search
            char[] wordsToexclude = new char[] { ',', '.', ' ' };

            //return the most common words used and the number of times they are used.
            MostUsedWordsFrequency(url, TopWords, wordsToexclude);


            

        }
    }
}
