using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OnChotto.Models.Entities
{
    public class OrdersFee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
       

        [Key]
        public int Id { get; set; }

        [Display(Name = "Thuế liên bang")]
        public decimal? FederalTaxPrice { get; set; }

        [Display(Name = "Vận chuyển bang US")]
        public decimal? ShippingInLandPrice { get; set; }

        [Display(Name = "sort, pack, label...")]
        public decimal? HandlingFeePrice { get; set; }

        [Display(Name = " Vận chuyển US -> VN (A/F Fee)  ")]
        public decimal? AFFeePrice { get; set; }

        [Display(Name = "Thông quan")]
        public decimal? ClearanceFeePrice { get; set; }

        [Display(Name = "Lao vụ TECSFee ")]
        public decimal? TECSServicesFeePrice { get; set; }

        [Display(Name = "Giao dịch (TransactionFee)")]
        public decimal? TransactionFeePrice { get; set; }

        [Display(Name = "Phí VAT")]
        public decimal? VATFeePrice { get; set; }

        [Display(Name = "Thuế nhập khẩu")]
        public decimal? ImportTaxPrice { get; set; }

        [Display(Name = "Lệ phí Hải quan")]
        public decimal? CustomFeePrice { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate { get; set; }

      

    }
}
