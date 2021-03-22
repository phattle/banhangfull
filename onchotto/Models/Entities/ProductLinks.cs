using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

 
namespace OnChotto.Models.Entities
{

    [Table("ProductLinks")]
    public partial class ProductLinks
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }
         
        [StringLength(50)]
        [Display(Name = "AsinCode")]
        public string AsinCode { get; set; }


        [StringLength(500)]
        [Display(Name = "ItemUrl")]
        public string ItemUrl { get; set; }

        [StringLength(4000)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual Product Product { get; set; }
    }
}
