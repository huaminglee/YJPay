using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJPay.BLL
{
    //sk 2011-04-25 19Pay接口
    public abstract class API
    {
        private static string agentAuthorizationURL = System.Configuration.ConfigurationManager.AppSettings["agentAuthorizationURL"];

        private static string agentkeyValue = System.Configuration.ConfigurationManager.AppSettings["agentkeyValue"];
        private static string queryOrderURL = System.Configuration.ConfigurationManager.AppSettings["queryOrderURL"];
        private static string agentmerhantId = System.Configuration.ConfigurationManager.AppSettings["agentmerhantId"];
        private static string queryTelURL = System.Configuration.ConfigurationManager.AppSettings["queryTelURL"];
        private static string queryProductURL = System.Configuration.ConfigurationManager.AppSettings["queryProductURL"];

        public static string AgentkeyValue
        {
            get { return API.agentkeyValue; }
            set { API.agentkeyValue = value; }
        }
        public static string AgentAuthorizationURL
        {
            get { return API.agentAuthorizationURL; }
            set { API.agentAuthorizationURL = value; }
        }
        public static string QueryOrderURL
        {
            get { return API.queryOrderURL; }
            set { API.queryOrderURL = value; }
        }
        public static string QueryTelURL
        {
            get { return API.queryTelURL; }
            set { API.queryTelURL = value; }
        }
        public static string AgentmerhantId
        {
            get { return API.agentmerhantId; }
            set { API.agentmerhantId = value; }
        }
        public static string QueryProductURL
        {
            get { return API.queryProductURL; }
            set { API.queryProductURL = value; }
        }
        //直冲请求
        public static string Req(string prodid, string agentid, string backurl, string returntype, string orderid, string mobilenum, string source, string mark, string verifystring)
        {
            string verify = YJPay.BLL.Utility.MD5(string.Format("prodid={0}&agentid={1}&backurl={2}&returntype={3}&orderid={4}&mobilenum={5}&source={6}&mark={7}&merchantKey={8}", prodid, agentid, backurl, returntype, orderid, mobilenum, source, mark, YJPay.BLL.API.AgentkeyValue));

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("?prodid={0}", prodid);
            sb.AppendFormat("&agentid={0}", YJPay.BLL.API.AgentmerhantId);
            sb.AppendFormat("&backurl={0}", backurl);
            sb.AppendFormat("&returntype={0}", returntype);
            sb.AppendFormat("&orderid={0}", orderid);
            sb.AppendFormat("&mobilenum={0}", mobilenum);
            sb.AppendFormat("&source={0}", source);
            sb.AppendFormat("&mark={0}", mark);
            sb.AppendFormat("&verifystring={0}", verify);

            return Utility.SendPost(API.AgentAuthorizationURL + sb.ToString());
        }
        //订单查询
        public static string OrderSearch(string agentid, string backurl, string returntype, string orderid, string source, string verifystring)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("?agentid={0}", YJPay.BLL.API.AgentmerhantId);
            sb.AppendFormat("&backurl={0}", backurl);
            sb.AppendFormat("&returntype={0}", returntype);
            sb.AppendFormat("&orderid={0}", orderid);
            sb.AppendFormat("&source={0}", source);
            sb.AppendFormat("&verifystring={0}", verifystring);
            return Utility.SendPost(API.QueryOrderURL + sb.ToString());
        }
        //号段查询
        public static string MobileSearch(string agentid, string source, string mobilenum, string verifystring)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("?agentid={0}", agentid);
            sb.AppendFormat("&source={0}", source);
            sb.AppendFormat("&mobilenum={0}", mobilenum);
            sb.AppendFormat("&verifystring={0}", verifystring);

            return Utility.SendPost(API.QueryTelURL + sb.ToString());
        }
        //产品查询
        public static string ProductSearch(string agentid, string source, string verifystring)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("?agentid={0}", agentid);
            sb.AppendFormat("&source={0}", source);
            sb.AppendFormat("&verifystring={0}", verifystring);

            return Utility.SendPost(API.QueryProductURL + sb.ToString());
        }

    }
}
