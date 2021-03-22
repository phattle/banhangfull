using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    [Table("PriceCurrency")]
    public class PriceCurrency
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        [Display (Name ="Loại tiền tệ")]
        public string CurrencyName { get; set; }

        [StringLength(250)]
        [Display(Name = "Mô tả tiền tệ")]
        public string Discription { get; set; }
    }
}