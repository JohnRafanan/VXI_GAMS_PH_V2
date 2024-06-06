using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class Vendors
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(250)]
        [Index(IsUnique = true)]
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int Year { get; set; } = DateTime.Now.Year;
        public int Month { get; set; } = DateTime.Now.Month;
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
