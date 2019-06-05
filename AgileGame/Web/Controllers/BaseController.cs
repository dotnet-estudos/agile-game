using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Util;

namespace Web.Controllers
{
   [Authorize]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {

            Global.Instance.GravaLog(filterContext.RouteData.ToString(), String.Format("ERROR: {0} \n INNER: {1} \n STACKTRACE: {2}",filterContext.Exception.Message, filterContext.Exception.InnerException?.Message, filterContext.Exception.StackTrace));

            base.OnException(filterContext);
        }
    }
}