using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    public partial class MenuAdminLocation
    {
        [Key]
        public int MenuParentId { get; set; }

        [StringLength(250)]
        [Display(Name = "Title")]
        public string Name { get; set; }

        [Display(Name ="STT")]
        public int STT { get; set; }


        [StringLength(250)]
        [Display(Name = "Icon")]
        public string TitleIcon { get; set; }

        [StringLength(250)]
        [Display(Name = "Page Layout")]
        public string PageLayout { get; set; }

        [StringLength(250)]
        [Display(Name = "Table Name")]
        public string TableName { get; set; }

        //public virtual ICollection<MenuAdmin> MenuAdmins { get; set; }
    }
}