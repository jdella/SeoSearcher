using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeoSearcher.Models.ViewModels;
using SeoSearcher.Services.SeoSearch;

namespace SeoSearcher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult SeoSearch()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //todo pass params...string keywords, string targetUrl, int maxResults=100
        public async System.Threading.Tasks.Task<ActionResult> SeoSearch()
        {
            var targetUrl = "infotrack.com.au";
            
            var searchResults = SeoSearchService.GetSeoRankingsForUrl("GET FROM SOMEHWRE", "GET FROM SOMEHWRE");
            var targetUrlRankings = SeoSearchService.GetTargetUrlRankings(targetUrl, searchResults);

            return View(new SeoSearchResultsViewModel
            {
                SearchResults = searchResults,
                TargetUrlRankings = targetUrlRankings
            });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}