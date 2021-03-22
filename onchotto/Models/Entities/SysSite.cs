using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OnChotto.Models.Entities
{
    [Table("SysSites")]
    public class SysSite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]


        [Key]
        public int SiteId { get; set; }
        public int? CompId { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Site Name")]
        [StringLength(50)]
        public string SiteName { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        public virtual SysCompany SysCompanys { get; set; }
    }
}