using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SeoSearcher.Models
{
    public class SeoSearchDbContext : DbContext
    {
        public DbSet<SeoSearch> SeoSearches { get; set; }
        public DbSet<FavSearch> FavSearches { get; set; }
        public DbSet<SeoSearchResult> SeoSearchResults { get; set; }
    }
}