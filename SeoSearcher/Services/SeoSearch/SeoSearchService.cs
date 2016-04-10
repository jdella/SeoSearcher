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

        /// <summary>
        /// Gets Google search results for provided keywords
        /// </summary>
        /// <param name="keywords">Keywords to search</param>
        /// <returns>Google search request response</returns>
        public async Task<string> GetSeoSearchResultsResponse(string keywords)
        {
           // var seoRankings = new List<SeoSearchResult>();

            var url = String.Format("https://www.google.com.au/search?num={0}&q={1}", MaxResults, keywords);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var searchResultsHtml = response.Content.ReadAsStringAsync().Result;
                    return searchResultsHtml;
                }
                return null;
            }
        }

        /// <summary>
        /// Takes Google search results response HTML and returns ranked list of results
        /// </summary>
        /// <param name="searchResultsHtml">Html response for Google search request</param>
        /// <returns>
        /// Ranked list of search result URLs (Rank & URL)
        /// </returns>
        public List<SeoSearchResult> ScrapeSearchResults(string searchResultsHtml)
        {
            var seoSearchResultRankings = new List<SeoSearchResult>();

            var searchResultSeperator = new string[] { GoogleSearchResultIdentifier };
            var splitResults = searchResultsHtml.Split(searchResultSeperator, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1).ToArray();   // Skip first element as not a search result

            var rank = 1;
            for (var i = 0; i < splitResults.Count() + 1; i++)
            {
                try
                {
                    var regexResults = Regex.Matches(splitResults[i], GoogleResultUrlRegex);

                    if (regexResults.Count > 0)
                    {
                        var searchResult = new SeoSearchResult
                        {
                            Rank = rank,
                            FullUrl = regexResults[0].Groups[1].ToString()
                        };
                        seoSearchResultRankings.Add(searchResult);
                        rank++;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    break;  // do not continue if somehow hit out of bounds
                }
            }

            return seoSearchResultRankings;
        }

        /// <summary>
        /// Processes a list of scraped Google results and returns list of rankings matching
        /// provided target URL
        /// </summary>
        /// <param name="targetUrl">URL to search for in rankings</param>
        /// <param name="searchResults">List of scraped search results</param>
        /// <returns></returns>
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


    }
}