using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ClosedXML.Excel;
using Microsoft.AspNet.Identity;
using MoreLinq;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.VIEWS.Entities;
using VXI_GAMS_US.VIEWS.View;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace VXI_GAMS_US.ApiControllers
{
    public class AssetApiController : ApiController
    {
        // GET api/AssetApi
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Assets.Where(x => x.IsActive).ToListAsync();
                    var final = entity
                        .Select(x => new AssetVm
                        {
                            IsActive = x.IsActive,
                            Manufacturer = db.Manufacturers.Find(x.ManufacturerId),
                            Description = x.Description,
                            Code = x.Code,
                            SerialNo = x.SerialNo,
                            Category = db.Categories.Find(x.CategoryId),
                            SubCategory = db.SubCategories.Find(x.SubCategoryId),
                            Status = db.Statuses.Find(x.StatusId),
                            DateUpdated = x.DateUpdated,
                            UpdatedBy = x.UpdatedBy,
                            CreatedBy = x.CreatedBy,
                            DateCreated = x.DateCreated,
                            Site = x.Site,
                            Classification = x.Classification,
                            Ram = x.Ram,
                            HdCapacity = x.HdCapacity,
                            MonitorSize = x.MonitorSize,
                            EmployeeStatus = x.EmployeeStatus,
                            EmployeeTitle = x.EmployeeTitle,
                            Id = x.Id
                        })
                        .ToList();
                    return Ok(final);
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpPost]
        [Route("api/AssetApi/{id}/table")]
        public async Task<IHttpActionResult> GetData(Guid id, [FromBody] DataTableRequest view)
        {
            try
            {
                var uris = Request.RequestUri.ParseQueryString();
                Console.WriteLine(uris);
                using (var db = new DataContext())
                {
                    if (view.Start > 0)
                        view.Start /= view.Length;
                    var filter = string.IsNullOrEmpty(view.Search?.Value) ? "NULL" : $"'%{view.Search?.Value}%'";
                    var site = uris["site"] ?? "All";
                    site = site.Equals("All") ? "NULL" : $"'{site}'";
                    var status = uris["?status"] ?? "All";
                    status = status.Equals("All") ? "NULL" : $"'{status}'";
                    var cat = uris["?category"] ?? "All";
                    cat = cat.Equals("All") ? "NULL" : $"'{cat}'";
                    var sub = uris["?subcategory"] ?? "All";
                    sub = sub.Equals("All") ? "NULL" : $"'{sub}'";
                    var man = uris["?manufacturer"] ?? "All";
                    man = man.Equals("All") ? "NULL" : $"'{man}'";
                    var work = uris["?worktype"] ?? "All";
                    work = work.Equals("All") ? "NULL" : $"'{work}'";

                    //var models = uris["?model"] ?? "All";
                    //models = models.Equals("All") ? "NULL" : $"'{models}'";
                    //var name = uris["?name"] ?? "All";
                    //name = name.Equals("All") ? "NULL" : $"'{name}'";
                    //var area = uris["?area"] ?? "All";
                    //area = area.Equals("All") ? "NULL" : $"'{area}'";
                    //var address = uris["?address"] ?? "All";
                    //address = address.Equals("All") ? "NULL" : $"'{address}'";
                    //var contact = uris["?contact"] ?? "All";
                    //contact = contact.Equals("All") ? "NULL" : $"'{contact}'";
                    //var email = uris["?email"] ?? "All";
                    //email = email.Equals("All") ? "NULL" : $"'{email}'";
                    //var tracking = uris["?tracking"] ?? "All";
                    //tracking = tracking.Equals("All") ? "NULL" : $"'{tracking}'";
                    //var ticket = uris["?ticket"] ?? "All";
                    //ticket = ticket.Equals("All") ? "NULL" : $"'{ticket}'";
                    //var hrid = uris["?hrid"] ?? "All";
                    //hrid = hrid.Equals("All") ? "NULL" : $"'{hrid}'";

                    var start = view.Start;
                    var end = view.Length;
                    //db.Database.CommandTimeout = 5000;
                    var sql = $@"
                           EXEC [dbo].[spGetAssetCount] 
                         @Filter = {filter.Trim()}, -- varchar(1000)
                         @Site = {site.Trim()}, -- varchar(1000)
                         @Status = {status.Trim()}, -- varchar(1000)
                         @Category = {cat.Trim()}, -- varchar(1000)
                         @SubCategory = {sub.Trim()}, -- varchar(1000)
                         @Manufacturer = {man.Trim()}, -- varchar(1000)
                         @WorkType = {work.Trim()} -- varchar(1000)";
                    var cnt = await db
                        .Database
                        .SqlQuery<int>(sql)
                        .SingleOrDefaultAsync();
                    sql = $@"
                         EXEC [dbo].[spGetAssets] @PageNumber = {start}, -- int
                         @PageSize = {end}, -- int
                         @Filter = {filter.Trim()}, -- varchar(1000)
                         @Site = {site.Trim()}, -- varchar(1000)
                         @Status = {status.Trim()}, -- varchar(1000)
                         @Category = {cat.Trim()}, -- varchar(1000)
                         @SubCategory = {sub.Trim()}, -- varchar(1000)
                         @Manufacturer = {man.Trim()}, -- varchar(1000)
                         @WorkType = {work.Trim()} -- varchar(1000)";

                    var data = await db
                        .Database
                        .SqlQuery<AssetVm2>(sql)
                        .ToListAsync();
                    var model = new DataTableResponse
                    {
                        data = data,
                        draw = view.Draw,
                        error = "",
                        recordsFiltered = !string.IsNullOrEmpty(view.Search?.Value) ? data.Count : cnt,
                        recordsTotal = cnt
                    };
                    return Ok(model);
                }
            }
            catch (Exception e)
            {
                var model = new DataTableResponse
                {
                    data = new List<object>().ToArray(),
                    draw = view.Draw,
                    error = ExceptionHandler.GetMessages(e),
                    recordsFiltered = 0,
                    recordsTotal = 0
                };
                return Ok(model);
            }
        }

        [HttpPost]
        [Route("api/Scanner")]
        public async Task<IHttpActionResult> GetQrCodeData([FromBody] ItemVm data)
        {
            try
            {
                var id = new SecurityHandler().Decrypt(data?.Item);
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Invalid Qr Code");
                AssetVm2 entity;
                //using (var db = new DataContext())
                //{
                //    entity = await db.Database.SqlQuery<AssetVm2>("EXEC [dbo].[spGetScannedAsset] @Id={0}", id).SingleOrDefaultAsync();
                //}

                using (var db = new DataContext())
                {
                    var idParameter = new SqlParameter("@Id", id);
                    var result = await db.Database.SqlQuery<AssetVm2>("EXEC [dbo].[spGetScannedAsset] @Id", idParameter).ToListAsync();

                    entity = result.FirstOrDefault();
                }

                return Ok(entity);
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }


        [HttpPost]
        [Route("api/AssetApi/{id}/tables/{site}")]
        [Obsolete("Use the GetData function")]
        public async Task<IHttpActionResult> GetData2(Guid id, string site, [FromBody] DataTableRequest view)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var cnt = await db.Assets
                        .CountAsync(x => x.IsActive);
                    var data = db.Assets
                        .Where(x => x.IsActive);
                    if (!string.IsNullOrEmpty(site) && !site.Equals("All"))
                        data = data.Where(x => x.Site == site);
                    if (!string.IsNullOrEmpty(view.Search.Value))
                    {
                        var category = await db.Categories
                            .Where(x => x.IsActive && x.Code.Trim().ToLower().Contains(view.Search.Value.Trim().ToLower()))
                            .Select(x => x.Id)
                            .ToListAsync();
                        var subcategory = await db.SubCategories
                            .Where(x => x.IsActive && x.Code.Trim().ToLower().Contains(view.Search.Value.Trim().ToLower()))
                            .Select(x => x.Id)
                            .ToListAsync();
                        var manufacturers = await db.Manufacturers
                            .Where(x => x.IsActive && x.Code.Trim().ToLower().Contains(view.Search.Value.Trim().ToLower()))
                            .Select(x => x.Id)
                            .ToListAsync();
                        data = data
                            .Where(
                                x =>
                                    category.Any(c => c == x.CategoryId) ||
                                    subcategory.Any(c => c == x.SubCategoryId) ||
                                    manufacturers.Any(c => c == x.ManufacturerId) ||
                                    x.Description.Trim().ToLower().Contains(view.Search.Value.Trim().ToLower()) ||
                                    x.Code.Trim().ToLower().Contains(view.Search.Value.Trim().ToLower())
                            );
                    }
                    var entity = await data
                        .OrderBy(x => x.DateCreated)
                        .Skip(view.Start)
                        .Take(view.Length)
                        .ToListAsync();
                    var final = entity
                        .Select(x => new AssetVm
                        {
                            IsActive = x.IsActive,
                            Manufacturer = db.Manufacturers.Find(x.ManufacturerId),
                            Description = x.Description,
                            Code = x.Code,
                            SerialNo = x.SerialNo,
                            Category = db.Categories.Find(x.CategoryId),
                            SubCategory = db.SubCategories.Find(x.SubCategoryId),
                            Status = db.Statuses.Find(x.StatusId),
                            DateUpdated = x.DateUpdated,
                            UpdatedBy = x.UpdatedBy,
                            CreatedBy = x.CreatedBy,
                            DateCreated = x.DateCreated,
                            Site = x.Site,
                            Ram = x.Ram,
                            HdCapacity = x.HdCapacity,
                            MonitorSize = x.MonitorSize,
                            Classification = x.Classification,
                            EmployeeStatus = x.EmployeeStatus == "true" ? "ACTIVE" : "INACTIVE",
                            EmployeeTitle = x.EmployeeTitle,
                            Id = x.Id,
                            AssetEmployee = db.AssignAssetEmployee.FirstOrDefault(v => v.Code == x.Code),
                            EmployeeName = db.Database.SqlQuery<string>($@"
                            SELECT 
                            LTRIM(RTRIM(LTRIM(RTRIM(CONCAT(b.FirstName, ' ', b.LastName)))))[Name] 
                            FROM dbo.AssignAssetEmployees a
                            INNER JOIN dbo.Bulks b ON b.ID = a.Hrid
                            WHERE a.Code = '{x.Code}'
                            ").SingleOrDefault()
                        })
                        .ToArray();
                    var model = new DataTableResponse
                    {
                        data = final,
                        draw = view.Draw,
                        error = "",
                        recordsFiltered = !string.IsNullOrEmpty(view.Search?.Value) ? final.Length : cnt,
                        recordsTotal = cnt
                    };
                    return Ok(model);
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpPost]
        [Route("api/AssetApi/{id}/sites")]
        public async Task<IHttpActionResult> GetSites(Guid id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var data = await db.Assets
                        .Where(x => x.IsActive)
                        .ToListAsync();
                    var sites = data
                        .GroupBy(x => x.Site)
                        .Select(x => x.Key)
                        .ToArray();
                    return Ok(sites);
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpPost]
        [Route("api/AssetApi/{id}/print")]
        public async Task<IHttpActionResult> PrintQrCodes(Guid id, [FromBody] SiteVm site)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var data = db.Assets
                        .Where(x => x.IsActive);
                    if (!string.IsNullOrEmpty(site?.Site) && !site.Site.Equals("All"))
                        data = data.Where(x => x.Site == site.Site);
                    var entity = await data
                        .OrderBy(x => x.DateCreated)
                        .ToListAsync();
                    var final = entity
                        .Select(x => new
                        {
                            Manufacturer = db.Manufacturers.Find(x.ManufacturerId),
                            x.Description,
                            x.Processor,
                            x.Ram,
                            x.Code,
                            x.SerialNo,
                            Category = db.Categories.Find(x.CategoryId),
                            SubCategory = db.SubCategories.Find(x.SubCategoryId),
                            Status = db.Statuses.Find(x.StatusId),
                            x.Site,
                            EmployeeStatus = x.EmployeeStatus,
                            EmployeeTitle = x.EmployeeTitle,
                            x.Id,
                            Selected = false
                        })
                        .ToArray();
                    var path = HttpContext.Current.Server.MapPath("~/qrcodes");
                    Directory.CreateDirectory(path);
                    var batches = final.Batch(500);
                    var tasks = batches.Select(b => b.ToList())
                        .Select(chunks => Task.Run(() =>
                            {
                                foreach (var chunk in chunks)
                                {
                                    var itemId = $"{chunk.Id}.png";
                                    var qrCodePath = Path.Combine(path, itemId);
                                    try
                                    {
                                        ToQrCode(new SecurityHandler().Encrypt(chunk.Id.ToString()), qrCodePath);
                                    }
                                    catch
                                    {
                                        //ignore
                                    }
                                }

                            }
                        )).ToList();
                    await Task.WhenAll(tasks);
                    return Ok(final);
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpPost]
        [Route("api/AssetApi/{id}/printserial")]
        //IF LOOKING FOR THE DISPLAY ON THE QR CODE CHECK THE Print.html
        public async Task<IHttpActionResult> PrintQrCodesBySerialNo(Guid id)
        {
            try
            {
                var user = User.Identity.GetUserName();
                //check if there is an uploaded excel file
                if (HttpContext.Current.Request.Files.Count <= 0)
                    throw new Exception("No file uploaded");
                var file = HttpContext.Current.Request.Files["File"];
                var bulkData = new List<string>();
                var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                var tmpFileName = $"{user}-SERIAL-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                var path = HttpContext.Current.Server.MapPath($"~/uploads/files/print-filters/{phDateTime:MM-dd-yyyy}/{user}/");
                if (file != null && file.ContentLength > 0)
                {
                    Directory.CreateDirectory(path);
                    path = Path.Combine(path, tmpFileName);
                    //save the temporary file for reading of data - will be erased after
                    file.SaveAs(path);
                    if (File.Exists(path))
                    {
                        var errors = new List<string>();
                        try
                        {
                            using (var db = new DataContext())
                            {

                                var userUploadHistory = new UserUploadHistory
                                {
                                    UploadedBy = user,
                                    UploadedTemplateFileDir = path
                                };
                                db.UserUploadHistory.Add(userUploadHistory);
                                await db.SaveChangesAsync();
                            }
                            using (var wb = new XLWorkbook(path))
                            {
                                var sheet = wb.Worksheet(1);//default do not change
                                var total = sheet.RowsUsed().Count() + 1;//default do not change
                                for (var i = 2; i < total; i++)//default do not change
                                {
                                    var row = sheet.Row(i);
                                    var serial = row.Cell(1).GetValue<string>();
                                    if (string.IsNullOrEmpty(serial))
                                    {
                                        errors.Add($"Row[{i + 1}]::[Serial No] not defined");
                                        continue;
                                    }
                                    //add all the needed value sa list
                                    bulkData.Add(serial);
                                }
                            }

                            if (errors.Any())
                                throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                        }
                        catch (Exception e)
                        {
                            //try
                            //{
                            //    File.Delete(path);
                            //}
                            //catch
                            //{
                            //    //ignore
                            //}
                            var error = ExceptionHandler.GetMessages(e);
                            throw new Exception(error);
                        }
                        //dont remove uploaded files
                        //try
                        //{
                        //    File.Delete(path);
                        //}
                        //catch
                        //{
                        //    //ignore
                        //}

                        if (bulkData.Any())
                        {
                            using (var db = new DataContext())
                            {
                                var data = db.Assets
                                    .Where(x => x.IsActive && bulkData.Any(serialNo => serialNo.Trim().ToLower().Equals(x.SerialNo.Trim().ToLower())));
                                var aa = data.ToString();
                                var entity = await data
                                    .OrderBy(x => x.DateCreated)
                                    .ToListAsync();
                                var final = entity
                                    .Select(x => new
                                    {
                                        Manufacturer = db.Manufacturers.Find(x.ManufacturerId),
                                        x.Description,
                                        x.Code,
                                        x.SerialNo,
                                        Category = db.Categories.Find(x.CategoryId),
                                        SubCategory = db.SubCategories.Find(x.SubCategoryId),
                                        Status = db.Statuses.Find(x.StatusId),
                                        x.Site,
                                        x.Id,
                                        x.Processor,
                                        x.Ram,
                                        Selected = false
                                    })
                                    .ToArray();
                                path = HttpContext.Current.Server.MapPath("~/qrcodes");
                                Directory.CreateDirectory(path);
                                var batches = final.Batch(500);
                                var tasks = batches.Select(b => b.ToList())
                                    .Select(chunks => Task.Run(() =>
                                        {
                                            foreach (var chunk in chunks)
                                            {
                                                var itemId = $"{chunk.Id}.png";
                                                var qrCodePath = Path.Combine(path, itemId);
                                                try
                                                {
                                                    ToQrCode(new SecurityHandler().Encrypt(chunk.Id.ToString()), qrCodePath);
                                                }
                                                catch
                                                {
                                                    //ignore
                                                }
                                            }

                                        }
                                    )).ToList();
                                await Task.WhenAll(tasks);
                                return Ok(final);
                            }
                        }
                    }
                }
                throw new Exception("No file uploaded");
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        // GET api/AssetApi/5
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var x = await db.Assets.FirstOrDefaultAsync(xx => xx.Id == id);
                    if (x == null)
                        throw new Exception("Record is either deleted or not existing.");
                    var final = new AssetVm
                    {
                        IsActive = x.IsActive,
                        Manufacturer = await db.Manufacturers.FindAsync(x.ManufacturerId),
                        Description = x.Description,
                        Code = x.Code,
                        SerialNo = x.SerialNo,
                        Category = await db.Categories.FindAsync(x.CategoryId),
                        SubCategory = await db.SubCategories.FindAsync(x.SubCategoryId),
                        Status = await db.Statuses.FindAsync(x.StatusId),
                        DateUpdated = x.DateUpdated,
                        UpdatedBy = x.UpdatedBy,
                        CreatedBy = x.CreatedBy,
                        DateCreated = x.DateCreated,
                        Id = x.Id,
                        AssetEmployee = await db.AssignAssetEmployee.FirstOrDefaultAsync(v => v.Code == x.Code),
                        EmployeeName = await db.Database.SqlQuery<string>($@"
SELECT 
LTRIM(RTRIM(LTRIM(RTRIM(CONCAT(b.FirstName, ' ', b.LastName)))))[Name] 
FROM dbo.AssignAssetEmployees a
INNER JOIN dbo.Bulks b ON b.ID = a.Hrid
WHERE a.Code = '{x.Code}'
").SingleOrDefaultAsync()
                    };
                    return Ok(final);
                }
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        // POST api/AssetApi
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] AssetVm value)
        {
        recheck:
            try
            {
                var today = DateTime.Now;
                using (var db = new DataContext())
                {
                    Guid electronicGuid = new Guid("8083F8EC-060C-4FA4-8D72-36C388CC8BDD");
                    var getData = await db.Assets.FindAsync(value.Id);
                    if (getData != null)
                        throw new Exception("Record already exist.");
                    var status = await db.Statuses.FirstOrDefaultAsync(x => x.Sort == 1);
                    var count = await db.Assets.CountAsync(x => x.Year == today.Year);
                    var classification = value.Category?.Id == electronicGuid ? "Critical" : "Public";

                    var entity = new Assets
                    {
                        Description = value.Description,
                        CategoryId = value.Category?.Id,
                        CreatedBy = User.Identity.GetUserName(),
                        SerialNo = value.SerialNo,
                        ManufacturerId = value.Manufacturer?.Id,
                        StatusId = status?.Id,
                        SubCategoryId = value.SubCategory?.Id,
                        Code = $"ASSET{today.Year:0000}{today.Month:00}{count:00000}"
                    };
                    var qrPath = $"{ConfigurationManager.AppSettings["QrCodePath"]}\\{value.Id}.png";
                    var fullPath = Path.GetFullPath(Path.Combine(HttpRuntime.AppDomainAppPath, qrPath.Replace('\\', '/')));
                    _ = Task.Run(() => { ToQrCode(entity.Id.ToString(), fullPath); });
                    var url = $"{Url.Request.RequestUri?.Scheme}://{Url.Request.RequestUri?.Authority}/{qrPath}";
                    //entity.QrCode = url;
                    db.Assets.Add(entity);
                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("duplicate"))
                            goto recheck;
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        // PUT api/AssetApi/5
        [HttpPut]
        public async Task<IHttpActionResult> Put(Guid id, [FromBody] AssetVm value)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Assets.FindAsync(id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing.");
                    entity.Description = value.Description;
                    entity.CategoryId = value.Category?.Id;
                    entity.SerialNo = value.SerialNo;
                    entity.StatusId = value.Status?.Id;
                    entity.SubCategoryId = value.SubCategory?.Id;
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

        ////SAVING STARTS HERE
        ////AssetReplacement bulk saving
        //[HttpPost]
        //[Route("api/AssetReplacement")]
        //public async Task<IHttpActionResult> AssetUploads_AssetReplacement()
        //{
        //    try
        //    {
        //        var user = User.Identity.GetUserName();

        //        //using (var db = new DataContext())
        //        //{
        //        //    await db.Database
        //        //        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssetUploads WHERE CreatedBy = '{user}'");
        //        //}

        //        if (HttpContext.Current.Request.Files.Count > 0)
        //        {
        //            var file = HttpContext.Current.Request.Files["File"];
        //            var bulkData = new List<AssetUploadsAssetReplacements>();
        //            if (file != null && file.ContentLength > 0)
        //            {
        //                var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
        //                var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
        //                var tmpFileName = $"{user}-ASSET-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
        //                var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssetUploads/{phDateTime:MM-dd-yyyy}/{user}/");
        //                Directory.CreateDirectory(path);
        //                path = Path.Combine(path, tmpFileName);
        //                //save the temporary file for reading of data - will be erased after
        //                file.SaveAs(path);

        //                if (File.Exists(path))
        //                {
        //                    var errors = new List<string>();
        //                    try
        //                    {
        //                        List<string> serials;
        //                        List<Category> categories;
        //                        List<SubCategory> subcategories;
        //                        List<string> manufacturers;
        //                        List<string> statuses;

        //                        using (var db = new DataContext())
        //                        {
        //                            var userUploadHistory = new UserUploadHistory
        //                            {
        //                                UploadedBy = user,
        //                                UploadedTemplateFileDir = path
        //                            };
        //                            db.UserUploadHistory.Add(userUploadHistory);
        //                            await db.SaveChangesAsync();
        //                            serials = await db.AssetUploadsAssetReplacements.Where(x => !string.IsNullOrEmpty(x.OldSerial)).Select(x => x.OldSerial).ToListAsync();
        //                            categories = await db.Categories.Where(x => x.IsActive).ToListAsync();
        //                            subcategories = await db.SubCategories.Where(x => x.IsActive).ToListAsync();
        //                            manufacturers = await db.Manufacturers.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
        //                            statuses = await db.Statuses.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
        //                        }

        //                        using (var wb = new XLWorkbook(path))
        //                        {
        //                            var sheet = wb.Worksheet(1);//default do not change
        //                            var total = sheet.RowsUsed().Count() + 1;//default do not change
        //                            for (var i = 2; i < total; i++)//default do not change
        //                            {
        //                                var row = sheet.Row(i);
        //                                var oldSerial = row.Cell(1).GetValue<string>();
        //                                var newSerial = row.Cell(2).GetValue<string>();
        //                                var newBrand = row.Cell(3).GetValue<string>();
        //                                var vendor = row.Cell(4).GetValue<string>();
        //                                var purchaseOrderNumber = row.Cell(5).GetValue<string>();
        //                                var costValue = row.Cell(6).GetValue<string>();

        //                                //if (string.IsNullOrEmpty(oldSerial) ||
        //                                //    string.IsNullOrEmpty(newSerial) ||
        //                                //    string.IsNullOrEmpty(newBrand) ||
        //                                //    string.IsNullOrEmpty(vendor) ||
        //                                //    string.IsNullOrEmpty(purchaseOrderNumber) ||
        //                                //    string.IsNullOrEmpty(costValue))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]:: All fields are required");
        //                                //    continue;
        //                                //}

        //                                oldSerial = Regex.Replace(oldSerial.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
        //                                newSerial = Regex.Replace(newSerial.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
        //                                oldSerial = Regex.Replace(oldSerial, @"[ ]{2,}", " ").ToUpper().Trim();
        //                                newSerial = Regex.Replace(newSerial, @"[ ]{2,}", " ").ToUpper().Trim();
        //                                oldSerial = Regex.Replace(oldSerial.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();
        //                                newSerial = Regex.Replace(newSerial.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();


        //                                //fix this validation add the new serial in conditions
        //                                int.TryParse(ConfigurationManager.AppSettings["SERIAL_NUMBER_LEN"], out var serialMinLen);
        //                                if (oldSerial.Length < serialMinLen && newSerial.Length < serialMinLen)
        //                                {
        //                                    errors.Add($"Row[{i + 1}]::Serial should be equal or greater than ({serialMinLen}) characters");
        //                                    continue;
        //                                }

        //                                if (!string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == oldSerial.ToUpper().Trim())) || !string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == newSerial.ToUpper().Trim())))
        //                                {
        //                                    //errors.Add($"Row[{i + 1}]::Serial [{SM_AssetTag}] is already used");
        //                                    errors.Add($"Row[{i + 1}]::Serial is already used");
        //                                    continue;
        //                                }

        //                                //var tmpCategory = categories.FirstOrDefault(x => x.Code.ToUpper().Trim() == category.ToUpper().Trim());
        //                                //if (string.IsNullOrEmpty(tmpCategory?.Code))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::[{category.ToUpper().Trim()}] is not a valid Category");
        //                                //    continue;
        //                                //}

        //                                //CHECK IF WHAT VARIABLE STATUS DO
        //                                //if (string.IsNullOrEmpty(statuses.FirstOrDefault(x => x.ToUpper().Trim() == status.ToUpper().Trim())))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::[{status}] is not a valid Status");
        //                                //    continue;
        //                                //}

        //                                //if (string.IsNullOrEmpty(subcategories.FirstOrDefault(x => x.CategoryId == tmpCategory.Id && x.Code.ToUpper().Trim() == subCategory.ToUpper().Trim())?.Code))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::SubCategory [{subCategory.ToUpper().Trim()}] is either invalid or not under [{tmpCategory.Code}] Category");
        //                                //    continue;
        //                                //}

        //                                //CHECK THIS VARIABLE MANUFACTURER
        //                                //if (string.IsNullOrEmpty(manufacturers.FirstOrDefault(x => x.ToUpper().Trim() == manufacturer.ToUpper().Trim())))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::Brand [{manufacturer.ToUpper().Trim()}] is not valid");
        //                                //    continue;
        //                                //}

        //                                if (bulkData.Exists(x => x.OldSerial == oldSerial.Trim().ToUpper() || x.NewSerial == newSerial.Trim().ToUpper()))
        //                                    errors.Add($"Row[{i}] Duplicate entry for Serial No. [{oldSerial.Trim().ToUpper()}]");
        //                                else
        //                                    bulkData.Add(new AssetUploadsAssetReplacements
        //                                    {
        //                                        OldSerial = oldSerial,
        //                                        NewSerial = newSerial,
        //                                        NewBrand = newBrand,
        //                                        Vendor = vendor,
        //                                        PurchaseOrderNumber = purchaseOrderNumber,
        //                                        CostValue = costValue
        //                                    });
        //                            }
        //                        }

        //                        if (errors.Any())
        //                            throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
        //                    }
        //                    catch (Exception e)
        //                    {
        //                        var error = ExceptionHandler.GetMessages(e);
        //                        throw new Exception(error);
        //                    }

        //                    if (bulkData.Any())
        //                    {
        //                        IEnumerable<IEnumerable<AssetUploadsAssetReplacements>> batches;
        //                        if (bulkData.Count > 1500)
        //                            batches = bulkData.Batch(1000);
        //                        else if (bulkData.Count > 1000)
        //                            batches = bulkData.Batch(500);
        //                        else if (bulkData.Count > 500)
        //                            batches = bulkData.Batch(250);
        //                        else if (bulkData.Count > 250)
        //                            batches = bulkData.Batch(125);
        //                        else if (bulkData.Count > 125)
        //                            batches = bulkData.Batch(60);
        //                        else if (bulkData.Count > 60)
        //                            batches = bulkData.Batch(30);
        //                        else if (bulkData.Count > 30)
        //                            batches = bulkData.Batch(15);
        //                        else batches = bulkData.Batch(5);
        //                        var tasks = batches.Select(b => b.ToList())
        //                            .Select(chunks => Task.Run(async () =>
        //                            {
        //                                try
        //                                {
        //                                    using (var context = new DataContext())
        //                                    {
        //                                        context.AssetUploadsAssetReplacements.AddRange(chunks);
        //                                        //var sample = context.AssetUploadsFarmouts.FirstOrDefault(x => x.SM_AssetTag == "003SAMP000");
        //                                        await context.SaveChangesAsync();
        //                                    }
        //                                }
        //                                catch (Exception exception)
        //                                {
        //                                    var error = ExceptionHandler.GetMessages(exception);
        //                                    Console.WriteLine(error);
        //                                }
        //                            }
        //                            )).ToList();
        //                        await Task.WhenAll(tasks);
        //                        using (var db = new DataContext())
        //                        {
        //                            foreach (var item in bulkData)
        //                            {
        //                                var oldSerials = item.OldSerial;
        //                                var newSerials = item.NewSerial;
        //                                await db.Database.ExecuteSqlCommandAsync("EXEC [dbo].[spAssetReplacement] @oldSerialNum = {0}, @newSerialNum = {1}", oldSerials, newSerials);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(ExceptionHandler.GetMessages(e));
        //    }
        //}

        //SAVING STARTS HERE
        //AssetReplacement bulk saving
        [HttpPost]
        [Route("api/AssetReplacement")]
        public async Task<IHttpActionResult> AssetUploads_AssetReplacement()
        {
            try
            {
                var user = User.Identity.GetUserName();
                using (var db = new DataContext())
                {
                    await db.Database
                        .ExecuteSqlCommandAsync($"DELETE FROM dbo.ReplaceUploads WHERE CreatedBy = '{user}'");

                    //check if there is an uploaded excel file
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        var file = HttpContext.Current.Request.Files["File"];
                        var bulkData = new List<AssetUploadsAssetReplacements>();
                        if (file != null && file.ContentLength > 0)
                        {
                            var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                            var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                            var tmpFileName = $"{user}-REPLACE-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                            var path = HttpContext.Current.Server.MapPath($"~/uploads/files/ReplaceUploads/{phDateTime:MM-dd-yyyy}/{user}/");
                            Directory.CreateDirectory(path);
                            path = Path.Combine(path, tmpFileName);
                            //save the temporary file for reading of data - will be erased after
                            file.SaveAs(path);
                            if (File.Exists(path))
                            {
                                var errors = new List<string>();
                                try
                                {
                                    var assets = await new DataContext()
                                        .Assets
                                        .Where(x => x.IsActive)
                                        .Select(x => new { x.Code, x.SerialNo })
                                        .ToListAsync();
                                    var vendorsCode = await new DataContext()
                                        .Vendors
                                        .Where(x => x.IsActive)
                                        .Select(x => x.Code)
                                        .ToListAsync();
                                    var refNos = assets
                                        .Select(x => x.Code)
                                        .ToList();
                                    var serials = assets
                                        .Select(x => x.SerialNo)
                                        .ToList();
                                    List<string> manufacturers;
                                    var userUploadHistory = new UserUploadHistory
                                    {
                                        UploadedBy = user,
                                        UploadedTemplateFileDir = path
                                    };
                                    db.UserUploadHistory.Add(userUploadHistory);
                                    await db.SaveChangesAsync();
                                    manufacturers = await db.Manufacturers
                                        .Where(x => x.IsActive)
                                        .Select(x => x.Code)
                                        .ToListAsync();
                                    using (var wb = new XLWorkbook(path))
                                    {
                                        var sheet = wb.Worksheet(1);//default do not change
                                        var total = sheet.RowsUsed().Count() + 1;//default do not change
                                        for (var i = 2; i < total; i++)//default do not change
                                        {
                                            var row = sheet.Row(i);
                                            var oldserial = row.Cell(1).GetValue<string>();
                                            var newserial = row.Cell(2).GetValue<string>();
                                            var manufacturer = row.Cell(3).GetValue<string>();
                                            var vendor = row.Cell(4).GetValue<string>();
                                            var purchaseOrder = row.Cell(5).GetValue<string>();
                                            var costValue = row.Cell(6).GetValue<string>();
                                            if (string.IsNullOrEmpty(oldserial) || string.IsNullOrEmpty(newserial) || string.IsNullOrEmpty(manufacturer))
                                            {
                                                errors.Add($"Row[{i + 1}]::All fields are required");
                                                continue;
                                            }

                                            newserial = Regex.Replace(newserial.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
                                            newserial = Regex.Replace(newserial, @"[ ]{2,}", " ").ToUpper().Trim();
                                            newserial = Regex.Replace(newserial.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();

                                            var AssetData = await db.Assets
                                                    .Where(x => x.SerialNo == oldserial)
                                                    .FirstOrDefaultAsync();

                                            if (string.IsNullOrEmpty(refNos.FirstOrDefault(x => x.ToUpper().Trim() == AssetData.Code.ToUpper().Trim())))
                                            {
                                                errors.Add($"Row[{i + 1}]:: Serial No [{oldserial}] is not valid");
                                                continue;
                                            }

                                            if (!string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == newserial.ToUpper().Trim())))
                                            {
                                                errors.Add($"Row[{i + 1}]::Serial [{newserial}] is already used");
                                                continue;
                                            }

                                            if (string.IsNullOrEmpty(vendorsCode.FirstOrDefault(x => x.ToUpper().Trim() == vendor.ToUpper().Trim())))
                                            {
                                                errors.Add($"Row[{i + 1}]::Brand [{vendor.ToUpper().Trim()}] is not valid");
                                                continue;
                                            }

                                            if (string.IsNullOrEmpty(manufacturers.FirstOrDefault(x => x.ToUpper().Trim() == manufacturer.ToUpper().Trim())))
                                            {
                                                errors.Add($"Row[{i + 1}]::Manufacturer [{manufacturer.ToUpper().Trim()}] is not valid");
                                                continue;
                                            }

                                            if (bulkData.Exists(x => x.NewSerial.Trim().ToUpper() == newserial.Trim().ToUpper()))
                                                errors.Add($"Row[{i}] Duplicate entry for Serial No. [{newserial.Trim().ToUpper()}]");
                                            else
                                            {
                                                Guid statusIdGuid = new Guid("B5D6DFF2-FB43-4308-8447-FBEEED2B0495");

                                                if (AssetData.StatusId.HasValue && AssetData.StatusId.Value == statusIdGuid)
                                                {
                                                    //add all the needed value sa list
                                                    bulkData.Add(new AssetUploadsAssetReplacements
                                                    {
                                                        NewBrand = manufacturer?.Trim().ToUpper(),
                                                        OldSerial = oldserial?.Trim().ToUpper(),
                                                        NewSerial = newserial?.Trim().ToUpper(),
                                                        CostValue = costValue,
                                                        PurchaseOrderNumber = purchaseOrder,
                                                        Vendor = vendor,
                                                        CreatedBy = user,
                                                        Code = AssetData.Code
                                                    });
                                                }
                                                else
                                                {
                                                    errors.Add($"Return the [{oldserial.Trim().ToUpper()}] to the Asset before replacement");
                                                }
                                            }
                                        }
                                    }

                                    if (errors.Any())
                                        throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                                }
                                catch (Exception e)
                                {
                                    //try
                                    //{
                                    //    File.Delete(path);
                                    //}
                                    //catch
                                    //{
                                    //    //ignore
                                    //}
                                    var error = ExceptionHandler.GetMessages(e);
                                    throw new Exception(error);
                                }

                                //try
                                //{
                                //    File.Delete(path);
                                //}
                                //catch
                                //{
                                //    //ignore
                                //}

                                if (bulkData.Any())
                                {
                                    IEnumerable<IEnumerable<AssetUploadsAssetReplacements>> batches;
                                    if (bulkData.Count > 1500)
                                        batches = bulkData.Batch(1000);
                                    else if (bulkData.Count > 1000)
                                        batches = bulkData.Batch(500);
                                    else if (bulkData.Count > 500)
                                        batches = bulkData.Batch(250);
                                    else if (bulkData.Count > 250)
                                        batches = bulkData.Batch(125);
                                    else if (bulkData.Count > 125)
                                        batches = bulkData.Batch(60);
                                    else if (bulkData.Count > 60)
                                        batches = bulkData.Batch(30);
                                    else if (bulkData.Count > 30)
                                        batches = bulkData.Batch(15);
                                    else batches = bulkData.Batch(5);
                                    var tasks = batches.Select(b => b.ToList())
                                        .Select(chunks => Task.Run(async () =>
                                        {
                                            try
                                            {
                                                using (var context = new DataContext())
                                                {
                                                    context.AssetUploadsAssetReplacements.AddRange(chunks);
                                                    await context.SaveChangesAsync();
                                                }
                                            }
                                            catch (Exception exception)
                                            {
                                                var error = ExceptionHandler.GetMessages(exception);
                                                Console.WriteLine(error);
                                            }
                                        }
                                        )).ToList();
                                    await Task.WhenAll(tasks);
                                    await db.Database
                                            .ExecuteSqlCommandAsync($"EXEC [dbo].[spReplaceUploads2] @createdBy = '{user}'");
                                }
                            }
                        }
                    }
                }

                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        //AssetUpdate bulk saving
        [HttpPost]
        [Route("api/AssetUpdate")]
        public async Task<IHttpActionResult> AssetUploads_AssetUpdate()
        {
            try
            {
                var user = User.Identity.GetUserName();

                //using (var db = new DataContext())
                //{
                //    await db.Database
                //        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssetUploads WHERE CreatedBy = '{user}'");
                //}

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files["File"];
                    var bulkData = new List<AssetUploadsAssetUpdates>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                        var tmpFileName = $"{user}-ASSET-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                        var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssetUploads/{phDateTime:MM-dd-yyyy}/{user}/");
                        Directory.CreateDirectory(path);
                        path = Path.Combine(path, tmpFileName);
                        //save the temporary file for reading of data - will be erased after
                        file.SaveAs(path);

                        if (File.Exists(path))
                        {
                            var errors = new List<string>();
                            try
                            {
                                List<string> serials;
                                List<Category> categories;
                                List<SubCategory> subcategories;
                                List<string> manufacturers;
                                List<string> statuses;

                                using (var db = new DataContext())
                                {
                                    var userUploadHistory = new UserUploadHistory
                                    {
                                        UploadedBy = user,
                                        UploadedTemplateFileDir = path
                                    };
                                    db.UserUploadHistory.Add(userUploadHistory);
                                    await db.SaveChangesAsync();
                                    serials = await db.Assets.Where(x => !string.IsNullOrEmpty(x.SerialNo)).Select(x => x.SerialNo).ToListAsync();
                                    categories = await db.Categories.Where(x => x.IsActive).ToListAsync();
                                    subcategories = await db.SubCategories.Where(x => x.IsActive).ToListAsync();
                                    manufacturers = await db.Manufacturers.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                    statuses = await db.Statuses.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                }

                                using (var wb = new XLWorkbook(path))
                                {
                                    var sheet = wb.Worksheet(1);//default do not change
                                    var total = sheet.RowsUsed().Count() + 1;//default do not change
                                    for (var i = 2; i < total; i++)//default do not change
                                    {
                                        var row = sheet.Row(i);
                                        var SM_Assettag = row.Cell(1).GetValue<string>();
                                        var status = row.Cell(2).GetValue<string>();
                                        var iTrackTicketNumber = row.Cell(3).GetValue<string>();
                                        var issue = row.Cell(4).GetValue<string>();
                                        var approvedLoa = row.Cell(5).GetValue<string>();
                                        var approvedPezaForm8106Number = row.Cell(6).GetValue<string>();
                                        var approvedPezaForm8105Number = row.Cell(7).GetValue<string>();
                                        var returnedDate = row.Cell(8).GetValue<string>();
                                        var dateReportedAsLostMissingStolen = row.Cell(9).GetValue<string>();
                                        var salDedAmount = row.Cell(10).GetValue<string>();
                                        var disposalDate = row.Cell(11).GetValue<string>();
                                        var remarks = row.Cell(12).GetValue<string>();

                                        //if (string.IsNullOrEmpty(SM_Assettag) ||
                                        //    string.IsNullOrEmpty(status) ||
                                        //    string.IsNullOrEmpty(iTrackTicketNumber) ||
                                        //    string.IsNullOrEmpty(issue) ||
                                        //    string.IsNullOrEmpty(approvedLoa) ||
                                        //    string.IsNullOrEmpty(approvedPezaForm8106Number) ||
                                        //    string.IsNullOrEmpty(approvedPezaForm8105Number) ||
                                        //    string.IsNullOrEmpty(returnedDate) ||
                                        //    string.IsNullOrEmpty(dateReportedAsLostMissingStolen) ||
                                        //    string.IsNullOrEmpty(salDedAmount) ||
                                        //    string.IsNullOrEmpty(disposalDate) ||
                                        //    string.IsNullOrEmpty(remarks))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]:: All fields are required");
                                        //    continue;
                                        //}

                                        SM_Assettag = Regex.Replace(SM_Assettag.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
                                        SM_Assettag = Regex.Replace(SM_Assettag, @"[ ]{2,}", " ").ToUpper().Trim();
                                        SM_Assettag = Regex.Replace(SM_Assettag.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();

                                        int.TryParse(ConfigurationManager.AppSettings["SERIAL_NUMBER_LEN"], out var serialMinLen);
                                        if (SM_Assettag.Length < serialMinLen)
                                        {
                                            errors.Add($"Row[{i + 1}]::Serial should be equal or greater than ({serialMinLen}) characters");
                                            continue;
                                        }

                                        //if (!string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == SM_AssetTag.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::Serial [{SM_AssetTag}] is already used");
                                        //    continue;
                                        //}

                                        //var tmpCategory = categories.FirstOrDefault(x => x.Code.ToUpper().Trim() == category.ToUpper().Trim());
                                        //if (string.IsNullOrEmpty(tmpCategory?.Code))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::[{category.ToUpper().Trim()}] is not a valid Category");
                                        //    continue;
                                        //}

                                        //CHECK IF WHAT VARIABLE STATUS DO
                                        //if (string.IsNullOrEmpty(statuses.FirstOrDefault(x => x.ToUpper().Trim() == status.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::[{status}] is not a valid Status");
                                        //    continue;
                                        //}

                                        //if (string.IsNullOrEmpty(subcategories.FirstOrDefault(x => x.CategoryId == tmpCategory.Id && x.Code.ToUpper().Trim() == subCategory.ToUpper().Trim())?.Code))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::SubCategory [{subCategory.ToUpper().Trim()}] is either invalid or not under [{tmpCategory.Code}] Category");
                                        //    continue;
                                        //}

                                        //CHECK THIS VARIABLE MANUFACTURER
                                        //if (string.IsNullOrEmpty(manufacturers.FirstOrDefault(x => x.ToUpper().Trim() == manufacturer.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::Brand [{manufacturer.ToUpper().Trim()}] is not valid");
                                        //    continue;
                                        //}

                                        if (bulkData.Exists(x => x.SM_Assettag == SM_Assettag.Trim().ToUpper()))
                                            errors.Add($"Row[{i}] Duplicate entry for Serial No. [{SM_Assettag.Trim().ToUpper()}]");
                                        else
                                        {
                                            var createdBy = user.Replace("PH-", "").Trim();
                                            using (var db = new DataContext())
                                            {
                                                var AssetData = await db.Assets.FirstOrDefaultAsync(x => x.SerialNo == SM_Assettag);

                                                bulkData.Add(new AssetUploadsAssetUpdates
                                                {
                                                    Code = AssetData.Code,
                                                    SM_Assettag = SM_Assettag,
                                                    Status = status,
                                                    ITrackTicketNumber = iTrackTicketNumber,
                                                    Issue = issue,
                                                    ApprovedLoa = approvedLoa,
                                                    ApprovedPezaForm8106Number = approvedPezaForm8106Number,
                                                    ApprovedPezaForm8105Number = approvedPezaForm8105Number,
                                                    ReturnedDate = returnedDate,
                                                    DateReportedAsLostMissingStolen = dateReportedAsLostMissingStolen,
                                                    SalDedAmount = salDedAmount,
                                                    DisposalDate = disposalDate,
                                                    Remarks = remarks,
                                                    CreatedBy = createdBy
                                                });
                                            }
                                        }
                                    }
                                }

                                if (errors.Any())
                                    throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                            }
                            catch (Exception e)
                            {
                                var error = ExceptionHandler.GetMessages(e);
                                throw new Exception(error);
                            }

                            if (bulkData.Any())
                            {
                                IEnumerable<IEnumerable<AssetUploadsAssetUpdates>> batches;
                                if (bulkData.Count > 1500)
                                    batches = bulkData.Batch(1000);
                                else if (bulkData.Count > 1000)
                                    batches = bulkData.Batch(500);
                                else if (bulkData.Count > 500)
                                    batches = bulkData.Batch(250);
                                else if (bulkData.Count > 250)
                                    batches = bulkData.Batch(125);
                                else if (bulkData.Count > 125)
                                    batches = bulkData.Batch(60);
                                else if (bulkData.Count > 60)
                                    batches = bulkData.Batch(30);
                                else if (bulkData.Count > 30)
                                    batches = bulkData.Batch(15);
                                else batches = bulkData.Batch(5);
                                var tasks = batches.Select(b => b.ToList())
                                    .Select(chunks => Task.Run(async () =>
                                    {
                                        try
                                        {
                                            using (var context = new DataContext())
                                            {
                                                context.AssetUploadsAssetUpdates.AddRange(chunks);
                                                //var sample = context.AssetUploadsFarmouts.FirstOrDefault(x => x.SM_AssetTag == "003SAMP000");
                                                await context.SaveChangesAsync();
                                            }
                                        }
                                        catch (Exception exception)
                                        {
                                            var error = ExceptionHandler.GetMessages(exception);
                                            Console.WriteLine(error);
                                        }
                                    }
                                    )).ToList();
                                await Task.WhenAll(tasks);

                                using (var db = new DataContext())
                                {
                                    //foreach (var item in bulkData)
                                    //{
                                    //    var serial = item.SM_Assettag;
                                    //    await db.Database.ExecuteSqlCommandAsync("EXEC [dbo].[spAssetUpdate] @serialNum = {0}", serial);
                                    //}

                                    string numericPart = Regex.Match(user, @"\d+").Value;
                                    await db.Database
                                            .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssetUpdateUploads2] @createdBy = '{numericPart}'");
                                }
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        ////AssetAssignment bulk saving
        //[HttpPost]
        //[Route("api/AssetAssignment")]
        //public async Task<IHttpActionResult> AssetUploads_AssetAssignment()
        //{
        //    try
        //    {
        //        var user = User.Identity.GetUserName();

        //        //using (var db = new DataContext())
        //        //{
        //        //    await db.Database
        //        //        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssetUploads WHERE CreatedBy = '{user}'");
        //        //}

        //        if (HttpContext.Current.Request.Files.Count > 0)
        //        {
        //            var file = HttpContext.Current.Request.Files["File"];
        //            var bulkData = new List<AssetUploadsAssetAssignments>();
        //            if (file != null && file.ContentLength > 0)
        //            {
        //                var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
        //                var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
        //                var tmpFileName = $"{user}-ASSET-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
        //                var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssetUploads/{phDateTime:MM-dd-yyyy}/{user}/");
        //                Directory.CreateDirectory(path);
        //                path = Path.Combine(path, tmpFileName);
        //                //save the temporary file for reading of data - will be erased after
        //                file.SaveAs(path);

        //                if (File.Exists(path))
        //                {
        //                    var errors = new List<string>();
        //                    try
        //                    {
        //                        List<string> serials;
        //                        List<Category> categories;
        //                        List<SubCategory> subcategories;
        //                        List<string> manufacturers;
        //                        List<string> statuses;
        //                        List<Bulk> employeeApi;

        //                        using (var db = new DataContext())
        //                        {
        //                            var userUploadHistory = new UserUploadHistory
        //                            {
        //                                UploadedBy = user,
        //                                UploadedTemplateFileDir = path
        //                            };
        //                            db.UserUploadHistory.Add(userUploadHistory);
        //                            await db.SaveChangesAsync();
        //                            serials = await db.Assets.Where(x => !string.IsNullOrEmpty(x.SerialNo)).Select(x => x.SerialNo).ToListAsync();
        //                            categories = await db.Categories.Where(x => x.IsActive).ToListAsync();
        //                            employeeApi = await db.BulkApiEmployee.Where(x => x.IsActive).ToListAsync();
        //                            subcategories = await db.SubCategories.Where(x => x.IsActive).ToListAsync();
        //                            manufacturers = await db.Manufacturers.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
        //                            statuses = await db.Statuses.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
        //                        }

        //                        using (var wb = new XLWorkbook(path))
        //                        {
        //                            var sheet = wb.Worksheet(1);//default do not change
        //                            var total = sheet.RowsUsed().Count() + 1;//default do not change
        //                            for (var i = 2; i < total; i++)//default do not change
        //                            {
        //                                var row = sheet.Row(i);
        //                                var iTrackTicket = row.Cell(1).GetValue<string>();
        //                                var smAssetTag = row.Cell(2).GetValue<string>();
        //                                var workType = row.Cell(3).GetValue<string>();
        //                                var site = row.Cell(4).GetValue<string>();
        //                                var hrid = row.Cell(5).GetValue<string>();
        //                                var floor = row.Cell(6).GetValue<string>();
        //                                var area = row.Cell(7).GetValue<string>();
        //                                var address = row.Cell(8).GetValue<string>();
        //                                var contactNumber = row.Cell(9).GetValue<string>();
        //                                var employeeEmail = row.Cell(10).GetValue<string>();
        //                                var deploymentIssuanceDate = row.Cell(11).GetValue<string>();
        //                                var trackingNumber = row.Cell(12).GetValue<string>();

        //                                //if (
        //                                //    string.IsNullOrEmpty(iTrackTicket) ||
        //                                //    string.IsNullOrEmpty(smAssetTag) ||
        //                                //    string.IsNullOrEmpty(workType) ||
        //                                //    string.IsNullOrEmpty(site) ||
        //                                //    string.IsNullOrEmpty(hrid) ||
        //                                //    string.IsNullOrEmpty(floor) ||
        //                                //    string.IsNullOrEmpty(area) ||
        //                                //    string.IsNullOrEmpty(address) ||
        //                                //    string.IsNullOrEmpty(contactNumber) ||
        //                                //    string.IsNullOrEmpty(employeeEmail) ||
        //                                //    string.IsNullOrEmpty(deploymentIssuanceDate) ||
        //                                //    string.IsNullOrEmpty(trackingNumber))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]:: All fields are required");
        //                                //    continue;
        //                                //}

        //                                smAssetTag = Regex.Replace(smAssetTag.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
        //                                smAssetTag = Regex.Replace(smAssetTag, @"[ ]{2,}", " ").ToUpper().Trim();
        //                                smAssetTag = Regex.Replace(smAssetTag.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();

        //                                int.TryParse(ConfigurationManager.AppSettings["SERIAL_NUMBER_LEN"], out var serialMinLen);
        //                                if (smAssetTag.Length < serialMinLen)
        //                                {
        //                                    errors.Add($"Row[{i + 1}]::Serial should be equal or greater than ({serialMinLen}) characters");
        //                                    continue;
        //                                }

        //                                //if (!string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == SM_AssetTag.ToUpper().Trim())))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::Serial [{SM_AssetTag}] is already used");
        //                                //    continue;
        //                                //}

        //                                //var tmpCategory = categories.FirstOrDefault(x => x.Code.ToUpper().Trim() == category.ToUpper().Trim());
        //                                //if (string.IsNullOrEmpty(tmpCategory?.Code))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::[{category.ToUpper().Trim()}] is not a valid Category");
        //                                //    continue;
        //                                //}

        //                                //CHECK IF WHAT VARIABLE STATUS DO
        //                                //if (string.IsNullOrEmpty(statuses.FirstOrDefault(x => x.ToUpper().Trim() == status.ToUpper().Trim())))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::[{status}] is not a valid Status");
        //                                //    continue;
        //                                //}

        //                                //if (string.IsNullOrEmpty(subcategories.FirstOrDefault(x => x.CategoryId == tmpCategory.Id && x.Code.ToUpper().Trim() == subCategory.ToUpper().Trim())?.Code))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::SubCategory [{subCategory.ToUpper().Trim()}] is either invalid or not under [{tmpCategory.Code}] Category");
        //                                //    continue;
        //                                //}

        //                                //CHECK THIS VARIABLE MANUFACTURER
        //                                //if (string.IsNullOrEmpty(manufacturers.FirstOrDefault(x => x.ToUpper().Trim() == manufacturer.ToUpper().Trim())))
        //                                //{
        //                                //    errors.Add($"Row[{i + 1}]::Brand [{manufacturer.ToUpper().Trim()}] is not valid");
        //                                //    continue;
        //                                //}

        //                                var apiData = employeeApi.Where(x => x.ID == hrid).Select(x =>
        //                                {
        //                                    x.IsActive = true; // Modify property
        //                                    return new
        //                                    {
        //                                        x.ID,
        //                                        x.FirstName,
        //                                        x.LastName,
        //                                        x.TitleName,
        //                                        x.Email,
        //                                        x.IsActive // Select specific data
        //                                                   // Add more fields if needed
        //                                    };
        //                                }).FirstOrDefault();

        //                                if (bulkData.Exists(x => x.SM_Assettag == smAssetTag.Trim().ToUpper()))
        //                                    errors.Add($"Row[{i}] Duplicate entry for Serial No. [{smAssetTag.Trim().ToUpper()}]");
        //                                else
        //                                    bulkData.Add(new AssetUploadsAssetAssignments
        //                                    {
        //                                        Code = null,
        //                                        ITrackTicket = iTrackTicket,
        //                                        SM_Assettag = smAssetTag,
        //                                        WorkType = workType,
        //                                        Site = site,
        //                                        Hrid = hrid,
        //                                        Floor = floor,
        //                                        Area = area,
        //                                        Address = address,
        //                                        ContactNumber = contactNumber,
        //                                        EmployeeEmail = employeeEmail,
        //                                        DeploymentIssuanceDate = deploymentIssuanceDate,
        //                                        TrackingNumber = trackingNumber,
        //                                        Name = apiData.FirstName + ' ' + apiData.LastName,
        //                                        EmployeeStatus = apiData.IsActive,
        //                                        EmployeeTitle = apiData.TitleName
        //                                    });
        //                            }
        //                        }

        //                        if (errors.Any())
        //                            throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
        //                    }
        //                    catch (Exception e)
        //                    {
        //                        var error = ExceptionHandler.GetMessages(e);
        //                        throw new Exception(error);
        //                    }

        //                    if (bulkData.Any())
        //                    {
        //                        IEnumerable<IEnumerable<AssetUploadsAssetAssignments>> batches;
        //                        if (bulkData.Count > 1500)
        //                            batches = bulkData.Batch(1000);
        //                        else if (bulkData.Count > 1000)
        //                            batches = bulkData.Batch(500);
        //                        else if (bulkData.Count > 500)
        //                            batches = bulkData.Batch(250);
        //                        else if (bulkData.Count > 250)
        //                            batches = bulkData.Batch(125);
        //                        else if (bulkData.Count > 125)
        //                            batches = bulkData.Batch(60);
        //                        else if (bulkData.Count > 60)
        //                            batches = bulkData.Batch(30);
        //                        else if (bulkData.Count > 30)
        //                            batches = bulkData.Batch(15);
        //                        else batches = bulkData.Batch(5);
        //                        var tasks = batches.Select(b => b.ToList())
        //                            .Select(chunks => Task.Run(async () =>
        //                            {
        //                                try
        //                                {
        //                                    using (var context = new DataContext())
        //                                    {
        //                                        context.AssetUploadsAssetAssignments.AddRange(chunks);
        //                                        //var sample = context.AssetUploadsFarmouts.FirstOrDefault(x => x.SM_AssetTag == "003SAMP000");
        //                                        await context.SaveChangesAsync();
        //                                    }
        //                                }
        //                                catch (Exception exception)
        //                                {
        //                                    var error = ExceptionHandler.GetMessages(exception);
        //                                    Console.WriteLine(error);
        //                                }
        //                            }
        //                            )).ToList();
        //                        await Task.WhenAll(tasks);

        //                        using (var db = new DataContext())
        //                        {
        //                            foreach (var item in bulkData)
        //                            {
        //                                var serial = item.SM_Assettag;
        //                                //await db.Database.ExecuteSqlCommandAsync(@"
        //                                //    EXEC dbo.spAssignAssetEmployeeUploads @createdBy;
        //                                //    ", new SqlParameter("@createdBy", User.Identity.GetUserName())
        //                                //    );
        //                                await db.Database
        //                               .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssignAssetEmployeeUploads] @createdBy = '{user}'");
        //                                await db.Database.ExecuteSqlCommandAsync("EXEC [dbo].[spAssetAssignment] @serialNum = {0}", serial);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(ExceptionHandler.GetMessages(e));
        //    }
        //}

        //AssetAssignment bulk saving
        [HttpPost]
        [Route("api/AssetAssignment")]
        public async Task<IHttpActionResult> AssetUploads_AssetAssignment()
        {
            try
            {
                var user = User.Identity.GetUserName();
                using (var db = new DataContext())
                {
                    await db.Database
                        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssignAssetEmployeeUploads WHERE CreatedBy = '{user}'");

                    //check if there is an uploaded excel file
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        var file = HttpContext.Current.Request.Files["File"];
                        var bulkData = new List<AssetUploadsAssetAssignments>();
                        if (file != null && file.ContentLength > 0)
                        {
                            var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                            var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                            var tmpFileName = $"{user}-DEPLOY-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                            var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssignAsset/{phDateTime:MM-dd-yyyy}/{user}/");
                            Directory.CreateDirectory(path);
                            path = Path.Combine(path, tmpFileName);
                            //save the temporary file for reading of data - will be erased after
                            file.SaveAs(path);
                            if (File.Exists(path))
                            {
                                var errors = new List<string>();
                                try
                                {
                                    List<string> refCodes;
                                    List<string> hridListAsync;
                                    var userUploadHistory = new UserUploadHistory
                                    {
                                        UploadedBy = user,
                                        UploadedTemplateFileDir = path
                                    };
                                    db.UserUploadHistory.Add(userUploadHistory);
                                    await db.SaveChangesAsync();
                                    refCodes = await db
                                        .Assets
                                        .Where(x => x.IsActive)
                                        .Select(x => x.SerialNo).ToListAsync();
                                    hridListAsync = await db
                                        .BulkApiEmployee
                                        .Where(x => x.IsActive)
                                        .Select(x => x.ID).ToListAsync();
                                    using (var wb = new XLWorkbook(path))
                                    {
                                        hridListAsync = hridListAsync.Distinct().OrderBy(x => x).ToList();
                                        var sheet = wb.Worksheet(1);//default do not change
                                        var total = sheet.RowsUsed().Count() + 1;//default do not change
                                        for (var i = 2; i < total; i++)//default do not change
                                        {
                                            var row = sheet.Row(i);
                                            var itrackTicketNo = row.Cell(1).GetValue<string>();
                                            var serial = row.Cell(2).GetValue<string>();
                                            var workType = row.Cell(3).GetValue<string>();
                                            var site = row.Cell(4).GetValue<string>();
                                            var hrid = row.Cell(5).GetValue<string>();
                                            var floor = row.Cell(6).GetValue<string>();
                                            var area = row.Cell(7).GetValue<string>();
                                            var address = row.Cell(8).GetValue<string>();
                                            var contactNo = row.Cell(9).GetValue<string>();
                                            var email = row.Cell(10).GetValue<string>();
                                            var issuanceDate = row.Cell(11).GetValue<string>();
                                            var trackingNo = row.Cell(12).GetValue<string>();
                                            if (string.IsNullOrEmpty(serial) || string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(hrid))
                                            {
                                                errors.Add($"Row[{i + 1}]::[Serial], [WorkType], [Hrid] fields are required");
                                                continue;
                                            }

                                            if (workType.Trim().ToUpper().Equals("WAS"))
                                            {
                                                contactNo = address = string.Empty;
                                                if (string.IsNullOrEmpty(floor) || string.IsNullOrEmpty(area))
                                                {
                                                    errors.Add($"Row[{i + 1}]::[Floor], [Area] is required for [WorkType] - [WAS]");
                                                    continue;
                                                }
                                            }
                                            else if (workType.Trim().ToUpper().Equals("WAH"))
                                            {
                                                floor = area = string.Empty;
                                                if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(contactNo))
                                                {
                                                    errors.Add($"Row[{i + 1}]::[Address], [Contact_No] is required for [WorkType] - [WAH]");
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                errors.Add($"Row[{i + 1}]::[WorkType] is not valid");
                                                continue;
                                            }

                                            hrid = Regex.Replace(hrid.Trim(), @"[^a-zA-Z0-9]", "").ToUpper().Trim();

                                            if (string.IsNullOrEmpty(hridListAsync.FirstOrDefault(x => x.ToUpper().Trim() == hrid.ToUpper().Trim())))
                                            {
                                                errors.Add($"Unable to find Row[{i + 1}]:: HRID [{hrid}] in HRIS");
                                                continue;
                                            }

                                            if (string.IsNullOrEmpty(refCodes.FirstOrDefault(x => x.ToUpper().Trim() == serial.ToUpper().Trim())))
                                            {
                                                errors.Add($"Row[{i + 1}]:: Ref.No [{serial}] is not valid");
                                                continue;
                                            }

                                            if (bulkData.Exists(x => x.SM_Assettag.Trim().ToUpper() == serial.Trim().ToUpper()))
                                                errors.Add($"Row[{i}] Duplicate entry for Ref.No [{serial.Trim().ToUpper()}]");
                                            else
                                            {
                                                var createdBy = user.Replace("PH-", "").Trim();
                                                var bulkEmployee = await db.BulkApiEmployee.FirstOrDefaultAsync(x => x.ID == hrid && x.IsActive == true);
                                                var AssetData = await db.Assets.FirstOrDefaultAsync(x => x.SerialNo == serial);
                                                //add all the needed value sa list
                                                bulkData.Add(new AssetUploadsAssetAssignments
                                                {
                                                    Code = AssetData.Code,
                                                    ITrackTicket = itrackTicketNo?.Trim().ToUpper(),
                                                    SM_Assettag = serial?.Trim().ToUpper(),
                                                    WorkType = workType?.Trim().ToUpper(),
                                                    //Site = site?.Trim().ToUpper(),
                                                    Site = site,
                                                    Hrid = hrid?.Trim().ToUpper(),
                                                    Floor = floor?.Trim().ToUpper(),
                                                    Area = area?.Trim().ToUpper(),
                                                    Address = address?.Trim().ToUpper(),
                                                    ContactNumber = contactNo?.Trim().ToUpper(),
                                                    EmployeeEmail = email?.Trim().ToUpper(),
                                                    DeploymentIssuanceDate = FormatDate(issuanceDate?.Trim().ToUpper()),
                                                    TrackingNumber = trackingNo?.Trim().ToUpper(),
                                                    Name = bulkEmployee.FirstName.ToUpper() + " " + bulkEmployee.LastName.ToUpper(),
                                                    EmployeeStatus = bulkEmployee.IsActive,
                                                    EmployeeTitle = bulkEmployee.TitleName.ToUpper(),
                                                    CreatedBy = createdBy
                                                });
                                            }
                                        }
                                    }

                                    if (errors.Any())
                                        throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                                }
                                catch (Exception e)
                                {
                                    var error = ExceptionHandler.GetMessages(e);
                                    throw new Exception(error);
                                }

                                if (bulkData.Any())
                                {
                                    IEnumerable<IEnumerable<AssetUploadsAssetAssignments>> batches;
                                    if (bulkData.Count > 1500)
                                        batches = bulkData.Batch(1000);
                                    else if (bulkData.Count > 1000)
                                        batches = bulkData.Batch(500);
                                    else if (bulkData.Count > 500)
                                        batches = bulkData.Batch(250);
                                    else if (bulkData.Count > 250)
                                        batches = bulkData.Batch(125);
                                    else if (bulkData.Count > 125)
                                        batches = bulkData.Batch(60);
                                    else if (bulkData.Count > 60)
                                        batches = bulkData.Batch(30);
                                    else if (bulkData.Count > 30)
                                        batches = bulkData.Batch(15);
                                    else batches = bulkData.Batch(5);
                                    var tasks = batches.Select(b => b.ToList())
                                        .Select(chunks => Task.Run(async () =>
                                        {
                                            try
                                            {
                                                using (var context = new DataContext())
                                                {
                                                    context.AssetUploadsAssetAssignments.AddRange(chunks);
                                                    await context.SaveChangesAsync();
                                                }
                                            }
                                            catch (Exception exception)
                                            {
                                                var error = ExceptionHandler.GetMessages(exception);
                                                Console.WriteLine(error);
                                            }
                                        }
                                        )).ToList();
                                    await Task.WhenAll(tasks);
                                    string numericPart = Regex.Match(user, @"\d+").Value;
                                    await db.Database
                                            .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssignAssetEmployeeUploads2] @createdBy = '{numericPart}'");
                                }
                            }
                        }
                    }
                }

                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }


        //AssetDetails bulk saving
        //Check the upload history
        [HttpPost]
        [Route("api/AssetDetails")]
        public async Task<IHttpActionResult> AssetUploads_AssetDetails()
        {
            try
            {
                var user = User.Identity.GetUserName();

                //using (var db = new DataContext())
                //{
                //    await db.Database
                //        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssetUploads WHERE CreatedBy = '{user}'");
                //}

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files["File"];
                    var bulkData = new List<AssetUploadsAssetDetails>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                        var tmpFileName = $"{user}-ASSET-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                        var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssetUploads/{phDateTime:MM-dd-yyyy}/{user}/");
                        Directory.CreateDirectory(path);
                        path = Path.Combine(path, tmpFileName);
                        //save the temporary file for reading of data - will be erased after
                        file.SaveAs(path);

                        if (File.Exists(path))
                        {
                            var errors = new List<string>();
                            try
                            {
                                List<string> serials;
                                List<Category> categories;
                                List<SubCategory> subcategories;
                                List<string> manufacturers;
                                List<string> statuses;

                                using (var db = new DataContext())
                                {
                                    var userUploadHistory = new UserUploadHistory
                                    {
                                        UploadedBy = user,
                                        UploadedTemplateFileDir = path
                                    };
                                    db.UserUploadHistory.Add(userUploadHistory);
                                    await db.SaveChangesAsync();
                                    serials = await db.AssetUploadsAssetDetails.Where(x => !string.IsNullOrEmpty(x.SerialNumber)).Select(x => x.SerialNumber).ToListAsync();
                                    categories = await db.Categories.Where(x => x.IsActive).ToListAsync();
                                    subcategories = await db.SubCategories.Where(x => x.IsActive).ToListAsync();
                                    manufacturers = await db.Manufacturers.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                    statuses = await db.Statuses.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                }

                                using (var wb = new XLWorkbook(path))
                                {
                                    var sheet = wb.Worksheet(1);//default do not change
                                    var total = sheet.RowsUsed().Count() + 1;//default do not change
                                    for (var i = 2; i < total; i++)//default do not change
                                    {
                                        var row = sheet.Row(i);
                                        var purchaseOrderNum = row.Cell(1).GetValue<string>();
                                        var itemDesc = row.Cell(2).GetValue<string>();
                                        var category = row.Cell(3).GetValue<string>();
                                        var subCategory = row.Cell(4).GetValue<string>();
                                        var model = row.Cell(5).GetValue<string>();
                                        var brand = row.Cell(6).GetValue<string>();
                                        var processor = row.Cell(7).GetValue<string>();
                                        var ram = row.Cell(8).GetValue<string>();
                                        var screenSize = row.Cell(9).GetValue<string>();
                                        var serialNumber = row.Cell(10).GetValue<string>();
                                        var lobOwner = row.Cell(11).GetValue<string>();
                                        var status = row.Cell(12).GetValue<string>();
                                        var floor = row.Cell(13).GetValue<string>();
                                        var areaWorkstation = row.Cell(14).GetValue<string>();

                                        //var description = row.Cell(1).GetValue<string>();
                                        //var serial = row.Cell(2).GetValue<string>();
                                        //var category = row.Cell(3).GetValue<string>();
                                        //var subCategory = row.Cell(4).GetValue<string>();
                                        //var manufacturer = row.Cell(5).GetValue<string>();
                                        //var status = row.Cell(6).GetValue<string>();
                                        //var site = row.Cell(7).GetValue<string>();
                                        //var vendor = row.Cell(8).GetValue<string>();
                                        //var purchaseOrder = row.Cell(9).GetValue<string>();
                                        //var costValue = row.Cell(10).GetValue<string>();
                                        //var ram = row.Cell(11).GetValue<string>();
                                        //var hdcapacity = row.Cell(12).GetValue<string>();
                                        //var monitorsize = row.Cell(13).GetValue<string>();
                                        //var yearmodel = row.Cell(14).GetValue<string>();

                                        //if (string.IsNullOrEmpty(purchaseOrderNum) ||
                                        //    string.IsNullOrEmpty(itemDesc) ||
                                        //    string.IsNullOrEmpty(category) ||
                                        //    string.IsNullOrEmpty(subCategory) ||
                                        //    string.IsNullOrEmpty(model) ||
                                        //    string.IsNullOrEmpty(brand) ||
                                        //    string.IsNullOrEmpty(processor) ||
                                        //    string.IsNullOrEmpty(ram) ||
                                        //    string.IsNullOrEmpty(screenSize) ||
                                        //    string.IsNullOrEmpty(serialNumber) ||
                                        //    string.IsNullOrEmpty(lobOwner) ||
                                        //    string.IsNullOrEmpty(status) ||
                                        //    string.IsNullOrEmpty(floor) ||
                                        //    string.IsNullOrEmpty(areaWorkstation))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]:: All fields are required");
                                        //    continue;
                                        //}

                                        serialNumber = Regex.Replace(serialNumber.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
                                        serialNumber = Regex.Replace(serialNumber, @"[ ]{2,}", " ").ToUpper().Trim();
                                        serialNumber = Regex.Replace(serialNumber.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();

                                        var statusId = statuses.FirstOrDefault(x => x == status);

                                        int.TryParse(ConfigurationManager.AppSettings["SERIAL_NUMBER_LEN"], out var serialMinLen);
                                        if (serialNumber.Length < serialMinLen)
                                        {
                                            errors.Add($"Row[{i + 1}]::Serial should be equal or greater than ({serialMinLen}) characters");
                                            continue;
                                        }

                                        //if (!string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == serialNumber.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::Serial [{serialNumber}] is already used");
                                        //    continue;
                                        //}

                                        var tmpCategory = categories.FirstOrDefault(x => x.Code.ToUpper().Trim() == category.ToUpper().Trim());
                                        if (string.IsNullOrEmpty(tmpCategory?.Code))
                                        {
                                            errors.Add($"Row[{i + 1}]::[{category.ToUpper().Trim()}] is not a valid Category");
                                            continue;
                                        }

                                        //CHECK IF WHAT VARIABLE STATUS DO
                                        //if (string.IsNullOrEmpty(statuses.FirstOrDefault(x => x.ToUpper().Trim() == status.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::[{status}] is not a valid Status");
                                        //    continue;
                                        //}

                                        //if (string.IsNullOrEmpty(subcategories.FirstOrDefault(x => x.CategoryId == tmpCategory.Id && x.Code.ToUpper().Trim() == subCategory.ToUpper().Trim())?.Code))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::SubCategory [{subCategory.ToUpper().Trim()}] is either invalid or not under [{tmpCategory.Code}] Category");
                                        //    continue;
                                        //}

                                        //CHECK THIS VARIABLE MANUFACTURER
                                        if (string.IsNullOrEmpty(manufacturers.FirstOrDefault(x => x.ToUpper().Trim() == brand.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 1}]::Brand [{brand.ToUpper().Trim()}] is not valid");
                                            continue;
                                        }

                                        if (bulkData.Exists(x => x.SerialNumber == serialNumber.Trim().ToUpper()))
                                        {
                                            errors.Add($"Row[{i}] Duplicate entry for Serial No. [{serialNumber.Trim().ToUpper()}]");
                                        }


                                        else
                                        {
                                            bulkData.Add(new AssetUploadsAssetDetails
                                            {
                                                PurchaseOrderNum = purchaseOrderNum,
                                                ItemDesc = itemDesc,
                                                Category = category,
                                                SubCategory = subCategory,
                                                Model = model,
                                                Brand = brand,
                                                Processor = processor,
                                                Ram = ram,
                                                ScreenSize = screenSize,
                                                SerialNumber = serialNumber,
                                                LobOwner = lobOwner,
                                                Status = statusId,
                                                Floor = floor,
                                                AreaWorkstation = areaWorkstation
                                            });
                                        }
                                    }
                                }

                                if (errors.Any())
                                    throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                            }
                            catch (Exception e)
                            {
                                var error = ExceptionHandler.GetMessages(e);
                                throw new Exception(error);
                            }

                            if (bulkData.Any())
                            {
                                IEnumerable<IEnumerable<AssetUploadsAssetDetails>> batches;
                                if (bulkData.Count > 1500)
                                    batches = bulkData.Batch(1000);
                                else if (bulkData.Count > 1000)
                                    batches = bulkData.Batch(500);
                                else if (bulkData.Count > 500)
                                    batches = bulkData.Batch(250);
                                else if (bulkData.Count > 250)
                                    batches = bulkData.Batch(125);
                                else if (bulkData.Count > 125)
                                    batches = bulkData.Batch(60);
                                else if (bulkData.Count > 60)
                                    batches = bulkData.Batch(30);
                                else if (bulkData.Count > 30)
                                    batches = bulkData.Batch(15);
                                else batches = bulkData.Batch(5);
                                var tasks = batches.Select(b => b.ToList())
                                    .Select(chunks => Task.Run(async () =>
                                    {
                                        try
                                        {
                                            using (var context = new DataContext())
                                            {
                                                context.AssetUploadsAssetDetails.AddRange(chunks);
                                                //var sample = context.AssetUploadsFarmouts.FirstOrDefault(x => x.SM_AssetTag == "003SAMP000");
                                                await context.SaveChangesAsync();
                                            }
                                        }
                                        catch (Exception exception)
                                        {
                                            var error = ExceptionHandler.GetMessages(exception);
                                            Console.WriteLine(error);
                                        }
                                    }
                                    )).ToList();
                                await Task.WhenAll(tasks);

                                //FIX THIS, THIS IS ONLY FOR 1 ROW
                                //var serial = bulkData[0].SerialNumber;

                                //using (var db = new DataContext())
                                //{
                                //    await db.Database
                                //        .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssetDetails] @serialNum = '{serial}'");
                                //}

                                using (var db = new DataContext())
                                {
                                    foreach (var item in bulkData)
                                    {
                                        var serial = item.SerialNumber;
                                        await db.Database.ExecuteSqlCommandAsync("EXEC [dbo].[spAssetDetails] @serialNum = {0}", serial);
                                    }
                                }
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        //FarmOut bulk saving
        [HttpPost]
        [Route("api/FarmOut")]
        public async Task<IHttpActionResult> AssetUploads_Farmout()
        {
            try
            {
                var user = User.Identity.GetUserName();

                //using (var db = new DataContext())
                //{
                //    await db.Database
                //        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssetUploads WHERE CreatedBy = '{user}'");
                //}

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files["File"];
                    var bulkData = new List<AssetUploadsFarmouts>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                        var tmpFileName = $"{user}-ASSET-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                        var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssetUploads/{phDateTime:MM-dd-yyyy}/{user}/");
                        Directory.CreateDirectory(path);
                        path = Path.Combine(path, tmpFileName);
                        //save the temporary file for reading of data - will be erased after
                        file.SaveAs(path);

                        if (File.Exists(path))
                        {
                            var errors = new List<string>();
                            try
                            {
                                List<string> serials;
                                List<Category> categories;
                                List<SubCategory> subcategories;
                                List<string> manufacturers;
                                List<string> statuses;

                                using (var db = new DataContext())
                                {
                                    var userUploadHistory = new UserUploadHistory
                                    {
                                        UploadedBy = user,
                                        UploadedTemplateFileDir = path
                                    };
                                    db.UserUploadHistory.Add(userUploadHistory);
                                    await db.SaveChangesAsync();
                                    serials = await db.Assets.Where(x => !string.IsNullOrEmpty(x.SerialNo)).Select(x => x.SerialNo).ToListAsync();
                                    categories = await db.Categories.Where(x => x.IsActive).ToListAsync();
                                    subcategories = await db.SubCategories.Where(x => x.IsActive).ToListAsync();
                                    manufacturers = await db.Manufacturers.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                    statuses = await db.Statuses.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                }

                                using (var wb = new XLWorkbook(path))
                                {
                                    var sheet = wb.Worksheet(1);//default do not change
                                    var total = sheet.RowsUsed().Count() + 1;//default do not change
                                    for (var i = 2; i < total; i++)//default do not change
                                    {
                                        var row = sheet.Row(i);
                                        var SM_AssetTag = row.Cell(1).GetValue<string>();
                                        var transactionType = row.Cell(2).GetValue<string>();
                                        var iTrackTicketNumber = row.Cell(3).GetValue<string>();
                                        var purchaseOrderNumber = row.Cell(4).GetValue<string>();
                                        var quantity = row.Cell(5).GetValue<string>();
                                        var originatingSite = row.Cell(6).GetValue<string>();
                                        var destinationSite = row.Cell(7).GetValue<string>();
                                        var loaNumber = row.Cell(8).GetValue<string>();
                                        var validity = row.Cell(9).GetValue<string>();
                                        var loaORnumber = row.Cell(10).GetValue<string>();
                                        var loaFee = row.Cell(11).GetValue<string>();
                                        var bondIssuer = row.Cell(12).GetValue<string>();
                                        var bondableAmount = row.Cell(13).GetValue<string>();
                                        var suretyBondPolicyNumber = row.Cell(14).GetValue<string>();
                                        var validityPeriod = row.Cell(15).GetValue<string>();
                                        var suretyBondOfficialReceiept = row.Cell(16).GetValue<string>();
                                        var amountPaid = row.Cell(17).GetValue<string>();
                                        var pezaForm8106Number = row.Cell(18).GetValue<string>();
                                        var pezaPermitNumber = row.Cell(19).GetValue<string>();
                                        var pezaApprovalDate = row.Cell(20).GetValue<string>();

                                        //var description = row.Cell(1).GetValue<string>();
                                        //var serial = row.Cell(2).GetValue<string>();
                                        //var category = row.Cell(3).GetValue<string>();
                                        //var subCategory = row.Cell(4).GetValue<string>();
                                        //var manufacturer = row.Cell(5).GetValue<string>();
                                        //var status = row.Cell(6).GetValue<string>();
                                        //var site = row.Cell(7).GetValue<string>();
                                        //var vendor = row.Cell(8).GetValue<string>();
                                        //var purchaseOrder = row.Cell(9).GetValue<string>();
                                        //var costValue = row.Cell(10).GetValue<string>();
                                        //var ram = row.Cell(11).GetValue<string>();
                                        //var hdcapacity = row.Cell(12).GetValue<string>();
                                        //var monitorsize = row.Cell(13).GetValue<string>();
                                        //var yearmodel = row.Cell(14).GetValue<string>();

                                        //if (string.IsNullOrEmpty(SM_AssetTag) ||
                                        //    string.IsNullOrEmpty(transactionType) ||
                                        //    string.IsNullOrEmpty(iTrackTicketNumber) ||
                                        //    string.IsNullOrEmpty(purchaseOrderNumber) ||
                                        //    string.IsNullOrEmpty(quantity) ||
                                        //    string.IsNullOrEmpty(originatingSite) ||
                                        //    string.IsNullOrEmpty(destinationSite) ||
                                        //    string.IsNullOrEmpty(loaNumber) ||
                                        //    string.IsNullOrEmpty(validity) ||
                                        //    string.IsNullOrEmpty(loaORnumber) ||
                                        //    string.IsNullOrEmpty(loaFee) ||
                                        //    string.IsNullOrEmpty(bondIssuer) ||
                                        //    string.IsNullOrEmpty(bondableAmount) ||
                                        //    string.IsNullOrEmpty(suretyBondPolicyNumber) ||
                                        //    string.IsNullOrEmpty(validityPeriod) ||
                                        //    string.IsNullOrEmpty(suretyBondOfficialReceiept) ||
                                        //    string.IsNullOrEmpty(amountPaid) ||
                                        //    string.IsNullOrEmpty(pezaForm8106Number) ||
                                        //    string.IsNullOrEmpty(pezaPermitNumber) ||
                                        //    string.IsNullOrEmpty(pezaApprovalDate))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]:: All fields are required");
                                        //    continue;
                                        //}

                                        SM_AssetTag = Regex.Replace(SM_AssetTag.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
                                        SM_AssetTag = Regex.Replace(SM_AssetTag, @"[ ]{2,}", " ").ToUpper().Trim();
                                        SM_AssetTag = Regex.Replace(SM_AssetTag.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();

                                        int.TryParse(ConfigurationManager.AppSettings["SERIAL_NUMBER_LEN"], out var serialMinLen);
                                        if (SM_AssetTag.Length < serialMinLen)
                                        {
                                            errors.Add($"Row[{i + 1}]::Serial should be equal or greater than ({serialMinLen}) characters");
                                            continue;
                                        }

                                        //if (!string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == SM_AssetTag.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::Serial [{SM_AssetTag}] is already used");
                                        //    continue;
                                        //}

                                        //var tmpCategory = categories.FirstOrDefault(x => x.Code.ToUpper().Trim() == category.ToUpper().Trim());
                                        //if (string.IsNullOrEmpty(tmpCategory?.Code))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::[{category.ToUpper().Trim()}] is not a valid Category");
                                        //    continue;
                                        //}

                                        //CHECK IF WHAT VARIABLE STATUS DO
                                        //if (string.IsNullOrEmpty(statuses.FirstOrDefault(x => x.ToUpper().Trim() == status.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::[{status}] is not a valid Status");
                                        //    continue;
                                        //}

                                        //if (string.IsNullOrEmpty(subcategories.FirstOrDefault(x => x.CategoryId == tmpCategory.Id && x.Code.ToUpper().Trim() == subCategory.ToUpper().Trim())?.Code))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::SubCategory [{subCategory.ToUpper().Trim()}] is either invalid or not under [{tmpCategory.Code}] Category");
                                        //    continue;
                                        //}

                                        //CHECK THIS VARIABLE MANUFACTURER
                                        //if (string.IsNullOrEmpty(manufacturers.FirstOrDefault(x => x.ToUpper().Trim() == manufacturer.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::Brand [{manufacturer.ToUpper().Trim()}] is not valid");
                                        //    continue;
                                        //}

                                        if (bulkData.Exists(x => x.SM_AssetTag == SM_AssetTag.Trim().ToUpper()))
                                            errors.Add($"Row[{i}] Duplicate entry for Serial No. [{SM_AssetTag.Trim().ToUpper()}]");
                                        else
                                            bulkData.Add(new AssetUploadsFarmouts
                                            {
                                                SM_AssetTag = SM_AssetTag,
                                                Transaction_Type = transactionType,
                                                iTrack_Ticket_Number = iTrackTicketNumber,
                                                Purchase_Order_Number = purchaseOrderNumber,
                                                Quantity = quantity,
                                                Originating_Site = originatingSite,
                                                Destination_Site = destinationSite,
                                                LOA_Number = loaNumber,
                                                Validity = validity,
                                                LOA_OR_Number = loaORnumber,
                                                LOA_Fee = loaFee,
                                                Bond_Issuer = bondIssuer,
                                                Bondable_Ammount = bondableAmount,
                                                Surety_Bond_Policy_Number = suretyBondPolicyNumber,
                                                Validity_Period = validityPeriod,
                                                Surety_Bond_Official_Receiept = suretyBondOfficialReceiept,
                                                Amount_Paid = amountPaid,
                                                Peza_Form_8106_Number = pezaForm8106Number,
                                                Peza_Permit_Number = pezaPermitNumber,
                                                Peza_Approval_Date = pezaApprovalDate
                                            });
                                    }
                                }

                                if (errors.Any())
                                    throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                            }
                            catch (Exception e)
                            {
                                var error = ExceptionHandler.GetMessages(e);
                                throw new Exception(error);
                            }

                            if (bulkData.Any())
                            {
                                IEnumerable<IEnumerable<AssetUploadsFarmouts>> batches;
                                if (bulkData.Count > 1500)
                                    batches = bulkData.Batch(1000);
                                else if (bulkData.Count > 1000)
                                    batches = bulkData.Batch(500);
                                else if (bulkData.Count > 500)
                                    batches = bulkData.Batch(250);
                                else if (bulkData.Count > 250)
                                    batches = bulkData.Batch(125);
                                else if (bulkData.Count > 125)
                                    batches = bulkData.Batch(60);
                                else if (bulkData.Count > 60)
                                    batches = bulkData.Batch(30);
                                else if (bulkData.Count > 30)
                                    batches = bulkData.Batch(15);
                                else batches = bulkData.Batch(5);
                                var tasks = batches.Select(b => b.ToList())
                                    .Select(chunks => Task.Run(async () =>
                                    {
                                        try
                                        {
                                            using (var context = new DataContext())
                                            {
                                                context.AssetUploadsFarmouts.AddRange(chunks);
                                                //var sample = context.AssetUploadsFarmouts.FirstOrDefault(x => x.SM_AssetTag == "003SAMP000");
                                                await context.SaveChangesAsync();
                                            }
                                        }
                                        catch (Exception exception)
                                        {
                                            var error = ExceptionHandler.GetMessages(exception);
                                            Console.WriteLine(error);
                                        }
                                    }
                                    )).ToList();
                                await Task.WhenAll(tasks);

                                //will check if what are the needs on this stored proc
                                //using (var db = new DataContext())
                                //{
                                //    await db.Database
                                //        .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssetUploads] @createdBy = '{user}'");
                                //}
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        //New Asset bulk saving
        [HttpPost]
        [Route("api/AssetApi/{id}/bulk")]
        public async Task<IHttpActionResult> AssetUploads()
        {
            try
            {
                var user = User.Identity.GetUserName();
                using (var db = new DataContext())
                {
                    await db.Database
                        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssetUploads WHERE CreatedBy = '{user}'");
                }

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files["File"];
                    var bulkData = new List<AssetUploads>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                        var tmpFileName = $"{user}-ASSET-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                        var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssetUploads/{phDateTime:MM-dd-yyyy}/{user}/");
                        Directory.CreateDirectory(path);
                        path = Path.Combine(path, tmpFileName);
                        //save the temporary file for reading of data - will be erased after
                        file.SaveAs(path);

                        if (File.Exists(path))
                        {
                            var errors = new List<string>();
                            try
                            {
                                List<string> serials;
                                List<Category> categories;
                                List<SubCategory> subcategories;
                                List<string> manufacturers;
                                List<string> statuses;
                                List<string> vendorsCode;

                                using (var db = new DataContext())
                                {
                                    var userUploadHistory = new UserUploadHistory
                                    {
                                        UploadedBy = user,
                                        UploadedTemplateFileDir = path
                                    };
                                    db.UserUploadHistory.Add(userUploadHistory);
                                    await db.SaveChangesAsync();
                                    serials = await db.Assets.Where(x => !string.IsNullOrEmpty(x.SerialNo)).Select(x => x.SerialNo).ToListAsync();
                                    categories = await db.Categories.Where(x => x.IsActive).ToListAsync();
                                    subcategories = await db.SubCategories.Where(x => x.IsActive).ToListAsync();
                                    manufacturers = await db.Manufacturers.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                    statuses = await db.Statuses.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                    vendorsCode = await new DataContext()
                                        .Vendors
                                        .Where(x => x.IsActive)
                                        .Select(x => x.Code)
                                        .ToListAsync();
                                }

                                using (var wb = new XLWorkbook(path))
                                {
                                    var sheet = wb.Worksheet(1);//default do not change
                                    var total = sheet.RowsUsed().Count() + 1;//default do not change
                                    for (var i = 2; i < total; i++)//default do not change
                                    {
                                        var row = sheet.Row(i);
                                        var SM_AssetTag = row.Cell(1).GetValue<string>();
                                        var site = row.Cell(2).GetValue<string>();
                                        var category = row.Cell(3).GetValue<string>();
                                        var subCategory = row.Cell(4).GetValue<string>();
                                        var itemDesc = row.Cell(5).GetValue<string>();
                                        var quantity = row.Cell(6).GetValue<string>();
                                        var currency = row.Cell(7).GetValue<string>();
                                        var unitPrice = row.Cell(8).GetValue<string>();
                                        var totalValue = row.Cell(9).GetValue<string>();
                                        var vendor = row.Cell(10).GetValue<string>();
                                        var vendorAddress = row.Cell(11).GetValue<string>();
                                        var purchaseOrderNumber = row.Cell(12).GetValue<string>();
                                        var purchaseOrderDate = row.Cell(13).GetValue<string>();
                                        var invoiceNumber = row.Cell(14).GetValue<string>();
                                        var invoiceDate = row.Cell(15).GetValue<string>();
                                        var deliveryReceiptNumber = row.Cell(16).GetValue<string>();
                                        var deliveryReceiptDate = row.Cell(17).GetValue<string>();
                                        var receivedBy = row.Cell(18).GetValue<string>();
                                        var warrantyStartDate = row.Cell(19).GetValue<string>();
                                        var warrantyEndDate = row.Cell(20).GetValue<string>();
                                        var pezaForm8105Number = row.Cell(21).GetValue<string>();
                                        var pezaPermitNumber = row.Cell(22).GetValue<string>();
                                        var pezaApprovalDate = row.Cell(23).GetValue<string>();
                                        var importationPermitNumber = row.Cell(24).GetValue<string>();
                                        var validUntil = row.Cell(25).GetValue<string>();
                                        var billOfLandingNumber = row.Cell(26).GetValue<string>();
                                        var taxesPaid = row.Cell(27).GetValue<string>();
                                        var bondIssuer = row.Cell(28).GetValue<string>();
                                        var suretyBondPolicyNumber = row.Cell(29).GetValue<string>();
                                        var validityPeriod = row.Cell(30).GetValue<string>();
                                        var suretyBondOfficialReceiept = row.Cell(31).GetValue<string>();
                                        var amountPaid = row.Cell(32).GetValue<string>();
                                        var remarks = row.Cell(33).GetValue<string>();

                                        //var description = row.Cell(1).GetValue<string>();
                                        //var serial = row.Cell(2).GetValue<string>();
                                        //var category = row.Cell(3).GetValue<string>();
                                        //var subCategory = row.Cell(4).GetValue<string>();
                                        //var manufacturer = row.Cell(5).GetValue<string>();
                                        //var status = row.Cell(6).GetValue<string>();
                                        //var site = row.Cell(7).GetValue<string>();
                                        //var vendor = row.Cell(8).GetValue<string>();
                                        //var purchaseOrder = row.Cell(9).GetValue<string>();
                                        //var costValue = row.Cell(10).GetValue<string>();
                                        //var ram = row.Cell(11).GetValue<string>();
                                        //var hdcapacity = row.Cell(12).GetValue<string>();
                                        //var monitorsize = row.Cell(13).GetValue<string>();
                                        //var yearmodel = row.Cell(14).GetValue<string>();

                                        //if (string.IsNullOrEmpty(SM_AssetTag) ||
                                        //    string.IsNullOrEmpty(site) ||
                                        //    string.IsNullOrEmpty(category) ||
                                        //    string.IsNullOrEmpty(subCategory) ||
                                        //    string.IsNullOrEmpty(itemDesc) ||
                                        //    string.IsNullOrEmpty(quantity) ||
                                        //    string.IsNullOrEmpty(currency) ||
                                        //    string.IsNullOrEmpty(unitPrice) ||
                                        //    string.IsNullOrEmpty(totalValue) ||
                                        //    string.IsNullOrEmpty(vendor) ||
                                        //    string.IsNullOrEmpty(vendorAddress) ||
                                        //    string.IsNullOrEmpty(purchaseOrderNumber) ||
                                        //    string.IsNullOrEmpty(purchaseOrderDate) ||
                                        //    string.IsNullOrEmpty(invoiceNumber) ||
                                        //    string.IsNullOrEmpty(invoiceDate) ||
                                        //    string.IsNullOrEmpty(deliveryReceiptNumber) ||
                                        //    string.IsNullOrEmpty(deliveryReceiptDate) ||
                                        //    string.IsNullOrEmpty(receivedBy) ||
                                        //    string.IsNullOrEmpty(warrantyStartDate) ||
                                        //    string.IsNullOrEmpty(warrantyEndDate) ||
                                        //    string.IsNullOrEmpty(pezaForm8105Number) ||
                                        //    string.IsNullOrEmpty(pezaPermitNumber) ||
                                        //    string.IsNullOrEmpty(pezaApprovalDate) ||
                                        //    string.IsNullOrEmpty(importationPermitNumber) ||
                                        //    string.IsNullOrEmpty(validUntil) ||
                                        //    string.IsNullOrEmpty(billOfLandingNumber) ||
                                        //    string.IsNullOrEmpty(taxesPaid) ||
                                        //    string.IsNullOrEmpty(bondIssuer) ||
                                        //    string.IsNullOrEmpty(suretyBondPolicyNumber) ||
                                        //    string.IsNullOrEmpty(validityPeriod) ||
                                        //    string.IsNullOrEmpty(suretyBondOfficialReceieptAmountPaid) ||
                                        //    string.IsNullOrEmpty(remarks))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]:: All fields are required");
                                        //    continue;
                                        //}

                                        SM_AssetTag = Regex.Replace(SM_AssetTag.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
                                        SM_AssetTag = Regex.Replace(SM_AssetTag, @"[ ]{2,}", " ").ToUpper().Trim();
                                        SM_AssetTag = Regex.Replace(SM_AssetTag.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();

                                        int.TryParse(ConfigurationManager.AppSettings["SERIAL_NUMBER_LEN"], out var serialMinLen);
                                        if (SM_AssetTag.Length < serialMinLen)
                                        {
                                            errors.Add($"Row[{i + 1}]::Serial should be equal or greater than ({serialMinLen}) characters");
                                            continue;
                                        }

                                        if (!string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == SM_AssetTag.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 1}]::Serial [{SM_AssetTag}] is already used");
                                            continue;
                                        }

                                        var tmpCategory = categories.FirstOrDefault(x => x.Code.ToUpper().Trim() == category.ToUpper().Trim());
                                        if (string.IsNullOrEmpty(tmpCategory?.Code))
                                        {
                                            errors.Add($"Row[{i + 1}]::[{category.ToUpper().Trim()}] is not a valid Category");
                                            continue;
                                        }

                                        if (string.IsNullOrEmpty(vendorsCode.FirstOrDefault(x => x.ToUpper().Trim() == vendor.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 1}]::Brand [{vendor.ToUpper().Trim()}] is not valid");
                                            continue;
                                        }

                                        //CHECK IF WHAT VARIABLE STATUS DO
                                        //if (string.IsNullOrEmpty(statuses.FirstOrDefault(x => x.ToUpper().Trim() == status.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::[{status}] is not a valid Status");
                                        //    continue;
                                        //}

                                        if (string.IsNullOrEmpty(subcategories.FirstOrDefault(x => x.CategoryId == tmpCategory.Id && x.Code.ToUpper().Trim() == subCategory.ToUpper().Trim())?.Code))
                                        {
                                            errors.Add($"Row[{i + 1}]::SubCategory [{subCategory.ToUpper().Trim()}] is either invalid or not under [{tmpCategory.Code}] Category");
                                            continue;
                                        }

                                        //CHECK THIS VARIABLE MANUFACTURER
                                        //if (string.IsNullOrEmpty(manufacturers.FirstOrDefault(x => x.ToUpper().Trim() == manufacturer.ToUpper().Trim())))
                                        //{
                                        //    errors.Add($"Row[{i + 1}]::Brand [{manufacturer.ToUpper().Trim()}] is not valid");
                                        //    continue;
                                        //}

                                        if (bulkData.Exists(x => x.SerialNo == SM_AssetTag.Trim().ToUpper()))
                                            errors.Add($"Row[{i}] Duplicate entry for Serial No. [{SM_AssetTag.Trim().ToUpper()}]");
                                        else
                                        {
                                            bulkData.Add(new AssetUploads
                                            {
                                                SM_AssetTag = SM_AssetTag,
                                                Site = site,
                                                Category = category,
                                                SubCategory = subCategory,
                                                ItemDesc = itemDesc,
                                                Quantity = quantity,
                                                Currency = currency,
                                                UnitPrice = unitPrice,
                                                TotalValue = totalValue,
                                                Vendor = vendor,
                                                VendorAddress = vendorAddress,
                                                PurchaseOrderNumber = purchaseOrderNumber,
                                                PurchaseOrderDate = FormatDateNoTime(purchaseOrderDate),
                                                InvoiceNumber = invoiceNumber,
                                                InvoiceDate = FormatDateNoTime(invoiceDate),
                                                DeliveryReceiptNumber = deliveryReceiptNumber,
                                                DeliveryReceivedDate = FormatDate(deliveryReceiptDate),
                                                ReceivedBy = receivedBy,
                                                WarrantyStartDate = FormatDateNoTime(warrantyStartDate),
                                                WarrantyEndDate = FormatDateNoTime(warrantyEndDate),
                                                PezaForm8105Number = pezaForm8105Number,
                                                PezaPermitNumber = pezaPermitNumber,
                                                PezaApprovalDate = FormatDate(pezaApprovalDate),
                                                ImportationPermitNumber = importationPermitNumber,
                                                ValidUntil = FormatDateNoTime(validUntil),
                                                BillOfLandingNumber = billOfLandingNumber,
                                                TaxesPaid = taxesPaid,
                                                BondIssuer = bondIssuer,
                                                SuretyBondPolicyNumber = suretyBondPolicyNumber,
                                                ValidityPeriod = FormatDateNoTime(validityPeriod),
                                                SuretyBondOfficialReceiept = suretyBondOfficialReceiept,
                                                AmountPaid = amountPaid,
                                                Remarks = remarks,
                                                CreatedBy = user

                                                //Description = description.Trim().ToUpper(),
                                                //SerialNo = serial.Trim().ToUpper(),
                                                //Category = category.Trim().ToUpper(),
                                                //SubCategory = subCategory.Trim().ToUpper(),
                                                //Manufacturer = manufacturer.Trim().ToUpper(),
                                                //Status = status.Trim().ToUpper(),
                                                //Site = site,
                                                //CostValue = costValue,
                                                //Ram = ram,
                                                //HdCapacity = hdcapacity,
                                                //MonitorSize = monitorsize,
                                                //YearModel = yearmodel,
                                                //PurchaseOrder = purchaseOrder,
                                                //Vendor = vendor
                                            });
                                        }
                                    }
                                }

                                if (errors.Any())
                                    throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                            }
                            catch (Exception e)
                            {
                                var error = ExceptionHandler.GetMessages(e);
                                throw new Exception(error);
                            }

                            if (bulkData.Any())
                            {
                                IEnumerable<IEnumerable<AssetUploads>> batches;
                                if (bulkData.Count > 1500)
                                    batches = bulkData.Batch(1000);
                                else if (bulkData.Count > 1000)
                                    batches = bulkData.Batch(500);
                                else if (bulkData.Count > 500)
                                    batches = bulkData.Batch(250);
                                else if (bulkData.Count > 250)
                                    batches = bulkData.Batch(125);
                                else if (bulkData.Count > 125)
                                    batches = bulkData.Batch(60);
                                else if (bulkData.Count > 60)
                                    batches = bulkData.Batch(30);
                                else if (bulkData.Count > 30)
                                    batches = bulkData.Batch(15);
                                else batches = bulkData.Batch(5);
                                var tasks = batches.Select(b => b.ToList())
                                    .Select(chunks => Task.Run(async () =>
                                        {
                                            try
                                            {
                                                using (var context = new DataContext())
                                                {
                                                    context.AssetUploads.AddRange(chunks);
                                                    var sample = context.AssetUploads.FirstOrDefault(x => x.SerialNo == "003SAMP000");
                                                    await context.SaveChangesAsync();
                                                }
                                            }
                                            catch (Exception exception)
                                            {
                                                var error = ExceptionHandler.GetMessages(exception);
                                                Console.WriteLine(error);
                                            }
                                        }
                                    )).ToList();
                                await Task.WhenAll(tasks);
                                using (var db = new DataContext())
                                {
                                    await db.Database
                                        .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssetUploads] @createdBy = '{user}'");
                                }
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        //DATE FORMATTER
        string FormatDate(string inputDate)
        {
            if (DateTime.TryParse(inputDate, out DateTime parsedDate))
            {
                return parsedDate.ToString("MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            }
            return null; // or handle invalid dates differently
        }
        string FormatDateNoTime(string inputDate)
        {
            if (DateTime.TryParse(inputDate, out DateTime parsedDate))
            {
                return parsedDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            return null; // or handle invalid dates differently
        }

        private static void ToQrCode(string text, string docPath)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (File.Exists(docPath)) return;
            // instantiate a writer object
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 400,
                    Width = 400,
                    Margin = 0
                },
                Renderer = new BitmapRenderer()
            };

            // write text and generate a 2-D barcode as a bitmap
            using (var bitmap = barcodeWriter.Write(text))
            using (var stream = new FileStream(docPath, FileMode.Create))
                bitmap.Save(stream, ImageFormat.Png);
        }

        // DELETE api/AssetApi/5
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Assets.FirstOrDefaultAsync(x => x.Id == id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing");
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

        #region 6.12.21
        [HttpPost]
        public async Task<IHttpActionResult> Update(Guid id, [FromBody] AssetVm2 vm)
        {
            try
            {

                using (var db = new DataContext())
                {
                    await db.Database.ExecuteSqlCommandAsync(@"
                    DELETE FROM dbo.AssetStatusUploads WHERE CreatedBy = @User;
                    DELETE FROM dbo.AssetUpdateUploads WHERE CreatedBy = @User;
                    DELETE FROM dbo.ReplaceUploads WHERE CreatedBy = @User;
                    ", new SqlParameter("@User", User.Identity.GetUserName()));
                    var entity = await db.Assets.FindAsync(id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing.");

                    if (string.IsNullOrEmpty(vm.Description))
                        throw new Exception("DESCRIPTION is required");
                    if (string.IsNullOrEmpty(vm.SerialNo))
                        throw new Exception("SERIAL # is required");
                    if (string.IsNullOrEmpty(vm.Category))
                        throw new Exception("CATEGORY is required");
                    if (string.IsNullOrEmpty(vm.SubCategory))
                        throw new Exception("SUBCATEGORY is required");
                    if (string.IsNullOrEmpty(vm.Manufacturer))
                        throw new Exception("BRAND is required");
                    if (string.IsNullOrEmpty(vm.Status))
                        throw new Exception("STATUS is required");
                    if (string.IsNullOrEmpty(vm.Site))
                        throw new Exception("SITE is required");

                    vm.SerialNo = Regex.Replace(vm.SerialNo.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
                    vm.SerialNo = Regex.Replace(vm.SerialNo, @"[ ]{2,}", " ").ToUpper().Trim();
                    vm.SerialNo = Regex.Replace(vm.SerialNo.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();

                    int.TryParse(ConfigurationManager.AppSettings["SERIAL_NUMBER_LEN"], out var serialMinLen);
                    if (vm.Description.Trim().Length < serialMinLen)
                        throw new Exception($"DESCRIPTION should be equal or greater than ({serialMinLen}) characters");
                    if (vm.SerialNo.Trim().Length < serialMinLen)
                        throw new Exception($"SERIAL # should be equal or greater than ({serialMinLen}) characters");

                    if (!entity.Description.Equals(vm.Description))
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.Description),
                            From = entity.Description,
                            To = vm.Description,
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.Description = vm.Description;

                    if (!entity.SerialNo.Equals(vm.SerialNo))
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.SerialNo),
                            From = entity.SerialNo,
                            To = vm.SerialNo,
                            CreatedBy = User.Identity.GetUserName()
                        });
                        db.ReplaceUploads.Add(new ReplaceUploads
                        {
                            Code = entity.Code,
                            Serial = vm.SerialNo,
                            Manufacturer = vm.Manufacturer,
                            Vendor = vm.Vendor,
                            PurchaseOrder = vm.PurchaseOrder,
                            CostValue = vm.CostValue,
                            //Ram = vm.Ram,
                            //HdCapacity = vm.HdCapacity,
                            //MonitorSize = vm.MonitorSize,
                            //YearModel = vm.YearModel,
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.SerialNo = vm.SerialNo;

                    var category = await db.Categories.FirstOrDefaultAsync(x => x.Code == vm.Category);
                    if (entity.CategoryId != category?.Id)
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.CategoryId),
                            From = entity.CategoryId?.ToString(),
                            To = category?.Id.ToString(),
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.CategoryId = category?.Id;

                    var subCategory = await db.SubCategories.FirstOrDefaultAsync(x => x.Code == vm.SubCategory);
                    if (entity.SubCategoryId != subCategory?.Id)
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.SubCategoryId),
                            From = entity.SubCategoryId?.ToString(),
                            To = subCategory?.Id.ToString(),
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.SubCategoryId = subCategory?.Id;

                    var status = await db.Statuses.FirstOrDefaultAsync(x => x.Code == vm.Status);
                    if (entity.StatusId != status?.Id)
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.StatusId),
                            From = entity.StatusId?.ToString(),
                            To = status?.Id.ToString(),
                            CreatedBy = User.Identity.GetUserName()
                        });
                        db.AssetStatusUploads.Add(new AssetStatusUploads
                        {
                            Code = entity.Code,
                            Status = status?.Code,
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.StatusId = status?.Id;

                    var manufacturer = await db.Manufacturers.FirstOrDefaultAsync(x => x.Code == vm.Manufacturer);
                    if (entity.ManufacturerId != manufacturer?.Id)
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.ManufacturerId),
                            From = entity.ManufacturerId?.ToString(),
                            To = manufacturer?.Id.ToString(),
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.ManufacturerId = manufacturer?.Id;

                    if (entity.PurchaseOrder != vm.PurchaseOrder)
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.PurchaseOrder),
                            From = entity.PurchaseOrder,
                            To = vm.PurchaseOrder,
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.PurchaseOrder = vm.PurchaseOrder;

                    if (entity.CostValue != vm.CostValue)
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.CostValue),
                            From = entity.CostValue,
                            To = vm.CostValue,
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.CostValue = vm.CostValue;

                    //if (entity.Ram != vm.Ram)
                    //{
                    //    db.AssetUpdateUploads.Add(new AssetUpdateUploads
                    //    {
                    //        Code = entity.Code,
                    //        Column = nameof(entity.CostValue),
                    //        From = entity.Ram,
                    //        To = vm.Ram,
                    //        CreatedBy = User.Identity.GetUserName()
                    //    });
                    //}
                    //entity.Ram = vm.Ram;

                    //if (entity.HdCapacity != vm.HdCapacity)
                    //{
                    //    db.AssetUpdateUploads.Add(new AssetUpdateUploads
                    //    {
                    //        Code = entity.Code,
                    //        Column = nameof(entity.CostValue),
                    //        From = entity.HdCapacity,
                    //        To = vm.HdCapacity,
                    //        CreatedBy = User.Identity.GetUserName()
                    //    });
                    //}
                    //entity.HdCapacity = vm.HdCapacity;

                    //if (entity.MonitorSize != vm.MonitorSize)
                    //{
                    //    db.AssetUpdateUploads.Add(new AssetUpdateUploads
                    //    {
                    //        Code = entity.Code,
                    //        Column = nameof(entity.CostValue),
                    //        From = entity.MonitorSize,
                    //        To = vm.MonitorSize,
                    //        CreatedBy = User.Identity.GetUserName()
                    //    });
                    //}
                    //entity.MonitorSize = vm.MonitorSize;

                    //if (entity.YearModel != vm.YearModel)
                    //{
                    //    db.AssetUpdateUploads.Add(new AssetUpdateUploads
                    //    {
                    //        Code = entity.Code,
                    //        Column = nameof(entity.CostValue),
                    //        From = entity.YearModel,
                    //        To = vm.YearModel,
                    //        CreatedBy = User.Identity.GetUserName()
                    //    });
                    //}
                    //entity.YearModel = vm.YearModel;

                    if (entity.Vendor != vm.Vendor)
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.Vendor),
                            From = entity.Vendor,
                            To = vm.Vendor,
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.Vendor = vm.Vendor;

                    entity.UpdatedBy = User.Identity.GetUserName();
                    entity.DateUpdated = DateTime.Now;
                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    await db.Database.ExecuteSqlCommandAsync(@"
                    EXEC dbo.spAssetStatusUpdate @createdBy;
                    EXEC dbo.spAssetUpdateUploads @createdBy;
                    EXEC dbo.spReplaceUploads @createdBy;
                    ", new SqlParameter("@createdBy", entity.UpdatedBy)
                    );
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpPost, Route("api/AssetApi/{id}/transfer")]
        public async Task<IHttpActionResult> Transfer(Guid id, [FromBody] TransferVm vm)
        {
            try
            {
                using (var db = new DataContext())
                {
                    //await db.Database.ExecuteSqlCommandAsync(@"
                    //DELETE FROM dbo.LocationTransferUploads WHERE CreatedBy = @User;
                    //DELETE FROM dbo.AssetUpdateUploads WHERE CreatedBy = @User;
                    //", new SqlParameter("@User", User.Identity.GetUserName()));
                    var entity = await db.Assets.FindAsync(id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing.");

                    if (string.IsNullOrEmpty(vm.Site) ||
                        string.IsNullOrEmpty(vm.TrackingNo) ||
                        string.IsNullOrEmpty(vm.TicketNo))
                        throw new Exception("All fields are required");

                    if (!entity.Site.Equals(vm.Site))
                    {
                        db.AssetUpdateUploads.Add(new AssetUpdateUploads
                        {
                            Code = entity.Code,
                            Column = nameof(entity.Site),
                            From = entity?.Site,
                            To = vm.Site,
                            CreatedBy = User.Identity.GetUserName()
                        });
                        db.LocationTransferUploads.Add(new LocationTransferUploads
                        {
                            Code = entity.Code,
                            Site = vm.Site,
                            TrackingNo = vm.TrackingNo,
                            TicketNo = vm.TicketNo,
                            CreatedBy = User.Identity.GetUserName()
                        });
                    }
                    entity.Site = vm.Site;

                    entity.UpdatedBy = User.Identity.GetUserName();
                    entity.DateUpdated = DateTime.Now;
                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    await db.Database.ExecuteSqlCommandAsync(@"
                    EXEC dbo.spLocationTransferUploads @createdBy;
                    EXEC dbo.spAssetUpdateUploads @createdBy;
                    ", new SqlParameter("@createdBy", entity.UpdatedBy)
                    );
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }


        //single
        [HttpPost, Route("api/AssetApi/{id}/assign")]
        public async Task<IHttpActionResult> Assign(Guid id, [FromBody] AssignAssetEmployeeUploads vm)
        {
            try
            {
                using (var db = new DataContext())
                {
                    await db.Database.ExecuteSqlCommandAsync(@"
                    DELETE FROM dbo.AssignAssetEmployeeUploads WHERE CreatedBy = @User;
                    ", new SqlParameter("@User", User.Identity.GetUserName()));
                    var entity = await db.Assets.FindAsync(id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing.");

                    vm.Hrid = Regex.Replace((vm.Hrid ?? string.Empty).Trim(), @"[^a-zA-Z0-9]", "").ToUpper().Trim();

                    if (string.IsNullOrEmpty(vm.WorkType) ||
                        string.IsNullOrEmpty(vm.Hrid))
                        throw new Exception("WORK TYPE and HRID are required");


                    switch (vm.WorkType)
                    {
                        case "WAS":
                            if (string.IsNullOrEmpty(vm.Floor) ||
                                string.IsNullOrEmpty(vm.WorkType))
                                throw new Exception("FLOOR and AREA are required for WAS");
                            break;
                        case "WAH":
                            if (string.IsNullOrEmpty(vm.ContactNo) ||
                                string.IsNullOrEmpty(vm.Address))
                                throw new Exception("ADDRESS and CONTACT # are required for WAH");
                            break;
                        default:
                            throw new Exception("WORK TYPE is required");
                    }

                    if (!string.IsNullOrEmpty(vm.Email) && !IsValidEmail(vm.Email))
                        throw new Exception("EMAIL is in incorrect format");
                    vm.Code = entity.Code;
                    vm.CreatedBy = User.Identity.GetUserName();
                    db.AssignAssetEmployeeUploads.Add(vm);
                    await db.SaveChangesAsync();
                    await db.Database.ExecuteSqlCommandAsync(@"
                    EXEC dbo.spAssignAssetEmployeeUploads @createdBy;
                    ", new SqlParameter("@createdBy", User.Identity.GetUserName())
                    );
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpPost, Route("api/AssetApi/{id}/remove")]
        public async Task<IHttpActionResult> RemoveAssigned(Guid id, [FromBody] TrackTicketVm vm)
        {
            try
            {
                using (var db = new DataContext())
                {
                    await db.Database.ExecuteSqlCommandAsync(@"
                    DELETE FROM dbo.RemoveAssignedAssetEmployeeUploads WHERE CreatedBy = @User;
                    ", new SqlParameter("@User", User.Identity.GetUserName()));
                    var entity = await db.Assets.FindAsync(id);
                    if (entity == null)
                        throw new Exception("Record is either deleted or not existing.");

                    db.RemoveAssignedAssetEmployeeUploads.Add(new RemoveAssignedAssetEmployeeUploads
                    {
                        CreatedBy = User.Identity.GetUserName(),
                        Code = entity.Code,
                        TrackingNo = vm.TrackingNo,
                        TicketNo = vm.TicketNo
                    });

                    await db.SaveChangesAsync();
                    await db.Database.ExecuteSqlCommandAsync(@"
                    EXEC dbo.spRemoveAssetAssignment @createdBy;
                    ", new SqlParameter("@createdBy", User.Identity.GetUserName())
                    );
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }

        [HttpGet, Route("api/AssetApi/{id}/history")]
        public async Task<IHttpActionResult> GetHistory(Guid id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var asset = await db.Assets.FindAsync(id);
                    if (string.IsNullOrEmpty(asset?.Code))
                        throw new Exception("Invalid Asset");
                    return Ok(db.DataTable(
                        "EXEC dbo.GetAssetHistory @Code", new SqlParameter("@Code", asset.Code)
                        ));

                    //var assetHistory = db.Database.ExecuteSqlCommandAsync(
                    //    "EXEC dbo.GetAssetHistory @Code",
                    //    new SqlParameter("@Code", asset.Code)
                    //);

                    //var additionalHistory = db.Database.ExecuteSqlCommandAsync(
                    //    "EXEC dbo.spAssetAssignmentHistory @serialNum",
                    //    new SqlParameter("@serialNum", asset.SerialNo)
                    //);

                    //var combinedResult = new
                    //{
                    //    AssetHistory = assetHistory,
                    //    AdditionalHistory = additionalHistory
                    //};

                    //return Ok(combinedResult);
                }
                //return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }
        #endregion

        #region StaticMethods

        public static bool IsValidEmail(string emailAddress)
        {
            try
            {
                var m = new MailAddress(emailAddress);
                Console.WriteLine(m);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        //bulkupload
        [HttpPost]
        [Route("api/AssetApi/fedexbulk")]
        public async Task<IHttpActionResult> AssetUploadsFedex(List<AssetUploads> model)
        {
            try
            {
                var user = "FedEx";
                using (var db = new DataContext())
                {
                    await db.Database
                        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssetUploads WHERE CreatedBy = '{user}'");
                }
                //check if there is an uploaded excel file
                var bulkData = new List<AssetUploads>();
                if (model != null)
                {
                    var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                    var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                    var tmpFileName = $"{user}-ASSET-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                    var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssetUploads/{phDateTime:MM-dd-yyyy}/{user}/");
                    Directory.CreateDirectory(path);
                    path = Path.Combine(path, tmpFileName);
                    //save the temporary file for reading of data - will be erased after

                    var errors = new List<string>();
                    try
                    {
                        List<string> serials;
                        List<Category> categories;
                        List<SubCategory> subcategories;
                        List<string> manufacturers;
                        List<string> statuses;

                        using (var db = new DataContext())
                        {
                            var userUploadHistory = new UserUploadHistory
                            {
                                UploadedBy = user,
                                UploadedTemplateFileDir = path
                            };
                            db.UserUploadHistory.Add(userUploadHistory);
                            await db.SaveChangesAsync();
                            serials = await db.Assets.Where(x => !string.IsNullOrEmpty(x.SerialNo)).Select(x => x.SerialNo).ToListAsync();
                            categories = await db.Categories.Where(x => x.IsActive).ToListAsync();
                            subcategories = await db.SubCategories.Where(x => x.IsActive).ToListAsync();
                            manufacturers = await db.Manufacturers.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                            statuses = await db.Statuses.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                        }

                        using (var db = new DataContext())
                        {
                            foreach (var s in model)
                            {
                                s.SerialNo = Regex.Replace(s.SerialNo.Trim(), @"[^a-zA-Z0-9-]", "").ToUpper().Trim();
                                s.SerialNo = Regex.Replace(s.SerialNo, @"[ ]{2,}", " ").ToUpper().Trim();
                                s.SerialNo = Regex.Replace(s.SerialNo.Trim(), @"[-]{2,}", "-").TrimEnd('-').TrimStart('-').ToUpper().Trim();



                                int.TryParse(ConfigurationManager.AppSettings["SERIAL_NUMBER_LEN"], out var serialMinLen);
                                if (s.SerialNo.Length < serialMinLen)
                                {
                                    errors.Add(s.SerialNo + $"Serial should be equal or greater than ({serialMinLen}) characters");
                                    continue;
                                }

                                if (!string.IsNullOrEmpty(serials.FirstOrDefault(x => x.ToUpper().Trim() == s.SerialNo.ToUpper().Trim())))
                                {
                                    errors.Add($"Serial [{s.SerialNo}] is already used");
                                    continue;
                                }

                                var tmpCategory = categories.FirstOrDefault(x => x.Code.ToUpper().Trim() == s.Category.ToUpper().Trim());
                                if (string.IsNullOrEmpty(tmpCategory?.Code))
                                {
                                    errors.Add($"{s.Category.ToUpper().Trim()}] is not a valid Category");
                                    continue;
                                }

                                if (string.IsNullOrEmpty(subcategories.FirstOrDefault(x => x.CategoryId == tmpCategory.Id && x.Code.ToUpper().Trim() == s.SubCategory.ToUpper().Trim())?.Code))
                                {
                                    errors.Add($"{s.SubCategory.ToUpper().Trim()}] is either invalid or not under [{tmpCategory.Code}] Category");
                                    continue;
                                }

                                if (string.IsNullOrEmpty(manufacturers.FirstOrDefault(x => x.ToUpper().Trim() == s.Manufacturer.ToUpper().Trim())))
                                {
                                    errors.Add($"{s.Manufacturer.ToUpper().Trim()}] is not valid");
                                    continue;
                                }

                                if (bulkData.Exists(x => x.SerialNo == s.SerialNo.Trim().ToUpper()))
                                {
                                    errors.Add($"Duplicate entry for Serial No. [{s.SerialNo.Trim().ToUpper()}]");
                                    continue;
                                }
                                else
                                {
                                    bulkData.Add(new AssetUploads
                                    {
                                        Description = s.Description.Trim().ToUpper(),
                                        SerialNo = s.SerialNo.Trim().ToUpper(),
                                        Category = s.Category.Trim().ToUpper(),
                                        SubCategory = s.SubCategory.Trim().ToUpper(),
                                        Manufacturer = s.Manufacturer.Trim().ToUpper(),
                                        Status = "IN-STORAGE",
                                        CreatedBy = user,
                                        Site = s.Site.Trim().ToUpper(),
                                        CostValue = s.CostValue,
                                        Ram = s.Ram,
                                        HdCapacity = s.HdCapacity,
                                        MonitorSize = s.MonitorSize,
                                        YearModel = s.YearModel,
                                        PurchaseOrder = s.PurchaseOrder,
                                        Vendor = s.Vendor
                                    });
                                }
                            }


                        }

                        //if (errors.Any())
                        //    throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                    }
                    catch (Exception e)
                    {
                        var error = ExceptionHandler.GetMessages(e);
                        throw new Exception(error);
                    }

                    if (bulkData.Any())
                    {
                        IEnumerable<IEnumerable<AssetUploads>> batches;
                        if (bulkData.Count > 1500)
                            batches = bulkData.Batch(1000);
                        else if (bulkData.Count > 1000)
                            batches = bulkData.Batch(500);
                        else if (bulkData.Count > 500)
                            batches = bulkData.Batch(250);
                        else if (bulkData.Count > 250)
                            batches = bulkData.Batch(125);
                        else if (bulkData.Count > 125)
                            batches = bulkData.Batch(60);
                        else if (bulkData.Count > 60)
                            batches = bulkData.Batch(30);
                        else if (bulkData.Count > 30)
                            batches = bulkData.Batch(15);
                        else batches = bulkData.Batch(5);
                        var tasks = batches.Select(b => b.ToList())
                            .Select(chunks => Task.Run(async () =>
                            {
                                try
                                {
                                    using (var context = new DataContext())
                                    {
                                        context.AssetUploads.AddRange(chunks);
                                        await context.SaveChangesAsync();
                                    }
                                }
                                catch (Exception exception)
                                {
                                    var error = ExceptionHandler.GetMessages(exception);
                                    Console.WriteLine(error);
                                }
                            }
                            )).ToList();
                        await Task.WhenAll(tasks);
                        using (var db = new DataContext())
                        {
                            await db.Database
                                .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssetUploads] @createdBy = '{user}'");
                        }
                    }

                }





                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }


        [HttpPost]
        [Route("api/AssetApi/fedexbulkAssig")]
        public async Task<IHttpActionResult> BulkUpload(List<TempTable> model)
        {
            try
            {
                var user = "FedEx";
                using (var db = new DataContext())
                {
                    await db.Database
                        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssignAssetEmployeeUploads WHERE CreatedBy = '{user}'");
                }
                //check if there is an uploaded excel file
                var file = HttpContext.Current.Request.Files["File"];
                var bulkData = new List<AssignAssetEmployeeUploads>();
                var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                var tmpFileName = $"{user}-DEPLOY-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssignAsset/{phDateTime:MM-dd-yyyy}/{user}/");
                Directory.CreateDirectory(path);
                path = Path.Combine(path, tmpFileName);
                //save the temporary file for reading of data - will be erased after
                //file.SaveAs(path);
                if (model != null)
                {
                    var errors = new List<string>();
                    try
                    {
                        List<string> refCodes;
                        List<string> hridListAsync;
                        using (var db = new DataContext())
                        {

                            var userUploadHistory = new UserUploadHistory
                            {
                                UploadedBy = user,
                                UploadedTemplateFileDir = path
                            };
                            db.UserUploadHistory.Add(userUploadHistory);
                            await db.SaveChangesAsync();
                            refCodes = await db
                                .Assets
                                .Where(x => x.IsActive)
                                .Select(x => x.Code).ToListAsync();
                            hridListAsync = await db
                                .BulkApiEmployee
                                .Where(x => x.IsActive)
                                .Select(x => x.ID).ToListAsync();
                        }



                        using (var db = new DataContext())
                        {

                            foreach (var s in model)
                            {
                                Assets _data = new Assets();
                                _data = db.Assets.Where(x => x.SerialNo == s.SerialNumber).FirstOrDefault();

                                bulkData.Add(new AssignAssetEmployeeUploads
                                {
                                    Area = "",
                                    Address = s.Address1.Trim().ToUpper(),
                                    WorkType = "WAH",
                                    Code = _data.Code,
                                    Hrid = s.HrId,
                                    ContactNo = s.PhoneNumber.Trim().ToUpper(),
                                    Floor = "",
                                    CreatedBy = user,
                                    Email = s.EmailAddress,
                                    TrackingNo = s.TrackingNumber,
                                    TicketNo = s.TrackingNumber
                                });

                            }



                        }



                        if (errors.Any())
                            throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));
                    }
                    catch (Exception e)
                    {
                        var error = ExceptionHandler.GetMessages(e);
                        throw new Exception(error);
                    }

                    if (bulkData.Any())
                    {
                        IEnumerable<IEnumerable<AssignAssetEmployeeUploads>> batches;
                        if (bulkData.Count > 1500)
                            batches = bulkData.Batch(1000);
                        else if (bulkData.Count > 1000)
                            batches = bulkData.Batch(500);
                        else if (bulkData.Count > 500)
                            batches = bulkData.Batch(250);
                        else if (bulkData.Count > 250)
                            batches = bulkData.Batch(125);
                        else if (bulkData.Count > 125)
                            batches = bulkData.Batch(60);
                        else if (bulkData.Count > 60)
                            batches = bulkData.Batch(30);
                        else if (bulkData.Count > 30)
                            batches = bulkData.Batch(15);
                        else batches = bulkData.Batch(5);
                        var tasks = batches.Select(b => b.ToList())
                            .Select(chunks => Task.Run(async () =>
                            {
                                try
                                {
                                    using (var context = new DataContext())
                                    {
                                        context.AssignAssetEmployeeUploads.AddRange(chunks);
                                        await context.SaveChangesAsync();
                                    }
                                }
                                catch (Exception exception)
                                {
                                    var error = ExceptionHandler.GetMessages(exception);
                                    Console.WriteLine(error);
                                }
                            }
                            )).ToList();
                        await Task.WhenAll(tasks);
                        using (var db = new DataContext())
                        {
                            await db.Database
                                .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssignAssetEmployeeUploads] @createdBy = '{user}'");
                        }
                    }
                }
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(ExceptionHandler.GetMessages(e));
            }
        }
    }
}