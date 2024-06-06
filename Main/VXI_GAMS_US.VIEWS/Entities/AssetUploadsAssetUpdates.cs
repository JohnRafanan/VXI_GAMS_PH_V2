namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class AssetUploadsAssetUpdates
    {
        public int Id { get; set; }
        public string SM_Assettag { get; set; }
        public string Status { get; set; }
        public string ITrackTicketNumber { get; set; }
        public string Issue { get; set; }
        public string ApprovedLoa { get; set; }
        public string ApprovedPezaForm8106Number { get; set; }
        public string ApprovedPezaForm8105Number { get; set; }
        public string ReturnedDate { get; set; }
        public string DateReportedAsLostMissingStolen { get; set; }
        public string SalDedAmount { get; set; }
        public string DisposalDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string Code { get; set; }


    }
}
