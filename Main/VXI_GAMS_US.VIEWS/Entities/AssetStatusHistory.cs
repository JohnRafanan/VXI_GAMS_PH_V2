using System;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class AssetStatusHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}