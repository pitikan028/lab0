using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace simple_crawler;
/// <summary>
/// Class <c>Crawler</c> access a webpage based on the given url,
/// then retrieve contents of that webpage and recursively access
/// to linked pages from that web page
/// </summary>
public partial class Crawler
{
   protected String? basedFolder = null;
   protected int maxLinksPerPage = 3;
   // ===== เพิ่มเพื่อกัน recursive loop =====
   protected HashSet<string> visited = new();
   /// <summary>
   /// Method <c>SetBasedFolder</c> sets based folder to store retrieved contents.
   /// </summary>
   public void SetBasedFolder(String folder)
   {
       if (String.IsNullOrEmpty(folder))
       {
           throw new ArgumentNullException(nameof(folder));
       }
       basedFolder = folder;
   }
   /// <summary>
   /// Method <c>SetMaxLinksPerPage</c> sets the maximum number of links
   /// that will be recursively access from a page
   /// </summary>
   public void SetMaxLinksPerPage(int max)
   {
       maxLinksPerPage = max;
   }
   /// <summary>
   /// Method <c>GetPage</c> gets a web page based on the url,
   /// then recursively access the links in the web page
   /// to get the linked pages.
   /// </summary>
   public async Task GetPage(String url, int level)
   {
       // ===== stop condition for recursion =====
       if (level <= 0) return;
       if (visited.Contains(url)) return;
       visited.Add(url);
       if (basedFolder == null)
       {
           throw new Exception("Please set the value of base folder using SetBasedFolder method first.");
       }
       if (String.IsNullOrEmpty(url))
       {
           throw new ArgumentNullException(nameof(url));
       }
       Directory.CreateDirectory(basedFolder);
       // For simplicity, we will use HttpClient here
       HttpClient client = new();
       try
       {
           Console.WriteLine($"Crawling: {url}");
           HttpResponseMessage response = await client.GetAsync(url);
           if (response.IsSuccessStatusCode)
           {
               String responseBody = await response.Content.ReadAsStringAsync();
               // Reformat URL to a valid filename
               String fileName = url
                   .Replace("://", "_")
                   .Replace("/", "_")
                   .Replace("?", "_")
                   .Replace("&", "_")
                   .Replace("=", "_") + ".html";
               // Store content in file
               File.WriteAllText(basedFolder + "/" + fileName, responseBody);
               // Get list of links from content
               ISet<String> links = GetLinksFromPage(responseBody);
               int count = 0;
               // For each link, let's recursive!!!
               foreach (String link in links)
               {
                   // We only interested in http/https link
                   if (link.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                   {
                       // ===== recursive operation (คำตอบโจทย์หลัก) =====
                       if (level > 0)
                       {
                           await GetPage(link, level - 1);
                       }
                       // limit number of links in the page
                       if (++count >= maxLinksPerPage) break;
                   }
               }
           }
           else
           {
               Console.WriteLine("Can't load content with return status {0}", response.StatusCode);
           }
       }
       catch (HttpRequestException ex)
       {
           Console.WriteLine("\nException caught:");
           Console.WriteLine("Message :{0}", ex.Message);
       }
   }
   // Template for regular express to extract links
   [GeneratedRegex("(?<=<a\\s+href=(?:\"|'))[^\"']*(?=(?:\"|'))")]
   private static partial Regex MyRegex();
   /// <summary>
   /// Method <c>GetLinksFromPage</c> extracts links
   /// (i.e., <a href="link">...</a>) from web content.
   /// </summary>
   public static ISet<string> GetLinksFromPage(string content)
   {
       Regex regexLink = MyRegex();
       HashSet<string> newLinks = new();
       foreach (var match in regexLink.Matches(content))
       {
           String? mString = match.ToString();
           if (String.IsNullOrEmpty(mString))
           {
               continue;
           }
           newLinks.Add(mString);
       }
       return newLinks;
   }
}
class Program
{
   // ===== Extra task: improve Main method =====
   static async Task Main(string[] args)
   {
       Crawler cw = new();
       cw.SetBasedFolder("./pages");
       cw.SetMaxLinksPerPage(5);
       string startUrl = "https://dandadan.net/";
       int level = 2;
       if (args.Length >= 1)
           startUrl = args[0];
       if (args.Length >= 2 && int.TryParse(args[1], out int lv))
           level = lv;
       await cw.GetPage(startUrl, level);
       Console.WriteLine("Done.");
   }
}