using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OnChotto.Models.Entities
{
    public class OrdersFeeCost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]


        [Key]
        public int Id { get; set; }

        [Display(Name = "Thuế liên bang")]
        public decimal? FederalTaxCost { get; set; }

        [Display(Name = "Vận chuyển bang US")]
        public decimal? ShippingInLandCost { get; set; }

        [Display(Name = "sort, pack, label...")]
        public decimal? HandlingFeeCost { get; set; }

        [Display(Name = " Vận chuyển US -> VN (A/F Fee)  ")]
        public decimal? AFFeeCost { get; set; }

        [Display(Name = "Thông quan")]
        public decimal? ClearanceFeeCost { get; set; }

        [Display(Name = "Lao vụ TECSFee ")]
        public decimal? TECSServicesFeeCost { get; set; }

        [Display(Name = "Giao dịch (TransactionFee)")]
        public decimal? TransactionFeeCost { get; set; }

        [Display(Name = "Phí VAT")]
        public decimal? VATFeeCost { get; set; }

        [Display(Name = "Thuế nhập khẩu")]
        public decimal? ImportTaxCost { get; set; }

        [Display(Name = "Lệ phí Hải quan")]
        public decimal? CustomFeeCost { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate { get; set; }

    }
}