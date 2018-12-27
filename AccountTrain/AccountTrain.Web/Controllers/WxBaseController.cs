
using BusinessComponent;
using BusinessEntitys;
using Common;
using Common.WeChat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using WxPayAPI;

namespace AccountTrain.Web.Controllers
{
    public class WxBaseController : Controller
    {
        private AppSetting _setting = null;
        private string WeiXinAppToken;
        private string WeiXinEncodingAESKey;
        private string WeiXinAppId;
        private string IsDebug;

        public WxBaseController()
        {
            _setting = new AppSetting();
            WeiXinAppToken = _setting.WeiXinAppToken;
            WeiXinEncodingAESKey = _setting.WeiXinEncodingAESKey;
            WeiXinAppId = _setting.WeiXinAppId;
            _crypt = new WXBizMsgCrypt(WeiXinAppToken, WeiXinEncodingAESKey, WeiXinAppId);

        }

        [HttpPost]
        public void ClickLog(ClickModel model)
        {
            new BaseSetBC().SaveClickCount(model.obType, model.id, model.openid);
        }

      

        //通过code换取网页授权access_token
        public OpenId GetOpenId(string code)
        {
            try
            {
                var client = new RestClient("https://api.weixin.qq.com/sns/oauth2");

                var request = new RestRequest("access_token?appid={appid}&secret={appsecret}&code={code}&grant_type=authorization_code", Method.GET);
                request.AddUrlSegment("appid", _setting.WeiXinAppId);
                request.AddUrlSegment("appsecret", _setting.WeiXinAppSecret);
                request.AddUrlSegment("code", code);
                var response = client.Execute(request);
                var data = JsonConvert.DeserializeObject<OpenId>(response.Content);

                LogHelp.WriteLog("GetOpenId:::" + data);

                if (data != null)
                {
                    var userInfo = GetUserInfo(data.openid, data.access_token);

                    if (userInfo != null)
                    {
                        WxUserEntity Entry = new WxUserEntity();
                        Entry.Openid = userInfo.openid;
                        Entry.Nickname = userInfo.nickname;
                        Entry.Sex = userInfo.sex;
                        Entry.City = userInfo.city;
                        Entry.Country = userInfo.country;
                        Entry.Province = userInfo.province;
                        Entry.UserLanguage = userInfo.language;
                        Entry.Headimgurl = userInfo.headimgurl;
                        new WxUserBC().SaveWxUser(Entry, userInfo.openid);
                    }
                }
                return data ?? new OpenId();
            }
            catch (Exception ex)
            {
                LogHelp.WriteLog(ex.Message);
                return new OpenId();
            }
            
        }


        public UserInfo GetUserInfo(string openId, string accessToken)
        {
            var client = new RestClient("https://api.weixin.qq.com/sns");

            var request = new RestRequest("userinfo?access_token={accessToken}&openid={openid}&lang=zh_CN", Method.GET);
            request.AddUrlSegment("openid", openId);
            request.AddUrlSegment("accessToken", accessToken);

            var response = client.Execute(request);
            var data = JsonConvert.DeserializeObject<UserInfo>(response.Content);

            return data;
        }

       

        public void SendCorpSms(string accessToken, string touser, string toparty, string agentid, string msg)
        {
            var client = new RestClient("https://qyapi.weixin.qq.com/cgi-bin ");

            var request = new RestRequest("message/send?access_token={access_token}", Method.POST);
            request.AddUrlSegment("access_token", accessToken);

            var post = new
            {
                touser = "",
                toparty = toparty,
                totag = "",
                msgtype = "text",
                agentid = agentid,
                text = new
                {
                    content = msg,
                },
                safe = 0,
            };

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(post);

            var result = JObject.Parse(client.Execute(request).Content);
        }

