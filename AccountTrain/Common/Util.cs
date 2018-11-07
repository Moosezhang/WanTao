using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

namespace Common
{
    public class Util
    {
        // 用于缓存微信 AccessToken，如果已经取过，在超时前不必再取
        private static List<AccessTokenObject> lstAccessToken = new List<AccessTokenObject>();
        private static List<AccessTokenObject> tokenList = new List<AccessTokenObject>();

     

        /// <summary>  
        /// 获取POST返回来的数据  
        /// </summary>  
        /// <returns></returns>  
        public static string GetPostInput(HttpRequest Request)
        {
            try
            {
                System.IO.Stream s = Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[s.Length];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, buffer.Length)) > 0)
                {
                    builder.Append(Request.ContentEncoding.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                return builder.ToString();
            }
            catch (Exception ex)
            { throw ex; }
        }

        // 取得整形时间
        public static int ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        // 在页面上运行Script
        public static void RunScript(HttpResponse Response, string str)
        {
            Response.Write("<script>" + str + "</script>");
        }
        // 使用 alter 弹出消息框
        public static void MsgBox(HttpResponse Response, string strMessage)
        {
            if (strMessage == null)
                strMessage = "";

            //弹出警告窗口
            Response.Write("<script type=\"text/javascript\" language=\"JavaScript\">");
            Response.Write(string.Format("alert('{0}');", strMessage.Replace("'", "\"").Replace(Environment.NewLine, "\\n")));
            Response.Write("</script>");
        }

        #region 创建 get post 访问
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();

                // 如果只有一个参数，就不加Key，这么做是为了Post微信数据可以成功
                if (parameters.Count == 1 && string.IsNullOrWhiteSpace(parameters.ElementAt(0).Key))
                {
                    buffer.Append(parameters.ElementAt(0).Value);
                }
                else
                {
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                }


                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            return request.GetResponse() as HttpWebResponse;
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        #endregion

        public static string ConvertToString(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return "";

            return obj.ToString();
        }
        public static bool ConvertToBool(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return false;

            if (obj is bool)
                return (bool)obj;

            return false;
        }
        public static int ConvertToInt(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            int iRtn = 0;
            int.TryParse(obj.ToString(), out iRtn);
            return iRtn;
        }

        public static double ConvertToDouble(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            double iRtn = 0;
            double.TryParse(obj.ToString(), out iRtn);
            return iRtn;
        }


        public static string ConvertToShrotString(object obj, int length)
        {
            if (obj == null || obj == DBNull.Value)
                return "";

            string s = obj.ToString();
            if (s.Length <= length)
                return s;

            int isEmoji = s.ToCharArray()[length - 1] == Convert.ToChar(55357) ? 1 : 0; // Emoji 字符判断

            return s.Substring(0, length - isEmoji) + "...";
        }

        // 去掉首尾的双引号
        public static string RemoveQuotation(string strIn)
        {
            if (strIn.Length > 0 && strIn.Substring(0, 1) == "\"")
                strIn = strIn.Substring(1);

            if (strIn.Length > 0 && strIn.Substring(strIn.Length - 1) == "\"")
                strIn = strIn.Substring(0, strIn.Length - 1);

            return strIn;
        }

        public static string GetValidDBStringValue(string strInput)
        {
            return strInput.Replace("'", "''");
        }


        #region 调用微信接口
        // 取得 AccessToken
        //public static string GetAccessToken(string sCorpID, string Secret)
        //{
        //    string AccessToken = "";
        //    if (lstAccessToken == null)
        //        lstAccessToken = new List<AccessTokenObject>();

        //    for (int i = 0; i < lstAccessToken.Count; i++)
        //    {
        //        // 删掉所有超过 1 小时的AccessToken， 
        //        if ((DateTime.Now - lstAccessToken[i].CreatedTime).TotalSeconds > 3600)
        //        {
        //            lstAccessToken.RemoveAt(i);
        //            i--;
        //            continue;
        //        }

        //        // 找到缓存中没有超时的 AccessToken
        //        // 注意：即使找到需要的 AccessToken 也不要立即返回，需要清除所有超时的AccessToken之后再返回
        //        if (lstAccessToken[i].CorpID == sCorpID && lstAccessToken[i].Secret == Secret)
        //            AccessToken = lstAccessToken[i].AccessToken;
        //    }

