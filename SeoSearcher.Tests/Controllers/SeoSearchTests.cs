using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeoSearcher;
using SeoSearcher.App_Start;
using SeoSearcher.Controllers;
using SeoSearcher.Models;
using SeoSearcher.Models.ViewModels;
using SeoSearcher.Services.SeoSearch;

namespace SeoSearcher.Tests.Controllers
{

    [TestClass]
    public class HomeControllerTests
    {
        private SeoSearch _default = new SeoSearch
        {
            TargetUrl = "newurl.com.au",
            KeyWords = "new key words"
        };

        // simple test to make sure ModelState is validating
        [TestMethod]
        public void HomeSearchValidating()
        {
            var controller = new HomeController();
            var result = (RedirectToRouteResult) controller.Index(new HomeViewModel
            {
                NewSearch = _default
            });

            Assert.AreEqual("SeoSearch", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }

    [TestClass]
    public class SeoSearchControllerTest
    {
        //test valid model is returning a view
        [TestMethod]
        public void SeoSearchValidating()
        {
            SeoSearch _default = new SeoSearch
            {
                TargetUrl = "newurl.com.au",
                KeyWords = "new key words"
            };
            var controller = new SeoSearchController();
            var result = controller.Index(_default).Result as ViewResult;
            Assert.IsNotNull(result);
        }
    }

    [TestClass]
    public class SeoSearchServiceTest
    {
        public static string GoogleSearchResultIdentifier = AppConfig.GoogleSearchResultIdentifier; 
        private static string GoogleResultUrlRegex = AppConfig.GoogleResultUrlRegex; 
        private static int GoogleMaxSearchResults = AppConfig.GoogleMaxSearchResults;

        private SeoSearchService SeoSvc = new SeoSearchService(
                GoogleSearchResultIdentifier,
                GoogleResultUrlRegex,
                GoogleMaxSearchResults
            );

        // make sure config settings exist
        [TestMethod]
        public void AppConfigSettings()
        {
            Assert.IsNotNull(GoogleSearchResultIdentifier);
            Assert.IsNotNull(GoogleResultUrlRegex);
            Assert.AreEqual(100, GoogleMaxSearchResults);   // test against actual value
        }

        // check for google response
        [TestMethod]
        public void GetSearchResponse()
        {
            var response = SeoSvc.GetSeoSearchResultsResponse("google").Result;
            Assert.IsNotNull(response);
        }

        // make sure right amount of results - failure indicates incorrect parsing
        [TestMethod]
        public void CheckScrapedResults()
        {
            var response = SeoSvc.GetSeoSearchResultsResponse("test").Result;  // verfied "test" returns 100
            var scrapedResults = SeoSvc.ScrapeSearchResults(response);

            //should be 100 results
            Assert.AreEqual(100, scrapedResults.Count());
        }

        // make sure URL is scraped successfully - failure indicates incorrect parsing
        [TestMethod]
        public void CheckScrapeUrl()
        {
            var response = SeoSvc.GetSeoSearchResultsResponse("google").Result;
            var scrapedResults = SeoSvc.ScrapeSearchResults(response);
            //search term was "google" - google wil be first
            var searchForGoogle = scrapedResults[0];
            Assert.IsTrue(searchForGoogle.Rank==1);
            Assert.IsTrue(searchForGoogle.FullUrl.Contains("google.com"));
        }

    }
}