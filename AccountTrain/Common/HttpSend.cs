using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace Common
{
    public class HttpSend
    {
        public static string sendURL = "http://www1.jc-chn.cn/smsSend.do";
        public static string sendDataURL = "http://www1.jc-chn.cn/sendData.do";
        public static string queryBalanceURL = "http://www1.jc-chn.cn/balanceQuery.do";
        public static string changePasswordURL = "http://www1.jc-chn.cn/passwordUpdate.do";
        private string userName;
        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }
        private string password;
        public string Passwrd
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }
        private string pwd;
        public string Pwd
        {
            get
            {
                return this.pwd;
            }
            set
            {
                this.pwd = value;
            }
        }


        /*
         * 构造函数
         */
        private HttpSend(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
            this.pwd = GetMD5(userName + GetMD5(password));
        }

        public HttpSend()
        {
        }

        /*
       * 获取单例的静态内部类
       */
        static class HttpSendInner
        {
            internal static HttpSend httpSendInstance = null;

            public static HttpSend getInstance(string userName, string password)
            {
                if (httpSendInstance == null)
                {
                    httpSendInstance = new HttpSend(userName, password);
                }
                return httpSendInstance;
            }

        }

        /*
         * 外部获取实例
         */
        public static HttpSend getInstance(string userName, string password)
        {
            return HttpSendInner.getInstance(userName, password);
        }

        /*
         * 外部类修改对象
         */
        public static void changeInstance(string userName, string password)
        {
            if (HttpSendInner.httpSendInstance == null)
            {
                throw new Exception("Get instance!");
            }
            HttpSendInner.httpSendInstance.UserName = userName;
            HttpSendInner.httpSendInstance.password = password;
            HttpSendInner.httpSendInstance.Pwd = GetMD5(userName + GetMD5(password));
        }

        /*
        * 方法名称：GetMD5
        * 功    能：字符串MD5加密
        * 参    数：待转换字符串
        * 返 回 值：加密之后字符串
        */
        public static string GetMD5(string sourceStr)
        {
            string resultStr = "";

            byte[] b = System.Text.Encoding.Default.GetBytes(sourceStr);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            for (int i = 0; i < b.Length; i++)
                resultStr += b[i].ToString("x").PadLeft(2, '0');

            return resultStr;
        }

        /*
       * 方法名称：HttpPost
       * 功    能：以post方式进行提交
       * 参    数：url(发送地址)，postData（发送的数据）
       * 返 回 值：提交后的返回状态
       */
        public string HttpPost(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.KeepAlive = false;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] bodyBytes = encoding.GetBytes(postData);
            request.ContentLength = bodyBytes.Length;
            using (Stream serviceRequestBodyStream = request.GetRequestStream())
            {
                serviceRequestBodyStream.Write(bodyBytes, 0, bodyBytes.Length);
                serviceRequestBodyStream.Close();

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string result = reader.ReadToEnd();
                            return result;
                        }
                    }
                }
                catch (WebException webex)
                {
                    HttpWebResponse res = (HttpWebResponse)webex.Response;
                    StreamReader reader = new StreamReader(res.GetResponseStream());
                    string html = reader.ReadToEnd();
                    throw new Exception(html, webex);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*
        * 方法名称：HttpGet
        * 功    能：以get方式进行提交
        * 参    数：url(发送地址)
        * 返 回 值：提交后的返回状态
        */
        public string HttpGet(string url, string postData)
        {
            string URL = url + "?" + postData;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.KeepAlive = false;
            request.Method = "GET";
            request.ContentType = "html/text;charset=utf-8";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();
                        return result;
                    }
                }
            }
            catch (WebException webex)
            {
                HttpWebResponse res = (HttpWebResponse)webex.Response;
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string html = reader.ReadToEnd();
                throw new Exception(html, webex);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> readTXT(string path)
        {
            List<string> str = new List<string>();
            string line;
            StreamReader sr = new StreamReader(path, false);
            int i = 0;
            while(i<598)//
            {
                line=sr.ReadLine().ToString();
                str.Add(line);
                Console.WriteLine(line);
                i++;
            }
            sr.Close();
            return str;
        }
    }
    
}
