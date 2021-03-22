namespace OnChotto.Models.Entities
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductSize
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        [Key]
        public Int64 Id { get; set; }
        [StringLength(1000)]
        [Display(Name = "Size code")]
        public string SizeCode { get; set; }
        [StringLength(1000)]
        [Display(Name = "Zone size")]
        public string ZoneSize { get; set; }
        [Display(Name = "Cat ID")]
        public int? CatID { get; set; }
        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

    }
}
