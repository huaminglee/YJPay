using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YJPay.Web
{
    //订单查询请求返回
    public partial class SearchCallBack : System.Web.UI.Page
    {
        private string orderid, resultno, finishmoney, verifystring;
        protected void Page_Load(object sender, EventArgs e)
        {
            orderid = Request.Form["orderid"];
            resultno = Request.Form["resultno"];
            finishmoney = Request.Form["finishmoney"];
            verifystring = Request.Form["verifystring"];

            string localVerify = YJPay.BLL.Utility.MD5(string.Format("orderid={0}&resultno={1}&finishmoney={2}&merchantKey={3}", orderid, resultno, finishmoney, YJPay.BLL.API.AgentkeyValue));
            //验证摘要串
            if (verifystring == localVerify)
            {
                switch (resultno)
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
    }
}