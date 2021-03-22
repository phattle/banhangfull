using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    [Table("UsersFunctionDetails")]
    public class UsersFunctionDetail
    {
        [Key]
        public int Id { get; set;}

        [StringLength(128)]
        [Display(Name = "User Name")]
        public string UsersId { get; set; }

        
        [StringLength(50)]
        [Display(Name = "Tên chức năng")]
        public string FunctionId { get; set; }

        //public virtual Function Functions { get; set; }
    }
}