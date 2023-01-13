using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMES.FX.PLC.Base;

namespace YMES.FX.PLC.LS
{
    public class CNET :LS_PLCBase, IPLC
    {
        private string CN_END_CHAR = "\x03";
        private string m_PortName = "";
        private int m_BaudRate = 19200;
        private int m_SerialBufferSize = 4096;
        private bool m_InnerConnected = false;
        private string m_StationNo = "00";
        private Dictionary<string, string> m_dicConnectionArgs = null;
        private string m_MemoryTag = "M";

        public string GetDicConnectionArgs(string key)
        {

            if (m_dicConnectionArgs.ContainsKey(key))
            {
                return m_dicConnectionArgs[key];
            }
            return string.Empty;
        }
        public string MemoryTag
        {
            get { return m_MemoryTag; }
            set { m_MemoryTag = value; }
        }
        public string StationNo
        {
            get { return m_StationNo; }
            set { m_StationNo = value; }
        }

        public enum CnetComEnum
        {
            UnitRead,
            BatchRead,
            UnitWrite,
            BatchWrite

        }
        public int SerialBufferSize
        {
            get { return m_SerialBufferSize; }
            set { m_SerialBufferSize = value; }
        }
        public string PortName
        {
            get { return m_PortName; }
            set { m_PortName = value; }
        }
        public int BaudRate
        {
            get { return m_BaudRate; }
            set { m_BaudRate = value; }
        }
        private System.IO.Ports.SerialPort m_plcPort = new System.IO.Ports.SerialPort();



        public bool Connected
        {
            get { return m_InnerConnected; }
        }
        public void Close()
        {
            m_InnerConnected = false;
            m_plcPort.Close();
        }
        public bool Connect(string comPort, int baudRate)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@PLCIP", comPort);
            param.Add("@PLC_PORT", baudRate.ToString());
            return Connect(param);
        }
        public bool Connect(Dictionary<string, string> dicConnectParam)
        {
            try
            {
                m_dicConnectionArgs = dicConnectParam;
                string ip = dicConnectParam["@PLCIP"].ToString();
                string port = dicConnectParam["@PLC_PORT"].ToString();
                string comPort = "";
                if (ip.ToUpper().Contains("COM") == false)
                {
                    comPort = "COM" + ip.ToString();
                }
                else
                {
                    comPort = ip;
                }
                PortName = comPort;
                BaudRate = Convert.ToInt32(port);
                if (m_plcPort.IsOpen == true)
                {
                    Close();
                }

                m_plcPort.PortName = comPort;
                m_plcPort.BaudRate = BaudRate;
                m_plcPort.Parity = System.IO.Ports.Parity.None;
                m_plcPort.DataBits = 8;
                m_plcPort.StopBits = System.IO.Ports.StopBits.One;
                m_plcPort.WriteBufferSize = m_SerialBufferSize;
                m_plcPort.ReadBufferSize = m_SerialBufferSize;

                m_plcPort.Handshake = System.IO.Ports.Handshake.None;
                /*
                m_plcPort.DtrEnable = false;
                m_plcPort.RtsEnable = false;
                */
                m_plcPort.ReadTimeout = 300;
                m_plcPort.WriteTimeout = 200;
                if (comPort.Contains("COM0") == false && comPort != "COM")
                {
                    m_plcPort.Open();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[None-Used Serial]" + comPort);
                }
                m_InnerConnected = true;


                return true;
            }
            catch (Exception eLog)
            {
                throw eLog;
            }
        }



