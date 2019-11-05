using System.Web;

namespace WebUI.Helpers
{
    public static class GeneralHelper
    {
        public static string GetIp()
        {
            var visitorsIpAddr = string.Empty;

            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    visitorsIpAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostAddress))
                    visitorsIpAddr = HttpContext.Current.Request.UserHostAddress;
            }
            catch { }

            return visitorsIpAddr;
        }
    }
}