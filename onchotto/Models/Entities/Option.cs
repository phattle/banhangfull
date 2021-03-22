using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnChotto.Models.Entities
{
    public partial class Option
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string Value { get; set; }

        public string Description { get; set; }
    }
}