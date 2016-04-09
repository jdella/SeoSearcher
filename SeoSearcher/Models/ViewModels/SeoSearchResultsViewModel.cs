using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeoSearcher.Models.ViewModels
{
    public class SeoSearchResultsViewModel
    {
        public List<SeoSearchResult> SearchResults { get; set; }
        public List<int> TargetUrlRankings { get; set; } 
    }
}