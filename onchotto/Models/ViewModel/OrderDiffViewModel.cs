using OnChotto.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.ViewModel
{
    public class OrderDiffViewModel
    {
        public OrderDiffViewModel()
        {
            OrderDetailDiffs = new HashSet<OrderDetailDiff>();
        }

        [Display(Name = "Khách hàng")]
        public string UserId { get; set; }

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

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại người nhận.")]
        [StringLength(50)]
        [Display(Name = "Số điện thoại")]
        public string ReceivePhone { get; set; }
        [Display(Name = "Quận huyện")]
        [StringLength(255)]
        public string DistrictId { get; set; }

        [Display(Name = "Tỉnh thành")]
        [StringLength(255)]
        public string ProvinceId { get; set; }

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

        [Display(Name = "% Tạm ứng")]
        public int Deposit { get; set; }

        [Display(Name = "Thanh toán tạm ứng")]
        public bool? IsDeposit { get; set; }

        [Display(Name = "Thanh toán đủ")]
        public bool? Ispayenough { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Ghi chú đơn hàng")]
        public string Note { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Ngày yêu cầu")]
        public DateTime? RequireDate { get; set; }

        //Oderdetail Diff
        [StringLength(50)]
        [Display(Name = "Mã đơn hàng")]
        public string OrderNo { get; set; }

        [StringLength(50)]
        [Display(Name = "Mã tracking")]
        public string OrderTrackingNo { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Store Name")]
        [StringLength(50)]
        [Display(Name = "Store name")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "Bạn chưa copy links sản phẩm")]
        [Display(Name = "Link SP")]
        public string ProductLink { get; set; }

        [Required(ErrorMessage = "Bạn chưa mô tả tả sản phẩm")]
        [StringLength(255)]
        [Display(Name = "Tên SP")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập size sản phẩm")]
        [StringLength(50)]
        [Display(Name = "Size")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số lượng")]
        [Display(Name = "Số lượng")]
        //Ghi nhầm số lương quantity thành amount
        public int? Amount { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số lượng")]
        [Display(Name = "Trọng lượng")]
        public string Weight { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập giá trên web")]
        [Display(Name = "Giá bán")]
        public string Price { get; set; }

        [Display(Name = "Giảm giá Off")]
        public string Discount { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập giá bán Off")]
        [Display(Name = "Giá bán Off")]
        public string PriceAfter { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Ghi chú sp")]
        public string Notes { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái SP")]
        public string ProductStatus { get; set; }

        [Display(Name = "Tổng tiền hàng(đ)")]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Tổng trọng lượng(kg)")]
        public decimal? TotalWeight { get; set; }
        public virtual ICollection<OrderDetailDiff> OrderDetailDiffs { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái ")]
        public string OrderStatusName { get; set; }

        public int StatusId { get; set; }

        [Display(Name = "Id đơn hàng")]
        public int OrderDiffId { get; set; }

        [StringLength(100)]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethodsName { get; set; }

        [Display(Name = "Đơn hàng")]
        public string ItemCode { get; set; }

        [StringLength(50)]
        [Display(Name = "MAWB")]
        public string MAWB { get; set; }

        [StringLength(50)]
        [Display(Name = "Đơn Vị")]
        public string WeightUnit { get; set; }
        [StringLength(50)]
        [Display(Name = "Tiền tệ")]
        public string Currency { get; set; }

        [StringLength(50)]
        [Display(Name = "Loại giao dịch")]
        public string TransType { get; set; }

    }
}