        //    //foreach (var item in lstAccessToken)
        //    //{
        //    //    // 删掉所有超过 1 小时的AccessToken， 
        //    //    // 注意：即使找到需要的 AccessToken 也不要立即返回，需要清除所有超时的AccessToken之后再返回
        //    //    if ((DateTime.Now - item.CreatedTime).TotalSeconds > 3600)
        //    //        lstAccessToken.Remove(item);

        //    //    // 找到缓存中没有超时的 AccessToken
        //    //    if (item.CorpID == sCorpID && item.Secret == Secret)
        //    //        AccessToken = item.AccessToken;
        //    //}

        //    // 使用没有超时的 AccessToken
        //    if (!string.IsNullOrWhiteSpace(AccessToken))
        //        return AccessToken;

        //    // 没有缓存的 AccessToken，去获取
        //    string strURL = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", sCorpID, Secret);
        //    string strRtn = "";
        //    HttpWebResponse response = null;
        //    Stream sr = null;
        //    StreamReader reader = null;
        //    try
        //    {
        //        response = CreateGetHttpResponse(strURL, null, null, null);
        //        sr = response.GetResponseStream();
        //        reader = new StreamReader(sr);
        //        strRtn = reader.ReadToEnd();

        //        JObject jObj = JObject.Parse(strRtn);
        //        if (jObj["errcode"] != null)
        //            return "";

        //        AccessTokenObject at = new AccessTokenObject();
        //        at.CorpID = sCorpID;
        //        at.Secret = Secret;
        //        at.CreatedTime = DateTime.Now;
        //        at.AccessToken = RemoveQuotation(jObj["access_token"].ToString()); // 要去掉两边的双引号

        //        lstAccessToken.Add(at);

        //        return at.AccessToken;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        if (sr != null)
        //            sr.Close();

        //        if (reader != null)
        //            reader.Close();

        //        if (response != null)
        //            response.Close();
        //    }

        //    return "";
        //}
        public static string GetAccessTokenOpen()
        {
            AppSetting appsetting = new AppSetting();
            string appid = appsetting.WeiXinAppId;
            string secret = appsetting.WeiXinAppSecret;
            string AccessToken = "";
            if (tokenList == null)
                tokenList = new List<AccessTokenObject>();

            for (int i = 0; i < tokenList.Count; i++)
            {
                // 删掉所有超过 1 小时的AccessToken， 
                if ((DateTime.Now - tokenList[i].CreatedTime).TotalSeconds > 3600)
                {
                    tokenList.RemoveAt(i);
                    i--;
                    continue;
                }

                // 找到缓存中没有超时的 AccessToken
                // 注意：即使找到需要的 AccessToken 也不要立即返回，需要清除所有超时的AccessToken之后再返回
                if (tokenList[i].APPID == appid && tokenList[i].AppSecret == secret)
                    AccessToken = tokenList[i].AccessToken;
            }

            //foreach (var item in lstAccessToken)
            //{
            //    // 删掉所有超过 1 小时的AccessToken， 
            //    // 注意：即使找到需要的 AccessToken 也不要立即返回，需要清除所有超时的AccessToken之后再返回
            //    if ((DateTime.Now - item.CreatedTime).TotalSeconds > 3600)
            //        lstAccessToken.Remove(item);

            //    // 找到缓存中没有超时的 AccessToken
            //    if (item.CorpID == sCorpID && item.Secret == Secret)
            //        AccessToken = item.AccessToken;
            //}

            // 使用没有超时的 AccessToken
            if (!string.IsNullOrWhiteSpace(AccessToken))
                return AccessToken;

            // 没有缓存的 AccessToken，去获取
            string strURL = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            string strRtn = "";
            HttpWebResponse response = null;
            Stream sr = null;
            StreamReader reader = null;
            try
            {
                response = CreateGetHttpResponse(strURL, null, null, null);
                sr = response.GetResponseStream();
                reader = new StreamReader(sr);
                strRtn = reader.ReadToEnd();

                JObject jObj = JObject.Parse(strRtn);
                if (jObj["errcode"] != null)
                    return "";

                AccessTokenObject at = new AccessTokenObject();
                at.APPID = appid;
                at.AppSecret = secret;
                at.CreatedTime = DateTime.Now;
                at.AccessToken = RemoveQuotation(jObj["access_token"].ToString()); // 要去掉两边的双引号

                tokenList.Add(at);

                return at.AccessToken;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();

                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }

            return "";
        }

