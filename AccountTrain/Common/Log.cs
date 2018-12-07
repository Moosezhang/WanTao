using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class LogHelp
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
