using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YMES.FX.PLC.Base;

namespace YMES.FX.PLC.Mitsubishi
{
    public class MELSEC_ETH : IPLC
    {
        private string m_prvStrBuffer = "";
        public enum MCFrameEnum
        {
            MC_4E,
            MC_3E,
            MC_2E,
            MC_1E

        }
        public enum MCCommandEnum
        {
            BatchRead,
            BatchWrite
        }
        public enum DeviceType
        {
            Word, Bit
        }
        private MCFrameEnum m_CurrentFrame = MCFrameEnum.MC_3E;
        private Socket m_clientSocket = null;
        private Dictionary<string, string> m_dicConnectionArgs = null;
        private bool m_bThreadConnecting = false;
        private bool m_InnerConnected = false;
        private bool m_bClose = false;
        private bool m_AutoReplay = true;
        private int m_AutoReplyTime = 5000;
        private int m_SendTimeout = 2000;
        private int m_ReceiveTimeout = 2000;
        private int m_SizeOfRcvBuffer = 50;
        System.Threading.ManualResetEvent m_allDone = new System.Threading.ManualResetEvent(false);
        private bool m_AutoReplyCondition = false;
        private string m_MemoryTag = "M";
        public string MemoryTag
        {
            get { return m_MemoryTag; }
            set { m_MemoryTag = value; }
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
        /// Max size of the receipt buffer
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public int SizeOfRcvBuffer
        {
            get { return m_SizeOfRcvBuffer; }
            set { m_SizeOfRcvBuffer = value; }
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
        public MCFrameEnum CurrentFrame
        {
            get { return m_CurrentFrame; }
        }
        /// <summary>
        /// PLC Send wait time(milisecond)
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public int SendTimeout
        {
            get { return m_SendTimeout; }
            set { m_SendTimeout = value; }
        }
        /// <summary>
        /// PLC Receive wait time(milisecond)
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public int ReceiveTimeout
        {
            get { return m_ReceiveTimeout; }
            set { m_ReceiveTimeout = value; }
        }
        public bool Connected
        {
            get
            {
                return m_InnerConnected;
            }
        }
        public string GetDicConnectionArgs(string key)
        {

            if (m_dicConnectionArgs.ContainsKey(key))
            {
                return m_dicConnectionArgs[key];
            }
            else
            {
                if (key == "@PC_NO")
                {
                    if (m_dicConnectionArgs.ContainsKey("@PLC_STATION_NO"))
                    {
                        return m_dicConnectionArgs["@PLC_STATION_NO"];
                    }
                }
                else if (key == "@REQ_MOD_IO")
                {
                    if (m_dicConnectionArgs.ContainsKey("@MODULE_NO"))
                    {
                        return m_dicConnectionArgs["@MODULE_NO"];
                    }
                }
                else if (key == "@REQ_MOD_SN")
                {
                    if (m_dicConnectionArgs.ContainsKey("@PC_STATION_NO"))
                    {
                        return m_dicConnectionArgs["@PC_STATION_NO"];
                    }
                }
            }
            return string.Empty;
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
                            string errMsg = "";
                            if (Ping(GetDicConnectionArgs("@PLCIP"), 1500, ref errMsg) == false)
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
                            Connect(m_dicConnectionArgs);
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
        private int SendSocket(string msg)
        {
            try
            {
                if (m_clientSocket == null)
                {
                    throw new Exception("Socket is not defined");
                }
                if (Connected == false)
                {
                    throw new Exception("Socket is not opened");
                }
                return m_clientSocket.Send(Encoding.ASCII.GetBytes(msg));
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return -1;

        }
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
        private void thConnection(object args)
        {


            try
            {

                if (args is Dictionary<string, string>)
                {
                    string ip = ((Dictionary<string, string>)args)["IP"];
                    string portNo = ((Dictionary<string, string>)args)["PORT"];
                    IPAddress ipAddr = IPAddress.Parse(ip);

                    if (m_clientSocket == null)
                    {
                        m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        m_clientSocket.ReceiveTimeout = ReceiveTimeout;
                        m_clientSocket.SendTimeout = SendTimeout;

                    }
                    else
                    {
                        m_clientSocket.Close();
                        m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        m_clientSocket.ReceiveTimeout = ReceiveTimeout;
                        m_clientSocket.SendTimeout = SendTimeout;

                    }

                    m_clientSocket.Connect(ipAddr, Convert.ToInt32(portNo));
                    //m_clientSocket.BeginConnect(ip, Convert.ToInt32(portNo), null, null);
                    m_InnerConnected = true;


                }
            }
            catch (Exception eLog)
            {
                if (((System.Net.Sockets.SocketException)eLog).ErrorCode != 10060)
                {   //
                    System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
                    m_InnerConnected = false;
                }
                else
                {
                    m_InnerConnected = false;
                }
            }
            finally
            {
                m_bThreadConnecting = false;
            }
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
                    try
                    {
                        if (errCode == SocketError.Success)
                        {
                            m_clientSocket.Blocking = true;
                            m_clientSocket.Receive(bufferRcv);
                            if (bufferRcv[0] == 68 && bufferRcv[1] == 48 && bufferRcv[2] == 48 && bufferRcv[3] == 48)
                            {   //Response(Write Response) - ACK / None Continuos Type
                                bRet = true;
                                break;
                            }
                        }
                        if ((DateTime.Now - startDT).TotalMilliseconds >= iPlcTimeOut)
                        {   //Time-out
                            System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + "TCP Receive time over-" + iPlcTimeOut);
                            bRet = false;
                            break;
                        }
                        System.Threading.Thread.Sleep(10);
                    }
                    catch (Exception eSCK)
                    {
                        System.Threading.Thread.Sleep(50);
                        m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                        System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "/INNER" + "]" + eSCK.Message.ToString());
                    }


                } while (true);
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return bRet;
        }
        public bool Connect(Dictionary<string, string> dicConnectParam)
        {
            string ip = dicConnectParam["@PLCIP"];
            string port = dicConnectParam["@PLC_PORT"];
            m_dicConnectionArgs = dicConnectParam;
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            if (string.IsNullOrEmpty(port))
            {
                port = "5050";
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

        public string GetAbsAddr(string dataType, string baseAddr, int seq)
        {
            return dataType + (Convert.ToInt32(baseAddr) + seq).ToString().PadLeft(6, '0');
        }

        public string GetBitTag()
        {
            return "B" + m_MemoryTag;
        }

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
        private string GetCommand(DeviceType iCmdType, MCCommandEnum cmdType, string strVar, int size, string writeVal = "")
        {
            string sVAR1 = strVar.Substring(0, 1);
            string sVAR2 = strVar.Substring(1).PadLeft(6, '0');
            string type = "";
            string sub_command = "0000";
            string dataLen = (int.Parse("18", System.Globalization.NumberStyles.HexNumber) + writeVal.Length).ToString("X").PadLeft(4, '0');
            switch (iCmdType)
            {
                case DeviceType.Word:
                    sub_command = "0000";
                    break;
                case DeviceType.Bit:
                    sub_command = "0001";
                    break;
            }
            switch (cmdType)
            {
                case MCCommandEnum.BatchRead:
                    type = "0401";

                    break;
                case MCCommandEnum.BatchWrite:
                    type = "1401";

                    break;
            }
            string cmd = cmd = "";
            cmd = cmd + "5000";                                 // sub HEAD (FIXED)
            cmd = cmd + GetDicConnectionArgs("@NETWORK_NO");    //NETWORK NO(Ox00)
            cmd = cmd + GetDicConnectionArgs("@PC_NO");         //PC NO(OxFF)
            cmd = cmd + GetDicConnectionArgs("@REQ_MOD_IO");    //REQUEST DESTINATION MODULE I/O NO(0x03FF)
            cmd = cmd + GetDicConnectionArgs("@REQ_MOD_SN");    //REQUEST DESTINATION MODULE STATION NO(0x00)
            cmd = cmd + dataLen;                                //Request data length and response data length
            cmd = cmd + "0000";                                 //CPU inspector data
            cmd = cmd + type;                                   //Read(0401)/Write(1401) command
            cmd = cmd + sub_command;                            //Sub command(MELSEC Q/L : 0000/0001, MELSEC iQ/R : 0002/0003)
            cmd = cmd + sVAR1 + "*";                            //device code
            cmd = cmd + sVAR2;                                  //Address(6 Digit)
            cmd = cmd + size.ToString().PadLeft(4, '0');        //size(4 Digit)
            cmd = cmd + writeVal;                               //if write mode, it can be used.
            return cmd;
        }
        public bool WriteBatch(DeviceType iCmdType, string strDeviceCode, string strStartDevice, int nLength, string strWriteValue, ref string strErrMsg, int iPlcTimeout = 3000)
        {
            return WriteBatch(iCmdType, strDeviceCode, strStartDevice, nLength.ToString(), strWriteValue, ref strErrMsg, iPlcTimeout);
        }
        public bool WriteBatch(DeviceType iCmdType, string strDeviceCode, string strStartDevice, string strLength, string strWriteValue, ref string strErrMsg, int iPlcTimeout = 3000)
        {
            bool bRet = false;
            strErrMsg = "";
            try
            {

                if (m_InnerConnected)
                {
                    int nSize = int.Parse(strLength, System.Globalization.NumberStyles.HexNumber);
                    string val = strWriteValue.PadLeft(iCmdType == DeviceType.Word ? 4 * nSize : 1 * nSize);

                    byte[] bytesData = (Encoding.ASCII.GetBytes(GetCommand(iCmdType, MCCommandEnum.BatchWrite, strDeviceCode.Replace("*", "") + strStartDevice.PadLeft(6, '0'), Convert.ToInt32(strLength), val)));

                    SocketError errCode = SocketError.Success;
                    //m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                    //m_clientSocket.BeginSend(bytesData, 0, bytesData.Length, SocketFlags.None, null, null);
                    SocketSend(bytesData, out errCode, iPlcTimeout);
                    switch (errCode)
                    {
                        case SocketError.Success:
                            strErrMsg = "";
                            bRet = true;
                            break;
                        default:
                            strErrMsg = errCode.ToString();
                            bRet = false;
                            break;
                    }

                }
            }
            catch (Exception eLog)
            {
                strErrMsg = eLog.Message.ToString();
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
                bRet = false;
            }
            return bRet;
        }
        public bool ReadBatch(DeviceType iCmdType, string strDeviceCode, string strStartDevice, int nLength, ref string strReadValue, ref string strErrMsg, int iPlcTimeout = 3000)
        {
            return ReadBatch(iCmdType, strDeviceCode, strStartDevice, nLength.ToString(), ref strReadValue, ref strErrMsg, iPlcTimeout);
        }
        public bool ReadBatch(DeviceType iCmdType, string strDeviceCode, string strStartDevice, string strLength, ref string strReadValue, ref string strErrMsg, int iPlcTimeout = 3000)
        {
            bool bRet = false;
            strErrMsg = "";
            try
            {

                if (m_InnerConnected)
                {
                    int mul = 4;

                    switch (iCmdType)
                    {
                        case DeviceType.Word:
                            mul = 4;
                            break;
                        case DeviceType.Bit:
                            mul = 1;
                            break;
                    }

                    int size = int.Parse(strLength, System.Globalization.NumberStyles.HexNumber) * mul;
                    byte[] bytesData = (Encoding.ASCII.GetBytes(GetCommand(iCmdType, MCCommandEnum.BatchRead, strDeviceCode.Replace("*", "") + strStartDevice, Convert.ToInt32(strLength))));

                    SocketError errCode = SocketError.Success;

                    m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);

                    byte[] bufferRcv = new byte[size + 22];
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
                            if (errCode == SocketError.Success && m_clientSocket.Available > 0)
                            {
                                m_clientSocket.Blocking = true;
                                int rcvCnt = m_clientSocket.Receive(bufferRcv);
                                string strRCV = Encoding.ASCII.GetString(bufferRcv);
                                if (rcvCnt == bufferRcv.Length)
                                {

                                    if (strRCV.Substring(0, 4) == "D000")
                                    {
                                        strReadValue = strRCV.Substring(22, size);
                                        m_prvStrBuffer = strReadValue;
                                        bRet = true;
                                        break;
                                    }
                                }
                                else if (rcvCnt > 22)
                                {
                                    string errChk = strRCV.Substring(0, rcvCnt);
                                    if (errChk.Substring(0, 4) == "D000" && errChk.Substring(18, 4) != "0000")
                                    {
                                        strReadValue = m_prvStrBuffer;
                                        strErrMsg = "ERROR-CODE:" + errChk.Substring(18, 4);
                                        bRet = false;
                                        break;
                                    }

                                }
                            }


                            if ((DateTime.Now - startDT).TotalMilliseconds >= iPlcTimeout)
                            {   //Time-out4
                                strReadValue = m_prvStrBuffer;
                                break;
                            }
                        }
                        catch (Exception eSCK)
                        {
                            strReadValue = m_prvStrBuffer;
                            System.Threading.Thread.Sleep(50);
                            m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                            System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "/INNER" + "]" + eSCK.Message.ToString());
                        }

                    } while (true);


                }
            }
            catch (Exception eLog)
            {
                strReadValue = m_prvStrBuffer;
                strErrMsg = eLog.Message.ToString();
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());

            }

