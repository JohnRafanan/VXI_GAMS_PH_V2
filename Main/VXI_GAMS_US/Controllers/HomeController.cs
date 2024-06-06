using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.DATA.Sql;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.VIEWS.Entities;
using static System.IO.File;

namespace VXI_GAMS_US.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return View();
            return RedirectToAction("Assets");
        }
        public ActionResult Assets()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Transactions()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult QrScanning()
        {
            return View();
        }

        public FileResult Template()
        {
            var contents = Server.MapPath("~/public/template");
            const string template = "AssetUploadingTemplate.xlsx";
            var path = $@"{contents}\{template}";
            var contentType = MimeMapping.GetMimeMapping(path);
            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("content-disposition", $"attachment;filename= {template}");
            Response.BufferOutput = false;
            return File(path, contentType);
        }

        private async Task<FileResult> CreateResult(string fileName, params SqlParameter[] parameters)
        {
            var file = await ExcelHelper.ToExcelAsync(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), parameters);
            var contentType = MimeMapping.GetMimeMapping(file);
            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("content-disposition", $"attachment;filename= {fileName}");
            Response.BufferOutput = false;
            return File(file, contentType);
        }

        [DeleteFile]
        public async Task<FileResult> Raw(string filters)
        {
            var filter = JsonConvert.DeserializeObject<RawFilter>(filters);
            filter.Filter = IsNull(filter.Filter);
            filter.Site = IsNull(filter.Site);
            filter.Status = IsNull(filter.Status);
            filter.Category = IsNull(filter.Category);
            filter.SubCategory = IsNull(filter.SubCategory);
            filter.Manufacturer = IsNull(filter.Manufacturer);
            filter.WorkType = IsNull(filter.WorkType);
            var sp = "EXEC [dbo].[spGetAssetsRaw] " +
                     $"@Filter = {filter.Filter}," +
                     $"@Site = {filter.Site}," +
                     $"@Status = {filter.Status}," +
                     $"@Category = {filter.Category}," +
                     $"@SubCategory = {filter.SubCategory}," +
                     $"@Manufacturer = {filter.Manufacturer}," +
                     $"@WorkType = {filter.WorkType}";
            var tmpFile = $"RAW-{DateTime.Now:MM.dd.yyyy_HH.mm.ss.fff_tt}.xlsx";
            return await CreateResult(tmpFile, new SqlParameter("Raw", sp));
        }

        [DeleteFile]
        public async Task<FileResult> RawHistory()
        {
            var sp = $"EXEC[dbo].[GetAssetHistoryReport]";
            var tmpFile = $"RAW-{DateTime.Now:MM.dd.yyyy_HH.mm.ss.fff_tt}.xlsx";
            return await CreateResult(tmpFile, new SqlParameter("Raw", sp));
        }

        public string IsNull(string filter)
        {
            var final = "NULL";
            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.Equals("All") || filter.Equals("NULL"))
                    final = "NULL";
                else
                    final = $"'{filter}'";
            }
            return final;
        }

        public class RawFilter
        {
            public string Filter { get; set; } = "NULL";
            public string Site { get; set; } = "NULL";
            public string Status { get; set; } = "NULL";
            public string Category { get; set; } = "NULL";
            public string SubCategory { get; set; } = "NULL";
            public string Manufacturer { get; set; } = "NULL";
            public string WorkType { get; set; } = "NULL";
        }

        public class DeleteFileAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                try
                {
                    filterContext.HttpContext.Response.Flush();
                    if (filterContext.Result is FilePathResult filePathResult)
                        Delete(filePathResult.FileName);
                }
                catch
                {
                    //IGNORE
                }
            }
        }
    }
}