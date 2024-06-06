namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AssetUpdateUploads
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; }

        public string Column { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string CreatedBy { get; set; }
    }
}
