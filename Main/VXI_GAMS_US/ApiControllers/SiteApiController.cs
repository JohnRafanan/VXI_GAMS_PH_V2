using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.VIEWS.View;

namespace VXI_GAMS_US.ApiControllers
{
    public class SiteApiController : ApiController
    {
        internal class SiteVm
        {
            public string Region { get; set; }
            public string Location { get; set; }
            public string LocationDesc { get; set; }
        }
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var data = await db.Assets.Select(x => new SiteVm
                        {
                            Region = x.Site,
                            Location = x.Site,
                            LocationDesc = x.Site
                        })
                        .Distinct()
                        .ToListAsync();
                    return Ok(data.OrderBy(x=>x.LocationDesc));
                }
                //var url = string.Format(ConfigurationManager.AppSettings["GLOBAL_API_LOCATIONS"],
                //    ConfigurationManager.AppSettings["GLOBAL_REGION"]);
                //var sites = await GlobalApi.GetLocationsFromApi(url);
                //return Ok(sites.Table);
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }
    }
}