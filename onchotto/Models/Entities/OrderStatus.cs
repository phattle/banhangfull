namespace OnChotto.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderStatus")]
    public partial class OrderStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái En")]
        public string NameEn { get; set; }

        [Column(TypeName = "ntext")]
        public string Note { get; set; }         


        [Display(Name = "SiteId")]
        public int SiteId { get; set; }
        [Display(Name = "CompanyID")]
        public int CompanyID { get; set; }

        [StringLength(50)]
        [Display(Name = "UserCreate")]
        public string UserCreate { get; set; }

        [Display(Name = "DateCreate")]
        public DateTime DateCreate { get; set; }

        [StringLength(50)]
        [Display(Name = "UserUpdate")]
        public string UserUpdate { get; set; }

        [Display(Name = "DateUpdate")]
        public DateTime DateUpdate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
