namespace OnChotto.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CaptureEvents")]
    public partial class CaptureEvent
    {
       [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        
        [Key]
        public int CaptureId { get; set; }

        [Display(Name = "Search ID")]
        public string SearchID { get; set; }
        public DateTime? TimeSeach { get; set; }

        [StringLength(255)]
        [Display(Name = "Keyword Seach")]
        public string KeywordSeach { get; set; }

        [Display(Name = "Category Id")]
        public int? CategoryId { get; set; }
         
        public string CategoryName { get; set; }

        [Display(Name = "Is Capture ")]
        public bool IsCapture { get; set; }

        [Display(Name = "Capture Count ")]
        public int CaptureCount { get; set; }

    }
}
