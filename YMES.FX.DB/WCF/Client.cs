using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.DB.WCF
{
    public class Client
    {
        private string m_Port = "";
        private string m_IP = "";
        private int m_timeOutMilsec = 1000;
        private WCF_DB m_Socket = null;

        public Client(string wcfURL, string ip, string port, int timeOutMilsec)
        {
            if (m_Socket == null)
            {
                m_Socket = new WCF.WCF_DB();
            }
            m_Socket.Url = wcfURL;
            m_Port = port;
            m_IP = ip;
            m_timeOutMilsec = timeOutMilsec;

        }

        public DataTable ExecuteQuery(string query, Dictionary<string, string> param)
        {
            var kvpList = new List<ArrayOfKeyValueOfstringstringKeyValueOfstringstring>();
            if (param != null)
            {
                foreach (var kv in param)
                {
                    var kvp = new ArrayOfKeyValueOfstringstringKeyValueOfstringstring();

                    kvp.Key = kv.Key;
                    kvp.Value = kv.Value;

                    kvpList.Add(kvp);
                }
            }
            try
            {
                if (false == ConnectTest(m_IP, Convert.ToInt32(m_Port), m_timeOutMilsec))
                {
                    throw (new Exception("[" + m_IP + ":" + m_Port + "]" + "WCF Connection Error"));
                }

                DataSet ds = m_Socket.ExecuteQuery(query, kvpList.ToArray());

                foreach (DataTable dt in ds.Tables)
                {
                    return dt;
                }

                return new DataTable();
            }
            catch (Exception eLog)
            {

                throw (new Exception("[" + query + "]" + eLog.Message));

            }
        }
        public int ExecuteNonQuery(string query, Dictionary<string, string> param)
        {
            var kvpList = new List<ArrayOfKeyValueOfstringstringKeyValueOfstringstring>();

            foreach (var kv in param)
            {
                var kvp = new ArrayOfKeyValueOfstringstringKeyValueOfstringstring();

                kvp.Key = kv.Key;
                kvp.Value = kv.Value;

                kvpList.Add(kvp);
            }

            int result;
            bool resultSpecified;

            try
            {
                m_Socket.ExecuteNonQuery(query, kvpList.ToArray(), out result, out resultSpecified);

                return result;
            }
            catch (Exception eLog)
            {
                throw (new Exception("[" + query + "]" + eLog.Message));

            };
        }
        public bool IsConnected()
        {
            return ConnectTest(m_IP, Convert.ToInt32(m_Port), m_timeOutMilsec);
        }
        private bool ConnectTest(string ip, int port, int millisecondsTimeout)
        {
            bool result = false;
            System.Net.Sockets.Socket socket = null;

            try
            {
                ip = System.Net.Dns.GetHostEntry(ip).AddressList[0].ToString();
                socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.DontLinger, false);
                IAsyncResult ret = socket.BeginConnect((new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), port)), null, null);

                result = ret.AsyncWaitHandle.WaitOne(millisecondsTimeout, false);
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine(eLog.Message);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
            return result;
        }
        public void Close()
        {
            m_Socket.Dispose();
        }


    }
}
