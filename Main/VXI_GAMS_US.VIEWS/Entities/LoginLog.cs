using System;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class LoginLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string User { get; set; }
        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}