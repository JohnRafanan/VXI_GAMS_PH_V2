namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class AssetUploadsFarmouts
    {
        public int Id { get; set; }
        public string SM_AssetTag { get; set; }
        public string Transaction_Type { get; set; }
        public string iTrack_Ticket_Number { get; set; }
        public string Purchase_Order_Number { get; set; }
        public string Quantity { get; set; }
        public string Originating_Site { get; set; }
        public string Destination_Site { get; set; }
        public string LOA_Number { get; set; }
        public string Validity { get; set; }
        public string LOA_OR_Number { get; set; }
        public string LOA_Fee { get; set; }
        public string Bond_Issuer { get; set; }
        public string Bondable_Ammount { get; set; }
        public string Surety_Bond_Policy_Number { get; set; }
        public string Validity_Period { get; set; }
        public string Surety_Bond_Official_Receiept { get; set; }
        public string Amount_Paid { get; set; }
        public string Peza_Form_8106_Number { get; set; }
        public string Peza_Permit_Number { get; set; }
        public string Peza_Approval_Date { get; set; }

    }
}
