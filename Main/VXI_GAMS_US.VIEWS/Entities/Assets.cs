namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Assets
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(250)]
        [Index(IsUnique = true)]

        public string Code { get; set; }

        public string Description { get; set; }

        [StringLength(256)]
        public string SerialNo { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? SubCategoryId { get; set; }

        public Guid? StatusId { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        public Guid? ManufacturerId { get; set; }

        public int Year { get; set; } = DateTime.Now.Year;
        public int Month { get; set; } = DateTime.Now.Month;

        public bool IsDisposed { get; set; }

        public string Site { get; set; }

        public string EmployeeStatus { get; set; }

        public string EmployeeTitle { get; set; }

        public string Vendor { get; set; }

        public string PurchaseOrder { get; set; }

        public string CostValue { get; set; }
        public string Processor { get; set; }

        public string Ram { get; set; }

        public string HdCapacity { get; set; }

        public string MonitorSize { get; set; }

        public string YearModel { get; set; }
        public string Classification { get; set; }
        public string WorkType { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string TrackingNumber { get; set; }
        public string ItTicketNumber { get; set; }
        public string DeployedDate { get; set; }
    }
}
