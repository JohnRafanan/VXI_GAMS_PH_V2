using System;
using System.Collections.Generic;
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
    public class AssetStatusApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> AssetStatusUploads()
        {
            try
            {
                var user = User.Identity.GetUserName();
                using (var db = new DataContext())
                {
                    await db.Database
                        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssetStatusUploads WHERE CreatedBy = '{user}'");
                }
                //check if there is an uploaded excel file
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files["File"];
                    var bulkData = new List<AssetStatusUploads>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                        var tmpFileName = $"{user}-STATUS-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                        var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssetStatus/{phDateTime:MM-dd-yyyy}/{user}/");
                        Directory.CreateDirectory(path);
                        path = Path.Combine(path, tmpFileName);
                        //save the temporary file for reading of data - will be erased after
                        file.SaveAs(path);
                        if (File.Exists(path))
                        {
                            var errors = new List<string>();
                            try
                            {
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
                                    statuses = await db.Statuses.Where(x => x.IsActive).Select(x => x.Code).ToListAsync();
                                }
                                using (var wb = new XLWorkbook(path))
                                {
                                    var sheet = wb.Worksheet(1);//default do not change
                                    var total = sheet.RowsUsed().Count() + 1;//default do not change
                                    for (var i = 2; i < total; i++)//default do not change
                                    {
                                        var row = sheet.Row(i);
                                        var code = row.Cell(1).GetValue<string>();
                                        var status = row.Cell(2).GetValue<string>();
                                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(status))
                                        {
                                            errors.Add($"Row[{i + 1}]::All fields are required");
                                            continue;
                                        }

                                        code = Regex.Replace(code.Trim(), @"[^\d]", "").ToUpper().Trim();

                                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(status))
                                        {
                                            errors.Add($"Row[{i + 1}]::All fields are required");
                                            continue;
                                        }

                                        if (string.IsNullOrEmpty(statuses.FirstOrDefault(x => x.ToUpper().Trim() == status.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 1}]::[{status}] is not a valid Status");
                                            continue;
                                        }

                                        if (bulkData.Exists(x => x.Code.Trim().ToUpper() == code.Trim().ToUpper()))
                                        {
                                            errors.Add($"Row[{i}] Duplicate entry for Ref.No. [{code.Trim().ToUpper()}]");
                                            continue;
                                        }
                                        //add all the needed value sa list
                                        bulkData.Add(new AssetStatusUploads
                                        {
                                            Code = code.Trim().ToUpper(),
                                            Status = status.Trim().ToUpper(),
                                            CreatedBy = user
                                        });
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
                                IEnumerable<IEnumerable<AssetStatusUploads>> batches;
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
                                                    context.AssetStatusUploads.AddRange(chunks);
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
                                        .ExecuteSqlCommandAsync($"EXEC [dbo].[spAssetStatusUpdate] @createdBy = '{user}'");
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
    }
}