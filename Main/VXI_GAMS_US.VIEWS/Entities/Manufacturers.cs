namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Manufacturers
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(250)]
        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }
    }
}