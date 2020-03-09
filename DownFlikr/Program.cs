using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DownFlickr
{
    class Program
    {
        private static WebClient webClient = new WebClient();

        private static string getSource(string url)
        {
            string source = "";

            source = webClient.DownloadString(url);

            return source;
        }

        private static void downloadFileFrom(string source)
        {
            if (!Directory.Exists("./result"))
                Directory.CreateDirectory("./result");

            var shortlinks = Regex.Matches(source, "k\":{\"displayUrl\":\"\\\\/\\\\/(?<url>.*?)\",\"width");

            foreach (Match i in shortlinks)
            {
                var link = "https://" + i.Groups["url"].Value.Replace("\\/", "/");
                var fileName = link.Substring(link.LastIndexOf("/"));
                webClient.DownloadFile((string)link, (string)("./result" + fileName));
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Link: ");
                string url = Console.ReadLine();

                Console.WriteLine("Downloading . . .");

                string source = getSource(url);

                downloadFileFrom(source);

                Process.Start(@"explorer", Directory.GetCurrentDirectory() + @"\result");
            }
        }
    }
}