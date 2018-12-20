using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessComponent;
using WxPayAPI;

namespace GroupOver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                LogHelp.WriteLog("begin::团购定时任务");
                //判断已经结束的团购
                OrderBC bc = new OrderBC();
                var entitys = bc.GetGroupBuyConfig();
                foreach (var item in entitys)
                {
                    var needCount = bc.GetGroupBuyConfigByClassId(item.ClassId).NeedCount;
                    LogHelp.WriteLog("needCount:::" + needCount.ToString());
                    var group = bc.GetGroupBuyByClassId(item.ClassId);
                    if (group != null)
                    {
                        if (needCount != group.NowCount)//人数不满足，退款
                        {
                            LogHelp.WriteLog("人数不满足，退款");
                            //找到付款订单
                            var members = bc.GetGroupBuyMember(group.GroupBuyId);
                            if (members != null && members.Count > 0)
                            {
                                foreach (var i in members)
                                {
                                    var order = bc.GetOrderByOpenIdandClassId(i.openId, group.ClassId);
                                    //根据订单退款
                                    AppSetting setting = new AppSetting();
                                    WxPayClient client = new WxPayClient();
                                    WxPayData data = new WxPayData();


                                    string RefundNumber = string.Format("{0}{1}", order.OrderNo.ToString(), DateTime.Now.ToString("fff"));
                                    LogHelp.WriteLog("RefundNumber:::" + RefundNumber);


                                    RefundOrderRequest req = new RefundOrderRequest();
                                    data.SetValue("out_trade_no", order.WXPayOutTradeNumber);
                                    data.SetValue("total_fee", 1);//订单总金额
                                    data.SetValue("refund_fee", 1);//退款金额
                                    
                                    data.SetValue("out_refund_no", WxPayApi.GenerateOutTradeNo());//随机生成商户退款单号
                                    //data.SetValue("total_fee", Convert.ToInt32(order.PayPrice * 100));//订单总金额
                                    //data.SetValue("refund_fee",  Convert.ToInt32(order.PayPrice * 100));//退款金额
                                    
                                    var resp = client.Refund(data);

                                    //WxPayData jsApiParam = new WxPayData();
                                    //jsApiParam.SetValue("appId", resp.AppId);
                                    //jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
                                    //jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
                                    //jsApiParam.SetValue("package", "prepay_id=" + resp.PrepayId);
                                    //jsApiParam.SetValue("signType", "MD5");
                                    //jsApiParam.SetValue("paySign", jsApiParam.MakeSign());
                                }
                            }


                        }
                        bc.UpdateGroupBuyStatus(group.GroupBuyId, 2);
                    }

                }
                LogHelp.WriteLog("end::团购定时任务");
            }
            catch (Exception ex)
            {
                LogHelp.WriteLog("end::团购定时任务出错" + ex.Message);
            }
          
            //
        }
    }
}
