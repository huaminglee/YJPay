using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace YJPay.Web
{
    public partial class Req : System.Web.UI.Page
    {
        private string prodid, agentid, backurl, returntype, orderid, mobilenum, source, mark, verifystring;
        private string GetProductId()
        { 
            string result="";
            XmlDocument xml = new XmlDocument();
            xml.Load(System.Web.HttpContext.Current.Server.MapPath("Product.xml"));
            XmlElement root = xml.DocumentElement;
            XmlNodeList nodeList = root.SelectNodes(@"//products/product[@value='浙江']");
            foreach (XmlNode node in nodeList) {
                if (node.SelectSingleNode(@"../product[@name='prodIsptype']").Attributes["value"].Value == "联通" && node.SelectSingleNode(@"../product[@name='prodContent']").Attributes["value"].Value == "300" && node.SelectSingleNode(@"../product[@name='prodType']").Attributes["value"].Value == "移动电话")
                {
                    result=(node.SelectSingleNode(@"../product[@name='prodId']").Attributes["value"].Value);
                }
            }
            return result;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string tprodId = GetProductId();
            if (string.IsNullOrEmpty(tprodId))
            { 
                //无产品
            }
            //产品id
            prodid = tprodId;
            //代理商id,平台的登录名
            agentid = YJPay.BLL.API.AgentmerhantId;
            //请求返回url
            //backurl = "http://localhost:2358/ReqCallback.aspx";
            backurl = "";
            //返回方式 1：post方式 2：xml方式
            //returntype = "1";
            returntype = "2";
            //订单号码
            orderid = "CZ" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + new System.Random().Next(100, 999).ToString();
            //充值号码
            mobilenum = "13216723455";
            //代理商来源
            source = "esales";
            //预留字段，原样返回
            mark = "";
            //验证字段
            verifystring = YJPay.BLL.Utility.MD5(string.Format("prodid={0}&agentid={1}&backurl={2}&returntype={3}&orderid={4}&mobilenum={5}&source={6}&mark={7}&merchantKey={8}", prodid, agentid, backurl, returntype, orderid, mobilenum, source, mark, YJPay.BLL.API.AgentkeyValue));

            try
            {
                string result = YJPay.BLL.API.Req(prodid, agentid, backurl, returntype, orderid, mobilenum, source, mark, verifystring);
                if(result.Length==4)
                {

                    YJPay.BLL.Utility.logstr("yjpay-agent.log", "[请求返回的信息]:" + YJPay.BLL.ErrorMsg.GetError(result) + "]");
                }
                Result(result);
                //Response.Write(result);
                //Response.End();
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        private void Result(string result)
        {
            //xml返回数据处理
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            //if (xml.parseError.errorCode != 0)
            {
                XmlElement root = xml.DocumentElement;
                //话费充值产品编号
                string prodid = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='prodid']").Attributes["value"].Value);
                //网站订单号
                string orderid = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='orderid']").Attributes["value"].Value);
                //直冲接口平台订单号
                string tranid = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='tranid']").Attributes["value"].Value);
                //直冲结果编码，只有在订单提交成功2.3接口返回0000时才会返回信息
                string resultno = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='resultno']").Attributes["value"].Value);
                //预留字段
                string mark = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='mark']").Attributes["value"].Value);
                //验证字符
                string verifystring = HttpUtility.UrlDecode(root.SelectSingleNode(@"//item[@name='verifystring']").Attributes["value"].Value);

                string reqverify = HttpUtility.UrlDecode(verifystring, System.Text.Encoding.UTF8);

                string localverify = YJPay.BLL.Utility.MD5(string.Format("prodid={0}&orderid={1}&tranid={2}&resultno={3}&mark={4}&merchantKey={5}", prodid, orderid, tranid, resultno, mark, YJPay.BLL.API.AgentkeyValue));

                if (reqverify == localverify)//验证处理
                {
                    switch (resultno)
                    {
                        case "0000"://下单成功处理
                            break;
                        default:
                            YJPay.BLL.Utility.logstr("yjpay-agent.log", "[请求返回的信息]:" + YJPay.BLL.ErrorMsg.GetError(resultno) + "]");
                            break;
                    }
                }
            }

        }
    }
}