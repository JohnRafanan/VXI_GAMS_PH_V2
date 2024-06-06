namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class AssetUploadsAssetReplacements
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string Code { get; set; }
        public string OldSerial { get; set; }
        public string NewSerial { get; set; }
        public string NewBrand { get; set; }
        public string Vendor { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string CostValue { get; set; }


    }
}
