using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI_GAMS_US.VIEWS.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string NtAccount { get; set; }
        public string Team { get; set; }

        public string Project { get; set; }

        public string PositionLevel { get; set; }

        public string ID { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string MiddleName { get; set; }

        public string CompleteName => $"{FirstName} {LastName}".Trim().ToUpper();

        public string HireDate { get; set; }

        public string Gender { get; set; }

        public string Department { get; set; }

        public string DepartmentName { get; set; }

        public string TeamID { get; set; }

        public string LocationDesc { get; set; }

        public string TitleName { get; set; }

        public string SupervisorID { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string ProjectID { get; set; }

        public string JobLevel { get; set; }

        public string Ranking { get; set; }
    }
}
