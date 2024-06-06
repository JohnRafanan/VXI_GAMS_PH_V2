using System;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class UserUploadHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UploadedTemplateFileDir { get; set; }
        public string UploadedBy { get; set; }
        public DateTime DateUploaded { get; set; } = DateTime.Now;
    }
}