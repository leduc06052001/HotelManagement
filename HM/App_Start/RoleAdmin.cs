using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HM.App_Start
{
    public class RoleAdmin : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            /*base.OnAuthorization(filterContext);*/
            var user = SessionConfig.GetUser();
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Dashboard",
                    action = "Login",
                    area = "Admin"
                }));
                return;
            }
            return;
        }

    }
}