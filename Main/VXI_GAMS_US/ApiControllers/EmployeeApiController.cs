using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.VIEWS.Entities;
using VXI_GAMS_US.VIEWS.View;

namespace VXI_GAMS_US.ApiControllers
{
    public class EmployeeApiController : ApiController
    {
        // GET api/AssetApi
        [HttpGet]
        [Route("api/EmployeeApi/{hrid}")]
        public async Task<IHttpActionResult> GetByHrid(string hrid)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db
                        .Database
                        .SqlQuery<RetrievalVM>("EXEC dbo.spGetAssetsByHrid @hrid", new SqlParameter("@hrid", hrid ?? string.Empty))
                        .ToListAsync();
                    return Ok(entity);
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpGet]
        [Route("api/EmployeeApi/{hrid}/search")]
        public async Task<IHttpActionResult> GetNameByHrid(string hrid)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.BulkApiEmployee.FirstOrDefaultAsync(x=>x.IsActive && x.ID.ToLower().Equals(hrid.ToLower()));
                    return Ok(entity);
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Save([FromBody] RetrievalVM[] model)
        {
            try
            {
                if (!model.Any())
                    throw new Exception("Operation Failed. No data to retrieve.");
                if (model.Any(x => string.IsNullOrEmpty(x.category)))
                    throw new Exception("Operation Failed. No data to retrieve.");
                var cnt = model.Count(x => string.IsNullOrEmpty(x.hrid));
                if (cnt > 0)
                    throw new Exception("Operation Failed. Employee Hrid is not set.");
                using (var db = new DataContext())
                {
                    var hrid = model.First().hrid;
                    //check for dirty data and throw if dirty
                    var list = await db
                        .AssignAssetEmployee.Where(x => x.Hrid == hrid).ToListAsync();
                    if (!list.Any())
                        throw new Exception($"Operation Failed. No asset has been assign to HRID[{hrid}]");
                    var modelSerials = model.Select(x => x.serialNo).ToList();
                    var listSerials = list.Select(x => x.Code).ToList();
                    var invalid = modelSerials.Except(listSerials).ToArray();
                    var errors = string.Empty;
                    if (invalid.Any()) 
                        throw new Exception(invalid.Aggregate(errors, (current, code) => current + $"Asset Code[{code}] is either invalid or not assigned to HRID [{hrid}]\r\n"));
                    //if everything is fine proceed saving
                    db.RemoveAssignedAssetEmployeeUploads.AddRange(modelSerials.Select(x=> new RemoveAssignedAssetEmployeeUploads
                    {
                        Code = x,
                        CreatedBy = "CR-USER"
                    }));
                    await db.SaveChangesAsync();
                    await db.Database
                        .ExecuteSqlCommandAsync("EXEC [dbo].[spRemoveAssignedAssetEmployeeUploads] @createdBy = 'CR-USER'");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }
        private class Serials
        {
            public string Serial { get; set; }
        }
    }
}