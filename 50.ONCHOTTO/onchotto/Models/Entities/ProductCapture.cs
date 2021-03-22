namespace OnChotto.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductCaptures")]
    public partial class ProductCapture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        [Key]
        public int Id { get; set; }
        public DateTime? TimeSeach { get; set; }

        [StringLength(50)]
        [Display(Name = "Asin Code")]
        public string AsinCode { get; set; }

        [StringLength(255)]
        [Display(Name = "Keyword Seach")]
        public string KeywordSeach { get; set; }

        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }

        [StringLength(4000)]
        [Display(Name = "ProductLink")]
        public string ProductLink { get; set; }

        [StringLength(4000)]
        [Display(Name = "Product Link Detail")]
        public string ProductLinkDetail { get; set; }

        [StringLength(4000)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [StringLength(4000)]
        [Display(Name = "ProductName Decrition")]
        public string ProductNamedecrition { get; set; }

        [Display(Name = "Item Weight")]
        public decimal? ItemWeight { get; set; }

        [StringLength(4000)]
        [Display(Name = "Product Dimensions")]
        public string ProductDimensions { get; set; }

        [StringLength(4000)]
        [Display(Name = "Image Link")]
        public string ImageLink { get; set; }

        public int CaptureId{ get; set; }

        public string CategoryName { get; set; }
        public string SearchID { get; set; }
        
    }
}
