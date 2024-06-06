using System;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class EmployeeUpdateHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsActive { get; set; } = true;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}