        protected string GetHeaderArea()
        {

            string strRet = "";
            strRet += "\x05";   //ENQ
            //strRet += m_StationNo.PadLeft(2,'0');   //Station No
            strRet += String.Format("{0:X2}", Convert.ToInt32(m_StationNo));
            return strRet;
        }
        private string GetComTypeString(CnetComEnum comType)
        {
            string commandType = "";
            switch (comType)
            {
                case CnetComEnum.UnitRead:
                    commandType = "R" + "SS";
                    break;
                case CnetComEnum.BatchRead:
                    commandType = "R" + "SB";
                    break;
                case CnetComEnum.UnitWrite:
                    commandType = "W" + "SS";

                    break;
                case CnetComEnum.BatchWrite:
                    commandType = "W" + "SB";

                    break;
            }
            return commandType;
        }
        protected string GetCommandArea(CnetComEnum comType, string strVar, string val = "", int blockSize = 1)
        {
            string strRet = "";

            string commandType = GetComTypeString(comType);
            int writeSize = 1;

            strRet += commandType;
            if (commandType.Contains("SS"))
            {
                strRet += (1).ToString().PadLeft(2, '0');
            }
            strRet += strVar.Length.ToString().PadLeft(2, '0');
            strRet += strVar;
            if (commandType.Contains("RSB"))
            {
                strRet += blockSize.ToString().PadLeft(2, '0');
            }
            else if (commandType.Contains("WSB"))
            {
                switch (strVar.Substring(2, 1))
                {
                    case "B":
                        writeSize = val.Length / 2;
                        break;
                    case "W":
                        writeSize = val.Length / 4;
                        break;
                    case "D":
                        writeSize = val.Length / 8;
                        break;
                    case "L":
                        writeSize = val.Length / 16;
                        break;
                }
                strRet += writeSize.ToString().PadLeft(2, '0');
            }
            if (commandType.Contains("W"))
            {
                strRet += val;
            }

            strRet += "\x04";   //EOT  


            return strRet;
        }
        public bool Ping(string strIpAddr, int iDelayTime, ref string strErrMsg)
        {
            strErrMsg = "Serial Port has not Ping command";
            return true;
        }
        public bool PlcRead(string strVar, ref string strRtnValue, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            string strErrCode = "";
            return PlcRead(strVar, ref strRtnValue, ref strErrCode, ref strErrMsg, iPlcTimeOut);
        }
        private bool GetResponse(CnetComEnum comType, string header, string command, int iPlcTimeOut, out string strErrMsg)
        {
            string strRtnValue = "";
            return GetResponse(comType, header, command, iPlcTimeOut, out strRtnValue, out strErrMsg);
        }
        private bool GetResponse(CnetComEnum comType, string header, string command, int iPlcTimeOut, out string strRtnValue, out string strErrMsg)
        {
            strRtnValue = "";
            strErrMsg = "";
            try
            {



                DateTime startDT = DateTime.Now;
                m_plcPort.Write(header + command);
                do
                {
                    try
                    {
                        string readData = m_plcPort.ReadTo(CN_END_CHAR);
                        readData = readData.Replace("\x02", "");
                        if (readData.Length > 5 && readData.Substring(0, 1) == "\x06" && readData.Substring(3, 3) == GetComTypeString(comType))
                        {   //ACK
                            m_InnerConnected = true;
                            if (readData.Length > 10)
                            {
                                strRtnValue = readData.Substring(10);
                            }
                            return true;
                        }
                        else
                        {   //NAK
                            m_plcPort.Write(header + command);
                        }
                        if ((DateTime.Now - startDT).TotalMilliseconds >= iPlcTimeOut)
                        {   //Time-out
                            strRtnValue = "";
                            strErrMsg = "Timeout(" + iPlcTimeOut + ")";
                            return false;
                        }
                    }
                    catch (Exception innerLog)
                    {
                        System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "/INNER]" + innerLog.Message.ToString());
                        if ((DateTime.Now - startDT).TotalMilliseconds >= iPlcTimeOut)
                        {   //Time-out
                            strRtnValue = "";
                            strErrMsg = "Timeout(" + iPlcTimeOut + ")";
                            m_InnerConnected = false;
                            return false;
                        }
                    }
                    finally
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                } while (true);
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
                m_InnerConnected = false;
                return false;
            }
        }
        public bool PlcRead(string strVar, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {

            try
            {

                if (m_plcPort.IsOpen == false)
                {
                    strErrMsg = "Connection Error";
                    return false;
                }
                m_plcPort.DiscardInBuffer();
                m_plcPort.DiscardOutBuffer();
                string header = GetHeaderArea();
                string command = GetCommandArea(CnetComEnum.UnitRead, strVar);
                return GetResponse(CnetComEnum.UnitRead, header, command, iPlcTimeOut, out strRtnValue, out strErrMsg);

            }
            catch (Exception eLog)
            {
                strErrMsg = eLog.Message;
                strErrCode = "EX";
                return false;
            }
        }
        public bool PlcReadBlock(string strVar, long lReadCnt, ref string strRtnValue, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            string strErrCode = "";
            return PlcReadBlock(strVar, lReadCnt, ref strRtnValue, ref strErrCode, ref strErrMsg, iPlcTimeOut);
        }

        public bool PlcReadBlock(string strVar, long lReadCnt, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {


            try
            {

                if (m_plcPort.IsOpen == false)
                {
                    strErrMsg = "Connection Error";
                    return false;
                }
                m_plcPort.DiscardInBuffer();
                m_plcPort.DiscardOutBuffer();
                string header = GetHeaderArea();
                string command = GetCommandArea(CnetComEnum.BatchRead, strVar, "", (int)lReadCnt);


                return GetResponse(CnetComEnum.BatchRead, header, command, iPlcTimeOut, out strRtnValue, out strErrMsg);

            }
            catch (Exception eLog)
            {
                strErrMsg = eLog.Message;
                strErrCode = "EX";
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());

                return false;
            }
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
            catch
            {

            }
            return bRet;
        }
        public bool PlcWrite(string strVar, string strWriteValue, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            string strErrCode = "";
            return PlcWrite(strVar, strWriteValue, ref strErrCode, ref strErrMsg, iPlcTimeOut);
        }
        public bool PlcWrite(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            try
            {

                if (m_plcPort.IsOpen == false)
                {
                    strErrMsg = "Connection Error";
                    return false;
                }
                string header = GetHeaderArea();
                string command = GetCommandArea(CnetComEnum.UnitWrite, strVar, strWriteValue);

                return GetResponse(CnetComEnum.UnitWrite, header, command, iPlcTimeOut, out strErrMsg);
            }
            catch (Exception eLog)
            {
                strErrMsg = eLog.Message;
                strErrCode = "EX";
                return false;
            }

        }
        public bool PlcWriteBlock(string strVar, string strWriteValue, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            string strErrCode = "";
            return PlcWriteBlock(strVar, strWriteValue, ref strErrCode, ref strErrMsg, iPlcTimeOut);
        }
        public bool PlcWriteBlock(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000)
        {
            try
            {
                if (m_plcPort.IsOpen == false)
                {
                    strErrMsg = "Connection Error";
                    return false;
                }
                string header = GetHeaderArea();
                string command = GetCommandArea(CnetComEnum.BatchWrite, strVar, strWriteValue);

                return GetResponse(CnetComEnum.BatchWrite, header, command, iPlcTimeOut, out strErrMsg);
            }
            catch (Exception eLog)
            {
                strErrMsg = eLog.Message;
                strErrCode = "EX";
                return false;
            }
        }
        public string GetBitTag()
        {
            return m_MemoryTag + "X";
        }
        public void Start(Dictionary<string, string> param)
        {
            string comPort = param["@PLCIP"];
            if (param.ContainsKey("@PLC_PORT") == false)
            {
                param["@PLC_PORT"] = "19200";
            }
            if (param.ContainsKey("@PLC_STATION_NO"))
            {
                this.StationNo = param["@PLC_STATION_NO"];
            }
            else
            {
                this.StationNo = "00";
            }

            Connect(param);

        }

        public void Start(string comPort, string baudRate, string stationNo)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("COMPORT", comPort);
            param.Add("BAUDRATE", baudRate);
            param.Add("STATION_NO", stationNo);
            Start(param);

        }
        public bool ReadBit(string baseAddr, int seq, string dataType)
        {

            return false;
        }
        public bool WriteBit(string baseAddr, int seq, bool val, string dataType)
        {
            if (string.IsNullOrEmpty(dataType))
            {
                dataType = DefaultBitType;
            }
            string strBitAddr = string.Format("%" + dataType + "{0:0000}", Convert.ToInt32(baseAddr) + seq);
            string strErrMsg = "";
            string strErrCode = "";
            return PlcWrite(strBitAddr, val ? "01" : "00", ref strErrCode, ref strErrMsg, 1000);
        }

        public bool WriteWord(string tag, string baseAddr, int seq, string val)
        {
            string strWordAddr = string.Format("%" + DefaultWordType + "{0:0000}", Convert.ToInt32(baseAddr) + seq);
            string strErrMsg = "";
            string strErrCode = "";
            return PlcWrite(strWordAddr, val.PadLeft(4, '0'), ref strErrCode, ref strErrMsg, 1000);
        }

        public bool WriteWord(string strVar, string strWriteValue, out string strErrCode)
        {
            strErrCode = "";
            string strErrMsg = "";
            return PlcWrite(strVar, strWriteValue.PadLeft(4, '0'), ref strErrCode, ref strErrMsg, 1000);
        }
    }
}
