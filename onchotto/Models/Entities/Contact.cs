namespace OnChotto.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Contact
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Display(Name = "Người liên hệ")]
        public string ContactName { get; set; }

        [StringLength(250)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [StringLength(250)]
        [Display(Name = "Điện thoại")]
        public string ContactPhone { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Nội dung liên hệ")]
        public string Message { get; set; }

        public bool? IsRead { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; }

        [Display(Name = "Ngày liên hệ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

    }
}
