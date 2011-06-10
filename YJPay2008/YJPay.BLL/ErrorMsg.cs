using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJPay.BLL
{
    public class ErrorMsg
    {
        public static string GetError(string errCode)
        {
            switch (errCode)
            {
                case "0000": return "下单成功"; break;
                case "0001": return "	支付失败"; break;
                case "0999": return "	未开通直冲功能"; break;
                case "1000": return "	下单失败，未扣款，重新提交订单"; break;
                case "1001": return "	传入参数不完整"; break;
                case "1002": return "	验证摘要串验证失败"; break;
                case "1022": return "	充值号码格式错误 "; break;
                case "1003": return "	代理商验证失败"; break;
                case "1004": return "	代理商未激活"; break;
                case "1005": return "	没有对应充值产品"; break;
                case "1006": return "	系统异常，请稍后重试"; break;
                case "1007": return "	账户余额不足"; break;
                case "1016": return "	交易密码验证错误，请联系管理员"; break;
                case "0009": return "	支付失败，发货未成功"; break;
                case "1008": return "	此产品超出当天限额 ，请联系业务人员"; break;
                case "1009": return "	没有对应订单"; break;
                case "1010": return "	产品与手机号不匹配"; break;
                case "1011": return "	定单号不允许重复"; break;
                case "1012": return "	IP地址不符合要求"; break;
                case "1013": return "	运营商系统升级，暂不能充值"; break;
                case "1015": return "	无法查到对应号段"; break;
                case "1017": return "	话费新加的充值限制，同一个号码10秒内只能充值一次"; break;

            }
            return string.Empty;
        }
    }
}
