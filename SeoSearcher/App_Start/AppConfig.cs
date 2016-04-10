using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SeoSearcher.App_Start
{
    public static class AppConfig
    {
        // Values From Config
        public static readonly int GoogleMaxSearchResults = int.Parse(ConfigurationManager.AppSettings["GoogleMaxSearchResults"]);
        public static readonly string GoogleSearchResultIdentifier = HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["GoogleSearchResultIdentifier"]);
        public static readonly string GoogleResultUrlRegex = HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["GoogleResultUrlRegex"]);
    }
}