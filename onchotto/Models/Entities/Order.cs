
namespace OnChotto.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Order
    { 
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public virtual string ItemCode
        {
            get
            {
                if (OrderDate.HasValue)
                {
                    return $"SBP{OrderDate.Value.Year.ToString("00")}{OrderDate.Value.Month:00}{Id:000000}";
                }
                return $"SBP{DateTime.Now.Year:00}{DateTime.Now.Month:00}{Id:000000}";
            }
        }

        [Display(Name = "Khách hàng")]
        public string UserId { get; set; }

        [Display(Name = "Trạng thái")]
        public int? StatusId { get; set; }

        [Display(Name = "Mã Giảm giá")]
        public string Coupon { get; set; }

        [Display(Name = "Mô tả giảm giá")]
        public string CouponDescription { get; set; }

        [Display(Name = "Giảm giá")]
        public decimal? Discount { get; set; }

        [Display(Name = "Khuyến mãi thêm")]
        public decimal? ExtraDiscount { get; set; }

        [Display(Name = "Tổng trọng lượng(kg)")]
        public decimal? TotalWeight { get; set; }

        [Display(Name = "Tổng tiền hàng(đ)")]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Tổng đơn hàng(đ)")]
        public decimal? TotalOrder { get; set; }

        [Display(Name = "% Tạm ứng")]
        public int Deposit { get; set; }

        [Display(Name = "Thanh toán tạm ứng")]
        public bool? IsDeposit { get; set; }
        [Display(Name = "Thanh toán đủ")]
        public bool? Ispayenough { get; set; }
        [Required]
        public int? TransportId { get; set; }

        public int? PaymentMethodId { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Bạn chưa nhập tên người nhận.")]
        [Display(Name = "Người nhận")]
        public string ReceiveName { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Bạn chưa nhập Email người nhận."), EmailAddress]
        [Display(Name = "Email Người nhận")]
        public string ReceiveEmail { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ nhận hàng.")]
        [StringLength(255)]
        public string ReceiveAddress { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại người nhận.")]
        [StringLength(50)]
        [Display(Name = "Số điện thoại")]
        public string ReceivePhone { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Yêu cầu của khách hàng đối với tất cả đơn hàng")]
        public string Note { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Ngày giao")]
        public DateTime? RequireDate { get; set; }

        [Display(Name = "Thuế liên bang")]
        public decimal? FederalTax { get; set; }

        [Display(Name = "Shipping In Land")]
        public decimal? ShippingInLand { get; set; }

        [Display(Name = "Handling Fee")]
        public decimal? HandlingFee { get; set; }

        [Display(Name = "A/F Fee  ")]
        public decimal? AFFee { get; set; }

        [Display(Name = "Phí DV thông quan VN")]
        public decimal? ClearanceFee { get; set; }

        [Display(Name = "Phí phát hàng")]
        public decimal? ShippingFee { get; set; }

        [Display(Name = "Phí dịch vụ TECS ")]
        public decimal? TECSServicesFee { get; set; }

        [Display(Name = "Phí giao dịch")]
        public decimal? TransactionFee { get; set; }

        [Display(Name = "Phí VAT")]
        public decimal? VATFee { get; set; }

        [Display(Name = "Thuế nhập khẩu")]
        public decimal? ImportTax { get; set; }

        [Display(Name = "Phí hải quan")]
        public decimal? CustomFee { get; set; }

        [Display(Name = "Mã khuyến mãi")]
        public string PromotionCode { get; set; }

        [Display(Name = "Giá trị khuyến mãi")]
        public string PromotionValue { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú của mua hàng")]
        public string Noteshopper { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú của CSKH")]
        public string NoteCustomerService { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú của kho hàng")]
        public string NoteWarehouseStaff { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }    

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual Transport Transport { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual ICollection<Product> Products { get; private set; }
    }
}
