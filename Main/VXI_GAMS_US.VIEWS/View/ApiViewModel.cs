using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI_GAMS_US.VIEWS.View
{
    public class ApiViewModel<T> where T : class
    {
        public ICollection<T> Table { get; set; } = new List<T>();
    }
}
