using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    [Table("FunctionMenus")]
    public class FunctionMenu
    {
        [Key]                
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Id User")]
        public string UserId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Menu Parent")]
        public string MenuParentId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Menu Child")]
        public string MenuChildId { get; set; }

        [Display(Name = "Type Menu")]
        public bool TypeMenu { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "STT")]
        public int STT { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}