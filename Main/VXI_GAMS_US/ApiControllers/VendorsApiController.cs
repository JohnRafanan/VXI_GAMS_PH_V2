using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.VIEWS.Entities;

namespace VXI_GAMS_US.ApiControllers
{
    public class VendorsApiController : ApiController, IApiController<Vendors>
    {
        #region Implementation of IApiController<in Vendors>

        public async Task<IHttpActionResult> Get()
        {
            try
            {
                using (var db = new DataContext())
                {
                    return Ok(await db.Vendors.Where(x => x.IsActive).ToListAsync());
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Vendors.Where(x => x.Id == id).FirstOrDefaultAsync();
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing");
                    return Ok(entity);
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        public async Task<IHttpActionResult> Post(Vendors value)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Vendors.FirstOrDefaultAsync(x => x.Id == value.Id);
                    if (entity == null)
                    {
                        value.CreatedBy = User.Identity.GetUserName();
                        value.Code = DataHelper.AlphaSpaceOnly(value.Code);
                        value.DateCreated = DateTime.Now;
                        db.Vendors.Add(value);
                    }
                    else
                    {
                        entity.UpdatedBy = User.Identity.GetUserName();
                        entity.DateUpdated = DateTime.Now;
                        entity.Code = DataHelper.AlphaSpaceOnly(value.Code);
                        db.Entry(entity).State = EntityState.Modified;
                    }
                    await db.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                if (error.ToLower().Contains("duplicate"))
                    error = "Failed. Item already exist";
                return BadRequest(error);
            }
        }

        public async Task<IHttpActionResult> Put(Guid id, Vendors value)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Vendors.FindAsync(id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing.");
                    entity.Code = DataHelper.AlphaSpaceOnly(value.Code);
                    entity.DateUpdated = DateTime.Now;
                    entity.UpdatedBy = User.Identity.GetUserName();
                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        public async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Vendors.FirstOrDefaultAsync(x => x.Id == id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing");
                    var count = await db.Assets.CountAsync(x => x.IsActive && x.Vendor == entity.Code);
                    if (count > 0)
                        throw new Exception("Failed. Vendors is in use");
                    entity.IsActive = false;
                    entity.UpdatedBy = User.Identity.GetUserName();
                    entity.DateUpdated = DateTime.Now;
                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        #endregion
    }
}