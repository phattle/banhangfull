namespace OnChotto.Models.Entities
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Attribute
    {


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        [Key]
        public int Id { get; set; }
        [Display(Name = "Danh mục nhóm thuộc tính")]
        public int? GroupAtrributeId { get; set; }

        [StringLength(4000)]
        [Display(Name = "Tên thuộc tính")]
        public string Name { get; set; }

        [StringLength(4000)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

    }
}
