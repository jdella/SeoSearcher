using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeoSearcher.App_Start;
using SeoSearcher.Models;
using SeoSearcher.Models.ViewModels;
using SeoSearcher.Services.SeoSearch;

namespace SeoSearcher.Controllers
{
    public class SeoSearchController : BaseController
    {
        // these values set the parameters for scraping Google's search results
        // they are stores in config file to try avoid having to recompile if Google makes any breaking changes to its search results structure
        private readonly SeoSearchService _seoSearchService = new SeoSearchService(
            AppConfig.GoogleSearchResultIdentifier,     // unique substring to identify search results - typically a div class
            AppConfig.GoogleResultUrlRegex,             // regex to capture the result url
            AppConfig.GoogleMaxSearchResults            // number of results to retrieve
            );

        // GET: SeoSearches
        public async System.Threading.Tasks.Task<ActionResult> Index(SeoSearch seoSearch)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            var response = await _seoSearchService.GetSeoSearchResultsResponse(seoSearch.KeyWords);
            seoSearch.Results = _seoSearchService.ScrapeSearchResults(response);
            seoSearch.MaxResults = AppConfig.GoogleMaxSearchResults;

            var targetUrlRankings = _seoSearchService.GetTargetUrlRankings(seoSearch.TargetUrl, seoSearch.Results);
            return View(new SeoSearchResultsViewModel
            {
                SeoSearch = seoSearch,
                TargetUrlRankings = targetUrlRankings
            });
        }

    }
}
