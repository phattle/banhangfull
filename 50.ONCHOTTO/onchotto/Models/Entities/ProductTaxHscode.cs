using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OnChotto.Models.Entities
{
    public class ProductTaxHscode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductTaxHscode()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int HsCodeId { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mã Hscode.")]
        [StringLength(255)]
        [Display(Name = "Mã Hscode")]
        public string HsCode { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tên sản phẩm.")]
        [StringLength(255)]
        [Display(Name = "Tên sản phẩm")]
        public string PoductName { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập phần % thuế nhập khẩu.")]
        [Display(Name = "Nhập khẩu %")]
        public decimal? TaxPercentage { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập phần % VAT thuế nhập khẩu.")]
        [Display(Name = "Nhập khẩu VAT %")]
        public decimal? VATPercentage { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập phần % thuế liên Bang")]
        [Display(Name = "Thuế bang %")]
        public decimal? FederalTaxPercentage { get; set; }

        //Pricehandling
        [Required(ErrorMessage = "Bạn chưa nhập Handling")]
        [Display(Name = "Handling usd/hawb")]
        public decimal? Pricehandling { get; set; }
        //ShippinglandPercentage
        [Required(ErrorMessage = "Bạn chưa nhập phần % vận chuyển bang")]
        [Display(Name = "Vận chuyển bang % /Total amount")]
        public decimal? ShippinglandPercentage { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập thông quan")]
        [Display(Name = "Thông quan usd/hawb")]
        public decimal? PriceClearanceFee { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập A/F")]
        [Display(Name = "A/F usd/kg")]
        public decimal? PriceAF { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập lao vụ")]
        [Display(Name = "Lao vụ đ/kg")]
        public decimal? PriceTECSServicesFee { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập lệ phí Hải quan")]
        [Display(Name = "Lệ phí Hải quan đ")]
        public decimal? PriceCustomFee { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập phân % phí xử lý giao dịch")]
        [Display(Name = "Xử lý giao dịch %")]
        public decimal? TransactionPercentage { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập VAT phí xử lý giao dịch")]
        [Display(Name = "VAT Xử lý giao dịch %")]
        public decimal? VATFeeTransaction { get; set; }

        [StringLength(255)]
        [Display(Name = "Ghi chú đơn hàng")]
        public string Note { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get ; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}
