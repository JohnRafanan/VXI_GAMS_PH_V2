using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.DynamicData;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.VIEWS.Entities;

namespace VXI_GAMS_US.ApiControllers
{
    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    public class CategoryApiController : ApiController, IApiController<Category>
    {
        #region Implementation of IApiController<in Category>

        public async Task<IHttpActionResult> Get()
        {
            try
            {
                using (var db = new DataContext())
                {
                    return Ok(await db.Categories.Where(x => x.IsActive).ToListAsync());
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
                    var entity = await db.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();
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

        public async Task<IHttpActionResult> Post(Category value)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Categories.FirstOrDefaultAsync(x => x.Id == value.Id);
                    if (entity == null)
                    {
                        value.CreatedBy = User.Identity.GetUserName();
                        value.Code = DataHelper.AlphaSpaceOnly(value.Code);
                        db.Categories.Add(value);
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

        public async Task<IHttpActionResult> Put(Guid id, Category value)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Categories.FindAsync(id);
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
                    var entity = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing");
                    var count = await db.Database
                        .SqlQuery<int>(
                            $"EXEC dbo.GetActiveSubCategoryCountByCategoryId '{id}' ;")
                        .SingleOrDefaultAsync();
                    if (count > 0)
                        throw new Exception("Failed. Category is in use");
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