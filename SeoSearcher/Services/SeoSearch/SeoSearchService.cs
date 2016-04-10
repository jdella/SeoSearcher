using System;
using System.CodeDom;
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
    public class SeoSearchService
    {
        private string GoogleSearchResultIdentifier { get; set; }
        private string GoogleResultUrlRegex { get; set; }
        private int MaxResults { get; set; }

        public SeoSearchService(string googleSearchResultIdentifier, string googleResultUrlRegex, int maxResults)
        {
            GoogleSearchResultIdentifier = googleSearchResultIdentifier;
            GoogleResultUrlRegex = googleResultUrlRegex;
            MaxResults = maxResults;
        }

        public List<int> GetTargetUrlRankings(string targetUrl, ICollection<SeoSearchResult> searchResults)
        {
            var rankings = new List<int>();
            if (!string.IsNullOrWhiteSpace(targetUrl))
            {
                foreach (var result in searchResults)
                {
                    if (result.FullUrl.ToLowerInvariant().Contains(targetUrl.ToLowerInvariant()))
                    {
                        rankings.Add(result.Rank);
                    }
                }
            }
            return rankings;
        }


        //todo commoents + get async to work
        public List<SeoSearchResult> GetSeoSearchRankings(Models.SeoSearch seoSearch)
        {
            var seoTargetRankings = new List<SeoSearchResult>();

            var url = String.Format("https://www.google.com.au/search?num={0}&q={1}", seoSearch.MaxResults, seoSearch.KeyWords);

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
                    seoTargetRankings = ScrapeSearchResults(searchResultsHtml);
                }
            }

            return seoTargetRankings;
        }

        //todo commoents + error checking
        private List<SeoSearchResult> ScrapeSearchResults(string searchResultsHtml)
        {
            var seoSearchResultRankings = new List<SeoSearchResult>();

            //var searchResultSeperator = new string[] {"<h3 class=\"r\">"};
            var searchResultSeperator = new string[] { GoogleSearchResultIdentifier };
            var splitResults = searchResultsHtml.Split(searchResultSeperator, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1).ToArray();   // Skip first element as not a search result

           // Regex regex = new Regex("(<a href=\\\")(.*?)(<)");

            var rank = 1;
            for (var i = 0; i < splitResults.Count() + 1; i++) //start from 1 as 
            {
                try
                {
                    //var regexResults = Regex.Matches(splitResults[i], "<a href=\\\"/url[?]q=(.*?)&amp;");
                    var regexResults = Regex.Matches(splitResults[i], GoogleResultUrlRegex);

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
                        //error with finding search result url
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    //todo better error handling??
                    break;  // do not continue if somehow hit out of bounds
                }
            }

            return seoSearchResultRankings;
        }
    }
}