using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Core.General
{
    public static class SmsHelper
    {
        public static Tuple<bool, string, int> SmsGonder(string header, string phoneNumber, string message)
        {
            if (HttpContext.Current.Request.IsLocal)
                return new Tuple<bool, string, int>(true, "-1", 0);

            try
            {
                var ayarlar = AyarlarService.Get();

                var xml = $"<sms><username>{ayarlar.SmsUserName}</username><password>{ayarlar.SmsPassword}</password><header>{header}</header><validity>2880</validity><message><gsm><no>{phoneNumber}</no></gsm><msg><![CDATA[{message}]]></msg></message></sms>";
                var url = ayarlar.SmsUrl;

                var response = string.Empty;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                var requestBytes = Encoding.UTF8.GetBytes(xml);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "text/xml;charset=utf-8";
                httpWebRequest.ContentLength = requestBytes.Length;

                using (var requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                    requestStream.Close();

                    using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.Default))
                    {
                        response = streamReader.ReadToEnd();

                        streamReader.Close();
                        httpWebResponse.Close();
                    }
                }

                var splitted = response.Split(' ');

                if (splitted.Length > 1)
                {
                    return new Tuple<bool, string, int>(splitted[0] == "00", splitted[0], Convert.ToInt32(splitted[1]));
                }

                return new Tuple<bool, string, int>(false, response, 0);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, int>(false, ex.Message, 0);
            }
        }
    }
}
