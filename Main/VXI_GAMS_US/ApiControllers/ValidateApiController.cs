using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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

namespace VXI_GAMS_US.ApiControllers
{
    public class ValidateApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> BulkUpload()
        {
            try
            {
                var user = User.Identity.GetUserName();
                //using (var db = new DataContext())
                //{
                //    await db.Database
                //        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssignAssetEmployeeUploads WHERE CreatedBy = '{user}'");
                //}
                //check if there is an uploaded excel file
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files["File"];
                    var bulkData = new List<AssignAssetEmployeeUploads>();
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
                                using (var wb = new XLWorkbook(path))
                                {
                                    hridListAsync = hridListAsync.Distinct().OrderBy(x => x).ToList();
                                    var sheet = wb.Worksheet(1);//default do not change
                                    var total = sheet.RowsUsed().Count() + 1;//default do not change
                                    for (var i = 2; i < total; i++)//default do not change
                                    {
                                        var row = sheet.Row(i);
                                      
                                        var hrid = row.Cell(1).GetValue<string>();
                                      
                                        if (string.IsNullOrEmpty(hrid))
                                        {
                                            errors.Add($"Row[{i + 1}]::[Hrid] fields are required");
                                            continue;
                                        }
                                                                           
                                        hrid = Regex.Replace(hrid.Trim(), @"[^a-zA-Z0-9]", "").ToUpper().Trim();

                                        if (string.IsNullOrEmpty(hridListAsync.FirstOrDefault(x => x.ToUpper().Trim() == hrid.ToUpper().Trim())))
                                        {
                                            errors.Add($"Unable to find Row[{i + 1}]:: HRID [{hrid}] in HRIS");
                                            continue;
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