            return bRet;
        }
        public bool Read(string strVar, int size, out string strRtnValue, out string strErrCode, out string strErrMsg, int iPlcTimeOut = 3000)
        {
            strErrCode = "";
            strRtnValue = "";
            strErrMsg = "";
            DeviceType curDeviceType = DeviceType.Word;
            switch (strVar.Substring(0, 1))
            {
                case "B":
                    curDeviceType = DeviceType.Bit;
                    break;
                case "W":
                    curDeviceType = DeviceType.Word;
                    break;
            }
            return ReadBatch(curDeviceType, strVar.Replace("*", "").Substring(1, 1), strVar.Replace("*", "").Substring(2), size, ref strRtnValue, ref strErrMsg, iPlcTimeOut);
        }
        public bool Read(string strVar, out string strRtnValue, out string strErrCode, out string strErrMsg, int iPlcTimeOut = 3000)
        {
            strErrCode = "";
            strRtnValue = "";
            strErrMsg = "";
            return Read(strVar, SizeOfRcvBuffer, out strRtnValue, out strErrCode, out strErrMsg, iPlcTimeOut);
        }
        /// <summary>
        /// Read Plc Memory Value
        /// </summary>
        /// <param name="strVar">Memory Address(ex: Bit M000 -> BM000000, Word M0000 -> WM000000)</param>
        /// <param name="strRtnValue">Return Value of memory</param>
        /// <param name="strErrCode">Error code of command</param>
        /// <param name="strErrMsg">Error Message of command</param>
        /// <param name="iPlcTimeOut">waiting time of command</param>
        /// <returns></returns>
        public bool PlcRead(string strVar, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut)
        {
            return Read(strVar, out strRtnValue, out strErrCode, out strErrMsg, iPlcTimeOut);
        }

        public bool PlcReadBlock(string strVar, long lReadCnt, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut)
        {
            return Read(strVar, Convert.ToInt32(lReadCnt), out strRtnValue, out strErrCode, out strErrMsg, iPlcTimeOut);
        }

        public bool PlcWrite(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut)
        {
            strErrCode = "";

            strErrMsg = "";
            DeviceType curDeviceType = DeviceType.Word;
            switch (strVar.Substring(0, 1))
            {
                case "B":
                    curDeviceType = DeviceType.Bit;
                    break;
                case "W":
                    curDeviceType = DeviceType.Word;
                    break;
            }
            return WriteBatch(curDeviceType, strVar.Replace("*", "").Substring(1, 1), strVar.Replace("*", "").Substring(2), strWriteValue.Length, strWriteValue, ref strErrMsg, iPlcTimeOut);
        }

        public bool PlcWriteBlock(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut)
        {
            strErrCode = "";

            strErrMsg = "";
            DeviceType curDeviceType = DeviceType.Word;
            switch (strVar.Substring(0, 1))
            {
                case "B":
                    curDeviceType = DeviceType.Bit;
                    break;
                case "W":
                    curDeviceType = DeviceType.Word;
                    break;
            }
            return WriteBatch(curDeviceType, strVar.Replace("*", "").Substring(1, 1), strVar.Replace("*", "").Substring(2), SizeOfRcvBuffer, strWriteValue, ref strErrMsg, iPlcTimeOut);
        }

        public bool ReadBit(string baseAddr, int seq, string dataType)
        {
            string strRtnValue = "";
            string strErrMsg = "";
            string strErrCode = "";
            Read(dataType + string.Format("{0:000000}", Convert.ToInt32(baseAddr) + seq), Convert.ToInt32(1), out strRtnValue, out strErrCode, out strErrMsg, 1000);
            if (!string.IsNullOrEmpty(strRtnValue))
            {
                return strRtnValue.Substring(0, 1) == "1" ? true : false;
            }
            return false;
        }

        public bool SplitValue(string strReadBuf, ref Common.PLC_DeviceArray[] buf)
        {
            strReadBuf = strReadBuf.Replace("\0", "");
            if (string.IsNullOrEmpty(strReadBuf))
            {
                return false;
            }
            if (strReadBuf.Replace("\0", "").Length % 4 != 0)
            {   //devide by word(4byte)
                return false;
            }
            bool bRet = false;
            int i = 0;
            int nWord = 0;
            try
            {
                Common.MemSet(ref buf);

                int maxSize = strReadBuf.Length / 4;

                for (i = 0; i < maxSize; i++)
                {
                    if (buf.Length > i)
                    {

                        nWord = int.Parse(strReadBuf.Substring(i * 4, 2) + strReadBuf.Substring(i * 4 + 2, 2), System.Globalization.NumberStyles.HexNumber);
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
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return bRet;
        }

        public void Start(Dictionary<string, string> param)
        {
            try
            {

                m_dicConnectionArgs = param;
                if (!string.IsNullOrEmpty(GetDicConnectionArgs("@BLOCK_SIZE")))
                {
                    SizeOfRcvBuffer = Convert.ToInt32(GetDicConnectionArgs("@BLOCK_SIZE"));
                }

                switch (GetDicConnectionArgs("@MC_FRAME"))
                {
                    case "1E":
                        m_CurrentFrame = MCFrameEnum.MC_1E;
                        break;
                    case "2E":
                        m_CurrentFrame = MCFrameEnum.MC_2E;
                        break;
                    case "4E":
                        m_CurrentFrame = MCFrameEnum.MC_4E;
                        break;
                    default:
                        m_CurrentFrame = MCFrameEnum.MC_3E;
                        break;

                }
                Connect(param);
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
        }

        public bool WriteBit(string baseAddr, int seq, bool val, string dataType)
        {
            string strVar = (dataType.Length >= 2 ? dataType : "B" + dataType) + (Convert.ToInt32(baseAddr) + seq);
            string strErrMsg = "";
            // System.Threading.Thread.Sleep(50);
            return WriteBatch(DeviceType.Bit, strVar.Replace("*", "").Substring(1, 1), strVar.Replace("*", "").Substring(2), 1, val ? "1" : "0", ref strErrMsg, 1000);
        }

        public bool WriteWord(string tag, string baseAddr, int seq, string val)
        {
            string strVar = (tag.Length >= 2 ? tag : "W" + tag) + +(Convert.ToInt32(baseAddr) + (16 * seq));
            string strErrMsg = "";

            return WriteBatch(DeviceType.Word, strVar.Replace("*", "").Substring(1, 1), strVar.Replace("*", "").Substring(2), 4, val.PadLeft(4, '0').PadRight(16, '0'), ref strErrMsg, 1000);
        }

        public bool WriteWord(string strVar, string strWriteValue, out string strErrCode)
        {
            strErrCode = "";
            return WriteBatch(DeviceType.Word, strVar.Replace("*", "").Substring(1, 1), strVar.Replace("*", "").Substring(2), 4, strWriteValue.PadLeft(4, '0').PadRight(16, '0'), ref strErrCode, 1000);
        }

    }
}
