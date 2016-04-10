using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeoSearcher.Models;

namespace SeoSearcher.DAL.Managers
{
    public class SeoSearchDataManager
    {
        private SeoSearchDbContext _db = new SeoSearchDbContext();

        public FavSearch GetFavSearch()
        {
            if (!_db.FavSearches.Any())
            {
                _db.FavSearches.Add(new FavSearch
                {
                    KeyWords = "online title search",
                    TargetUrl = "infotrack.com.au"
                });
            }
            return _db.FavSearches.First();
        }
    }
}