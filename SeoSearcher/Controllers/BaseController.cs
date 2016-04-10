using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using SeoSearcher.DAL.Managers;

namespace SeoSearcher.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewData["FavSearch"] = new SeoSearchDataManager().GetFavSearch();
        }

    }
}