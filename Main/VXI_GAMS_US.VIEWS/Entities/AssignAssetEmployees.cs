namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssignAssetEmployees
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(250)]
        public string Code { get; set; }

        public string WorkType { get; set; }

        public string Hrid { get; set; }

        public string Floor { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }

        public string ContactNo { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public string Email { get; set; }

        public string TrackingNo { get; set; }

        public string TicketNo { get; set; }
    }
}
