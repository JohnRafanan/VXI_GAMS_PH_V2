namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class AssetUploadsAssetAssignments
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ITrackTicket { get; set; }
        public string SM_Assettag { get; set; }
        public string WorkType { get; set; }
        public string Site { get; set; }
        public string Hrid { get; set; }
        public string Floor { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string EmployeeEmail { get; set; }
        public string DeploymentIssuanceDate { get; set; }
        public string TrackingNumber { get; set; }
        public string Name { get; set; }
        public bool EmployeeStatus { get; set; }
        public string EmployeeTitle { get; set; }
        public string CreatedBy { get; set; }


    }
}
