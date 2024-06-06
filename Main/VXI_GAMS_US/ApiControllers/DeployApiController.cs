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
    public class DeployApiController : ApiController
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
                        .ExecuteSqlCommandAsync($"DELETE FROM dbo.AssignAssetEmployeeUploads WHERE CreatedBy = '{user}'");
                }
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
                                        var code = row.Cell(1).GetValue<string>();
                                        var workType = row.Cell(2).GetValue<string>();
                                        var hrid = row.Cell(3).GetValue<string>();
                                        var floor = row.Cell(4).GetValue<string>();
                                        var area = row.Cell(5).GetValue<string>();
                                        var address = row.Cell(6).GetValue<string>();
                                        var contactNo = row.Cell(7).GetValue<string>();
                                        var email = row.Cell(8).GetValue<string>();
                                        var trackingNo = row.Cell(9).GetValue<string>();
                                        var ticketNo = row.Cell(10).GetValue<string>();
                                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(hrid))
                                        {
                                            errors.Add($"Row[{i + 1}]::[Code], [WorkType], [Hrid] fields are required");
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

                                        if (string.IsNullOrEmpty(refCodes.FirstOrDefault(x => x.ToUpper().Trim() == code.ToUpper().Trim())))
                                        {
                                            errors.Add($"Row[{i + 1}]:: Ref.No [{code}] is not valid");
                                            continue;
                                        }

                                        if (bulkData.Exists(x => x.Code.Trim().ToUpper() == code.Trim().ToUpper()))
                                            errors.Add($"Row[{i}] Duplicate entry for Ref.No [{code.Trim().ToUpper()}]");
                                        else
                                            //add all the needed value sa list
                                            bulkData.Add(new AssignAssetEmployeeUploads
                                            {
                                                Area = area?.Trim().ToUpper(),
                                                Address = address?.Trim().ToUpper(),
                                                WorkType = workType?.Trim().ToUpper(),
                                                Code = code?.Trim().ToUpper(),
                                                Hrid = hrid?.Trim().ToUpper(),
                                                ContactNo = contactNo?.Trim().ToUpper(),
                                                Floor = floor?.Trim().ToUpper(),
                                                CreatedBy = user,
                                                Email = email,
                                                TrackingNo = trackingNo,
                                                TicketNo = ticketNo
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