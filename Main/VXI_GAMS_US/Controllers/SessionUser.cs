using System.Collections.Generic;

namespace VXI_GAMS_US.Controllers
{
    public class SessionUser
    {
        public string HRID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string BuildingAssignment { get; set; }

        public string SamAccount { get; set; }

        public string Email { get; set; }

        public string Team { get; set; }

        public string LineOfBusiness { get; set; }

        public IEnumerable<FilterItemViewModel> UserRole { get; set; } 
    }
}