using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YMES.FX.PLC.Base;

namespace YMES.FX.PLC.LS
{
    public class FENET : LS_PLCBase, IPLC
    {
        private Dictionary<string, string> m_dicConnectionArgs = null;
        private string m_CompanyID = "LGIS-GLOFA";
        private WordStringEnum m_WordString = WordStringEnum.D;
        private string m_MemoryTag = "D";
        private bool m_bThreadConnecting = false;
        public string MemoryTag
        {
            get { return m_MemoryTag; }
            set { m_MemoryTag = value; }
        }
        public enum WordStringEnum
        {
            M,
            D,
            P,
            L,
            F,
            T,
            C,
            S
        }

        private bool m_IsDebug = false;
        private bool m_AutoReplay = true;

        private Socket m_clientSocket = null;
        private int m_AutoReplyTime = 2000;
        private string m_PLC_IP = "";
        private string m_PLC_PORT = "";

        private bool m_InnerConnected = false;
        private bool m_bClose = false;
        private string m_prvStrBuffer = "";
        private bool m_AutoReplyCondition = false;
        private bool m_AutoProtectDisconnection = false;

        /// <summary>
        /// Protect the disconnection of timer
        /// If you use the PLC control in FX.DEVICE, you don't need.
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public bool AutoProtectDisconnection
        {
            get { return m_AutoProtectDisconnection; }
            set { m_AutoProtectDisconnection = value; }
        }

        /// <summary>
        /// If you want to see the log, use this
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public bool IsDebug
        {
            get { return m_IsDebug; }
            set { m_IsDebug = value; }
        }
        /// <summary>
        /// ex] D3210 / M3210 -> D or M ...
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public WordStringEnum WordString
        {
            get { return m_WordString; }
            set { m_WordString = value; }
        }



