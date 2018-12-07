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
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using BusinessEntity.Model;


namespace Common
{
    public class Util
    {
        // 用于缓存微信 AccessToken，如果已经取过，在超时前不必再取
        private static List<AccessTokenObject> lstAccessToken = new List<AccessTokenObject>();
        private static List<AccessTokenObject> tokenList = new List<AccessTokenObject>();

        public static void WriteLog(HttpServerUtility Server, string strMsg)
        {
            string TrunOffLog = ConfigurationManager.AppSettings["TrunOffLog"];
            if (!string.IsNullOrWhiteSpace(TrunOffLog) && TrunOffLog.ToLower() == "true")
                return;

            string filename = Server.MapPath("~/logs/log.txt");
            if (!Directory.Exists(Server.MapPath("~/logs")))
                Directory.CreateDirectory(Server.MapPath("~/logs"));

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
                sr.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strMsg));
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

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

    #region 图片处理


    /*
* 
*  使用说明：
* 　建议先定义一个WaterImage实例
* 　然后利用实例的属性，去匹配需要进行操作的参数
* 　然后定义一个WaterImageManager实例
* 　利用WaterImageManager实例进行DrawImage（）印图片水印
* 　DrawWords（）印文字水印
* 
*/

    /// <summary>
    /// 图片位置
    /// </summary>
    public enum ImagePosition
    {
        LeftTop,    //左上
        LeftBottom,  //左下
        RightTop,    //右上
        RigthBottom, //右下
        TopMiddle,   //顶部居中
        BottomMiddle, //底部居中
        Center      //中心
    }

    /// <summary>
    /// 字体集
    /// </summary>
    public enum FontFamilys
    {
        CUSTOM,
        Arial,
        Batang,
        BatangChe,
        Calibri,
        Cambria,
        Candara,
        Consolas,
        Ebrima,
        Footlight_MT_Light,
        Kalinga,
        Kokila,
        Mangal,
        Symbol,
        Times_New_Roman,
        Webdings,
        仿宋,
        华文中宋,
        华文仿宋,
        华文宋体,
        华文彩云,
        华文新魏,
        华文楷体,
        华文琥珀,
        华文细黑,
        华文行楷,
        华文隶书,
        宋体,
        幼圆,
        微软雅黑,
        新宋体,
        方正姚体,
        方正舒体,
        楷体,
        隶书,
        黑体
    }

    /// <summary>
    /// 水印图片的操作管理
    /// </summary>
    public class WaterImageManager
    {
        private int padding = 0;//内容间隔
        private string targetPicName = "_mixture_pic";//默认生成图片的文件名字
        private string targetPicPath = "";//默认生成图片的目录
        private ImageFormat picFormat = ImageFormat.Png;//默认生成图片的格式

        public int Padding { get; set; }
        public string TargetPicName { get; set; }
        public string TargetPicPath { get; set; }
        public ImageFormat PicFormat { get; set; }

        /// <summary>
        /// 生成一个新的水印图片制作实例(默认)
        /// </summary>
        public WaterImageManager()
        {
            Padding = padding;
            TargetPicName = targetPicName;
            TargetPicPath = targetPicPath;
            PicFormat = picFormat;
        }

