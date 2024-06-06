using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI_GAMS_US.VIEWS.View
{
    public class TempTable
    {

        public int Id { get; set; }
        public string Team { get; set; }
        public string Project { get; set; }

        public string PositionLevel { get; set; }

     
        public string HrId { get; set; }

    
        public string FirstName { get; set; }

  
        public string LastName { get; set; }

     
        public string HireDate { get; set; }

    
        public string Gender { get; set; }

   
        public string Department { get; set; }

       
        public string LocationDesc { get; set; }

    
        public string TitleName { get; set; }

     
        public string SupervisorId { get; set; }

   
        public string EmailAddress { get; set; }

      
        public string Country { get; set; }

    
        public string Address1 { get; set; }

     
        public string Address2 { get; set; }

     
        public string PostalCode { get; set; }

        public DateTime? SKODate { get; set; }

      
        public string PhoneNumber { get; set; }

    
        public string Class { get; set; }

     
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

     
        public string AssetCategory { get; set; }

    
        public string AssetSubCategory { get; set; }

        public string SerialNumber { get; set; }
      
        public string Model { get; set; }
      
        public string TrackingNumber { get; set; }
       
        public string Brand { get; set; }

        public bool? IsActive { get; set; }
    }
}
