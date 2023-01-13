using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YMES.FX.PLC.Base;

namespace YMES.FX.PLC.Omron
{
    public class FINS_ETH : IPLC
    {
        private string m_prvStrBuffer = "";
        public enum DeviceType
        {
            Word, Bit
        }

        private byte m_PCNode = 0x00;
        private byte m_PLCNode = 0x00;
        private bool m_InnerConnected = false;
        private Socket m_clientSocket = null;
        private bool m_bClose = false;
        private Dictionary<string, string> m_dicConnectionArgs = null;
        private bool m_bThreadConnecting = false;
        private int m_AutoReplyTime = 5000;
        private bool m_AutoReplyCondition = false;
        private bool m_AutoReplay = true;
        private int m_SendTimeout = 2000;
        private int m_ReceiveTimeout = 2000;
        private int m_SizeOfRcvBuffer = 50;
        private string m_PLC_NETWORK = "00";
        private string m_PC_NETWORK = "00";
        private string m_MemoryTag = "DM";
        public string MemoryTag
        {
            get { return m_MemoryTag; }
            set { m_MemoryTag = value; }
        }
        public string PLC_NETWORK
        {
            get { return m_PLC_NETWORK; }
            set { m_PLC_NETWORK = value; }
        }
        public string PC_NETWORK
        {
            get { return m_PC_NETWORK; }
            set { m_PC_NETWORK = value; }
        }
        public byte PCNode
        {
            get { return m_PCNode; }
        }
        public byte PLCNode
        {
            get { return m_PLCNode; }
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
        /// <summary>
        /// Time of response(millisecond) 
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public int AutoReplyTime
        {
            get { return m_AutoReplyTime; }
            set { m_AutoReplyTime = value; }
        }
        public bool Connected
        {
            get
            {
                return m_InnerConnected;
            }
        }


        public void Close()
        {
            m_InnerConnected = false;
            m_clientSocket.Close();
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
        private void SetNode()
        {
            if (m_InnerConnected)
            {
                string cmd = "FINS";
                cmd += "\x00" + "\x00" + "\x00" + "\x0C";
                cmd += "\x00" + "\x00" + "\x00" + "\x00";
                cmd += "\x00" + "\x00" + "\x00" + "\x00";
                cmd += "\x00" + "\x00" + "\x00" + "\x00";
                SendSocket(cmd);
                byte[] bufferRcv = new byte[24];
                DateTime startDT = DateTime.Now;
                do
                {
                    if (m_clientSocket.Receive(bufferRcv) == bufferRcv.Length)
                    {
                        string strRCV = Encoding.ASCII.GetString(bufferRcv);
                        if (strRCV.Substring(0, 4) == "FINS" && bufferRcv[11] == 1)
                        {
                            m_PCNode = bufferRcv[19];
                            m_PLCNode = bufferRcv[23];
                            break;
                        }
                        else
                        {
                            SendSocket(cmd);
                        }
                    }

                    if ((DateTime.Now - startDT).TotalMilliseconds >= 3000)
                    {   //Time-out
                        m_PCNode = 0x00;
                        m_PLCNode = 0x00;
                        m_InnerConnected = false;
                        return;
                    }

                } while (true);
            }
        }
        private void thConnection(object args)
        {
            try
            {

                if (args is Dictionary<string, string>)
                {
                    if (m_clientSocket != null)
                    {
                        m_clientSocket.Close();
                    }
                    m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_clientSocket.ReceiveTimeout = ReceiveTimeout;
                    m_clientSocket.SendTimeout = SendTimeout;

                    string ip = ((Dictionary<string, string>)args)["IP"];
                    string portNo = ((Dictionary<string, string>)args)["PORT"];
                    IPAddress ipAddr = IPAddress.Parse(ip);

                    m_clientSocket.Connect(ipAddr, Convert.ToInt32(portNo));

                    m_InnerConnected = true;
                    SetNode();
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
                            if (Ping(GetDicConnectionArgs("@PLCIP"), 200, ref errMsg) == false)
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
        public bool Connect(Dictionary<string, string> dicConnectParam)
        {
            string ip = dicConnectParam["@PLCIP"];
            string port = dicConnectParam["@PLC_PORT"];
            m_PLC_NETWORK = dicConnectParam["@PLC_STATION_NO"];
            m_PC_NETWORK = dicConnectParam["@PC_STATION_NO"];
            m_dicConnectionArgs = dicConnectParam;
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            if (string.IsNullOrEmpty(port))
            {
                port = "9600";
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
            return dataType + (Convert.ToInt32(baseAddr) + seq).ToString().PadLeft(4, '0');
        }

        public string GetBitTag()
        {
            return m_MemoryTag;
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
        public bool Write(DeviceType readType, string strVar, string strWriteValue, out string strErrCode, out string strErrMsg, int iPlcTimeOut = 3000)
        {
            int startIDX_POS = 30;
            bool bRet = false;
            strErrMsg = "";
            strErrCode = "";
            try
            {
                int size = 0;
                if (readType == DeviceType.Bit)
                {
                    size = strWriteValue.Length + ((strWriteValue.Length % 2) != 0 ? 1 : 0);
                }
                else if (readType == DeviceType.Word)
                {
                    //size = strWriteValue.Length / 2 + ((strWriteValue.Length % 2) != 0 ? 1 : 0);
                    size = strWriteValue.Length / 2;
                    size += ((strWriteValue.Length % 2) != 0 ? 1 : 0);
                }
                byte[] bytesData = GetCommand(readType, strVar.Substring(2), "00", size, strVar.Substring(0, 2), strWriteValue);

                SocketError errCode = SocketError.Success;
                m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);


                byte[] bufferRcv = new byte[size + startIDX_POS];
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
            catch (Exception eLog)
            {
                strErrMsg = eLog.Message.ToString();
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return bRet;
        }
        public bool Read(DeviceType readType, string strVar, int size, out string strRtnValue, out string strErrCode, out string strErrMsg, int iPlcTimeOut = 3000)
        {
            int startIDX_POS = 30;
            bool bRet = false;
            strErrMsg = "";
            strErrCode = "";
            strRtnValue = "";
            try
            {
                if (m_InnerConnected)
                {
                    int innerSize = 0;
                    if (readType == DeviceType.Word)
                    {
                        innerSize = size * 4;
                    }
                    else
                    {
                        innerSize = size * 2;
                    }
                    byte[] bytesData = GetCommand(readType, strVar.Substring(2), "00", size, strVar.Substring(0, 2));

                    SocketError errCode = SocketError.Success;

                    m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);


                    byte[] bufferRcv = new byte[innerSize + startIDX_POS];
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
                        if (errCode == SocketError.Success)
                        {
                            try
                            {
                                if (m_clientSocket.Available > 0)
                                {
                                    m_clientSocket.Receive(bufferRcv, SocketFlags.None);
                                }
                            }
                            catch (SocketException eSKerr)
                            {
                                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eSKerr.Message.ToString());
                            }
                        }
                        if (bufferRcv[0] == 70 && bufferRcv[1] == 73 && bufferRcv[2] == 78 && bufferRcv[3] == 83
                                && bufferRcv[16] == 192 && bufferRcv[17] == 0 && bufferRcv[18] == 2
                                && bufferRcv[26] == 1 && bufferRcv[27] == 1)
                        {
                            string strRCV = GetHexString(bufferRcv);
                            strRtnValue = strRCV.Substring(startIDX_POS * 2, innerSize);
                            m_prvStrBuffer = strRtnValue;
                            bRet = true;
                            break;
                        }
                        else
                        {
                            m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                            System.Threading.Thread.Sleep(50);
                        }
                        if ((DateTime.Now - startDT).TotalMilliseconds >= iPlcTimeOut)
                        {   //Time-out
                            strRtnValue = m_prvStrBuffer;
                            break;
                        }

                    } while (true);


                }
            }
            catch (Exception eLog)
            {
                strRtnValue = m_prvStrBuffer;
                strErrMsg = eLog.Message.ToString();
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return bRet;
        }

        public bool PlcRead(string strVar, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            return Read(DeviceType.Word, strVar, m_SizeOfRcvBuffer, out strRtnValue, out strErrCode, out strErrMsg, iPlcTimeOut);
        }
        private byte[] GetByteFromStr(string str, int len)
        {
            byte[] byRet = new byte[len];
            int idx = 0;
            for (int i = 0; i < len; i++)
            {
                byRet[idx] = Convert.ToByte(String.Format("{0:X2}", Convert.ToByte(str.PadLeft(2, '0').Substring(i * 2, 2), 16)), 16);
                idx++;
            }

            return byRet;
        }

        private byte[] GetCommand_Old(DeviceType type, string strWordAddr, string strBitAddr, int size, string tag = "DM", string writeVal = "")
        {
            string sVAR1 = tag;
            string sVAR2 = string.Format("{0:X4}", Convert.ToInt32(strWordAddr));
            string sVAR3 = string.Format("{0:00}", Convert.ToInt32(strBitAddr));
            string hexSize = string.Format("{0:X2}", Convert.ToInt32(size.ToString()));

            List<byte> lstRet = new List<byte>();
            Common.CatListString(ref lstRet, "\x46" + "\x49" + "\x4E" + "\x53");  //FINS
            if (string.IsNullOrEmpty(writeVal))
            {   //Read Mode
                Common.CatListString(ref lstRet, "\x00" + "\x00" + "\x00" + "\x1A");
            }
            else if (string.IsNullOrEmpty(writeVal) == false)
            {   //Write Mode
                Common.CatListString(ref lstRet, "\x00" + "\x00" + "\x00");
                if (type == DeviceType.Word)
                {
                    foreach (byte by in GetByteFromStr(string.Format("{0:X2}", 26 + (size * 2)), 1))
                    {
                        lstRet.Add(by);
                    }
                }
                else
                {
                    foreach (byte by in GetByteFromStr(string.Format("{0:X2}", 26 + (size * 1)), 1))
                    {
                        lstRet.Add(by);
                    }
                }
            }
            Common.CatListString(ref lstRet, "\x00" + "\x00" + "\x00" + "\x02");
            Common.CatListString(ref lstRet, "\x00" + "\x00" + "\x00" + "\x00");
            Common.CatListString(ref lstRet, "\x80" + "\x00" + "\x02");
            lstRet.Add(Convert.ToByte(m_PLC_NETWORK, 16));
            lstRet.Add(m_PLCNode);
            Common.CatListString(ref lstRet, "\x00");
            lstRet.Add(Convert.ToByte(m_PC_NETWORK, 16));
            lstRet.Add(m_PCNode);
            Common.CatListString(ref lstRet, "\x00");
            Common.CatListString(ref lstRet, "\x00");
            if (string.IsNullOrEmpty(writeVal))
            {   //Read Mode
                Common.CatListString(ref lstRet, "\x01" + "\x01");
            }
            else
            {   //Write Mode
                Common.CatListString(ref lstRet, "\x01" + "\x02");
            }

            if (type == DeviceType.Bit)
            {
                Common.CatListString(ref lstRet, "\x02");
            }
            else
            {
                Common.CatListString(ref lstRet, "\x82");
            }
            foreach (byte by in GetByteFromStr(sVAR2.ToString(), 2))
            {
                lstRet.Add(by);
            }
            lstRet.Add(Convert.ToByte(sVAR3));
            Common.CatListString(ref lstRet, "\x00");
            foreach (byte by in GetByteFromStr(hexSize.ToString(), 1))
            {
                lstRet.Add(by);
            }
            if (string.IsNullOrEmpty(writeVal) == false)
            {
                int cSIZE = size;
                if (type == DeviceType.Word)
                {
                    cSIZE = size * 2;
                    string cvtVal = writeVal.PadRight(cSIZE, '\x00');
                    cvtVal = cvtVal.Substring(0, cSIZE - 2) + cvtVal.Substring(cSIZE - 1, 1) + cvtVal.Substring(cSIZE - 2, 1);
                    for (int i = 0; i < cSIZE; i++)
                    {
                        Common.CatListString(ref lstRet, cvtVal.Substring(i, 1));
                    }
                }
                else if (type == DeviceType.Bit)
                {
                    cSIZE = size * 1;
                    string cvtVal = string.Format("{0:X2}", Convert.ToInt32(writeVal.ToString()));

                    foreach (byte by in GetByteFromStr(cvtVal, cSIZE))
                    {
                        lstRet.Add(by);
                    }
                }

            }

            return lstRet.ToArray();
        }

        private byte[] GetCommand(DeviceType type, string strWordAddr, string strBitAddr, int size, string tag = "DM", string writeVal = "")
        {
            string sVAR1 = tag;
            string sVAR2 = string.Format("{0:X4}", Convert.ToInt32(strWordAddr));
            string sVAR3 = string.Format("{0:00}", Convert.ToInt32(strBitAddr));
            string hexSize = string.Format("{0:X2}", Convert.ToInt32(size.ToString()));

            byte iHigh = 0x00;
            byte iLow = 0x00;

            List<byte> lstRet = new List<byte>();

            //HEADER - FINS
            lstRet.Add(0x46);   //0
            lstRet.Add(0x49);   //1
            lstRet.Add(0x4E);   //2
            lstRet.Add(0x53);   //3

            int iCommandLength = 0x00;

            //READ MODE
            if (string.IsNullOrEmpty(writeVal))
            {
                lstRet.Add(0x00);       //4
                lstRet.Add(0x00);       //5                

                //Command Length = FINS/TCP Header (16) + FINS Header (10) + Fins Memory Area Read Command (8)
                iCommandLength = 16 + 10 + 8;
                iLow = Convert.ToByte(iCommandLength - 8 & 0xFF);
                iHigh = Convert.ToByte((iCommandLength - 8 & 0xFF00) >> 8);

                lstRet.Add(iHigh);      //6 (High)
                lstRet.Add(iLow);       //7 (Low)
            }
            //WRITE MODE
            else if (string.IsNullOrEmpty(writeVal) == false)
            {
                lstRet.Add(0x00);       //4
                lstRet.Add(0x00);       //5

                //Command Length = FINS/TCP Header (16) + FINS Header (10) + Fins Memory Area Read Command (8) + size
                iCommandLength = 16 + 10 + 8 + size;
                iLow = Convert.ToByte(iCommandLength - 8 & 0xFF);
                iHigh = Convert.ToByte((iCommandLength - 8 & 0xFF00) >> 8);

                lstRet.Add(iHigh);      //6 (High)
                lstRet.Add(iLow);       //7 (Low)
            }

            //Command 
            lstRet.Add(0x00);       //8
            lstRet.Add(0x00);       //9
            lstRet.Add(0x00);       //10
            lstRet.Add(0x02);       //11

            //Error Code
            lstRet.Add(0x00);       //12
            lstRet.Add(0x00);       //13
            lstRet.Add(0x00);       //14
            lstRet.Add(0x00);       //15

            //ICF
            lstRet.Add(0x80);       //16

            //RSV (Reserved by System) - Set to &h0
            lstRet.Add(0x00);       //17

            //GCT (Permissible Number of Gateways) - Set to &h2
            lstRet.Add(0x02);       //18

            //Destination Network No
            lstRet.Add(Convert.ToByte(m_PLC_NETWORK, 16));      //19

            //Destination Node No
            lstRet.Add(m_PLCNode);  //20

            //Common.CatListString(ref lstRet, "\x00");

            //Destination Unit No
            lstRet.Add(0x00);       //21

            //Source Network No
            lstRet.Add(Convert.ToByte(m_PC_NETWORK, 16));       //22

            //Source Node No
            lstRet.Add(m_PCNode);   //23

            //Source Unit No
            lstRet.Add(0x00);       //24

            //Sequence
            lstRet.Add(0x00);       //25

            //READ MODE
            if (string.IsNullOrEmpty(writeVal))
            {
                //Main request code
                lstRet.Add(0x01);       //26

                //Sub-request code
                lstRet.Add(0x01);       //27

            }
            //WRITE MODE
            else
            {
                //Main request code
                lstRet.Add(0x01);       //26

                //Sub-request code
                lstRet.Add(0x02);       //27
            }

            if (type == DeviceType.Bit)
            {
                //Memory Area Code
                lstRet.Add(0x02);       //28
            }
            else
            {
                //Memory Area Code
                lstRet.Add(0x82);       //28
            }

            //Write Address (Word) 
            int iAddr = Convert.ToInt32(strWordAddr);
            iLow = Convert.ToByte(iAddr & 0xFF);
            iHigh = Convert.ToByte((iAddr & 0xFF00) >> 8);

            lstRet.Add(iHigh);          //29
            lstRet.Add(iLow);           //30

            //Write Address (Bit) 
            lstRet.Add(Convert.ToByte(strBitAddr));           //31

            //Data Length 
            int iDataSize = 0x00;

            if (string.IsNullOrEmpty(writeVal) == false)
            {
                if (type == DeviceType.Word)
                {
                    iDataSize = size / 2;
                }
                else if (type == DeviceType.Bit)
                {
                    iDataSize = size / 1;
                }
            }
            else
            {
                iDataSize = size;
            }

            iLow = Convert.ToByte(iDataSize & 0xFF);
            iHigh = Convert.ToByte((iDataSize & 0xFF00) >> 8);

            lstRet.Add(iHigh);      //32 (High)
            lstRet.Add(iLow);       //33 (Low)

            //Write Data    34~
            if (string.IsNullOrEmpty(writeVal) == false)
            {
                for (int iLoop = 0; iLoop < size; iLoop++)
                {
                    string strByteData = "";

                    if (type == DeviceType.Word)
                    {
                        strByteData = writeVal.Substring(iLoop * 2, 2);
                    }
                    else if (type == DeviceType.Bit)
                    {
                        strByteData = writeVal.Substring(iLoop * 1, 1);
                    }

                    byte byteData = Convert.ToByte(strByteData, 16);
                    lstRet.Add(byteData);       //34~
                }
            }

            return lstRet.ToArray();
        }

        private string GetHexString(byte[] arrBY)
        {
            string ret = "";
            foreach (byte by in arrBY)
            {
                ret += string.Format("{0:X2}", by);
            }
            return ret;
        }
        public bool PlcReadBlock(string strVar, long lReadCnt, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            return Read(DeviceType.Word, strVar, (int)lReadCnt, out strRtnValue, out strErrCode, out strErrCode, iPlcTimeOut);
        }

        public bool PlcWrite(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            strErrCode = "";
            strErrMsg = "";
            return Write(DeviceType.Word, strVar, strWriteValue, out strErrCode, out strErrMsg, iPlcTimeOut);
        }

        public bool PlcWriteBlock(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            strErrCode = "";
            strErrMsg = "";
            return Write(DeviceType.Word, strVar, strWriteValue, out strErrCode, out strErrMsg, iPlcTimeOut);
        }

        public bool ReadBit(string baseAddr, int seq, string dataType)
        {
            int startIDX_POS = 30;


            bool bRet = false;
            string strErrMsg = "";
            string strRtnValue = "";
            try
            {
                string strWordAddr = ((seq / 16) + Convert.ToInt32(baseAddr)).ToString();

                string strBitAddr = string.Format("{0:00}", seq % 16);

                int size = 1;

                byte[] bytesData = GetCommand(DeviceType.Bit, strWordAddr, strBitAddr, size, dataType);

                SocketError errCode = SocketError.Success;
                m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                int innerSize = 0;
                innerSize = size * 2;


                byte[] bufferRcv = new byte[innerSize + startIDX_POS];
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
                    if (errCode == SocketError.Success)
                    {
                        if (m_clientSocket.Available > 0)
                        {
                            m_clientSocket.Receive(bufferRcv, SocketFlags.None);
                        }
                    }
                    if (bufferRcv[0] == 70 && bufferRcv[1] == 73 && bufferRcv[2] == 78 && bufferRcv[3] == 83
                            && bufferRcv[16] == 192 && bufferRcv[17] == 0 && bufferRcv[18] == 2
                            && bufferRcv[26] == 1 && bufferRcv[27] == 1)
                    {
                        string strRCV = GetHexString(bufferRcv);
                        strRtnValue = strRCV.Substring(startIDX_POS * 2, innerSize);

                        bRet = Convert.ToInt32(strRtnValue) > 0 ? true : false;
                        break;
                    }
                    else
                    {
                        m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);
                        System.Threading.Thread.Sleep(50);
                    }
                    if ((DateTime.Now - startDT).TotalMilliseconds >= 1000)
                    {   //Time-out

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
        public string GetDicConnectionArgs(string key)
        {

            if (m_dicConnectionArgs.ContainsKey(key))
            {
                return m_dicConnectionArgs[key];
            }
            return string.Empty;
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
                Connect(param);
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
        }

        public bool WriteBit(string baseAddr, int seq, bool val, string dataType)
        {


            string strWordAddr = ((seq / 16) + Convert.ToInt32(baseAddr)).ToString();

            string strBitAddr = string.Format("{0:00}", seq % 16);

            bool bRet = false;
            string strWriteValue = "";
            if (val)
            {
                strWriteValue = "1";
            }
            else
            {
                strWriteValue = "0";
            }
            try
            {
                int size = 1;

                byte[] bytesData = GetCommand(DeviceType.Bit, strWordAddr, strBitAddr, size, dataType, strWriteValue);

                SocketError errCode = SocketError.Success;
                m_clientSocket.Send(bytesData, 0, bytesData.Length, SocketFlags.None, out errCode);


                switch (errCode)
                {
                    case SocketError.Success:
                        bRet = true;
                        break;
                    default:
                        bRet = false;
                        break;
                }


            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
            return bRet;
        }

        public bool WriteWord(string tag, string baseAddr, int seq, string val)
        {
            string errMsg = "";
            string errCode = "";
            return Write(DeviceType.Word, tag + string.Format("{0:0000}", Convert.ToInt32(baseAddr) + seq), val, out errCode, out errMsg, 1000);
        }

        public bool WriteWord(string strVar, string strWriteValue, out string strErrCode)
        {
            strErrCode = "";
            string errCode = "";
            return Write(DeviceType.Word, strVar, strWriteValue, out errCode, out strErrCode, 1000);
        }
    }
}
