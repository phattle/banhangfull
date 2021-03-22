using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    [Table("UserProductSites")]
    public class UserProductSite
    {
        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        [Display(Name = "User Name")]
        public string UsersId { get; set; }

        [StringLength(250)]
        [Display(Name = "Product Site Name")]
        public string ProductSiteId { get; set; }
    }
}