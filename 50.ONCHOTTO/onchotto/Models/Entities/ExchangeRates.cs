using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{ 
    public partial class ExchangeRates
    {

        public int Id { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string Code { get; set; }
         
        public decimal ExchangeRate { get; set; }
         
        public DateTime DateTime { get; set; }

        public int FiscalMonth { get; set; }

        public int FiscalYear { get; set; }
    }
}