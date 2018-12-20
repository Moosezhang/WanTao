using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GroupOver
{
    public class LogHelp
    {
        public static void WriteLog(string strMsg)
        {
            string filename = "/logs/" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt";
            if (!Directory.Exists("/logs"))
                Directory.CreateDirectory("/logs");
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
