using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class CommonHelper
    {
        public const string SESSION_KEY_USERINFO = "";

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            var addressArray = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (addressArray == null || addressArray.Count() == 0)
            {
                return string.Empty;
            }
            var address = addressArray.FirstOrDefault(a => a.AddressFamily.ToString() == "InterNetwork");
            if (address == null)
            {
                return string.Empty;
            }
            return address.ToString();
        }

        /// <summary>
        /// 是否正确格式邮箱
        /// </summary>
        /// <param name="emailStr"></param>
        /// <returns></returns>
        public static bool IsEmail(string emailStr)
        {
            return Regex.IsMatch(emailStr, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 是否正确格式电话号
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsTelephone(string phoneNumber)
        {
            //电信手机号码正则       
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex dReg = new Regex(dianxin);
            //联通手机号正则       
            string liantong = @"^1[34578][01256]\d{8}$";
            Regex tReg = new Regex(liantong);
            //移动手机号正则        
            string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
            Regex yReg = new Regex(yidong);

            if (dReg.IsMatch(phoneNumber) || tReg.IsMatch(phoneNumber) || yReg.IsMatch(phoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
            //return Regex.IsMatch(phoneNumber, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        /// 是否正确格式手机号
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public bool IsMobile(string mobileNumber)
        {
            return Regex.IsMatch(mobileNumber, @"^[1]+[3,5]+\d{9}");
        }

        /// <summary>
        /// 获取微信重定向
        /// </summary>
        /// <param name="RedirectUrl">地址</param>
        /// <returns></returns>
        public static string GetRedirect(string RedirectUrl) 
        {
            AppSetting setting = new AppSetting();
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=http%3a%2f%2f{1}%2f{2}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", setting.WeiXinAppId, setting.AppDomainName.Replace("http://", ""), RedirectUrl);
        }

        /// <summary>  
        /// 生成随机数字  
        /// </summary>  
        /// <param name="Length">生成长度</param>  
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>  
        public static string CreateNumber(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(1);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        /// <summary>  
        /// 生成订单号（yyyyMMdd）
        /// </summary>  
        /// <param name="Length">生成长度</param>  
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>  
        public static string CreateOrderNo()
        {
            
            string result = "";
            result = DateTime.Now.ToString("yyyyMMdd") + CreateNumber(4,false);
            return result;
        }

        public static string LinkImageUrl(string url)
        {
            return ConfigurationManager.AppSettings["imgMapPath"] + url;
        }
        public static int GetRandNum(int min, int max)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return r.Next(min, max);
        }
    }
}
