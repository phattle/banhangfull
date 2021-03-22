﻿namespace OnChotto.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Slider
    {
        [Key]
        public int Id { get; set; }

        public string Name  { get; set; }

        public string Image  { get; set; }

        public string ImageEN { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }

        public string CaptionSmall { get; set; }

        public double ? Price { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        public Target Target  { get; set; }
    }
}
