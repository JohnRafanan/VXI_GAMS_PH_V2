using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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
    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    public class TemplateApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            try
            {
                var user = User.Identity.GetUserName();
                //check if there is an uploaded excel file
                if (HttpContext.Current.Request.Files.Count <= 0)
                    throw new Exception("No file uploaded");
                var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                var phDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);
                var path = HttpContext.Current.Server.MapPath($"~/uploads/files/templates/{phDateTime:MM-dd-yyyy}/{user}/");
                var templatePath = HttpContext.Current.Server.MapPath($"~/public/template");
                var templateBackupPath = HttpContext.Current.Server.MapPath($"~/public/template/backup/{phDateTime:MM-dd-yyyy}/{phDateTime:HHmmsstt}/{user}/");
                foreach (string file in HttpContext.Current.Request.Files)
                {
                    var httpPostedFile = HttpContext.Current.Request.Files[file];
                    if (httpPostedFile == null)
                        continue;
                    var tmpFileName = httpPostedFile.FileName;
                    tmpFileName = tmpFileName.Replace(".", $"_{phDateTime:HHmmsstt}.");
                    var tmpPath = Path.Combine(path, tmpFileName);
                    Directory.CreateDirectory(path);
                    httpPostedFile.SaveAs(tmpPath);
                    if (File.Exists(tmpPath))
                    {
                        var origTemplate = Path.Combine(templatePath, httpPostedFile.FileName);
                        var origTemplateBak = Path.Combine(templateBackupPath, httpPostedFile.FileName);
                        if (File.Exists(origTemplate))
                        {
                            using (var db = new DataContext())
                            {
                                var entity = new TemplateHistory
                                {
                                    DateUploaded = phDateTime,
                                    TemplateBackupFileDir = origTemplateBak,
                                    UploadedTemplateFileDir = tmpPath,
                                    UploadedBy = user
                                };
                                db.TemplateHistory.Add(entity);
                                await db.SaveChangesAsync();
                            }

                            Directory.CreateDirectory(templateBackupPath);
                            File.Copy(origTemplate, origTemplateBak, true);
                            File.Copy(tmpPath, origTemplate, true);
                        }
                        else throw new Exception("Invalid Template. The Template you are trying to upload does not match any existing Templates. Use the correct template file name");
                    }
                    Console.WriteLine(tmpPath);
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