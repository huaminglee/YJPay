using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YJPay.Web
{
    public partial class ReqCallback : System.Web.UI.Page
    {
        private string prodid, orderid, tranid, resultno, mark, verifystring;
        protected void Page_Load(object sender, EventArgs e)
        {
            //话费充值产品编号
            prodid = Request.Form["prodid"];
            //网站订单号
            orderid = Request.Form["orderid"];

            //直冲接口平台订单号
            tranid = Request.Form["tranid"];
            //直冲结果编码，只有在订单提交成功2.3接口返回0000时才会返回信息
            resultno = Request.Form["resultno"];
            //预留字段
            mark = Request.Form["mark"];
            //验证字符
            verifystring = Request.Form["verifystring"];
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