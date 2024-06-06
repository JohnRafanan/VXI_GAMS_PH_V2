namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssignedAssetChangeHistories
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CodeFrom { get; set; }

        public string CodeTo { get; set; }

        public string TrackingNo { get; set; }

        public string TicketNo { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public string ReturnTrackingNo { get; set; }

        public Guid AssetAssignId { get; set; }
    }
}
