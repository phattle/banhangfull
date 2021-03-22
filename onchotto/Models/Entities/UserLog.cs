

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OnChotto.Models.Entities
{

    public partial class UserLog
    {

        public int Id { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(4000)]
        public string Message { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(4000)]
        public string Title { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string UserName { get; set; }
         
    }
}
