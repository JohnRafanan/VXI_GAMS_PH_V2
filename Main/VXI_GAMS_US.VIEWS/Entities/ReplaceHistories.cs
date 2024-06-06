namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReplaceHistories
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; }

        public string FromSerial { get; set; }

        public string ToSerial { get; set; }

        public string FromManufacturer { get; set; }

        public string ToManufacturer { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public string FromVendor { get; set; }

        public string ToVendor { get; set; }

        public string FromPurchaseOrder { get; set; }

        public string ToPurchaseOrder { get; set; }

        public string FromCostValue { get; set; }

        public string ToCostValue { get; set; }
    }
}
