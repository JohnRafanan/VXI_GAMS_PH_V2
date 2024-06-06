using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace VXI_GAMS_US.ApiControllers
{
    public interface IApiController<in T> where T : class
    {
        Task<IHttpActionResult> Get();
        Task<IHttpActionResult> Get(Guid id);
        Task<IHttpActionResult> Post([FromBody] T value);
        Task<IHttpActionResult> Put(Guid id, [FromBody] T value);
        Task<IHttpActionResult> Delete(Guid id);
    }
}