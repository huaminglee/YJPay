using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace YJPay.BLL
{
    //sk 2011-04-25 工具库
    public class Utility
    {
        private static string encodingName = "utf-8";
        //POST方式请求
        public static string SendPost(string url)
        {
            string result, para = string.Empty;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            byte[] postData = null;
            if (url.IndexOf('?') > 0)
            {
                para = url.Substring(url.IndexOf('?'));
            }
            if (!string.IsNullOrEmpty(para))
            {
                postData = System.Text.Encoding.GetEncoding(encodingName).GetBytes(HttpUtility.UrlEncode(para));
                myRequest.ContentLength = postData.Length;
                Stream newStream = myRequest.GetRequestStream();
                // Send the data.
                newStream.Write(postData, 0, postData.Length);
                newStream.Close();
            }
            else
            {
                myRequest.ContentLength = 0;
            }
            // Get response
            try
            {
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.GetEncoding(encodingName));
                
                result = reader.ReadToEnd();
            }
            catch
            {
                result = "";
            }
            return result;
        }
        //GET方式请求
        public static string Get(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.MaximumAutomaticRedirections = 4;
            myRequest.Method = "GET";
            //myRequest.Timeout = 50000;
            try
            {
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.GetEncoding(encodingName));
                return reader.ReadToEnd();

            }
            catch { return string.Empty; }
        }
        //md5加密
        public static string MD5(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            string result = null;
            result = BitConverter.ToString(b);
            return result.Replace("-", "").ToLower();
        }
        //日志记录
        public static void logstr(string logFileName, string str)
        {
            string logdir;
            logdir = System.Web.HttpContext.Current.Server.MapPath(logFileName);
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(logdir, true);
                sw.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + DateTime.Now.Millisecond + "]" + str + "");
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        //日期比较
        public static int DateDiff(string dateInterval, DateTime dateTime1, DateTime dateTime2)
        {
            int dateDiff = 0;
            try
            {
                TimeSpan timeSpan = new TimeSpan(dateTime2.Ticks - dateTime1.Ticks);

                switch (dateInterval.ToLower())
                {
                    case "day":
                    case "d":
                        dateDiff = (int)timeSpan.TotalDays;
                        break;
                    case "hour":
                    case "h":
                        dateDiff = (int)timeSpan.TotalHours;
                        break;
                    case "minute":
                    case "n":
                        dateDiff = (int)timeSpan.TotalMinutes;
                        break;
                    case "second":
                    case "s":
                        dateDiff = (int)timeSpan.TotalSeconds;
                        break;
                    case "milliseconds":
                    case "ms":
                        dateDiff = (int)timeSpan.TotalMilliseconds;
                        break;
                    default:
                        dateDiff = (int)timeSpan.TotalMinutes;
                        break;
                }
            }
            catch
            {

            }
            return dateDiff;
        }
    }
}
