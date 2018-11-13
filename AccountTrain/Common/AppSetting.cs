using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AppSetting
    {

        public int AppCacheTime
        {
            get
            {
                int parseValue = 0;

                if (int.TryParse(ConfigurationManager.AppSettings["App_CacheTime"], out parseValue))
                {
                    return parseValue;
                }
                return parseValue;
            }
        }

        public int AppSessionTime
        {
            get
            {
                int parseValue = 0;

                if (int.TryParse(ConfigurationManager.AppSettings["App_SessionTime"], out parseValue))
                {
                    return parseValue;
                }
                return parseValue;
            }
        }

        public string AppDESKey
        {
            get
            {
                return ConfigurationManager.AppSettings["App_DESKey"];
            }
        }

        public string APPMD5Salt
        {
            get
            {
                return ConfigurationManager.AppSettings["App_MD5Salt"];
            }
        }

        public string AppUploadFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["App_UploadFolder"];
            }
        }

        public string AppDomainName
        {
            get
            {
                return ConfigurationManager.AppSettings["App_DomainName"];
            }
        }

        public int AppDevMode
        {
            get
            {
                int parseValue = 0;

                if (int.TryParse(ConfigurationManager.AppSettings["App_DevMode"], out parseValue))
                {
                    return parseValue;
                }
                return parseValue;
            }
        }

        public string WeiXinAppId
        {
            get
            {
                return ConfigurationManager.AppSettings["WeiXin_AppId"];
            }
        }

        public string WeiXinAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["WeiXin_AppSecret"];
            }
        }

        public string WeiXinAppToken
        {
            get
            {
                return ConfigurationManager.AppSettings["WeiXin_AppToken"];
            }
        }

        public string IsDebug
        {
            get
            {
                return ConfigurationManager.AppSettings["IsDebug"];
            }
        }

        public string WeiXinEncodingAESKey
        {
            get
            {
                return ConfigurationManager.AppSettings["WeiXin_EncodingAESKey"];
            }
        }


        public string AdminAccount
        {
            get
            {
                return ConfigurationManager.AppSettings["Admin_Account"];
            }
        }

        public string AdminPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["Admin_Password"];
            }
        }

        public static string SendMessageHearder
        {
            get
            {
                return ConfigurationManager.AppSettings["SendMessageHearder"];
            }
        }
        public static string SendMessageUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["SendMessageUserName"];
            }
        }
        public static string SendMessagePassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SendMessagePassword"];
            }
        }
        public static string StoreRegisteredPath
        {
            get
            {
                return ConfigurationManager.AppSettings["StoreRegisteredPath"];
            }
        }

        public string Sms_Corp_SubCreate
        {
            get
            {
                return ConfigurationManager.AppSettings["Sms_Corp_SubCreate"];
            }
        }

        public string MCHID
        {
            get
            {
                return ConfigurationManager.AppSettings["MCHID"];
            }
        }

        public string KEY
        {
            get
            {
                return ConfigurationManager.AppSettings["KEY"];
            }
        }

        public string SSLCERT_PATH
        {
            get
            {
                return ConfigurationManager.AppSettings["SSLCERT_PATH"];
            }
        }

        public string SSLCERT_PASSWORD
        {
            get
            {
                return ConfigurationManager.AppSettings["SSLCERT_PASSWORD"];
            }
        }

        public string NotifyUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["NotifyUrl"];
            }
        }

        public int PayRate
        {
            get
            {
                int parseValue = 0;

                if (int.TryParse(ConfigurationManager.AppSettings["PayRate"], out parseValue))
                {
                    return parseValue;
                }
                return parseValue;
            }
        }

        public int PayDate
        {
            get
            {

                int parseValue = 0;

                if (int.TryParse(ConfigurationManager.AppSettings["PayDate"], out parseValue))
                {
                    return parseValue;
                }
                return parseValue;

            }
        }

        public static string Settings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }

}
