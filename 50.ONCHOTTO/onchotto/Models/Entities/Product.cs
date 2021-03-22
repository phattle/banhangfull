namespace OnChotto.Models.Entities
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product() // Gọi quan hệ một nhiều với bảng product
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductMetas = new HashSet<ProductMeta>();
            Orders = new HashSet<Order>();
            ProductLinks = new HashSet<ProductLinks>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Danh mục sp")]
        public int? CatId { get; set; }

        [StringLength(4000)]
        [Display(Name = "Tên SP")]
        public string Name { get; set; }

        [StringLength(4000)]
        public string Slug { get; set; }

        public string ASIN { get; set; }

        public string ParentASIN { get; set; }

        public string DetailPageURL { get; set; }


        public string LargeImageURL { get; set; }


        [Display(Name = "Nhà SX")]
        public int? ManufactId { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Đặc điểm nổi bật")]
        public string Featured { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Điều kiện sử dụng")]
        public string Condition { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        [Display(Name = "Số lượng")]
        public int Amount { get; set; }

        [Display(Name = "Kiểu SP")]
        public int? TypeId { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Hình ảnh khác")]
        public string Images { get; set; }

        [StringLength(4000)]
        [Display(Name = "Hình đại diện")]
        public string FeaturedImage { get; set; }

        [Display(Name = "Giá gốc")]
        public decimal? Price { get; set; }

        [Display(Name = "Giảm giá (%)")]
        public decimal? Discount { get; set; }

        [Display(Name = "Giá bán")]
        public decimal? PriceAfter { get; set; }


        [Display(Name = "Trọng lượng (Kg)")]
        public decimal? WeightKG { get; set; }


        [Display(Name = "Trọng lượng (lbs)")]
        public decimal? WeightLbs { get; set; }

        [Display(Name = "VLWeightKG")]
        public decimal? VLWeightKG { get; set; }

        [Display(Name = "VLWeightLbs")]
        public decimal? VLWeightLbs { get; set; }


        [Display(Name = "Charge Weight")]
        public decimal? ChargeWeight { get; set; }

        [Display(Name = "Charge Weight KG")]
        public decimal? ChargeWeightKG { get; set; }

        [Display(Name = "Charge Weight Lbs")]
        public decimal? ChargeWeightLbs { get; set; }


        [Display(Name = "DVT Trọng lượng")]
        public string WeightUnit { get; set; }

        [Display(Name = "Size")]
        public string Size { get; set; }

        [Display(Name = "Ngày hết hạn")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Người đăng")]
        public string UserId { get; set; }

        [Display(Name = "Ngày đăng")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Khuyến mãi cộng thêm")]
        public decimal? ExtraDiscount { get; set; }

        [Display(Name = "Sản phẩm mới")]
        public bool? IsNew { get; set; }

        [Display(Name = "Đánh dấu nổi bật")]
        public bool? IsFeatured { get; set; }

        [Display(Name = "Sản phẩm bán chạy")]
        public bool? IsSpecial { get; set; }

        [Display(Name = "Kích hoạt sản phẩm")]
        public bool? Actived { get; set; }

        [Display(Name = "Lượt xem")]
        public int Views { get; set; }

        [Display(Name = "Tiêu đề")]
        public string MetaTitle { get; set; }

        [Display(Name = "Mô tả")]
        public string MetaDescription { get; set; }

        [Display(Name = "Từ khóa")]
        public string MetaKeyword { get; set; }

        [Display(Name = "Zone SP")]
        public string ProductZone { get; set; }

        [Display(Name = "Money Unit")]
        public string MoneyUnit { get; set; }

        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        [Display(Name = "Search ID")]
        public string SearchID { get; set; }

        [Display(Name = "Phí liên bang")]
        public decimal? FederalTax { get; set; }
        [Display(Name = "Phí vận chuyển nội hạt liên bang")]
        public decimal? ShippingInLand { get; set; } //No use đã thay đổi use trên orders

        [Display(Name = "Phí thuế nhập khẩu")]
        public decimal? TaxExport { get; set; }

        public int? HsCodeId { get; set; }

        [Display(Name = "Product Site Name")]
        public int? ProductSiteId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Thuế nhập khẩu SP")]
        public decimal? PriceTaxPercentage { get; set; }
        [Display(Name = "VAT nhập khẩu SP")]
        public decimal? PriceTaxVatPercentage { get; set; }

        [Display(Name = "Pakage Dimensions")]
        public string Dimensions { get; set; }

        [Display(Name = "Số lượng ngoại tệ")]
        public int? AmountPriceCurrRank { get; set; }

        [Display(Name = "Giá ngoại tệ")]
        public decimal? PriceCurrRank { get; set; }
        

        [Display(Name = "Đơn vị ngoại tệ")]
        public string UnitCurrRank { get; set; }

        public virtual Manufact Manufact { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ProductType ProductType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMeta> ProductMetas { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<ProductLinks> ProductLinks { get; set; }

        public virtual ProductTaxHscode ProductTaxHscodes { get; set; }
    }
}
