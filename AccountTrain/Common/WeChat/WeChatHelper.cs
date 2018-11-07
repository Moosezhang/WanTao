using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WeChat
{
    public class WeChatHelper
    {
        public static DateTime ConvertTimestampToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(timeStamp + "0000000");

            TimeSpan toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);
        }

        public static int CovertDatetimeToTimestamp(DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(dateTime - startTime).TotalSeconds;
        }

        public static string GetJsApiSignature(string ticket, string nonce, int timestamp, string url, Func<string, string> hash)
        {
            string rtn = string.Empty;

            string org = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, nonce, timestamp, url);

            rtn = hash(org);

            return rtn;
        }
    }
}
