using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnChotto.Models.Entities
{
    public enum Sections
    {
        Homepage, ProductCategory, Product, Blog , PostDetail
    };

    public partial class Banner
    {
        [Key]
        public int BannerId { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Name { get; set; }

        [Display(Name = "Loại banner")]
        public int BannerTypeId { get; set; }

        [Display(Name = "Áp dụng cho")]
        public Sections ItemFor { get; set; }

        [Display(Name = "ID Áp dụng")]
        public int? ItemForId  { get; set; }

        [Display(Name = "Vị trí banner")]
        public int? BannerPositionId { get; set; }

        [Display(Name = "Hình banner")]
        public string Image { get; set; }

        [Display(Name = "Liên kết")]
        public string Url { get; set; }

        [Display(Name = "Mô tả")]
        [Column(TypeName = "nText")]
        public string Description { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool Active { get; set; }

        public virtual BannerType BannerType { get; set; }
        public virtual BannerPosition BannerPosition { get; set; }
    }
}
