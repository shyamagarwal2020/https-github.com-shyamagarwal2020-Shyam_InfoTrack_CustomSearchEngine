using CustomSearchEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CustomSearchEngine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

     
        public ActionResult ShowResults()
        {
            string searchQuery = Request["search"];
            string searchurl = Request["searchurl"];

            Uri url = new Uri(searchurl);
            List<int> positions = GetPosition(url, searchQuery);
            Result result = new Result();
            result.Results = string.Join<int>(",", positions);
            result.Keywords = searchQuery;
            result.Url = searchurl;
            result.NumberTimes = positions.Count;
            return View(result);
        }

       // <summary> 
       //Retrives  the position of the url from a search on
       //www.google.com using the specified search term. 
       // </summary> 
        public static List<int> GetPosition(Uri  url, string searchTerm)
      {
            string raw = "http://www.google.com/search?num=100&q={0}&btnG=Search"; 
            string search = string.Format(raw,HttpUtility.UrlEncode(searchTerm));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search); 
            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
          { 
            using (StreamReader reader = new StreamReader(response.GetResponseStream(),  Encoding.ASCII)) 
            { 
                string html = reader.ReadToEnd(); 
                return FindPosition(html, url);
            } 
          } 
      } 
         //<summary>
         //Examins  the search result and retrieves the position. 
         //</summary> 
        private static List<int> FindPosition(string html,  Uri url) 
        {
            var urlTagPattern = new Regex(@"<a.*?href=(?<url>.*?)[""'].*?>(?<name>.*?)</a>", RegexOptions.IgnoreCase);
            var matches = urlTagPattern.Matches(html);
            int position = 0;
            var returnList = new List<int>();
            foreach (Match match in matches)
            {
                var valueMatch = match.Value;
                if(valueMatch.Contains("/url"))
                {
                    if (match.Value.Contains(url.Host))
                    {
                        returnList.Add(position);
                    }
                    position++;
                }
            }
            return returnList;
         }
       
    }
}