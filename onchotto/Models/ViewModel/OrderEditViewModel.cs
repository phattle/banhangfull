using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Foolproof;
using System;
using System.Collections.Generic;

namespace OnChotto.Models.ViewModel
{
    public class OrderEditViewModel
    {
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

        [StringLength(250)]
        [Required(ErrorMessage = "Bạn chưa nhập tên người nhận.")]
        [Display(Name = "Tên Người nhận")]
        public string ReceiveName { get; set; }
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ nhận hàng.")]
        [Display(Name = "Địa chỉ")]
        [StringLength(255)]
        public string ReceiveAddress { get; set; }

        [Display(Name = "Quận huyện")]
        [StringLength(255)]
        public string DistrictId { get; set; }

        [Display(Name = "Tỉnh thành")]
        [StringLength(255)]
        public string ProvinceId { get; set; }

        [Display(Name = "Nhà vận chuyển")]
        [Required]
        public int? TransportId { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại người nhận.")]
        [StringLength(50)]
        [Display(Name = "Số điện thoại")]
        public string ReceivePhone { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Y/C của KH đối với tất cả đơn hàng")]
        public string Note { get; set; }

        public int? PaymentMethodId { get; set; }

        [Display(Name = "Trạng thái")]
        public int? StatusId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày giao")]
        public DateTime? RequireDate { get; set; }

        [Display(Name = "% Tạm ứng")]
        public int Deposit { get; set; }

        [Display(Name = "Giảm giá")]
        public decimal? Discount { get; set; }

        [Display(Name = "Vận chuyển bang")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        public decimal? ShippingInLand { get; set; }
        
        [Display(Name = "Handling(Sort, pack, label...)  ")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        public decimal? HandlingFee { get; set; }

        [Display(Name = " Vận chuyển US -> VN (A/F)  ")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        public decimal? AFFee { get; set; }

        [Display(Name = "Thông quan")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        public decimal? ClearanceFee { get; set; }

        [Display(Name = "Lao vụ")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        public decimal? TECSServicesFee { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        [Display(Name = "Xử lý giao dịch")]
        public decimal? TransactionFee { get; set; }

        [Display(Name = "Tổng tiền hàng(đ)")]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Tổng đơn hàng(đ)")]
        public decimal? TotalOrder { get; set; }

        //[Display(Name = "Phí VAT")]
        //public decimal? VATFee { get; set; }

        //[Display(Name = "Thuế nhập khẩu")]
        //public decimal? ImportTax { get; set; }

        [Display(Name = "Lệ phí Hải quan")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        public decimal? CustomFee { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        public virtual ApplicationUser User { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú của mua hàng")]
        public string Noteshopper { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú của CSKH")]
        public string NoteCustomerService { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú của kho hàng")]
        public string NoteWarehouseStaff { get; set; }

        public virtual ICollection<Entities.OrderDetail> OrderDetails { get; set; }

        public virtual Entities.OrderStatus OrderStatus { get; set; }

        public virtual Entities.Transport Transport { get; set; }

        public virtual Entities.PaymentMethod PaymentMethod { get; set; }
        public virtual ICollection<Entities.Product> Products { get; private set; }

    }

}