using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using SeoSearcher.Models;

namespace SeoSearcher.DAL
{
    public class SeoSearchDataManager : DbContext
    {
        private SeoSearchDbContext _db = new SeoSearchDbContext();

        public FavSearch GetFavouriteSearch()
        {
            Debug.Write(Database.Connection.ConnectionString);
            if (!_db.FavSearches.Any())
            {
                _db.FavSearches.Add(new FavSearch
                {
                    TargetUrl = "infotrack.com.au",
                    KeyWords = "online title search"
                });
                _db.SaveChanges();
            }
            return _db.FavSearches.First();
        }
    }
}