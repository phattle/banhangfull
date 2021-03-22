using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    [Table("ProductSites")]
    public class ProductSite
    {
        [Key]
        public int ProductSiteId { get; set; }

        [StringLength(250)]
        [Display(Name = "Product Site Name")]
        public string ProductSiteName { get; set; }

        [StringLength(250)]
        [Display(Name = "Code Order")]
        public string IdCode { get; set; }
    }
}