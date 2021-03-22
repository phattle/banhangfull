using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    [Table("Wards")]
    public class Wards
    {
        [Key]
        public string WardId { get; set; }

        [StringLength(255)]
        [Display(Name = "Phường/Xã")]
        public string Name { get; set; }

        public string districtid { get; set; }

        public string type { get; set; }

        public virtual District District { get; set; }
    }
}