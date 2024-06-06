using System;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class AssignedAssetChangeHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AssetAssignId { get; set; }
        public string CodeFrom { get; set; }
        public string CodeTo { get; set; }
        public string TrackingNo { get; set; }
        public string ReturnTrackingNo { get; set; }
        public string TicketNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}