namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class AssetUploadsAssetDetails
    {
        public int Id { get; set; }
        public string PurchaseOrderNum { get; set; }
        public string ItemDesc { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Processor { get; set; }
        public string Ram { get; set; }
        public string ScreenSize { get; set; }
        public string SerialNumber { get; set; }
        public string LobOwner { get; set; }
        public string Status { get; set; }
        public string Floor { get; set; }
        public string AreaWorkstation { get; set; }
        

    }
}
