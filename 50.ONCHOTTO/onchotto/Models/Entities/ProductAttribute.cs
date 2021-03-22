namespace OnChotto.Models.Entities
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductAttribute
    {


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        [Key]
        public int Id { get; set; }
        [Display(Name = "Id sản phẩm")]
        public int? ProductId { get; set; }
        [Display(Name = "Id thuộc tính")]
        public int? AttributeId { get; set; }

        [Display(Name = "Id nhóm thuộc tính")]
        public int? GroupAtrributeId { get; set; }

        [Display(Name = "Giá gốc")]
        public decimal? Price { get; set; }
        [StringLength(4000)]
        [Display(Name = "Mô tả")]
        public string ProductImage { get; set; }
        [StringLength(4000)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

    }
}
