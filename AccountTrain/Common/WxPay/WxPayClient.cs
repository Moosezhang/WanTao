
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WxPayAPI;


namespace Common.WxPay
{
    public class WxPayClient
    {
        //private ILogger _logger;
        public UnifiedOrderResponse UnifiedOrder(UnifiedOrderRequest request)
        {
            
            //_logger = new LoggerFactory().SetCurrent(new Log4NetLoggerFactory("Wechat_Verify")).CreateLogger();
            UnifiedOrderResponse resp = new UnifiedOrderResponse();
            //统一下单
            try
            {
                //_logger.Debug("exPay:in");
                WxPayData data = new WxPayData();
                data.SetValue("body", request.Body);
                data.SetValue("attach", request.Attach);
                data.SetValue("out_trade_no", request.OutTradeNo);
                data.SetValue("total_fee", request.TotalFee);
                data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
                data.SetValue("goods_tag", request.GoodTag);
                data.SetValue("trade_type", request.TradeType);
                data.SetValue("openid", request.OpenId);


                data.SetValue("notify_url", request.NotifyUrl);//异步通知url
                data.SetValue("appid", WxPayConfig.APPID);//公众账号ID
                data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
                data.SetValue("spbill_create_ip", WxPayConfig.IP);//终端ip	  	    
                data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串

                //签名
                data.SetValue("sign", data.MakeSign());

                string xml = data.ToXml();
                LogHelp.WriteLog(DateTime.Now + "payInxml:" + xml);
                RestClient client = new RestClient("https://api.mch.weixin.qq.com/pay/unifiedorder");
                RestRequest req = new RestRequest(Method.POST);
                req.RequestFormat = DataFormat.Xml;
                req.AddParameter("text/xml", xml, ParameterType.RequestBody);

                var content = client.Execute(req).Content;
                LogHelp.WriteLog(DateTime.Now + "payIn:" + content);
                WxPayData result = new WxPayData();
                result.FromXml(content);


                if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
                {
                    resp.Success = false;

                    if (result.IsSet("return_msg"))
                    {
                        resp.ErrMsg = (string)result.GetValue("return_msg");
                    }

                    return resp;
                }

                resp.Success = (string)result.GetValue("return_code") == "SUCCESS";
                if (!resp.Success)
                {
                    resp.ErrMsg = (string)result.GetValue("return_msg");
                    return resp;
                }
                else
                    resp.ErrMsg = string.Empty;

                resp.AppId = (string)result.GetValue("appid");
                resp.MchId = (string)result.GetValue("mch_id");
                resp.NonceStr = (string)result.GetValue("nonce_str");
                resp.Sign = (string)result.GetValue("sign");
                resp.PrepayId = (string)result.GetValue("prepay_id");
                resp.TradeType = (string)result.GetValue("trade_type");
            }
            catch (Exception ex)
            {
                LogHelp.WriteLog(DateTime.Now + "UnifiedOrderError:" + ex.Message);
            }

            return resp;
        }

        public RefundOrderResponse RefundOrder(RefundOrderRequest request)
        {
            RefundOrderResponse resp = new RefundOrderResponse();

            RestClient client = new RestClient("https://api.mch.weixin.qq.com/secapi/pay/refund");

            X509Certificate2[] certs = new X509Certificate2[] {
                new X509Certificate2(request.SSLCertRootPath + WxPayConfig.SSLCERT_PATH, WxPayConfig.SSLCERT_PASSWORD)
                };

            client.ClientCertificates = new X509CertificateCollection(certs);

            WxPayData data = new WxPayData();
            data.SetValue("transaction_id", request.TransactionId);
            data.SetValue("out_refund_no", request.OutRefundNo);
            data.SetValue("total_fee", request.TotalFee);
            data.SetValue("refund_fee", request.RefundFee);
            data.SetValue("op_user_id", WxPayConfig.MCHID);

            data.SetValue("appid", WxPayConfig.APPID);
            data.SetValue("mch_id", WxPayConfig.MCHID);
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
            data.SetValue("sign", data.MakeSign());

            string xml = data.ToXml();

            RestRequest req = new RestRequest(Method.POST);
            req.RequestFormat = DataFormat.Xml;
            req.AddParameter("text/xml", xml, ParameterType.RequestBody);

            var content = client.Execute(req).Content;

            WxPayData result = new WxPayData();
            result.FromXml(content);

            resp.Success = (string)result.GetValue("return_code") == "SUCCESS";
            if (!resp.Success)
            {
                resp.ErrCode = (string)result.GetValue("err_code");
                resp.ErrMsg = (string)result.GetValue("err_code_des");
                return resp;
            }
            else
            {
                resp.ErrCode = string.Empty;
                resp.ErrMsg = string.Empty;
            }


            resp.AppId = (string)result.GetValue("appid");
            resp.MchId = (string)result.GetValue("mch_id");

            resp.TransactionId = (string)result.GetValue("transaction_id");
            resp.OutTradeNo = (string)result.GetValue("out_trade_no");
            resp.OutRefundNo = (string)result.GetValue("out_refund_no");
            resp.RefundId = (string)result.GetValue("refund_id");
            resp.RefundChannel = (string)result.GetValue("refund_channel");
            resp.RefundFee = int.Parse((string)result.GetValue("refund_fee"));
            resp.TotalFee = int.Parse((string)result.GetValue("total_fee"));

            return resp;
        }


        public  WxPayData Refund(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数refund_fee！");
            }


            inputObj.SetValue("op_user_id", WxPayConfig.MCHID);
            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            LogHelp.WriteLog("Refund request : " + xml);
            string response = HttpService.Post(xml, url, true, timeOut);//调用HTTP通信接口提交数据到API
            LogHelp.WriteLog("Refund response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

           

            return result;
        }

        public string GenerateTimeStamp()
        {
            return WxPayApi.GenerateTimeStamp();
        }

        public string GenerateOutTradeNo()
        {
            return WxPayApi.GenerateOutTradeNo();
        }

        public string GenerateNonceStr()
        {
            return WxPayApi.GenerateNonceStr();
        }



    }
    

    public class UnifiedOrderRequest
    {
        public string Body { get; set; }
        public string Attach { get; set; }
        public int TotalFee { get; set; }
        public string GoodTag { get; set; }
        public string OpenId { get; set; }
        public string TradeType { get; set; }
        public string OutTradeNo { get; set; }
        public string NotifyUrl { get; set; }
    }

    public class UnifiedOrderResponse
    {
        public bool Success { get; set; }
        public string ErrMsg { get; set; }
        public string AppId { get; set; }
        public string MchId { get; set; }
        public string NonceStr { get; set; }
        public string Sign { get; set; }
        public string PrepayId { get; set; }
        public string TradeType { get; set; }
    }

    public class RefundOrderRequest
    {
        public string SSLCertRootPath { get; set; }
        public string TransactionId { get; set; }
        public string OutRefundNo { get; set; }
        public int TotalFee { get; set; }
        public int RefundFee { get; set; }
    }

    public class RefundOrderResponse
    {
        public bool Success { get; set; }
        public string ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public string AppId { get; set; }
        public string MchId { get; set; }
        public string TransactionId { get; set; }
        public string OutTradeNo { get; set; }
        public string OutRefundNo { get; set; }
        public string RefundId { get; set; }
        public string RefundChannel { get; set; }
        public int RefundFee { get; set; }
        public int TotalFee { get; set; }
    }

}
