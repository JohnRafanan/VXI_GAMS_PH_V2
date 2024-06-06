using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class Asset
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(250)]
        [Index(IsUnique = true)]
        public string Code { get; set; }
        public string Description { get; set; }
        public string SerialNo { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public Guid? StatusId { get; set; }
        public string Vendor { get; set; }
        public string PurchaseOrder { get; set; }
        public string CostValue { get; set; }
        public string Site { get; set; }
        //public string EmployeeStatus { get; set; }
        //public string EmployeeTitle { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDisposed { get; set; } = false;
        public int Year { get; set; } = DateTime.Now.Year;
        public int Month { get; set; } = DateTime.Now.Month;
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}