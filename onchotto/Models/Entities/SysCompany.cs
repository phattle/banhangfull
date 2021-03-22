using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OnChotto.Models.Entities
{
    [Table("SysCompanys")]
    public class SysCompany
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public SysCompany()
        //{
        //    SysSites = new HashSet<SysSite>();
        //}
        [Key]
        public int CompId { get; set; }
        public int SiteId { get; set; }
        public int CompanyID { get; set; }
        public int? ParentComId { get; set; }
        public int? ComTypeId { get; set; }
        public int? RegionId { get; set; }

        //[Required(ErrorMessage = "Bạn chưa nhập mã Công ty")]
        [StringLength(50)]
        public string CompCode { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Tên công ty")]
        [StringLength(255)]
        public string CompName { get; set; }

        [StringLength(255)]
        public string CompNameEN { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string BusinessAddress { get; set; }
        public bool otherAddress { get; set; }

        [StringLength(255)]
        public string BusinessFile { get; set; }
        [StringLength(255)]
        public string BusinessRegistrationNo { get; set; }
        [StringLength(255)]
        public string BusinessOwnerName { get; set; }

        [StringLength(255)]
        public string AccountName { get; set; }
        [StringLength(255)]
        public string AccountNumber { get; set; }
        [StringLength(255)]
        public string Bank { get; set; }
        [StringLength(255)]
        public string BrachName { get; set; }
        [StringLength(255)]
        public string BankCode { get; set; }
        [StringLength(255)]
        public string Swift { get; set; }

        [StringLength(255)]
        public string Tel { get; set; }

        [StringLength(255)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string TaxCode { get; set; }

        [StringLength(50)]
        public string EMail { get; set; }

        [StringLength(4)]
        public string CuryId { get; set; }

        [StringLength(6)]
        public string BeginPerPost { get; set; }

        [StringLength(50)]
        public string SerialNbrDefault { get; set; }

        public string DueDate { get; set; }

        public decimal? MaxAmount { get; set; }

        public int? IsDependent { get; set; }
        public string UserId { get; set; }
        //public virtual ICollection<SysSite> SysSites { get; set; }
        //public virtual ApplicationUser User { get; set; }

    }
}