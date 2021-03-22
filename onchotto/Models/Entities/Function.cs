using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    [Table("Function")]
    public class Function
    {
        [Key]
        [StringLength(50)]
        [Display(Name ="Id chức năng")]
        public string FunctionId { get; set; }

        [StringLength(250)]
        [Display(Name ="Tên chức năng")]
        public string TitleName { get; set; }

        [StringLength(250)]
        [Display(Name = "Tên View")]
        public string ViewName { get; set; }

        [StringLength(250)]
        [Display(Name = "Model Id")]
        public string ModelId { get; set; }

        [StringLength(250)]
        [Display(Name = "Class Id")]
        public string ClassId { get; set; }

        [StringLength(250)]
        [Display(Name = "Sự kiện Id")]
        public string OnclickId { get; set; }


        [Column(TypeName = "ntext")]
        [Display(Name ="Diễn giả")]
        public string Description { get; set; }

        [Display(Name ="Trạng thái")]
        public bool Status { get; set; }

        //public virtual ICollection<UsersFunctionDetail> UsersFunctionDetails { get; set; }
        //public virtual ICollection<RolesFunctionDetail> RolesFunctionDetails { get; set; }


    }
}