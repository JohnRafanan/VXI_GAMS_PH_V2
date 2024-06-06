namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssignedAssetChangeUploads
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FromCode { get; set; }

        public string ToCode { get; set; }

        public string TrackingNo { get; set; }

        public string TicketNo { get; set; }

        public string CreatedBy { get; set; }

        public string ReturnTrackingNo { get; set; }
    }
}