        /// <summary>
        /// 生成一个新的水印图片制作实例(有参)
        /// </summary>
        /// <param name="tragetPicName">生成合成图片的文件名称</param>
        /// <param name="tragetPicPath">生成合成图片的文件路径</param>
        /// <param name="padding">指定水印距离父容器边距</param>
        /// <param name="picFormat">指定生成合成图片的图片格式</param>
        public WaterImageManager(string targetPicName, string targetPicPath, int padding, ImageFormat picFormat)
        {
            this.Padding = padding;
            this.TargetPicName = targetPicName;
            this.TargetPicPath = targetPicPath.EndsWith(@"\") ? targetPicPath : targetPicPath + @"\";
            this.PicFormat = picFormat;
        }

        /// <summary>
        /// 合成图片
        /// </summary>
        /// <param name="sourcePictureName">源文件名(包括后缀)</param>
        /// <param name="sourcePicturePath">源文件路径</param>
        /// <param name="waterPictureName">水印文件名(包括后缀)</param>
        /// <param name="waterPicturePath">水印文件路径</param>
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <param name="position">位置</param>
        /// <returns>合成图片的完整路径</returns>
        public string DrawImage(string sourcePictureName,
                         string sourcePicturePath,
                         string waterPictureName,
                         string waterPicturePath,
                         float alpha,
                         ImagePosition position)
        {
            //
            // 判断参数是否有效
            //
            if (sourcePictureName == string.Empty || waterPictureName == string.Empty || alpha == 0.0 || sourcePicturePath == string.Empty || waterPicturePath == string.Empty)
            {
                return sourcePicturePath + sourcePictureName + "." + PicFormat.ToString().ToLower();
            }

            if (!sourcePicturePath.EndsWith(@"\"))
                sourcePicturePath = sourcePicturePath + @"\";
            if (!waterPicturePath.EndsWith(@"\"))
                waterPicturePath = waterPicturePath + @"\";

            //
            // 源图片，水印图片全路径
            //
            string _sourcePictureName = sourcePicturePath + sourcePictureName;
            string _waterPictureName = waterPicturePath + waterPictureName;

            if (!File.Exists(_sourcePictureName))
                throw new FileNotFoundException(_sourcePictureName + " file not found!");

            if (!File.Exists(_waterPictureName))
                throw new FileNotFoundException(_waterPictureName + " file not found!");

            string fileSourceExtension = System.IO.Path.GetExtension(_sourcePictureName).ToLower();
            string fileWaterExtension = System.IO.Path.GetExtension(_waterPictureName).ToLower();
            //
            // 判断文件是否存在,以及类型是否正确
            //
            if (System.IO.File.Exists(_sourcePictureName) == false ||
              System.IO.File.Exists(_waterPictureName) == false || (
              fileSourceExtension != ".gif" &&
              fileSourceExtension != ".jpg" &&
              fileSourceExtension != ".png") || (
              fileWaterExtension != ".gif" &&
              fileWaterExtension != ".jpg" &&
              fileWaterExtension != ".png")
              )
            {
                return sourcePicturePath + sourcePictureName + "." + PicFormat.ToString().ToLower();
            }

            //

            // 目标图片名称及全路径
            //
            //TargetPicPath = TargetPicPath.EndsWith(@"\") ? TargetPicPath : TargetPicPath + @"\";
            //string targetImage = TargetPicPath == string.Empty ?
            //    _sourcePictureName.Replace(System.IO.Path.GetExtension(_sourcePictureName), "") + TargetPicName + "." + PicFormat.ToString().ToLower() :
            //    TargetPicPath + TargetPicName + "." + PicFormat.ToString().ToLower();

            TargetPicName = "HelpQR_" + sourcePictureName;
            //string targetImage = TargetPicPath == string.Empty ?
            //    _sourcePictureName.Replace(System.IO.Path.GetExtension(_sourcePictureName), "") + TargetPicName + "." + PicFormat.ToString().ToLower() :
            //    TargetPicPath + TargetPicName + "." + PicFormat.ToString().ToLower();
            TargetPicPath = sourcePicturePath;
            string targetImage = TargetPicPath + TargetPicName + "." + PicFormat.ToString().ToLower();

            //
            // 将需要加上水印的图片装载到Image对象中
            //
            Image imgPhoto = Image.FromFile(_sourcePictureName);

            //
            // 确定其长宽
            //
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //
            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。
            //
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, imgPhoto.PixelFormat);//phWidth, phHeight, PixelFormat.Format24bppRgb  imgPhoto

            //
            // 设定分辨率
            // 
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //
            // 定义一个绘图画面用来装载位图
            //
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //
            //同样，由于水印是图片，我们也需要定义一个Image来装载它
            //
            Image imgWatermark = new Bitmap(_waterPictureName);

            //
            // 获取水印图片的高度和宽度
            //
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。
            // 成员名称  说明 
            // AntiAlias   指定消除锯齿的呈现。 
            // Default    指定不消除锯齿。

            // HighQuality 指定高质量、低速度呈现。 
            // HighSpeed  指定高速度、低质量呈现。 
            // Invalid    指定一个无效模式。 
            // None     指定不消除锯齿。 
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //
            // 第一次描绘，将我们的底图描绘在绘图画面上
            //
            grPhoto.DrawImage(imgPhoto,
                          new Rectangle(0, 0, phWidth, phHeight),
                          0,
                          0,
                          phWidth,
                          phHeight,
                          GraphicsUnit.Pixel);

            //
            // 与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率
            //
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //
            // 继续，将水印图片装载到一个绘图画面grWatermark
            //
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //
            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。
            //   

            ImageAttributes imageAttributes = new ImageAttributes();

            //
            //Colormap: 定义转换颜色的映射
            //
            ColorMap colorMap = new ColorMap();

            //
            //我的水印图被定义成拥有绿色背景色的图片被替换成透明
            //
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f}, //red红色
                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f}, //green绿色
                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f}, //blue蓝色    
                new float[] {0.0f, 0.0f, 0.0f, alpha, 0.0f},//透明度   
                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};//

            // ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。
            // ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
            ColorAdjustType.Bitmap);