        public static string GetAccessToken(string APPID, string AppSecret)
        {
            string AccessToken = "";
            if (lstAccessToken == null)
                lstAccessToken = new List<AccessTokenObject>();

            for (int i = 0; i < lstAccessToken.Count; i++)
            {
                // 删掉所有超过 1 小时的AccessToken， 
                if ((DateTime.Now - lstAccessToken[i].CreatedTime).TotalSeconds > 3600)
                {
                    lstAccessToken.RemoveAt(i);
                    i--;
                    continue;
                }

                // 找到缓存中没有超时的 AccessToken
                // 注意：即使找到需要的 AccessToken 也不要立即返回，需要清除所有超时的AccessToken之后再返回
                if (lstAccessToken[i].APPID == APPID && lstAccessToken[i].AppSecret == AppSecret)
                    AccessToken = lstAccessToken[i].AccessToken;
            }

            // 使用没有超时的 AccessToken
            if (!string.IsNullOrWhiteSpace(AccessToken))
                return AccessToken;

            // 没有缓存的 AccessToken，去获取
            string strURL = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", APPID, AppSecret);
            string strRtn = "";
            HttpWebResponse response = null;
            Stream sr = null;
            StreamReader reader = null;
            try
            {
                response = CreateGetHttpResponse(strURL, null, null, null);
                sr = response.GetResponseStream();
                reader = new StreamReader(sr);
                strRtn = reader.ReadToEnd();

                JObject jObj = JObject.Parse(strRtn);
                if (jObj["errcode"] != null)
                    return "";

                AccessTokenObject at = new AccessTokenObject();
                at.APPID = APPID;
                at.AppSecret = AppSecret;
                at.CreatedTime = DateTime.Now;
                at.AccessToken = RemoveQuotation(jObj["access_token"].ToString()); // 要去掉两边的双引号

                lstAccessToken.Add(at);

                return at.AccessToken;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();

                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }

            return "";
        }
        // 取得 Auth2.0 URL
        public static string GetAuthURL(string APPID, HttpServerUtility server, string sCallbackURL)
        {
            string strURI = server.UrlEncode(sCallbackURL);
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect", APPID, strURI);
        }
        // 取得 Auth2.0 URL
        public static string GetAuthURLInfo(string APPID, HttpServerUtility server, string sCallbackURL)
        {
            string strURI = server.UrlEncode(sCallbackURL);
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", APPID, strURI);
        }
        // Auth2.0 授权，根据 code取得 OpenID
        public static string GetOpenIdByCode(string APPID, string AppSecret, string strCode, ref string strError)
        {
            strError = "";
            string strAccessToken = GetAccessToken(APPID, AppSecret);
            if (string.IsNullOrWhiteSpace(strAccessToken))
            {
                string sCryptedSecret = AppSecret;
                if (AppSecret.Length > 10)
                    sCryptedSecret = AppSecret.Substring(0, 3) + "********" + AppSecret.Substring(9);
                strError = string.Format("获取AccessToken失败, sCorpID={0}, sSecret={1}", APPID, sCryptedSecret);
                return "";
            }

            string strURL = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", APPID, AppSecret, strCode);
            string strRtn = "";
            HttpWebResponse response = null;
            Stream sr = null;
            StreamReader reader = null;
            try
            {
                response = CreateGetHttpResponse(strURL, null, null, null);
                sr = response.GetResponseStream();
                reader = new StreamReader(sr);
                strRtn = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(strRtn))
                {
                    strError = "没有返回消息";
                    return "";
                }

                strError = strRtn;

                JObject jObj = JObject.Parse(strRtn);

                if (jObj["openid"] != null)
                {
                    return RemoveQuotation(jObj["openid"].ToString());
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }
            finally
            {
                if (sr != null)
                    sr.Close();

                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }

            return "";
        }


