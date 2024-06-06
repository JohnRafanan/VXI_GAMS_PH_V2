namespace VXI_GAMS_US.VIEWS.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TemplateHistories
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UploadedTemplateFileDir { get; set; }

        public string TemplateBackupFileDir { get; set; }

        public string UploadedBy { get; set; }

        public DateTime DateUploaded { get; set; }
    }
}
