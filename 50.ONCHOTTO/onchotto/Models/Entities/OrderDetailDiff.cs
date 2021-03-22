using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace OnChotto.Models.Entities
{

    [Table("OrderDetailDiff")]
    public partial class OrderDetailDiff
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Mã đơn hàng

        public int OrderDiffId { get; set; }

        [StringLength(50)]
        [Display(Name = "Mã đơn hàng seller")]
        public string OrderNo { get; set; }

        [StringLength(50)]
        [Display(Name = "Mã tracking seller")]
        public string OrderTrackingNo { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Store Name")]
        [StringLength(50)]
        [Display(Name = "Store name")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "Bạn chưa copy links sản phẩm")]
        [Display(Name = "Tên Link")]
        public string ProductLink { get; set; }

        [Required(ErrorMessage = "Bạn chưa mô tả tả sản phẩm")]
        [StringLength(255)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập size sản phẩm")]
        [StringLength(50)]
        [Display(Name = "Size")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số lượng")]
        [Display(Name = "Số lượng")]
        public int? Amount { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập trọng lượng")]
        [Display(Name = "Trọng lượng")]
        public decimal? Weight { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập giá trên web")]
        [Display(Name = "Giá bán")]
        public decimal? Price { get; set; }

        [Display(Name = "Giảm giá Off")]
        public decimal? Discount { get; set; }

        [Display(Name = "Giá bán")]
        public decimal? PriceAfter  { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái SP")]
        public string ProductStatus { get; set; }
        [StringLength(50)]
        [Display(Name = "Đơn Vị")]
        public string WeightUnit { get; set; }
        [StringLength(50)]
        [Display(Name = "Tiền tệ")]
        public string Currency { get; set; }

        [StringLength(50)]
        [Display(Name = "Loại giao dịch")]
        public string TransType { get; set; }
        //public virtual OrderDiff OrderDiff { get; set; }


    }

}
