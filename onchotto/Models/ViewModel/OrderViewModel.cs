using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.ViewModel
{
    public class OrderViewModel
    {
        [Display(Name = "Khách hàng")]
        public string UserId { get; set; }

        [Required]
        public int? TransportId { get; set; }

        public int? PaymentMethodId { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Bạn chưa nhập Email người nhận."), EmailAddress]
        [Display(Name = "Email Người nhận")]
        public string ReceiveEmail { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Bạn chưa nhập tên người nhận.")]
        [Display(Name = "Người nhận")]
        public string ReceiveName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ nhận hàng.")]
        [Display(Name = "Địa chỉ")]
        [StringLength(255)]
        public string ReceiveAddress { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không đúng.")]
        [StringLength(50)]
        [Display(Name = "Số điện thoại")]
        public string ReceivePhone { get; set; }

        [StringLength(255)]
        public string DistrictId { get; set; }

        [StringLength(255)]
        public string ProvinceId { get; set; }

        [StringLength(255)]
        public string WardId { get; set; }

        public bool OtherAddress { get; set; }

        [StringLength(250)]
        [Display(Name = "Người nhận")]
        public string OtherReceiveName { get; set; }

        [StringLength(255)]
        [Display(Name = "Địa chỉ")]
        public string OtherReceiveAddress { get; set; }

        [StringLength(50)]
        [Display(Name = "Số điện thoại")]
        public string OtherReceivePhone { get; set; }

        [StringLength(50)]
        [Display(Name = "Địa chỉ Email"), EmailAddress(ErrorMessage = "Định dạng Email không đúng")]
        public string OtherEmail { get; set; }

        [StringLength(255)]
        public string OtherDistrictId { get; set; }

        [StringLength(255)]
        public string OtherProvinceId { get; set; }

        [StringLength(255)]
        public string OtherWardId { get; set; }

        [Display(Name = "% Tạm ứng")]
        public int Deposit { get; set; }

        [Display(Name = "Thanh toán tạm ứng")]
        public bool? IsDeposit { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Ghi chú đơn hàng")]
        public string Note { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Bạn chưa đồng ý điều khoản.")]
        public bool accept { get; set; }

    }
}