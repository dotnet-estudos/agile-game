using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Domain.Util;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IniciaBanco();

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                Global.Instance.GravaLog("Erro Capturado", String.Format("ERROR: {0} \n INNER: {1} \n STACKTRACE: {2}", exception.Message, exception.InnerException?.Message, exception.StackTrace));
            }
        }

        public void IniciaBanco()
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "AgileGame.mdf")))
            {
                if (!Directory.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data")))
                {

                    //Criamos um com o nome folder
                    Directory.CreateDirectory(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data"));

                }

                string sourceFileMdf = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "AgileGame.mdf");
                string destFileMdf = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "AgileGame.mdf");


                string sourceFileLog = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "AgileGame_log.ldf");
                string destFileLog = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "AgileGame_log.ldf");

                //System.IO.File.Copy(sourceFileMdf, destFileMdf, true);
                //System.IO.File.Copy(sourceFileLog, destFileLog, true);
            }
        }
    }
}
