using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeoSearcher.Models;
using SeoSearcher.Models.ViewModels;
using SeoSearcher.Services.SeoSearch;

namespace SeoSearcher.Controllers
{
    public class SeoSearchController : Controller
    {
        private SeoSearchDbContext db = new SeoSearchDbContext();


        ////todo pass params...string keywords, string targetUrl, int maxResults=100
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

        // GET: SeoSearches
        public ActionResult Index()
        {
            var targetUrl = "infotrack.com.au";

            var searchResults = SeoSearchService.GetSeoRankingsForUrl("GET FROM SOMEHWRE", "GET FROM SOMEHWRE");
            var targetUrlRankings = SeoSearchService.GetTargetUrlRankings(targetUrl, searchResults);

            return View(new SeoSearchResultsViewModel
            {
                SearchResults = searchResults,
                TargetUrlRankings = targetUrlRankings
            });

            //return View(db.SeoSearches.ToList());
        }

        // GET: SeoSearches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeoSearch seoSearch = db.SeoSearches.Find(id);
            if (seoSearch == null)
            {
                return HttpNotFound();
            }
            return View(seoSearch);
        }

        // GET: SeoSearches/Create
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
                db.SeoSearches.Add(seoSearch);
                db.SaveChanges();
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
            SeoSearch seoSearch = db.SeoSearches.Find(id);
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
                db.Entry(seoSearch).State = EntityState.Modified;
                db.SaveChanges();
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
            SeoSearch seoSearch = db.SeoSearches.Find(id);
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
            SeoSearch seoSearch = db.SeoSearches.Find(id);
            db.SeoSearches.Remove(seoSearch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
