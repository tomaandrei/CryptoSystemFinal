using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoSystemDissertation.Common
{
    public class ExitHttpsIfNotRequiredAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // abort if it's not a secure connection
            if (!filterContext.HttpContext.Request.IsSecureConnection) return;

            // abort if a [RequireHttps] attribute is applied to controller or action
            if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(RequireHttpsAttribute), true).Length > 0) return;
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequireHttpsAttribute), true).Length > 0) return;

            // abort if a [RetainHttps] attribute is applied to controller or action
            if (filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(RetainHttpsAttribute), true).Length > 0) return;
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(RetainHttpsAttribute), true).Length > 0) return;

            // abort if it's not a GET request - we don't want to be redirecting on a form post
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)) return;

            // redirect to HTTP
            string url = "http://" + filterContext.HttpContext.Request.Url.Host + ":51515" + filterContext.HttpContext.Request.RawUrl;
            filterContext.Result = new RedirectResult(url);
        }
    }
    public class RetainHttpsAttribute : FilterAttribute { }

    public class CryptoAuthorizeAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (SessionManager.CheckSession(SessionKeys.User) == true)
                return true;
            else
                return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectResult("~/Account/UnauthorizedUser");
        }
    }
}