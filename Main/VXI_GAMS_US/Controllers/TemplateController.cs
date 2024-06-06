using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VXI_GAMS_US.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Template
        public FileContentResult File(string name = "")
        {
            if(string.IsNullOrEmpty(name))
                name = "File.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/public/template/") + name);
            Response.AddHeader("Content-Disposition", "inline;filename=\"" + name + "\"");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}