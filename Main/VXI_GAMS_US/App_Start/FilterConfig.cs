using System.Web;
using System.Web.Mvc;

namespace VXI_GAMS_US
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
