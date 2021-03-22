namespace OnChotto.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Quotation
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Điện thoại")]
        public string CustomerPhone { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Thông tin thêm")]
        public string ProductLinks { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        [Display(Name = "Thông tin thêm")]
        public string AdditionalInformation { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedAt { get; set; }

    }
}
