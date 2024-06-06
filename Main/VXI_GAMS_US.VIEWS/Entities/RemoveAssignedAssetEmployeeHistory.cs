using System;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class RemoveAssignedAssetEmployeeHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? AssetHistoryId { get; set; }
        public string Code { get; set; }
        public string TrackingNo { get; set; }
        public string TicketNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}