using System.Web.Mvc;

namespace VXI_GAMS_US.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Template
        public FileContentResult Read(int type)
        {
            var name = "NotFound.pdf";
            switch (type)
            {
                case 1:
                    name = "UserGuide.pdf";
                    break;
                case 2:
                    name = "FAQ.pdf";
                    break;
                default:
                    break;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/public/kb/") + name);
            Response.AddHeader("Content-Disposition", "inline;filename=\"" + name + "\"");
            return File(fileBytes, "application/pdf");
        }
    }
}