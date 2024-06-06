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
    public class TransferApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> LocationTransferUploads()
        {
            try
            {
                var user = User.Identity.GetUserName();
                using (var db = new DataContext())
                {
                    await db.Database
                        .ExecuteSqlCommandAsync($"DELETE FROM dbo.LocationTransferUploads WHERE CreatedBy = '{user}'");
                }
                //check if there is an uploaded excel file
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files["File"];
                    var bulkData = new List<LocationTransferUploads>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                        var tmpFileName = $"{user}-TRANSFER-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                        var path = HttpContext.Current.Server.MapPath($"~/uploads/files/LocationTransfer/{phDateTime:MM-dd-yyyy}/{user}/");
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
                                }
                                using (var wb = new XLWorkbook(path))
                                {
                                    var sheet = wb.Worksheet(1);//default do not change
                                    var total = sheet.RowsUsed().Count() + 1;//default do not change
                                    for (var i = 2; i < total; i++)//default do not change
                                    {
                                        var row = sheet.Row(i);
                                        var code = row.Cell(1).GetValue<string>();
                                        var site = row.Cell(2).GetValue<string>();
                                        var trackingNo = row.Cell(3).GetValue<string>();
                                        var ticketNo = row.Cell(4).GetValue<string>();
                                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(site))
                                        {
                                            errors.Add($"Row[{i + 1}]::All fields are required");
                                            continue;
                                        }

                                        code = Regex.Replace(code.Trim(), @"[^\d]", "").ToUpper().Trim();

                                        if (string.IsNullOrEmpty(code))
                                        {
                                            errors.Add($"Row[{i + 1}]::[Code] is required");
                                            continue;
                                        }

                                        if (string.IsNullOrEmpty(refCodes.FirstOrDefault(x => x.ToUpper().Trim() == code.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 1}]::Ref.No. [{code.ToUpper().Trim()}] is not valid");
                                            continue;
                                        }

                                        if (bulkData.Exists(x => x.Code.Trim().ToUpper() == code.Trim().ToUpper()))
                                            errors.Add($"Row[{i}] Duplicate entry for Ref.No. [{code.Trim().ToUpper()}]");
                                        else
                                            //add all the needed value sa list
                                            bulkData.Add(new LocationTransferUploads
                                            {
                                                Code = code?.Trim().ToUpper(),
                                                Site = site?.Trim(),
                                                CreatedBy = user,
                                                TicketNo = ticketNo,
                                                TrackingNo = trackingNo
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
                                IEnumerable<IEnumerable<LocationTransferUploads>> batches;
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
                                                    context.LocationTransferUploads.AddRange(chunks);
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
                                        .ExecuteSqlCommandAsync($"EXEC [dbo].[spLocationTransferUploads] @createdBy = '{user}'");
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