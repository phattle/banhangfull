using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace OnChotto.Models.Entities
{
    
    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        [Display(Name = "Giá bán")]
        public decimal? PriceAfter  { get; set; }

        [Display(Name = "Giảm giá")]
        public decimal? Discount { get; set; }

        [Display(Name = "Số lượng")]
        public int? Amount { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [Display(Name = "Phí liên bang")]
        public decimal? FederalTax { get; set; }
        [Display(Name = "Phí vận chuyển nội hạt liên bang")]
        public decimal? ShippingInLand { get; set; }

        [Display(Name = "Phí thuế nhập khẩu")]
        public decimal? TaxExport { get; set; }

        
        [StringLength(250)]
        [Display(Name = "Loại sản phẩm")]
        public string OrderType { get; set; }


        
        [Display(Name = "Handling Fee")]
        public virtual Order Order { get; set; } 

        public virtual Product Product { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }

    }
}
