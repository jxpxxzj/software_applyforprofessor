using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Webdisk.Backend.Helpers
{
    public static class ResponseHelper
    {
        public static string GetJson(string url)
        {
            var returnText = GetHtml(url);
            if (returnText.Contains("errcode"))
                return "";

            return returnText;
        }
        public static string GetHtml(string url, Encoding encoder)
        {
            var wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = encoder;
            var returnText = wc.DownloadString(url);
            return returnText;
        }
        public static string GetHtml(string url)
        {
            return GetHtml(url, Encoding.UTF8);
        }

        public static HttpResponseMessage GetNonJsonStringData(string raw, string content_type = "text/html")
        {
            var result = new HttpResponseMessage { Content = new StringContent(raw, Encoding.GetEncoding("UTF-8"), content_type) };
            return result;
        }

        public static HttpResponseMessage GetJsonp(object obj, string callback)
        {
            return GetNonJsonStringData(string.Format("{0}({1})", callback, JsonConvert.SerializeObject(obj)), "application/json");
        }

        public static int ConvertDateToTimestamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

    }
}