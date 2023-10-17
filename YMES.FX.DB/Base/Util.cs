using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.DB.Base
{
    public class Util
    {
        public static string GetRealIP(string httpAddress)
        {
            string ip = httpAddress.ToLower().Replace("http://", "").Replace("https://", "");
            ip = ip.Split(':')[0];
            ip = ip.Split('/')[0];
            ip = System.Net.Dns.GetHostEntry(ip).AddressList[0].ToString();
            return ip;
        }
        public static bool WriteTxtLog(string log, string writeDIR = ".\\DB_LOG")
        {
            bool bRet = true;
            try
            {

                if (!System.IO.Directory.Exists(writeDIR))
                {
                    System.IO.Directory.CreateDirectory(writeDIR);
                }
                string tagetFile = writeDIR + "\\"
                        + System.DateTime.Today.ToString("yyyyMMdd") + ".txt";

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(tagetFile, true))
                {
                    sw.WriteLine(DateTime.Now + "\t" + log);
                }


            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
                bRet = false;
            }

            return bRet;
        }
        
    }
}
