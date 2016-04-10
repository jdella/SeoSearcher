using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeoSearcher.Models
{
    public class SeoSearch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string KeyWords { get; set; }
        [Required]
        // [Url] todo too restrictive - a good regex might be better here?
        public string TargetUrl { get; set; }
        [Required]
        public int MaxResults { get; set; }
        [Required]
        public DateTime DateRun { get; set; }

        public virtual ICollection<SeoSearchResult> Results { get; set; }


        public SeoSearch()
        {   //defaults
            DateRun = DateTime.Now;
        }
    }

}