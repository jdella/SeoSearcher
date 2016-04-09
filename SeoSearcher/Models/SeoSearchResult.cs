using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeoSearcher.Models
{
    public class SeoSearchResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Rank { get; set; }
        //[Required]
        public string SiteDomain { get; set; }
        [Required]
        public string FullUrl { get; set; }

        public virtual SeoSearch SeoSearch { get; set; }
    }
}