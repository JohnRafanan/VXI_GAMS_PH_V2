namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReplaceUploads
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; }

        public string Serial { get; set; }

        public string Manufacturer { get; set; }

        public string CreatedBy { get; set; }

        public string Vendor { get; set; }

        public string PurchaseOrder { get; set; }

        public string CostValue { get; set; }

        //public string Ram { get; set; }

        //public string HdCapacity { get; set; }

        //public string MonitorSize { get; set; }

        //public string YearModel { get; set; }
        //public string Classification { get; set; }
    }
}
