using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeoSearcher.DAL.Managers;
using SeoSearcher.Models;
using SeoSearcher.Models.ViewModels;
using SeoSearcher.Services.SeoSearch;

namespace SeoSearcher.Controllers
{
    public class HomeController : BaseController
    {
       // private SeoSearchDbContext db = new SeoSearchDbContext();

        //public HomeController()
        //{
        //    ViewData["FavSearch"] = new SeoSearchDataManager().GetFavSearch();
        //}
        

        public ActionResult Index()
        {
            var fav = new SeoSearchDataManager().GetFavSearch();
            var defaultSearch = new SeoSearch
            {
                TargetUrl = "infotrack.com.au",
                KeyWords = "online title search"
            };

            return View(new HomeViewModel
            {
                NewSearch = defaultSearch,
                FavSearch = fav
            });
        }

        // POST: SeoSearches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "NewSearch, FavSearch")] HomeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
               // SeoSearchService.GetSeoSearchRankings(seoSearch.KeyWords, seoSearch.TargetUrl); //maxResults?

                //db.SeoSearches.Add(seoSearch);
                //db.SaveChanges();
                return RedirectToAction("Index", "SeoSearch", viewModel.NewSearch);
            }

            return View(viewModel);
        }

        public ActionResult FavSearch()
        {
            return RedirectToAction("Index", "SeoSearch", new SeoSearch
            {
                TargetUrl = "infotrack.com.au",
                KeyWords = "online title search"
            });
        }

        //public ActionResult SeoSearch()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        ////todo pass params...string keywords, string targetUrl, int maxResults=100
        //public async System.Threading.Tasks.Task<ActionResult> SeoSearch()
        //{
        //    var targetUrl = "infotrack.com.au";

        //    var searchResults = SeoSearchService.GetSeoSearchRankings("GET FROM SOMEHWRE", "GET FROM SOMEHWRE");
        //    var targetUrlRankings = SeoSearchService.GetTargetUrlRankings(targetUrl, searchResults);

        //    return View(new SeoSearchResultsViewModel
        //    {
        //        SearchResults = searchResults,
        //        TargetUrlRankings = targetUrlRankings
        //    });
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}