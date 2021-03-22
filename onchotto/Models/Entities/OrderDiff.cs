
namespace OnChotto.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OrderDiff
    { 
        public OrderDiff()
        { 

        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Mã đơn hàng
        public virtual string ItemCode
        {
            get
            {
                if (OrderDate.HasValue)
                {
                    return $"SmuaVN{OrderDate.Value.Year.ToString("00")}{OrderDate.Value.Month:00}{Id:000000}";
                }
                return $"SmuaVN{DateTime.Now.Year:00}{DateTime.Now.Month:00}{Id:000000}";
            }
        }

        [Display(Name = "Khách hàng")]
        public string UserId { get; set; }

        [Display(Name = "Trạng thái")]
        public int? StatusId { get; set; }

        [Display(Name = "Tổng trọng lượng(kg)")]
        public decimal? TotalWeight { get; set; }

        [Display(Name = "Tổng tiền hàng(đ)")]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Thanh toán tạm ứng")]
        public bool? IsDeposit { get; set; }
        [Display(Name = "Thanh toán đủ")]
        public bool? Ispayenough { get; set; }
        public int? PaymentMethodId { get; set; }

        [StringLength(50)]
        [Display(Name = "MAWB")] // Master Bill Air
        public string MAWB { get; set; }


        [StringLength(250)]
        [Required(ErrorMessage = "Bạn chưa nhập tên người nhận.")]
        [Display(Name = "Người nhận")]
        public string ReceiveName { get; set; }
                
        public string DistrictId { get; set; }       
        public string ProvinceId { get; set; }

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
        [Display(Name = "Ghi chú đơn hàng")]
        public string Note { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Ngày giao")]
        public DateTime? RequireDate { get; set; }
        public virtual ApplicationUser User { get; set; } 

    }
}
