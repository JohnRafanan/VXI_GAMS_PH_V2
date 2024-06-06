using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VXI_GAMS_US.Controllers
{
    /// <summary>
    /// Validates the session if User is enabled to access the pages
    /// </summary>
    public class SessionValidator : AuthorizeAttribute
    {
        private string _cntrlr = string.Empty;
        private string _act = string.Empty;
        private bool _hasSession;
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            _hasSession = false;
            var data = httpContext.Request.RequestContext.RouteData.Values;
            data.TryGetValue("controller", out var controller);
            _cntrlr = controller?.ToString() ?? string.Empty;
            data.TryGetValue("action", out var action);
            _act = action?.ToString() ?? string.Empty;
            
            if (string.IsNullOrEmpty(_cntrlr) || string.IsNullOrEmpty(_act) || !(httpContext.Session["Account"] is SessionUser e)) 
                return false;

            //means Session account exist
            _hasSession = true;
            
            //if ID is not null means user is valid and in API
            return !string.IsNullOrEmpty(e.HRID);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_hasSession)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Error",
                            action = "Index",
                            id = 602
                        })
                );
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Account",
                            action = "Login",
                            returnUrl = HttpContext.Current.Request.Path
                        })
                );
            }
        }
    }
}