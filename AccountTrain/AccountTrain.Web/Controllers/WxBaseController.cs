
using BusinessComponent;
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

namespace AccountTrain.Web.Controllers
{
    public class WxBaseController : Controller
    {
        private AppSetting _setting = null;
        private string WeiXinAppToken;
        private string WeiXinEncodingAESKey;
        private string WeiXinAppId;

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
            var client = new RestClient("https://api.weixin.qq.com/sns/oauth2");

            var request = new RestRequest("access_token?appid={appid}&secret={appsecret}&code={code}&grant_type=authorization_code", Method.GET);
            request.AddUrlSegment("appid", _setting.WeiXinAppId);
            request.AddUrlSegment("appsecret", _setting.WeiXinAppSecret);
            request.AddUrlSegment("code", code);

            var response = client.Execute(request);

            var data = JsonConvert.DeserializeObject<OpenId>(response.Content);

            return data??new OpenId();
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
            Log.WriteLog("BaseController_VerifyUrl start:");
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
