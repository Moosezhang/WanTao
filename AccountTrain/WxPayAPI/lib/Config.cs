using System;
using System.Collections.Generic;
using System.Web;

namespace WxPayAPI
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {

      
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        //public const string APPID = "wxa7f322fb262e5a7b";
        //public const string MCHID = "1269013001";
        //public const string KEY = "5b3e2e7ab3d5448cb611283f52b1j382";
        //public const string APPSECRET = "e98bba74ca94397c4b8ccae6f566cdc7";
        public const string APPID = "wx6f3ebf83d4ab1223";
        public const string MCHID = "1518437731";
        public const string KEY = "999e8b64e5341612cafb21f2afbf7099";
        public const string APPSECRET = "7f92b7c43082c0b1268286a79f79c0d0";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "/cert/apiclient_cert.p12";
        public const string SSLCERT_PASSWORD = "1518437731";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NOTIFY_URL = "http://www.wantaocs.com/WxBase/PayNotifyUrl";



        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "47.101.153.162";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 0;
    }
}