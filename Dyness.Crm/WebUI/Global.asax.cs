using Core.CrossCuttingConcerns.Security;
using Core.Utilities.MVC.Infrastructure;
using Services.DependencyResolvers.Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace WebUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(new BusinessModule()));
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "LangResources";
            DefaultModelBinder.ResourceClassKey = "LangResources";
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (!HttpContext.Current.Request.IsLocal)
            {
                var ex = Server.GetLastError().GetBaseException();

                var ip = HttpContext.Current.Request.UserHostAddress;

                var sb = new StringBuilder();

                sb.Append("Tarih : " + DateTime.Now + "\r\n");

                sb.Append("Url : " + Request.RawUrl.ToLower() + "\r\n");

                sb.Append("Ip : " + ip + "\r\n");

                sb.Append("Stack Trace:" + ex.StackTrace.Trim() + "\r\n");

                sb.Append("Error Message:" + ex.Message + "\r\n");

                var appPath = HttpContext.Current.Request.ApplicationPath;

                var path = HttpContext.Current.Request.MapPath(appPath + "\\AppError");

                if (Request.RawUrl.ToLower().IndexOf("webresource", StringComparison.Ordinal) != -1)
                {
                    Server.ClearError();
                }
                else
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    using (var sw = new StreamWriter(path + "\\" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt", true))
                    {
                        sw.WriteLine(sb + "\r\n------------------------");
                        sw.Flush();
                        sw.Close();
                    }

                    Server.ClearError();
                }

                HttpContext.Current.Response.Redirect("/Error/Hata");
            }
        }

        public override void Init()
        {
            PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
        }

        private void MvcApplication_PostAuthenticateRequest(object sender, System.EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        // if (Identity.SonGirisTarihi != null && Identity.CevrimIciDakika % 17 > 15)
                        // {
                        //    
                        // }

                        string kullaniciVeri = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        var roleDatas = kullaniciVeri.Split('|');

                        //var ipAddress = roleDatas[7];
                        //if (Request.UserHostAddress != ipAddress)
                        //{
                        //    Request.Cookies.Clear();
                        //    return;
                        //}

                        var roles = new List<string>();

                        var identity = new GenericPrincipal(new GenericIdentity(kullaniciVeri, "Forms"), roles.ToArray());



                        HttpContext.Current.User = identity;
                        Thread.CurrentPrincipal = identity;
                    } 
                    catch (Exception exc)
                    {
                        string logPath = Server.MapPath(string.Format("~/logs/kysLoginLogs/{0}/", DateTime.Now.ToString("dd-MM-yyyy")));
                        if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
                        var filePath = string.Format("{0}{1}.txt", logPath, DateTime.Now.ToString("HH-mm-ss"));
                        var loginParameters = "";
                        if (Thread.CurrentPrincipal.Identity != null && Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        {
                            loginParameters = Thread.CurrentPrincipal.Identity.Name;
                        }

                        string exceptionMessage = "Login Parameters: " + loginParameters + Environment.NewLine + exc.Message + Environment.NewLine + Environment.NewLine + exc.StackTrace;
                        exceptionMessage = exceptionMessage.Replace("\n", Environment.NewLine);
                        exceptionMessage = exceptionMessage.Replace("\t", "    ");

                        if (exc.InnerException != null)
                        {
                        innerExcLoop:
                            var innerException = exc.InnerException;
                            var innerExceptionMessage = innerException.Message;
                            exceptionMessage += innerExceptionMessage.Replace("\nInner Exception\n", Environment.NewLine);
                            exceptionMessage += innerExceptionMessage.Replace("\t", "    ");
                            if (innerException.InnerException != null)
                            {
                                var tempInner = innerException.InnerException;
                                innerException = tempInner;
                                goto innerExcLoop;
                            }
                        }
                        File.WriteAllText(filePath, exceptionMessage, Encoding.UTF8);
                    }
                }
            }
        }
    }
}