        /// <summary>
        /// IP address of PLC
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public string PLC_IP
        {
            get { return m_PLC_IP; }
            set { m_PLC_IP = value; }
        }
        /// <summary>
        /// Comunication port of PLC(2004)
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public string PLC_PORT
        {
            get { return m_PLC_PORT; }
            set { m_PLC_PORT = value; }
        }
        /// <summary>
        /// Reply to PLC(Automatically)
        /// If there is no response to PLC, it should be disconnected automatically
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public bool AutoReplay
        {
            get { return m_AutoReplay; }
            set { m_AutoReplay = value; }
        }
        /// <summary>
        /// Time of response(millisecond) 
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public int AutoReplyTime
        {
            get { return m_AutoReplyTime; }
            set { m_AutoReplyTime = value; }
        }
        /// <summary>
        /// Condition of Connection 
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public bool Connected
        {
            get
            {
                return m_InnerConnected;
            }
        }
        /// <summary>
        /// Header area's CompanyID
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public string CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; }
        }
        public string GetBitVar(PlcModelEnum mode, string tag, string baseAddr, int seq)
        {

            if (mode == PlcModelEnum.XGT)
            {
                return tag + string.Format("{0:00000}", Convert.ToInt32(baseAddr) * 16 + seq);
            }
            else if (mode == PlcModelEnum.XGB)
            {
                int overFlow = seq / 16;
                int subSeq = seq - (overFlow * 16);

                string hex = Common.ToHexString(subSeq);
                return tag + string.Format("{0:0000}", Convert.ToInt32(baseAddr) + overFlow) + hex;
            }
            return tag + string.Format("{0:00000}", Convert.ToInt32(baseAddr) * 16 + seq);

        }
        /// <summary>
        /// Get the PLC communication's Header Area
        /// </summary>
        /// <param name="length">Length of Command area</param>
        /// <returns>Header</returns>
        protected byte[] GetHeaderArea(int length)
        {

            List<byte> lstRet = new List<byte>();
            if (CompanyID.Length < 10)
            {
                Common.CatListString(ref lstRet, CompanyID.PadRight(10, '\x00'));    //Cmpany ID(10 Byte)
            }
            else
            {
                Common.CatListString(ref lstRet, CompanyID.Substring(0, 10));    //Cmpany ID(10 Byte)
            }
            Common.CatListString(ref lstRet, "\x00" + "\x00"); //PLC Info(2 Byte) 
            Common.CatListString(ref lstRet, "\x00"); //CPU Info(1 Byte)
            Common.CatListString(ref lstRet, "\x33"); //Source of Frame(1 Byte)
            Common.CatListString(ref lstRet, "\x00" + "\x00"); //Invoke ID(2 Byte) -- Little Endian

            Common.CatListString(ref lstRet, Encoding.ASCII.GetString(Common.GetByteFromLong((long)length, 2))); //Length(2 Byte)-- Little Endian
            Common.CatListString(ref lstRet, "\x00"); //FEnet Position(1 Byte)
            Common.CatListString(ref lstRet, "\x00"); //Reserved2(1 Byte)

            return lstRet.ToArray();
        }

        /// <summary>
        /// Get the communication's command area(reand Only)
        /// </summary>
        /// <param name="nAddr">Access address</param>
        /// <param name="dataType">prefix of variable name(ex] %DX3210 -> DX)</param>
        /// <returns>Command</returns>
        protected byte[] GetCommandArea(string nAddr, string dataType)
        {
            return GetCommandArea(false, nAddr, "", dataType);
        }
        /// <summary>
        /// Get the communication's command area(reand Only)
        /// </summary>
        /// <param name="nAddr">Access address</param>
        /// <param name="dataType">prefix of variable name(ex] %DX3210 -> DX)</param>
        /// <param name="bContinue">Continuos data</param>
        /// <param name="size">size of data</param>
        /// <returns>Command</returns>
        protected byte[] GetCommandArea(string nAddr, string dataType, bool bContinue, long size)
        {
            return GetCommandArea(false, nAddr, "", dataType, bContinue, size);
        }
        /// <summary>
        /// Get the communication's command area(reand/write)
        /// </summary>
        /// <param name="isWrite"></param>
        /// <param name="nAddr"></param>
        /// <param name="val"></param>
        /// <param name="dataType"></param>
        /// <param name="bContinue"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected byte[] GetCommandArea(bool isWrite, string nAddr, string val, string dataType, bool bContinue = false, long size = 0)
        {
            List<byte> lstRet = new List<byte>();
            try
            {
                long nSize = size;
                if (nSize == 0)
                {
                    nSize = SizeOfRcvBuffer;
                }
                string varName = "%" + dataType + nAddr.ToString();

                string hexDataType = "\x14";    //Continuous
                switch (dataType.Substring(1, 1))
                {
                    case "X":   //Bit
                        hexDataType = "\x00";
                        break;
                    case "B":   //Byte
                        hexDataType = "\x01";

                        break;
                    case "W":   //Word
                        hexDataType = "\x02";

                        break;
                    case "D":   //Double Word
                        hexDataType = "\x03";
                        break;
                    case "L":   //Long Word
                        hexDataType = "\x04";
                        break;

                }
                if (bContinue)
                {
                    hexDataType = "\x14";    //Continuous
                }


                if (isWrite)
                {   //Write
                    Common.CatListString(ref lstRet, "\x58" + "\x00");     //INSTRUCTION(2)
                }
                else
                {   //Read
                    Common.CatListString(ref lstRet, "\x54" + "\x00");     //INSTRUCTION(2)
                }
                Common.CatListString(ref lstRet, hexDataType + "\x00");     //DATA TYPE(2)
                Common.CatListString(ref lstRet, "\x00" + "\x00");     //RESERVERED(2)
                Common.CatListString(ref lstRet, "\x01" + "\x00");     //COUNT(2)
                Common.CatListString(ref lstRet, Encoding.ASCII.GetString(Common.GetByteFromLong(varName.Length, 2)));     //Variable Name Length
                Common.CatListString(ref lstRet, "%" + dataType + nAddr.ToString());   //ADDRESS
                if (isWrite == true && val.Length > 0)
                {

                    //byte[] bytmp = Common.GetByteFromLong(long.Parse(val, System.Globalization.NumberStyles.AllowHexSpecifier), val.Length);

                    byte[] bytmp = Common.GetByte(val);
                    if (val == "0")
                    {
                        val = "00";
                    }
                    else if (val == "1")
                    {
                        val = "01";
                    }
                    Common.CatListString(ref lstRet, Encoding.ASCII.GetString(Common.GetByteFromLong(val.Length, 2)));  //DATA SIZE(2)
                    //foreach (byte dataVal in bytmp)
                    for (int i = 0; i < val.Length; i++)
                    {
                        if (bytmp.Length > i)
                        {
                            lstRet.Add(bytmp[i]);   //DATA VALUE
                        }
                        else
                        {
                            lstRet.Add(0);
                        }

                    }
                }
                if (bContinue)
                {
                    Common.CatListString(ref lstRet, Encoding.ASCII.GetString(Common.GetByteFromLong(size, 2)));
                }
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return lstRet.ToArray();
        }
        public string GetBitTag()
        {
            return m_MemoryTag + "X";
        }
        /// <summary>
        /// Entry Point of PLC Module
        /// </summary>
        /// <param name="ip">ip address of PLC</param>
        /// <param name="port">port of PLC</param>
        public void Start(string ip, string port)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@PLCIP", ip);
            param.Add("@PLC_PORT", port);
            Start(param);
        }
        public void Start(Dictionary<string, string> param)
        {
            if (param.ContainsKey("@PLCIP"))
            {
                PLC_IP = param["@PLCIP"];
            }
            if (param.ContainsKey("@PLC_PORT"))
            {
                PLC_PORT = param["@PLC_PORT"];
            }
            Connect(param);

        }
        /// <summary>
        /// Print the debugging message
        /// </summary>
        /// <param name="tag">RCV/SND ...</param>
        /// <param name="buffer">Data</param>
        /// <returns>debugging message</returns>
        private string GetDebugAsyncResult(string tag, byte[] buffer)
        {
            string ret = "";
            try
            {
                if (buffer != null)
                {
                    byte[] rslt = buffer;

                    ret += tag + "-LEN:" + rslt.Length + " Bytes]";

                    for (int i = 0; i < rslt.Length; i++)
                    {
                        ret += rslt[i].ToString("X2");

                    }
                }
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return ret;
        }
        public void OnConnect(IAsyncResult asyn)
        {

        }
        /// <summary>
        /// Disconnect to PLC
        /// </summary>
        public void Close()
        {
            try
            {
                m_InnerConnected = false;
                m_bClose = true;
                m_clientSocket.Close();
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
        }
        public string GetDicConnectionArgs(string key)
        {

            if (m_dicConnectionArgs.ContainsKey(key))
            {
                return m_dicConnectionArgs[key];
            }
            return string.Empty;
        }
        public bool Connect(Dictionary<string, string> dicConnectParam)
        {
            m_dicConnectionArgs = dicConnectParam;
            string ip = dicConnectParam["@PLCIP"];
            string port = dicConnectParam["@PLC_PORT"];
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            if (string.IsNullOrEmpty(port))
            {
                port = "2004";
            }
            try
            {
                m_bClose = false;


                IPAddress ipAddr = IPAddress.Parse(ip);
                int iPortNo = System.Convert.ToInt16(port);

                // Connect to the remote host
                if (m_clientSocket != null)
                {
                    if (m_clientSocket.Connected && m_InnerConnected == true)
                    {
                        return true;
                    }
                }
                if (string.IsNullOrEmpty(CompanyID))
                {
                    CompanyID = "LGIS-GLOFA";
                }


                if (m_bThreadConnecting == false)
                {
                    m_bThreadConnecting = true;
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("IP", ip);
                    param.Add("PORT", port);
                    System.Threading.ThreadPool.QueueUserWorkItem(thConnection, param);
                }

                if (AutoReplyTime > 0 && m_AutoReplyCondition == false)
                {
                    m_AutoReplyCondition = true;
                    System.Threading.ThreadPool.QueueUserWorkItem(thAutoReply);
                }
                return true;

            }
            catch (Exception eLog)
            {

                m_InnerConnected = false;
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ip + "/" + eLog.Message.ToString());
                return false;
            }
        }
        private void thConnection(object args)
        {
            try
            {

                if (args is Dictionary<string, string>)
                {
                    if (m_clientSocket != null && m_clientSocket.Connected)
                    {
                        m_clientSocket.Close();
                    }
                    m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_clientSocket.ReceiveTimeout = ReceiveTimeout;
                    m_clientSocket.SendTimeout = SendTimeout;

                    string ip = ((Dictionary<string, string>)args)["IP"];
                    string portNo = ((Dictionary<string, string>)args)["PORT"];
                    IPAddress ipAddr = IPAddress.Parse(ip);
                    //        IAsyncResult result = m_clientSocket.BeginConnect(ipAddr, Convert.ToInt32(portNo), new AsyncCallback(OnConnect), null);
                    m_clientSocket.Connect(ipAddr, Convert.ToInt32(portNo));
                    // m_InnerConnected = result.AsyncWaitHandle.WaitOne(3000, true);
                    m_InnerConnected = true;
                }
            }
            catch (Exception eLog)
            {
                if (((System.Net.Sockets.SocketException)eLog).ErrorCode != 10060)
                {   //
                    System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
                }
                m_InnerConnected = false;
            }
            finally
            {
                m_bThreadConnecting = false;
            }
        }








        public bool WriteWord(string baseAddr, int seq, string val)
        {
            return WriteWord(DefaultWordType, baseAddr, seq, val);
        }
        public bool WriteWord(string tag, string baseAddr, int seq, string val)
        {
            try
            {
                bool bReturn = false;
                string strWordAddr = string.Format("{0:0000}", Convert.ToInt32(baseAddr) + seq);    //WORD is used by non-changed address
                bReturn = Write("%" + tag + strWordAddr, val);

                return bReturn;
            }
            catch
            {
                return false;
            }
        }
        public bool WriteWord(string absAddr, string val)
        {
            try
            {
                string strErrCode = "";
                string strErrmsg = "";
                bool bReturn = false;
                bReturn = PlcWrite(absAddr, val, ref strErrCode, ref strErrmsg);

                return bReturn;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteBit(int seq, bool val)
        {
            return WriteBit(BaseAddr, seq, val, DefaultBitType);
        }
        public bool WriteBit(string baseAddr, int seq, bool val)
        {
            return WriteBit(baseAddr, seq, val, DefaultBitType);
        }
        public bool WriteBit(string baseAddr, int seq, bool val, string dataType)
        {
            try
            {
                string nAddr = "";
                int iDecAddr = Convert.ToInt32(baseAddr.Substring(0, baseAddr.Length - 1)) * 16;

                int iHexaAddr = Convert.ToInt32(baseAddr.Substring(baseAddr.Length - 1)) + seq;
                string bitVar = "";
                switch (dataType)
                {
                    case "DX":
                        //nAddr = Convert.ToInt32(baseAddr) * 16 + seq;
                        bitVar = GetBitVar(PLCModel, "", baseAddr, seq);
                        nAddr = bitVar;
                        break;
                    case "MX":
                        nAddr = (iDecAddr + iHexaAddr).ToString();
                        break;
                }

                byte[] command = GetCommandArea(true, nAddr, val ? "01" : "00", dataType);
                byte[] bytesData = Common.CatByte(GetHeaderArea(command.Length), command);
                bool bRet = false;
                if (Connected)
                {
                    //ret = m_clientSocket.Send(bytesData);
                    //m_clientSocket.BeginSend(bytesData, 0, bytesData.Length, SocketFlags.None, new AsyncCallback(OnDataSend), bytesData);
                    bRet = SocketSend(bytesData);
                }
                return bRet;
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
                return false;
            }
        }

        public bool ReadBit(string baseAddr, int seq, string dataType)
        {
            string strErrCode = "";
            string strErrMsg = "";
            string strRtnValue = "";
            try
            {
                string nAddr = "";
                int iDecAddr = Convert.ToInt32(baseAddr.Substring(0, baseAddr.Length - 1)) * 16;

                int iHexaAddr = Convert.ToInt32(baseAddr.Substring(baseAddr.Length - 1)) + seq;
                string bitVar = "";
                switch (dataType)
                {
                    case "DX":
                        //nAddr = Convert.ToInt32(baseAddr) * 16 + seq;
                        bitVar = GetBitVar(PLCModel, "", baseAddr, seq);
                        nAddr = bitVar;
                        break;
                    case "MX":
                        nAddr = (iDecAddr + iHexaAddr).ToString();
                        break;
                }
                Read("%" + dataType + nAddr, out strRtnValue, out strErrCode, out strErrMsg);

            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
                return false;
            }
            return Common.GetBoolStr(strRtnValue);
        }
        public bool Read(string strVar, out string strRtnValue)
        {
            string strErrCode = "";
            string strErrMsg = "";
            return Read(strVar, out strRtnValue, out strErrCode, out strErrMsg);
        }
        public bool Read(string strVar, out string strRtnValue, out string strErrCode, out string strErrMsg, int iPlcTimeOut = 3000)
        {
            bool bRet = false;
            strErrCode = "";
            strErrMsg = "";
            strRtnValue = "";
            try
            {


                if (m_InnerConnected && m_clientSocket.Connected && strVar.Length >= 4)
                {
                    string sVAR1 = strVar.Substring(0, 1);
                    string sVAR2 = strVar.Substring(1, 2);
                    string sVAR3 = strVar.Substring(3);

                    byte[] command = GetCommandArea(sVAR3, sVAR2);

                    byte[] header = GetHeaderArea(command.Length);
                    byte[] bytesData = Common.CatByte(header, command);
                    byte[] bufferRcv = new byte[SizeOfRcvBuffer];
                    SocketError errCode = SocketError.Success;
                    m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);

                    strErrCode = Convert.ToInt32(errCode).ToString();

                    switch (errCode)
                    {
                        case SocketError.Success:
                            strErrMsg = "";
                            break;
                        default:
                            strErrMsg = errCode.ToString();
                            break;
                    }
                    DateTime startDT = DateTime.Now;
                    do
                    {
                        m_clientSocket.Receive(bufferRcv);
                        if (bufferRcv[20] == 85 && bufferRcv[22] != 20 && bufferRcv[26] == 0)
                        {
                            int nSize = bufferRcv[30];
                            strRtnValue = Common.ByteToString(bufferRcv, 32).Substring(0, nSize * 2);
                            break;
                        }
                        else
                        {
                            m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                        }
                        if ((DateTime.Now - startDT).TotalMilliseconds >= iPlcTimeOut)
                        {   //Time-out
                            strRtnValue = "";
                            break;
                        }

                    } while (true);

                    bRet = true;
                }
            }
            catch (Exception eLog)
            {
                strErrCode = "EXC";
                strErrMsg = eLog.Message.ToString();
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }

            return bRet;
        }



        public bool Write(string strVar, string val)
        {
            string strErrCode = "";
            string strErrMsg = "";
            return Write(strVar, val, out strErrCode, out strErrMsg, SendTimeout);
        }
        public bool Write(string strVar, string val, out string strErrCode, out string strErrMsg, int iPlcTimeOut = 3000)
        {
            bool bRet = false;
            strErrCode = "";
            strErrMsg = "";
            try
            {
                if (m_InnerConnected && m_clientSocket.Connected && strVar.Length >= 4)
                {
                    string sVAR1 = strVar.Substring(0, 1);
                    string sVAR2 = strVar.Substring(1, 2);
                    string sVAR3 = strVar.Substring(3);

                    byte[] command = GetCommandArea(true, sVAR3, val, sVAR2);

                    byte[] header = GetHeaderArea(command.Length);
                    byte[] bytesData = Common.CatByte(header, command);

                    SocketError errCode = SocketError.Success;
                    bRet = SocketSend(bytesData, out errCode);

                    strErrCode = Convert.ToInt32(errCode).ToString();

                    switch (errCode)
                    {
                        case SocketError.Success:
                            strErrMsg = "";
                            if (IsDebug)
                            {
                                System.Diagnostics.Debug.WriteLine("Write[" + strVar + "]" + val);
                            }
                            break;
                        default:
                            strErrMsg = errCode.ToString();
                            break;
                    }

                }
            }
            catch (Exception eLog)
            {
                strErrCode = "EXC";
                strErrMsg = eLog.Message.ToString();
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }

            return bRet;
        }
        private static string ToHexString(int by1)
        {

            byte[] bytes = BitConverter.GetBytes(by1);

            string hexString = "";
            hexString += bytes[0].ToString("X");
            return hexString;
        }
        private bool SocketSend(byte[] bytesData)
        {
            SocketError errCode = SocketError.Success;
            return SocketSend(bytesData, out errCode, 500);
        }
        private bool SocketSend(byte[] bytesData, out SocketError errCode, int iPlcTimeOut = 1000)
        {
            bool bRet = false;
            errCode = SocketError.Success;
            try
            {
                int ret = m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                byte[] bufferRcv = new byte[SizeOfRcvBuffer];
                DateTime startDT = DateTime.Now;
                do
                {
                    m_clientSocket.Receive(bufferRcv);
                    if (bufferRcv[20] == 89 && bufferRcv[22] != 20 && bufferRcv[26] == 0)
                    {   //Response(Write Response) - ACK / None Continuos Type
                        bRet = true;
                        break;
                    }
                    else
                    {
                        m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                    }
                    if ((DateTime.Now - startDT).TotalMilliseconds >= iPlcTimeOut)
                    {   //Time-out
                        System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + "TCP Receive time over-" + iPlcTimeOut);
                        bRet = false;
                        break;
                    }


                } while (true);
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return bRet;
        }

        private void thAutoReply(object arg)
        {
            try
            {
                do
                {

                    if (Connected)
                    {
                        try
                        {


                            if (m_clientSocket != null && m_clientSocket.Connected && m_AutoProtectDisconnection)
                            {
                                string rtn = "";
                                string errMsg = "";
                                string errID = "";
                                PlcRead("%M0000", ref rtn, ref errID, ref errMsg, 500);
                            }
                            else if (m_clientSocket == null || (m_clientSocket != null && m_clientSocket.Connected == false))
                            {
                                m_InnerConnected = false;
                            }
                        }
                        catch (Exception innerEX)
                        {
                            System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "/INNER]" + innerEX.Message.ToString());
                            m_InnerConnected = false;
                        }
                    }

                    if (Connected == false && m_bClose == false)
                    {
                        if (m_bThreadConnecting == false)
                        {
                            Dictionary<string, string> param = new Dictionary<string, string>();
                            param.Add("@PLCIP", m_PLC_IP);
                            param.Add("@PLC_PORT", m_PLC_PORT);
                            Connect(param);
                        }
                    }
                    System.Threading.Thread.Sleep(m_AutoReplyTime);
                } while (AutoReplay);
            }
            catch (Exception eLog)
            {

                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            finally
            {
                m_AutoReplyCondition = false;
            }
        }
        #region ORG Function
        /// <summary>
        /// Get the PLC data(BLOCK UNIT)(LEGACY)
        /// </summary>
        /// <param name="strVar">plc variable name(ex]%DB6420)</param>
        /// <param name="lReadCnt">size of block</param>
        /// <param name="strRtnValue">return value(2digit hexa code)</param>
        /// <param name="strErrCode">Code of error</param>
        /// <param name="strErrMsg">Message of error</param>
        /// <param name="iPlcTimeOut">response waiting time</param>
        /// <returns>condition of reading</returns>        
        public bool PlcReadBlock(string strVar, long lReadCnt, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {

            bool bRet = false;
            strErrCode = "";
            strErrMsg = "";
            try
            {

                if (m_InnerConnected && m_clientSocket.Connected)
                {
                    string sVAR1 = strVar.Substring(0, 1);
                    string sVAR2 = strVar.Substring(1, 2);
                    string sVAR3 = strVar.Substring(3);

                    switch (strVar.Substring(2, 1))
                    {   //Byte
                        case "B":   //Byte
                            lReadCnt = lReadCnt * 2;
                            break;
                    }

                    byte[] command = GetCommandArea(sVAR3, sVAR2, true, (int)lReadCnt);
                    byte[] header = GetHeaderArea(command.Length);
                    byte[] bytesData = Common.CatByte(header, command);
                    byte[] bufferRcv = new byte[SizeOfRcvBuffer];
                    SocketError errCode = SocketError.Success;

                    m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);

                    strErrCode = Convert.ToInt32(errCode).ToString();

                    switch (errCode)
                    {
                        case SocketError.Success:
                            strErrMsg = "";
                            break;
                        default:
                            strErrMsg = errCode.ToString();
                            break;
                    }
                    DateTime startDT = DateTime.Now;
                    do
                    {
                        try
                        {
                            if (m_InnerConnected && m_clientSocket.Connected)
                            {
                                m_clientSocket.Receive(bufferRcv, 0, bufferRcv.Length, SocketFlags.None, out errCode);
                                strErrCode = Convert.ToInt32(errCode).ToString();

                                switch (errCode)
                                {
                                    case SocketError.Success:
                                        strErrMsg = "";
                                        break;
                                    default:
                                        strErrMsg = errCode.ToString();
                                        break;
                                }
                                if (bufferRcv[20] == 85 && bufferRcv[22] == 20 && bufferRcv[26] == 0)
                                {   //Response(Normal Condition - ACK)
                                    strRtnValue = Common.ByteToString(bufferRcv, 32, (int)lReadCnt);
                                    m_prvStrBuffer = strRtnValue;
                                    break;
                                }
                                else if (errCode == SocketError.Success)
                                {
                                    m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                                }
                                else
                                {
                                    System.Threading.Thread.Sleep(10);
                                    strRtnValue = m_prvStrBuffer;
                                }
                                if ((DateTime.Now - startDT).TotalMilliseconds >= iPlcTimeOut)
                                {   //Time-out
                                    System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "/" + m_PLC_IP + "]" + "TCP Receive time over-" + iPlcTimeOut);
                                    strRtnValue = m_prvStrBuffer;
                                    return false;
                                }
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(10);
                            }
                        }
                        catch (Exception innerLog)
                        {

                            System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "/INNER]" + innerLog.Message);
                            strRtnValue = m_prvStrBuffer;
                            return false;
                        }

                    } while (true);

                    bRet = true;
                }
                else
                {
                    strErrMsg = "PLC is Disconnected.";
                    strErrCode = ToHexString(0x06);
                }

            }
            catch (Exception eLog)
            {
                strErrCode = "EXC";
                strErrMsg = eLog.Message.ToString();
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "/" + m_PLC_IP + "]" + eLog.Message.ToString());
            }

            return bRet;
        }
        /// <summary>
        /// Convert string to PLC DATA STRUCTURE(LEGACY)
        /// </summary>
        /// <param name="strReadBuf">original value</param>
        /// <param name="buf">converted value</param>
        /// <param name="useReverse"></param>
        /// <returns>Conversion condition</returns>
        public bool SplitValue(string strReadBuf, ref Common.PLC_DeviceArray[] buf)
        {
            if (strReadBuf.Length % 4 != 0)
            {   //devide by word(4byte)
                return false;
            }
            bool bRet = false;
            try
            {
                Common.MemSet(ref buf);

                int maxSize = strReadBuf.Length / 4;

                for (int i = 0; i < maxSize; i++)
                {
                    if (buf.Length > i)
                    {
                        int nWord = 0;
                        nWord = int.Parse(strReadBuf.Substring(i * 4 + 2, 2) + strReadBuf.Substring(i * 4, 2), System.Globalization.NumberStyles.HexNumber);
                        buf[i].iWordValue = (short)nWord;



                        string binary = Convert.ToString(nWord, 2).PadLeft(16, '0');
                        string rBinary = Common.Reverse(binary);

                        for (int j = 0; j < 16; j++)
                        {
                            buf[i].iBitValue[j] = Convert.ToByte(rBinary.Substring(j, 1));
                        }
                    }
                }
                bRet = true;
            }
            catch
            {

            }
            return bRet;
        }
        /// <summary>
        /// Write the PLC data(BLOCK UNIT)(LEGACY)
        /// </summary>
        /// <param name="strVar">plc variable name(ex]%DB6420)</param>
        /// <param name="strWriteValue">value(hexa code)</param>
        /// <param name="strErrCode">Code of error</param>
        /// <param name="strErrMsg">Message of error</param>
        /// <param name="iPlcTimeOut">response waiting time</param>
        /// <returns>condition of writing</returns>
        public bool PlcWriteBlock(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {

            bool bRet = false;
            strErrCode = "";
            strErrMsg = "";
            try
            {


                if (m_InnerConnected && m_clientSocket.Connected)
                {
                    string sVAR1 = strVar.Substring(0, 1);
                    string sVAR2 = strVar.Substring(1, 2);
                    string sVAR3 = strVar.Substring(3);

                    byte[] command = GetCommandArea(true, sVAR3, strWriteValue, sVAR2, true);
                    byte[] header = GetHeaderArea(command.Length);
                    byte[] bytesData = Common.CatByte(header, command);
                    byte[] bufferRcv = new byte[SizeOfRcvBuffer];
                    SocketError errCode = SocketError.Success;
                    m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                    //SocketSend(bytesData, out errCode);
                    strErrCode = Convert.ToInt32(errCode).ToString();

                    switch (errCode)
                    {
                        case SocketError.Success:
                            strErrMsg = "";
                            break;
                        default:
                            strErrMsg = errCode.ToString();
                            break;
                    }


                }
                bRet = true;
            }
            catch (Exception eLog)
            {
                strErrCode = "EXC";
                strErrMsg = eLog.Message.ToString();
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + strWriteValue + "/" + eLog.Message.ToString());
            }

            return bRet;
        }
        /// <summary>
        /// Disconnect to the PLC(LEGACY)
        /// </summary>
        /// <param name="strErrMsg">message of error</param>
        /// <returns>condition of disconnection</returns>
        public bool CloseFEnet(ref string strErrMsg)
        {
            Close();
            strErrMsg = "";
            return true;
        }
        /// <summary>
        /// Connect to the PLC(LEGACY)
        /// </summary>
        /// <param name="strIpAddr">IP of PLC</param>
        /// <param name="strErrMsg">message of error</param>
        /// <returns>condition of connection</returns>
        public bool OpenFEnet(string strIpAddr, ref string strErrMsg)
        {
            this.PLC_IP = strIpAddr;

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@PLCIP", strIpAddr);
            param.Add("@PLC_PORT", "2004");
            return Connect(param);
        }
        /// <summary>
        /// Reply to network condition(LEGACY)
        /// </summary>
        /// <param name="strIpAddr">ojbect ip</param>
        /// <param name="iDelayTime">delay time</param>
        /// <param name="strErrMsg">message of error</param>
        /// <returns>condition of response</returns>
        public bool Ping(string strIpAddr, int iDelayTime, ref string strErrMsg)
        {
            bool bRet = false;
            try
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply r = p.Send(strIpAddr, iDelayTime);

                if (r.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    bRet = true;
                }
            }
            catch (Exception eLog)
            {
                strErrMsg = eLog.Message;
                bRet = false;
            }
            return bRet;
        }
        /// <summary>
        /// Write the PLC data(LEGACY)
        /// </summary>
        /// <param name="strVar">plc variable name(ex]%DB6420)</param>
        /// <param name="strWriteValue">value(2digit hexa code)</param>
        /// <param name="strErrCode">Code of error</param>
        /// <param name="strErrMsg">Message of error</param>
        /// <param name="iPlcTimeOut">response waiting time</param>
        /// <returns>condition of writing</returns>
        public bool PlcWrite(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            strErrMsg = "";
            return Write(strVar, strWriteValue, out strErrCode, out strErrMsg, iPlcTimeOut);
        }
        /// <summary>
        /// Get the PLC data(LEGACY)
        /// </summary>
        /// <param name="strVar">plc variable name(ex]%DB6420)</param>
        /// <param name="strRtnValue">return value(2digit hexa code)</param>
        /// <param name="strErrCode">Code of error</param>
        /// <param name="strErrMsg">Message of error</param>
        /// <param name="iPlcTimeOut">response waiting time</param>
        /// <returns>condition of reading</returns>
        public bool PlcRead(string strVar, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            strErrCode = "";
            strErrMsg = "";
            strRtnValue = "";
            return Read(strVar, out strRtnValue, out strErrCode, out strErrMsg, iPlcTimeOut);
        }

        public bool WriteWord(string strVar, string strWriteValue, out string strErrCode)
        {
            strErrCode = "";
            string strErrMsg = "";
            return PlcWrite(strVar, strWriteValue.PadLeft(4, '0'), ref strErrCode, ref strErrMsg, 1000);
        }

        #endregion
    }
}
