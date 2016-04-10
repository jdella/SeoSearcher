using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeoSearcher.App_Start;
using SeoSearcher.DAL;
using SeoSearcher.Models;
using SeoSearcher.Models.ViewModels;
using SeoSearcher.Services.SeoSearch;

namespace SeoSearcher.Controllers
{
    public class SeoSearchController : Controller
    {
        private readonly SeoSearchDbContext _db = new SeoSearchDbContext();
        private readonly SeoSearchService _seoSearchService = new SeoSearchService(
            AppConfig.GoogleSearchResultIdentifier,
            AppConfig.GoogleResultUrlRegex,
            AppConfig.GoogleMaxSearchResults
            );

        // GET: SeoSearches
        public ActionResult Index(SeoSearch seoSearch)
        {
            //var targetUrl = "infotrack.com.au";

            var searchResults = _seoSearchService.GetSeoSearchRankings(seoSearch);
            var targetUrlRankings = _seoSearchService.GetTargetUrlRankings(seoSearch.TargetUrl, searchResults);

            return View(new SeoSearchResultsViewModel
            {
                SeoSearch = seoSearch,
                SearchResults = searchResults,
                TargetUrlRankings = targetUrlRankings
            });
        }

        public ActionResult RunFavourite()
        {
            var fav = new SeoSearchDataManager().GetFavouriteSearch();
            return RedirectToAction("Index", "SeoSearch", new SeoSearch
            {
                TargetUrl = "infotrack.com.au",
                KeyWords = "online title search"
            });
        }

        public ActionResult FavSearch()
        {
            return RedirectToAction("Index", "SeoSearch", new SeoSearch
            {
                TargetUrl = "infotrack.com.au",
                KeyWords = "online title search"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFavourite([Bind(Include = "KeyWords,TargetUrl")] FavSearch newFav)
        {
            if (ModelState.IsValid)
            {
                if (_db.FavSearches.Any())
                {
                    var fav = _db.FavSearches.First();
                    fav.KeyWords = newFav.KeyWords;
                    fav.TargetUrl = newFav.TargetUrl;
                }
                else
                {
                    _db.FavSearches.Add(new FavSearch
                    {
                        KeyWords = newFav.KeyWords,
                        TargetUrl = newFav.TargetUrl
                    });
                }
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //////todo pass params...string keywords, string targetUrl, int maxResults=100
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



        // GET: SeoSearches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeoSearch seoSearch = _db.SeoSearches.Find(id);
            if (seoSearch == null)
            {
                return HttpNotFound();
            }
            return View(seoSearch);
        }

        //// GET: SeoSearches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SeoSearches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,KeyWords,TargetUrl,DateRun")] SeoSearch seoSearch)
        {

            if (ModelState.IsValid)
            {
                _db.SeoSearches.Add(seoSearch);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(seoSearch);
        }

        // GET: SeoSearches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeoSearch seoSearch = _db.SeoSearches.Find(id);
            if (seoSearch == null)
            {
                return HttpNotFound();
            }
            return View(seoSearch);
        }

        // POST: SeoSearches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,KeyWords,TargetUrl,DateRun")] SeoSearch seoSearch)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(seoSearch).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seoSearch);
        }

        // GET: SeoSearches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeoSearch seoSearch = _db.SeoSearches.Find(id);
            if (seoSearch == null)
            {
                return HttpNotFound();
            }
            return View(seoSearch);
        }

        // POST: SeoSearches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SeoSearch seoSearch = _db.SeoSearches.Find(id);
            _db.SeoSearches.Remove(seoSearch);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