        public AdminUserInfo GetAdminUserInfo(string accessToken, string userId)
        {
            AdminUserInfo user = null;

            var client = new RestClient("https://qyapi.weixin.qq.com/cgi-bin/user");

            var request = new RestRequest("get?access_token={accessToken}&userid={userId}", Method.GET);
            request.AddUrlSegment("accessToken", accessToken);
            request.AddUrlSegment("userId", userId);

            var response = client.Execute(request);

            dynamic data = JObject.Parse(response.Content);

            //{"errcode":0,"errmsg":"ok","userid":"lynnroyal","name":"Lynn","department":[2],"position":"茶艺师","mobile":"18662430582","gender":"1","email":"cnblogs@vip.qq.com","weixinid":"lynnroyal","status":4,"extattr":{"attrs":[]}}
            if (data.errcode == 0)
            {
                user = new AdminUserInfo();
                user.UserId = data.userid;
                user.UserName = data.name;
                user.DepId = data.department.ToObject<int[]>();
                user.Position = data.position;
                user.Mobile = data.mobile;
                user.Gender = data.gender;
                user.Email = data.email;
                user.Wechat = data.weixinid;
                user.Status = data.status;
                user.Avatar = data.avatar;
            }

            return user;
        }

       
         private WXBizMsgCrypt _crypt;
        //private ILogger _logger;
        //配置参数
        

      
        [HttpGet]
        public ActionResult VerifyUrl()
        {
            //_logger.Debug("OneBoxService Verify Start");
            
            string echostr = Request.QueryString["echoStr"];
            string signature = Request.QueryString["signature"];
            string timestamp = Request.QueryString["timestamp"];
            string nonce = Request.QueryString["nonce"];

            string reply = string.Empty;

            StringBuilder sb = new StringBuilder();
            //_logger.Debug(string.Format("?echostr={0}&signature={1}&timestamp={2}&nonce={3}", echostr, signature, timestamp, nonce));

            if (WXBizMsgCrypt.CheckSignature(WeiXinAppToken, signature, timestamp, nonce, (a) => { return FormsAuthentication.HashPasswordForStoringInConfigFile(a, "SHA1"); }))
            {
                reply = echostr;
            }
            else
            {
                reply = Guid.NewGuid().ToString();
            }
            //_logger.Debug(string.Format("reply: {0}", reply));

            //_logger.Debug("OneBoxService Verify End");

            return Content(reply);
        }
        [HttpPost]
        public ActionResult VerifyUrl(string param)
        {
            string postString = string.Empty;
            string returnString = string.Empty;
            try
            {
                using (Stream stream = Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }

                if (!string.IsNullOrEmpty(postString))
                {
                    //_logger.Debug(DateTime.Now.ToString() + " OneBoxService Verify Start " + postString);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(postString);
                    var root = doc.FirstChild;

                    var fromUserName = root["FromUserName"].InnerText;
                    var toUserName = root["ToUserName"].InnerText;
                    var eventName = root["Event"].InnerText;
                    var eventKey = root["EventKey"].InnerText;

                    if (!string.IsNullOrEmpty(fromUserName))
                    {
                        //if (!string.IsNullOrEmpty(eventName) && eventName.Equals("subscribe"))
                        //{//关注
                        //    //_logger.Debug("subscribe detected: USER " + fromUserName);
                        //    string subscribeMsg = "嗨！你来啦！恭祝你猴年桃花朵朵开！据说，关注一箱的都是有趣又有品的一群人……上海滩最biger 的上门服务，尽在\"一箱\"，想\"玩物丧志\"，尽在\"一箱\"，快来一起玩吧！";
                        //    returnString = Msg_Text(toUserName , fromUserName, subscribeMsg);
                        //}
                        //if (!string.IsNullOrEmpty(eventName) && eventName.Equals("click"))
                        //{//点击链接跳转
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.Debug(postString, ex);
            }
            return Content(returnString);
        }

        [HttpPost]
        public ActionResult PayNotifyUrl()
        {

            try
            {
                //接收从微信后台POST过来的数据
                
                System.IO.Stream s = HttpContext.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                //Log.Info(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());
                WxPayData res = new WxPayData();

                //转换数据格式并验证签名
                WxPayData data = new WxPayData();
                try
                {
                    
                    data.FromXml(builder.ToString());
                }
                catch (WxPayException ex)
                {
                    //若签名错误，则立即返回结果给微信支付后台
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", ex.Message);
                    //Log.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                    //page.Response.Write(res.ToXml());
                    //page.Response.End();
                    
                    return Content(res.ToXml());
                }

                //检查支付结果中transaction_id是否存在
                if (!data.IsSet("transaction_id"))
                {
                    //若transaction_id不存在，则立即返回结果给微信支付后台
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "支付结果中微信订单号不存在");
                    //Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                    //page.Response.Write(res.ToXml());
                    //page.Response.End();
                    return Content(res.ToXml());

                }

                string transaction_id = data.GetValue("transaction_id").ToString();
                
                string attach = data.GetValue("attach").ToString();

                int id = int.Parse(attach);


                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                return Content(res.ToXml());

            }
            catch (Exception ex)
            {
            }
            return Content("");
        }


        #region 回复纯文本消息
        private string Msg_Text(string FromUserName, string ToUserName, string Content)
        {
            StringBuilder sbRtn = new StringBuilder();
            sbRtn.Append("<xml>");
            sbRtn.AppendFormat("<ToUserName><![CDATA[{0}]]></ToUserName>", FromUserName);
            sbRtn.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName>", ToUserName);
            sbRtn.AppendFormat("<CreateTime>{0}</CreateTime>", ConvertDateTimeToInt(DateTime.Now));
            sbRtn.Append("<MsgType><![CDATA[text]]></MsgType>");
            sbRtn.AppendFormat("<Content><![CDATA[{0}]]></Content>", Content);
            sbRtn.Append("</xml>");

            return sbRtn.ToString();
        }
        // 取得整形时间
        private int ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        #endregion
    }



    public class UserToken
    {
        public string UserId { get; set; }
        public string DeviceId { get; set; }
    }


   

    public class UserInfo
    {
        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
    }

    public class ClickModel
    {
        public string obType { get; set; }
        public string id { get; set; }
        public string openid { get; set; }
    }



    public class OpenId
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }

    public class AdminUserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Wechat { get; set; }
        public int Status { get; set; }
        public int[] DepId { get; set; }
        public string DepName { get; set; }
        public string Position { get; set; }
        public string Avatar { get; set; }
    }
}