        // Auth2.0 授权，根据 code取得 OpenID
        public static string GetOpenIdByCode(string APPID, string AppSecret, string strCode, ref string strError, ref string sAccessToken)
        {
            strError = "";
            string strAccessToken = GetAccessToken(APPID, AppSecret);
            if (string.IsNullOrWhiteSpace(strAccessToken))
            {
                string sCryptedSecret = AppSecret;
                if (AppSecret.Length > 10)
                    sCryptedSecret = AppSecret.Substring(0, 3) + "********" + AppSecret.Substring(9);
                strError = string.Format("获取AccessToken失败, sCorpID={0}, sSecret={1}", APPID, sCryptedSecret);
                return "";
            }

            string strURL = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", APPID, AppSecret, strCode);
            string strRtn = "";
            HttpWebResponse response = null;
            Stream sr = null;
            StreamReader reader = null;
            try
            {
                response = CreateGetHttpResponse(strURL, null, null, null);
                sr = response.GetResponseStream();
                reader = new StreamReader(sr);
                strRtn = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(strRtn))
                {
                    strError = "没有返回消息";
                    return "";
                }

                strError = strRtn;

                JObject jObj = JObject.Parse(strRtn);

                if (jObj["openid"] != null)
                {
                    sAccessToken = RemoveQuotation(jObj["access_token"].ToString());
                    return RemoveQuotation(jObj["openid"].ToString());
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }
            finally
            {
                if (sr != null)
                    sr.Close();

                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }

            return "";
        }

        // Auth2.0 授权，获取用户头像
        public static string GetUserInfoByOpenID(string sAccessToken, string sOpenID, string sLang, ref string strError)
        {
            strError = "";

            string strURL = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}", sAccessToken, sOpenID, sLang);
            string strRtn = "";
            HttpWebResponse response = null;
            Stream sr = null;
            StreamReader reader = null;
            try
            {
                response = CreateGetHttpResponse(strURL, null, null, null);
                sr = response.GetResponseStream();
                reader = new StreamReader(sr);
                strRtn = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(strRtn))
                {
                    strError = "没有返回消息";
                    return "";
                }

                strError = strRtn;

                JObject jObj = JObject.Parse(strRtn);

                if (jObj["headimgurl"] != null)
                {
                    return RemoveQuotation(jObj["headimgurl"].ToString());
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }
            finally
            {
                if (sr != null)
                    sr.Close();

                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }

