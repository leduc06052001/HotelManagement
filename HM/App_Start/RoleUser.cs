using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HM.App_Start
{
    public class RoleUser : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            /*base.OnAuthorization(filterContext);*/
            var User = SessionConfig.GetUser();
            if(User == null)
            {
                //user null thì chuyển hướng về trang chủ
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                new{
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