using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;

namespace YJPay.Web
{
    public partial class Search : System.Web.UI.Page
    {
        private string agentid, source, mobilenum, verifystring;
        private string backurl, returntype, orderid;
        protected void Page_Load(object sender, EventArgs e)
        {
            string t = "agentid=xiangtao12@19pay.com.cn&source=esales&merchantKey=536fb6c20142b046180b2c40c2b4d2125d4e";
            //Response.Write(YJPay.BLL.Utility.MD5(t));
            //Response.End();
            string action = Request.QueryString["action"];
            switch (action)
            {
                case "order":
                    OrderSearch();//订单查询请求
                    break;
                case "product":
                    ProductSearch();
                    break;
                case "mobile":
                    MobileSearch();//号段查询请求
                    break;
            }

        }
        private void ProductSearch()
        {
            Response.Charset = "GB2312";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            string result = "";
            agentid = YJPay.BLL.API.AgentmerhantId;
            source = "esales";
            verifystring = YJPay.BLL.Utility.MD5(string.Format("agentid={0}&source={1}&merchantKey={2}", agentid, source, YJPay.BLL.API.AgentkeyValue));
            //日期更新产品列表
            DateTime dt = File.GetLastWriteTime(System.Web.HttpContext.Current.Server.MapPath("Product.xml"));
            if (YJPay.BLL.Utility.DateDiff("d", dt, DateTime.Now) >= 2)
            {
                //if(result=="1002")//验证码失败
                result = HttpUtility.UrlDecode(YJPay.BLL.API.ProductSearch(agentid, source, verifystring), System.Text.Encoding.GetEncoding("utf-8"));
                File.WriteAllText(HttpContext.Current.Server.MapPath("Product.xml"), result, System.Text.Encoding.GetEncoding("gb2312"));
            }
            else
                result = File.ReadAllText(HttpContext.Current.Server.MapPath("Product.xml"),System.Text.Encoding.GetEncoding("gb2312"));
            Response.Write(result);
            
        }
        private void OrderSearch()
        {
            agentid = YJPay.BLL.API.AgentmerhantId;
            //backurl = "http://localhost:2358/SearchCallBack.aspx";
            //returntype = "1";
            backurl = "";
            returntype = "2";
            orderid = Request.QueryString["orderid"];   // "CZ20110503195413359142"
            source = "esales";
            verifystring = YJPay.BLL.Utility.MD5(string.Format("agentid={0}&backurl={1}&returntype={2}&orderid={3}&source={4}&merchantKey={5}", agentid, backurl, returntype, orderid, source, YJPay.BLL.API.AgentkeyValue));
            string result = YJPay.BLL.API.OrderSearch(agentid, backurl, returntype, orderid, source, verifystring);
            //出错处理
            if (result.Length == 4)
            {
                YJPay.BLL.Utility.logstr("yjpay-agent.log", "[请求返回的信息]:" + YJPay.BLL.ErrorMsg.GetError(result) + "]");
                return;
            }
            //xml返回数据处理
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            //if (xml.parseError.errorCode != 0)
            {
                XmlElement root = xml.DocumentElement;
                string rorderid = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='orderid']").Attributes["value"].Value);
                string rresultno = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='resultno']").Attributes["value"].Value);
                string rfinishmoney = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='finishmoney']").Attributes["value"].Value);
                string rverifystring = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='verifystring']").Attributes["value"].Value);

                string localVerify = YJPay.BLL.Utility.MD5(string.Format("orderid={0}&resultno={1}&finishmoney={2}&merchantKey={3}", orderid, rresultno, rfinishmoney, YJPay.BLL.API.AgentkeyValue));
                //验证摘要串
                if (rverifystring == localVerify)
                {
                    switch (rresultno)
                    {
                        case "1"://正在处理
                            break;
                        case "2"://充值成功
                            break;
                        case "3"://部分成功
                            break;
                        case "4"://充值失败
                            break;
                    }
                }
            }
            //Response.Write(result);
        }

        private void MobileSearch()
        {
            //isptype：运营商名称（移动、联通、电信）
            //provincename：省份名称
            //citycode：地市代码
            //detail：描述
            try
            {
                agentid = YJPay.BLL.API.AgentmerhantId;
                source = "esales";
                mobilenum = Request.QueryString["mobilenum"];
                verifystring = YJPay.BLL.Utility.MD5(string.Format("agentid={0}&source={1}&mobilenum={2}&merchantKey={3}", agentid, source, mobilenum, YJPay.BLL.API.AgentkeyValue));

                string result = YJPay.BLL.API.MobileSearch(agentid, source, mobilenum, verifystring);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                XmlElement root = xml.DocumentElement;
                string isptype = HttpUtility.UrlDecode(root.SelectSingleNode(@"//mobile[@name='isptype']").Attributes["value"].Value);
                string provincename = HttpUtility.UrlDecode(root.SelectSingleNode(@"//mobile[@name='provincename']").Attributes["value"].Value);
                string citycode = root.SelectSingleNode(@"//mobile[@name='citycode']").Attributes["value"].Value;
                string detail = HttpUtility.UrlDecode(root.SelectSingleNode(@"//mobile[@name='detail']").Attributes["value"].Value);
                Response.Write("{\"isptype\":\"" + isptype + "\",\"provincename\":\"" + provincename + "\",\"citycode\":\"" + citycode + "\",\"detail\":\"" + detail + "\"}");
                Response.End();

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
    }
}