            return "";
        }



        // 发送模板消息
        public static bool SendTemplateMessage(string APPID, string AppSecret, string sOpenID, JObject msg, ref string strError)
        {
            strError = "";
            string strAccessToken = GetAccessToken(APPID, AppSecret);
            if (string.IsNullOrWhiteSpace(strAccessToken))
            {
                string sCryptedSecret = AppSecret;
                if (AppSecret.Length > 10)
                    sCryptedSecret = AppSecret.Substring(0, 3) + "********" + AppSecret.Substring(9);
                strError = string.Format("获取AccessToken失败, sCorpID={0}, sSecret={1}", APPID, sCryptedSecret);
                return false;
            }

            string strURL = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", strAccessToken);
            string strRtn = "";
            HttpWebResponse response = null;
            Stream sr = null;
            StreamReader reader = null;
            try
            {
                string sJSON = JsonConvert.SerializeObject(msg);

                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("", sJSON);
                response = CreatePostHttpResponse(strURL, dict, null, null, Encoding.UTF8, null);
                sr = response.GetResponseStream();
                reader = new StreamReader(sr);
                strRtn = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(strRtn))
                {
                    strError = "没有返回消息";
                    return false;
                }

                strError = strRtn;  // 记录返回信息

                JObject jObj = JObject.Parse(strRtn);
                if (jObj["errcode"] == null || jObj["errcode"].ToString() != "0")
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
            finally
            {
                if (sr != null)
                    sr.Close();

                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }


            return true;
        }

        #endregion 调用微信接口

     

        public static string ConvertDataTableToJSON(DataTable dt)
        {
            JArray jRtn = new JArray();
            if (dt == null || dt.Rows.Count == 0)
                return JsonConvert.SerializeObject(jRtn);

            foreach (DataRow row in dt.Rows)
            {
                JObject jRow = new JObject();
                for (int i = 0; i < dt.Columns.Count; i++)
                    jRow[dt.Columns[i].ColumnName] = ConvertToString(row[i]);

                jRtn.Add(jRow);
            }

            return JsonConvert.SerializeObject(jRtn);
        }

        private static List<Wechat_JsApi_Ticket> lstWechatJSAPI = new List<Wechat_JsApi_Ticket>();
        public static string GetJSAPI_Ticket(string sCorpID, string Secret)
        {
            string strRtnTicket = "";
            if (lstWechatJSAPI == null)
                lstWechatJSAPI = new List<Wechat_JsApi_Ticket>();

            for (int i = 0; i < lstWechatJSAPI.Count; i++)
            {
                // 删掉所有超过 1 小时的AccessToken， 
                if ((DateTime.Now - lstWechatJSAPI[i].CreatedTime).TotalSeconds > 3600)
                {
                    lstWechatJSAPI.RemoveAt(i);
                    i--;
                    continue;
                }

                // 找到缓存中没有超时的 JS Api Ticket
                // 注意：即使找到需要的 Ticket 也不要立即返回，需要清除所有超时的 Ticket 之后再返回
                if (lstWechatJSAPI[i].APPID == sCorpID && lstWechatJSAPI[i].AppSecret == Secret)
                    strRtnTicket = lstWechatJSAPI[i].JSTicket;
            }

            // 使用没有超时的 AccessToken
            if (!string.IsNullOrWhiteSpace(strRtnTicket))
                return strRtnTicket;

            string sAccessToken = GetAccessToken(sCorpID, Secret);
            if (string.IsNullOrWhiteSpace(sAccessToken))
                return "";

            // 没有缓存的 JS Ticket，去获取
            string strURL = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", sAccessToken);
            string strRtn = "";
            HttpWebResponse response = null;
            Stream sr = null;
            StreamReader reader = null;
            try
            {
                response = CreateGetHttpResponse(strURL, null, null, null);
                sr = response.GetResponseStream();
                reader = new StreamReader(sr);
                strRtn = reader.ReadToEnd();

                JObject jObj = JObject.Parse(strRtn);
                if (jObj["errcode"] == null || jObj["errcode"].ToString() != "0")
                    return "";

                Wechat_JsApi_Ticket at = new Wechat_JsApi_Ticket();
                at.APPID = sCorpID;
                at.AppSecret = Secret;
                at.CreatedTime = DateTime.Now;
                at.JSTicket = RemoveQuotation(jObj["ticket"].ToString()); // 要去掉两边的双引号

                lstWechatJSAPI.Add(at);

                return at.JSTicket;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();

                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }

            return "";
        }


        public static string ConvertStringToBase64(string strText)
        {
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(strText);

            return Convert.ToBase64String(bytedata);
        }

        public static string ConvertBase64ToString(string strBase64)
        {
            try
            {
                byte[] bpath = Convert.FromBase64String(strBase64);
                return System.Text.ASCIIEncoding.Default.GetString(bpath);
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public static double ConvertFenToYuan(int Fen)
        {
            return ((double)Fen / 100);
        }


        #region 根据经纬度计算距离
        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
        #endregion 根据经纬度计算距离
    }

    public class AccessTokenObject
    {
        public string APPID;
        public string AppSecret;
        public DateTime CreatedTime;
        public string AccessToken;
    }

    public class Wechat_JsApi_Ticket
    {
        public string APPID;
        public string AppSecret;
        public DateTime CreatedTime;
        public string JSTicket;
    }

    public class NewsItem
    {
        public string Title;
        public string Description;
        public string PicURL;
        public string URL;

        public NewsItem(string sTitle, string sDescription, string sPicURL, string sURL)
        {
            Title = sTitle;
            Description = sDescription;
            PicURL = sPicURL;
            URL = sURL;
        }
    }

    /// <summary>
    /// 写日志(用于跟踪)
    /// </summary>
    public static class Log
    {
        public static void WriteLog(string strMsg)
        {
            string filename = HttpContext.Current.Server.MapPath("~/logs/" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt");
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(filename))
                {
                    sr = File.CreateText(filename);
                }
                else
                {
                    sr = File.AppendText(filename);
                }
                sr.WriteLine("----------------" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-------------------");
                sr.WriteLine(strMsg);
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
                sr.Dispose();
            }
        }

    }

   
}