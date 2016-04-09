using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using SeoSearcher.Models;

namespace SeoSearcher.Services.SeoSearch
{
    public static class SeoSearchService
    {
        public static List<int> GetTargetUrlRankings(string targetUrl, List<SeoSearchResult> searchResults)
        {
            var rankings = new List<int>();
            foreach (var result in searchResults)
            {
                if (result.FullUrl.ToLowerInvariant().Contains(targetUrl.ToLowerInvariant()))
                {
                    rankings.Add(result.Rank);
                }
            }
            return rankings;
        }


        //todo commoents + get async to work
        public static List<SeoSearchResult> GetSeoRankingsForUrl(string seoKeywords, string targetUrl, int maxResults = 100)
        {
            var seoRankingsHit = new List<SeoSearchResult>();

            var url = "https://www.google.com.au/search?num=100&q=online+title+search";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var searchResultsHtml = response.Content.ReadAsStringAsync().Result;

                    //var decoded = HttpUtility.HtmlDecode(searchResultsHtml);

                    seoRankingsHit = ScrapeSearchResults(searchResultsHtml, maxResults);
                }
            }

            return seoRankingsHit;
        }

        //todo commoents + error checking
        private static List<SeoSearchResult> ScrapeSearchResults(string searchResultsHtml, int maxResults)
        {
            var seoSearchResultRankings = new List<SeoSearchResult>();

            //var searchResultSeperator = new string[] {"<h3 class=\"r\">"};
            var searchResultSeperator = new string[]
            {
                HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["GoogleSearchResultIdentifier"])
            };
            var splitResults = searchResultsHtml.Split(searchResultSeperator, StringSplitOptions.RemoveEmptyEntries);

            // in case number of results in array is less than specified maxResults - prevents out of bound exception
            if (splitResults.Count() < maxResults)
            {
                maxResults = splitResults.Count();
            }

           // Regex regex = new Regex("(<a href=\\\")(.*?)(<)");

            var rank = 1;
            for (var i = 0; i < maxResults+1; i++)
            {
                //todo retrive regex from app settings
                //var regexResults = Regex.Matches(splitResults[i], "<a href=\\\"/url[?]q=(.*?)&amp;");
                var r = HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["GoogleResultUrlRegex"]);
                var regexResults = Regex.Matches(splitResults[i], r);

                if (regexResults.Count > 0)
                {
                    var searchResult = new SeoSearchResult
                    {
                        Rank = rank,
                        FullUrl = regexResults[0].Groups[1].ToString()
                        //todo domain??
                    };
                    seoSearchResultRankings.Add(searchResult);
                    rank++;
                }
                else
                {
                    //error
                }


            }

            return seoSearchResultRankings;
        }
    }
}