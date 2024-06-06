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
    public class ChangeApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> BulkUpload()
        {
            try
            {
                var user = User.Identity.GetUserName();
                using (var db = new DataContext())
                {
                    await db.Database
                        .ExecuteSqlCommandAsync("DELETE FROM dbo.AssignedAssetChangeUploads WHERE CreatedBy = @CreatedBy"
                            , new SqlParameter("@CreatedBy", user)
                        );
                }
                //check if there is an uploaded excel file
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files["File"];
                    var bulkData = new List<AssignedAssetChangeUploads>();
                    if (file != null && file.ContentLength > 0)
                    {
                        var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                        var tmpFileName = $"{user}-CHANGE-{phDateTime:yyyyddMMHHmmss}{phDateTime.TimeOfDay.Milliseconds}.xlsx";
                        var path = HttpContext.Current.Server.MapPath($"~/uploads/files/AssignedAssetChange/{phDateTime:MM-dd-yyyy}/{user}/");
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
                                List<string> assignedRefCodes;
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
                                        .Select(x => x.Code)
                                        .ToListAsync();
                                    assignedRefCodes = await db
                                        .AssignAssetEmployee
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
                                        var fromCode = row.Cell(1).GetValue<string>();
                                        var toCode = row.Cell(2).GetValue<string>();
                                        var trackingNo = row.Cell(3).GetValue<string>();
                                        var returnTrackingNo = row.Cell(4).GetValue<string>();
                                        var ticketNo = row.Cell(5).GetValue<string>();
                                        if (string.IsNullOrEmpty(fromCode) || string.IsNullOrEmpty(toCode))
                                        {
                                            errors.Add($"Row[{i + 0}]::[FromRef#] and [ToRef#] is required");
                                            continue;
                                        }

                                        fromCode = Regex.Replace(fromCode.Trim(), @"[^\d]", "").ToUpper().Trim();
                                        toCode = Regex.Replace(toCode.Trim(), @"[^\d]", "").ToUpper().Trim();

                                        if (string.IsNullOrEmpty(fromCode) || string.IsNullOrEmpty(toCode))
                                        {
                                            errors.Add($"Row[{i + 0}]::[FromRef#] and [ToRef#] is required");
                                            continue;
                                        }

                                        if (string.IsNullOrEmpty(refCodes.FirstOrDefault(x => x.ToUpper().Trim() == fromCode.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 0}]::[FromRef#]-[{fromCode.ToUpper().Trim()}] is not valid");
                                            continue;
                                        }

                                        if (string.IsNullOrEmpty(refCodes.FirstOrDefault(x => x.ToUpper().Trim() == toCode.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 0}]::[ToRef#]-[{toCode.ToUpper().Trim()}] is not valid");
                                            continue;
                                        }

                                        if (string.IsNullOrEmpty(assignedRefCodes.FirstOrDefault(x => x.ToUpper().Trim() == fromCode.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 0}]::[FromRef#]-[{fromCode.ToUpper().Trim()}] is not assigned to an employee");
                                            continue;
                                        }

                                        if (!string.IsNullOrEmpty(assignedRefCodes.FirstOrDefault(x => x.ToUpper().Trim() == toCode.ToUpper().Trim())))
                                        {
                                            if (bulkData.Exists(x =>
                                                    x.FromCode.Trim().ToUpper().Equals(toCode.Trim().ToUpper())
                                                    && !x.ToCode.Trim().ToUpper().Equals(toCode.Trim().ToUpper())
                                                )
                                            )
                                            {
                                                var tmpBulk = bulkData
                                                    .Where(x =>
                                                        x.FromCode.Trim().ToUpper().Equals(toCode.Trim().ToUpper())
                                                        && !x.ToCode.Trim().ToUpper().Equals(toCode.Trim().ToUpper())
                                                    )
                                                    .ToList();
                                                if (tmpBulk.Count > 1)
                                                {
                                                    errors.Add($"Row[{i + 0}]::[ToRef#]-[{toCode.ToUpper().Trim()}] can only be assigned once");
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                errors.Add($"Row[{i + 0}]::[ToRef#]-[{toCode.ToUpper().Trim()}] is already assigned to an employee");
                                                continue;
                                            }
                                        }

                                        if (bulkData.Exists(x => x.FromCode.Trim().ToUpper() == fromCode.Trim().ToUpper()))
                                            errors.Add($"Row[{i}] Duplicate entry for [FromRef#] [{fromCode.Trim().ToUpper()}]");
                                        else if (bulkData.Exists(x => x.ToCode.Trim().ToUpper() == toCode.Trim().ToUpper()))
                                            errors.Add($"Row[{i}] Duplicate entry for [ToRef#] [{toCode.Trim().ToUpper()}]");
                                        else
                                            //add all the needed value sa list
                                            bulkData.Add(new AssignedAssetChangeUploads
                                            {
                                                CreatedBy = user,
                                                FromCode = fromCode,
                                                ToCode = toCode,
                                                TrackingNo = trackingNo,
                                                TicketNo = ticketNo,
                                                ReturnTrackingNo = returnTrackingNo,
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
                                IEnumerable<IEnumerable<AssignedAssetChangeUploads>> batches;
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
                                var tasks = batches
                                    .Select(b => b.ToList())
                                    .Select(chunks => Task.Run(async () =>
                                        {
                                            try
                                            {
                                                using (var context = new DataContext())
                                                {
                                                    context.AssignedAssetChangeUploads.AddRange(chunks);
                                                    await context.SaveChangesAsync();
                                                }
                                            }
                                            catch (Exception exception)
                                            {
                                                var error = ExceptionHandler.GetMessages(exception);
                                                errors.Add(error);
                                            }
                                        }
                                    )).ToList();
                                await Task.WhenAll(tasks);
                                if (errors.Any())
                                    throw new Exception(errors.Aggregate(string.Empty, (current, error) => current + (error + "\r\n")));

                                using (var db = new DataContext())
                                {
                                    await db.Database
                                        .ExecuteSqlCommandAsync("EXEC [dbo].[spAssignedAssetChangeUploads] @createdBy"
                                            , new SqlParameter("@createdBy", user)
                                        );
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