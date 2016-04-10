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
        public ActionResult Index()
        {
            var defaultSearch = new SeoSearch
            {
                TargetUrl = "infotrack.com.au",
                KeyWords = "online title search"
            };
            return View(new HomeViewModel
            {
                NewSearch = defaultSearch
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "NewSearch")] HomeViewModel viewModel)
        {


            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "SeoSearch", viewModel.NewSearch);
            }
            return View(viewModel);
        }

    }
}