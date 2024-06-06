using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class SubCategory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? CategoryId { get; set; }
        [StringLength(250)]
        [Index(IsUnique = true)]
        public string Code { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}