using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    public partial class MenuAdmin
    {
        [Key]
        public int MenuChildId { get; set; }

        [Display(Name = "Parent Id")]
        public int? LocationId { get; set; }

        [StringLength(250)]
        [Display(Name ="Title")]
        public string Text { get; set; }

        [StringLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(250)]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "STT")]
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

        //public virtual MenuAdminLocation MenuAdminLocation { get; set; }
    }
}