            //
            //上面设置完颜色，下面开始设置位置
            //
            int xPosOfWm;
            int yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = Padding;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = Padding;
                    yPosOfWm = Padding;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - Padding;
                    yPosOfWm = Padding;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - Padding;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = Padding;
                    break;
                default:
                    xPosOfWm = Padding;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
            }

            imgPhoto.Dispose();//释放底图，解决图片保存时 “GDI+ 中发生一般性错误。”
            // 第二次绘图，把水印印上去
            //
            grWatermark.DrawImage(imgWatermark,
             new Rectangle(xPosOfWm,
                       yPosOfWm,
                       wmWidth,
                       wmHeight),
                       0,
                       0,
                       wmWidth,
                       wmHeight,
                       GraphicsUnit.Pixel,
                       imageAttributes);

            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            //
            // 保存文件到服务器的文件夹里面
            //
            if (!System.IO.Directory.Exists(TargetPicPath))
                System.IO.Directory.CreateDirectory(TargetPicPath);
            try
            {
                imgPhoto.Save(targetImage, PicFormat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            imgPhoto.Dispose();
            imgWatermark.Dispose();
            return TargetPicName + "." + PicFormat.ToString().ToLower();
        }

        /// <summary>
        /// 在图片上添加水印文字
        /// </summary>
        /// <param name="sourcePicture">源图片文件名(包含后缀)</param>
        /// <param name="waterWords">需要添加到图片上的文字</param>
        /// <param name="alpha">透明度（取值区间(0.0,1.0]）</param>
        /// <param name="position">位置</param>
        /// <param name="PicturePath">文件路径</param>
        /// <returns></returns>
        public string DrawWords(string sourcePictureName,
                         string sourcePicturePath,
                         string waterWords,
                         float alpha,
                         FontFamilys fontFamily,
                         FontStyle style,
                         ImagePosition position)
        {
            //
            // 判断参数是否有效
            //
            if (sourcePictureName == string.Empty || waterWords == string.Empty || alpha == 0.0 || sourcePicturePath == string.Empty)
            {
                return sourcePicturePath + sourcePictureName;
            }

            if (!sourcePicturePath.EndsWith(@"\"))
                sourcePicturePath = sourcePicturePath + @"\";
            //
            // 源图片全路径
            //
            string _sourcePictureName = sourcePicturePath + sourcePictureName;
            if (!File.Exists(_sourcePictureName))
                throw new FileNotFoundException(_sourcePictureName + " file not found!");
            string fileExtension = System.IO.Path.GetExtension(_sourcePictureName).ToLower();

            //
            // 判断文件是否存在,以及文件名是否正确
            //
            if (System.IO.File.Exists(_sourcePictureName) == false || (
              fileExtension != ".gif" &&
              fileExtension != ".jpg" &&
              fileExtension != ".png"))
            {
                return sourcePicturePath + sourcePictureName;
            }

            //
            // 目标图片名称及全路径
            //
            targetPicName ="HelpName"+ "sourcePictureName";
            string targetImage = TargetPicPath == string.Empty ?
                _sourcePictureName.Replace(System.IO.Path.GetExtension(_sourcePictureName), "") + TargetPicName + "." + PicFormat.ToString().ToLower() :
                TargetPicPath + TargetPicName + "." + PicFormat.ToString().ToLower();

            //创建一个图片对象用来装载要被添加水印的图片
            Image imgPhoto = Image.FromFile(_sourcePictureName);

            //获取图片的宽和高
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //
            //建立一个bitmap，和我们需要加水印的图片一样大小
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, imgPhoto.PixelFormat);

            //SetResolution：设置此 Bitmap 的分辨率
            //这里直接将我们需要添加水印的图片的分辨率赋给了bitmap
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //Graphics：封装一个 GDI+ 绘图图面。
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //设置图形的品质
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //将我们要添加水印的图片按照原始大小描绘（复制）到图形中
            grPhoto.DrawImage(
                imgPhoto,                    //  要添加水印的图片
                new Rectangle(0, 0, phWidth, phHeight), // 根据要添加的水印图片的宽和高
                0,                           // X方向从0点开始描绘
                0,                           // Y方向

                phWidth,                     // X方向描绘长度
                phHeight,                    // Y方向描绘长度
                GraphicsUnit.Pixel);         // 描绘的单位，这里用的是像素

            //根据图片的大小我们来确定添加上去的文字的大小
            //在这里我们定义一个数组来确定
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4, 2 };

            //字体
            Font crFont = null;
            //矩形的宽度和高度，SizeF有三个属性，分别为Height高，width宽，IsEmpty是否为空
            SizeF crSize = new SizeF();

            //利用一个循环语句来选择我们要添加文字的型号
            //直到它的长度比图片的宽度小
            for (int i = 0; i < 8; i++)
            {
                crFont = new Font(fontFamily.ToString(), sizes[i], style);

                //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。
                crSize = grPhoto.MeasureString(waterWords, crFont);

                // ushort 关键字表示一种整数数据类型
                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            //截边5%的距离，定义文字显示(由于不同的图片显示的高和宽不同，所以按百分比截取)
            int yPixlesFromBottom = (int)(phHeight * .05);

            //定义在图片上文字的位置
            float wmHeight = crSize.Height;
            float wmWidth = crSize.Width;

            float xPosOfWm;
            float yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = phHeight / 2;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth / 2 + wmWidth / 2;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth / 2 + wmWidth / 2;
                    yPosOfWm = wmHeight / 2 + Padding;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = wmWidth / 2 + Padding;
                    yPosOfWm = wmHeight / 2 + Padding;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = wmWidth / 2 + Padding;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = wmHeight / 2 + Padding;
                    break;
                default:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
            }

            imgPhoto.Dispose();//释放底图，解决图片保存时 “GDI+ 中发生一般性错误。”

            //封装文本布局信息（如对齐、文字方向和 Tab 停靠位），显示操作（如省略号插入和国家标准 (National) 数字替换）和 OpenType 功能。
            StringFormat StrFormat = new StringFormat();

            //定义需要印的文字居中对齐
            StrFormat.Alignment = StringAlignment.Center;

            //SolidBrush:定义单色画笔。画笔用于填充图形形状，如矩形、椭圆、扇形、多边形和封闭路径。
            //这个画笔为描绘阴影的画笔，呈灰色
            int m_alpha = Convert.ToInt32(255 * alpha);
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(m_alpha, 0, 0, 0));

            //描绘文字信息，这个图层向右和向下偏移一个像素，表示阴影效果
            //DrawString 在指定矩形并且用指定的 Brush 和 Font 对象绘制指定的文本字符串。
            grPhoto.DrawString(waterWords,                        //string of text
                          crFont,                                 //font
                          semiTransBrush2,                        //Brush
                          new PointF(xPosOfWm + 1, yPosOfWm + 1), //Position
                          StrFormat);

            //从四个 ARGB 分量（alpha、红色、绿色和蓝色）值创建 Color 结构，这里设置透明度为153
            //这个画笔为描绘正式文字的笔刷，呈白色
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //第二次绘制这个图形，建立在第一次描绘的基础上
            grPhoto.DrawString(waterWords,                //string of text
                          crFont,                         //font
                          semiTransBrush,                 //Brush
                          new PointF(xPosOfWm, yPosOfWm), //Position
                          StrFormat);

            //imgPhoto是我们建立的用来装载最终图形的Image对象
            //bmPhoto是我们用来制作图形的容器，为Bitmap对象
            imgPhoto = bmPhoto;
            //释放资源，将定义的Graphics实例grPhoto释放，grPhoto功德圆满
            grPhoto.Dispose();

            //将grPhoto保存
            if (!System.IO.Directory.Exists(TargetPicPath))
                System.IO.Directory.CreateDirectory(TargetPicPath);
            try
            {
                imgPhoto.Save(targetImage, PicFormat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            imgPhoto.Dispose();

            return TargetPicPath + TargetPicName + "." + PicFormat.ToString().ToLower();
        }

        public SaveIamge DrawWordsForSaveIamge(string sourcePictureName,
                         string sourcePicturePath,
                        string waterWords,
                        float alpha,
                        FontFamilys fontFamily,
                        FontStyle style,
                        ImagePosition position)
        {
            //
            // 判断参数是否有效
            //
            //
            // 判断参数是否有效
            //
            if (sourcePictureName == string.Empty || waterWords == string.Empty || alpha == 0.0 || sourcePicturePath == string.Empty)
            {
                return new SaveIamge();
            }

            if (!sourcePicturePath.EndsWith(@"\"))
                sourcePicturePath = sourcePicturePath + @"\";



            //
            // 源图片全路径
            //
            string _sourcePictureName = sourcePicturePath + sourcePictureName;
            if (!File.Exists(_sourcePictureName))
                throw new FileNotFoundException(_sourcePictureName + " file not found!");
            string fileExtension = System.IO.Path.GetExtension(_sourcePictureName).ToLower();

            //
            // 判断文件是否存在,以及文件名是否正确
            //
            if (System.IO.File.Exists(_sourcePictureName) == false || (
              fileExtension != ".gif" &&
              fileExtension != ".jpg" &&
              fileExtension != ".png"))
            {
                return new SaveIamge();
            }

            //
            // 目标图片名称及全路径
            //
            TargetPicName = "HelpName_" + sourcePictureName;
            //string targetImage = TargetPicPath == string.Empty ?
            //    _sourcePictureName.Replace(System.IO.Path.GetExtension(_sourcePictureName), "") + TargetPicName + "." + PicFormat.ToString().ToLower() :
            //    TargetPicPath + TargetPicName + "." + PicFormat.ToString().ToLower();
            TargetPicPath = sourcePicturePath;
            string targetImage=TargetPicPath+TargetPicName+"." + PicFormat.ToString().ToLower();
            //创建一个图片对象用来装载要被添加水印的图片
            Image imgPhoto = Image.FromFile(_sourcePictureName);

            //获取图片的宽和高
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //
            //建立一个bitmap，和我们需要加水印的图片一样大小
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, imgPhoto.PixelFormat);

            //SetResolution：设置此 Bitmap 的分辨率
            //这里直接将我们需要添加水印的图片的分辨率赋给了bitmap
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //Graphics：封装一个 GDI+ 绘图图面。
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //设置图形的品质
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //将我们要添加水印的图片按照原始大小描绘（复制）到图形中
            grPhoto.DrawImage(
                imgPhoto,                    //  要添加水印的图片
                new Rectangle(0, 0, phWidth, phHeight), // 根据要添加的水印图片的宽和高
                0,                           // X方向从0点开始描绘
                0,                           // Y方向

                phWidth,                     // X方向描绘长度
                phHeight,                    // Y方向描绘长度
                GraphicsUnit.Pixel);         // 描绘的单位，这里用的是像素

            //根据图片的大小我们来确定添加上去的文字的大小
            //在这里我们定义一个数组来确定
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4, 2 };

            //字体
            Font crFont = null;
            //矩形的宽度和高度，SizeF有三个属性，分别为Height高，width宽，IsEmpty是否为空
            SizeF crSize = new SizeF();

            //利用一个循环语句来选择我们要添加文字的型号
            //直到它的长度比图片的宽度小
            //for (int i = 0; i < 8; i++)
            //{
            //    crFont = new Font(fontFamily.ToString(), sizes[i], style);

            //    //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。
            //    crSize = grPhoto.MeasureString(waterWords, crFont);

            //    // ushort 关键字表示一种整数数据类型
            //    if ((ushort)crSize.Width < (ushort)phWidth)
            //        break;
            //}

            crFont = new Font(fontFamily.ToString(), 20, style);

            //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。
            crSize = grPhoto.MeasureString(waterWords, crFont);

            // ushort 关键字表示一种整数数据类型
            //if ((ushort)crSize.Width < (ushort)phWidth)
            //    break;

            //截边5%的距离，定义文字显示(由于不同的图片显示的高和宽不同，所以按百分比截取)
            int yPixlesFromBottom = (int)(phHeight * .05);

            //定义在图片上文字的位置
            float wmHeight = crSize.Height;
            float wmWidth = crSize.Width;

            float xPosOfWm;
            float yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = phHeight / 2;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth / 2 + wmWidth / 2;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth / 2 + wmWidth / 2;
                    yPosOfWm = wmHeight / 2 + Padding;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = wmWidth / 2 + Padding;
                    yPosOfWm = wmHeight / 2 + Padding;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = wmWidth / 2 + Padding;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = wmHeight / 2 + Padding;
                    break;
                default:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - Padding;
                    break;
            }

            imgPhoto.Dispose();//释放底图，解决图片保存时 “GDI+ 中发生一般性错误。”

            //封装文本布局信息（如对齐、文字方向和 Tab 停靠位），显示操作（如省略号插入和国家标准 (National) 数字替换）和 OpenType 功能。
            StringFormat StrFormat = new StringFormat();

            //定义需要印的文字居中对齐
            StrFormat.Alignment = StringAlignment.Center;
            
            //SolidBrush:定义单色画笔。画笔用于填充图形形状，如矩形、椭圆、扇形、多边形和封闭路径。
            //这个画笔为描绘阴影的画笔，呈灰色
            int m_alpha = Convert.ToInt32(255 * alpha);
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(m_alpha, 0, 0, 0));

            //描绘文字信息，这个图层向右和向下偏移一个像素，表示阴影效果
            //DrawString 在指定矩形并且用指定的 Brush 和 Font 对象绘制指定的文本字符串。
            grPhoto.DrawString(waterWords,                        //string of text
                          crFont,                                 //font
                          semiTransBrush2,                        //Brush
                          new PointF(xPosOfWm + 1, yPosOfWm + 1), //Position
                          StrFormat);

            //从四个 ARGB 分量（alpha、红色、绿色和蓝色）值创建 Color 结构，这里设置透明度为153
            //这个画笔为描绘正式文字的笔刷，呈白色
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

            //第二次绘制这个图形，建立在第一次描绘的基础上
            grPhoto.DrawString(waterWords,                //string of text
                          crFont,                         //font
                          semiTransBrush,                 //Brush
                          new PointF(xPosOfWm, yPosOfWm), //Position
                          StrFormat);

            //imgPhoto是我们建立的用来装载最终图形的Image对象
            //bmPhoto是我们用来制作图形的容器，为Bitmap对象
            imgPhoto = bmPhoto;
            //释放资源，将定义的Graphics实例grPhoto释放，grPhoto功德圆满
            grPhoto.Dispose();

            //将grPhoto保存
            if (!System.IO.Directory.Exists(TargetPicPath))
                System.IO.Directory.CreateDirectory(TargetPicPath);
            try
            {
                imgPhoto.Save(targetImage, PicFormat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            imgPhoto.Dispose();

            SaveIamge model = new SaveIamge()
            {
                filename = TargetPicName + "." + PicFormat.ToString().ToLower(),
                showImg = TargetPicPath
            };

            return model;
        }

        /// 无损压缩图片    
        /// <param name="sFile">原图片</param>    
        /// <param name="dFile">压缩后保存位置</param>    
        /// <param name="dHeight">高度</param>    
        /// <param name="dWidth">宽度</param>    
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>    
        /// <returns></returns>    

        public bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放  
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量    
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100    
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径    
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
    }

    /// <summary>
    /// 装载水印图片的相关信息
    /// </summary>
    public class WaterImage
    {
        public WaterImage()
        {

        }

        private string m_sourcePicture;
        /// <summary>
        /// 源图片地址名字(带后缀)

        /// </summary>
        public string SourcePicture
        {
            get { return m_sourcePicture; }
            set { m_sourcePicture = value; }
        }

        private string m_waterImager;
        /// <summary>
        /// 水印图片名字(带后缀)
        /// </summary>
        public string WaterPicture
        {
            get { return m_waterImager; }
            set { m_waterImager = value; }
        }

        private float m_alpha;
        /// <summary>
        /// 水印图片文字的透明度
        /// </summary>
        public float Alpha
        {
            get { return m_alpha; }
            set { m_alpha = value; }
        }

        private ImagePosition m_postition;
        /// <summary>
        /// 水印图片或文字在图片中的位置
        /// </summary>
        public ImagePosition Position
        {
            get { return m_postition; }
            set { m_postition = value; }
        }

        private string m_words;
        /// <summary>
        /// 水印文字的内容
        /// </summary>
        public string Words
        {
            get { return m_words; }
            set { m_words = value; }
        }

    }

    #endregion


}