using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YJPay.Web
{
    public partial class Notify : System.Web.UI.Page
    {
        private string orderid, status, ordermoney, verifystring;
        protected void Page_Load(object sender, EventArgs e)
        {
            orderid = Request["orderid"];
            status = Request["status"];
            ordermoney = Request["ordermoney"];
            verifystring = Request["verifystring"];
            string localverify = YJPay.BLL.Utility.MD5(string.Format("orderid={0}&status={1}&ordermoney={2}&merchantKey={3}", orderid, status, ordermoney, YJPay.BLL.API.AgentkeyValue));
            if (localverify == verifystring)//验证
            {
                if (status == "2")
                {
                    //充值成功

                }
                else if (status == "3")
                {
                    //部分成功
                }
                else if (status == "4")
                {
                    //充值失败
                }

                Response.Write(status);
                Response.End();
            }
            
        }
    }
}