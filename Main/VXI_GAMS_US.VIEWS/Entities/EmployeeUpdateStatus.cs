namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmployeeUpdateStatus
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsActive { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
