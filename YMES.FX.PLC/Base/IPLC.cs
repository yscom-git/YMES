using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.PLC.Base
{
    public interface IPLC
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param">IP, PORT, STATION_NO, COMPORT, BAUDRATE</param>
        void Start(Dictionary<string, string> param);
        void Close();
        bool Connect(Dictionary<string, string> dicConnectParam);
        bool WriteWord(string tag, string baseAddr, int seq, string val);
        bool WriteBit(string baseAddr, int seq, bool val, string dataType);
        bool ReadBit(string baseAddr, int seq, string dataType);
        bool WriteWord(string strVar, string strWriteValue, out string strErrCode);
        bool SplitValue(string strReadBuf, ref Common.PLC_DeviceArray[] buf);
        string GetBitTag();
        string GetAbsAddr(string dataType, string baseAddr, int seq);
        string GetDicConnectionArgs(string key);

        bool Connected { get; }
        string MemoryTag { get; set; }

        #region LEGACY
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
        bool PlcReadBlock(string strVar, long lReadCnt, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000);
        /// <summary>
        /// Write the PLC data(BLOCK UNIT)(LEGACY)
        /// </summary>
        /// <param name="strVar">plc variable name(ex]%DB6420)</param>
        /// <param name="strWriteValue">value(hexa code)</param>
        /// <param name="strErrCode">Code of error</param>
        /// <param name="strErrMsg">Message of error</param>
        /// <param name="iPlcTimeOut">response waiting time</param>
        /// <returns>condition of writing</returns>
        bool PlcWriteBlock(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000);
        /// <summary>
        /// Reply to network condition(LEGACY)
        /// </summary>
        /// <param name="strIpAddr">ojbect ip</param>
        /// <param name="iDelayTime">delay time</param>
        /// <param name="strErrMsg">message of error</param>
        /// <returns>condition of response</returns>
        bool Ping(string strIpAddr, int iDelayTime, ref string strErrMsg);
        /// <summary>
        /// Write the PLC data(LEGACY)
        /// </summary>
        /// <param name="strVar">plc variable name(ex]%DB6420)</param>
        /// <param name="strWriteValue">value(2digit hexa code)</param>
        /// <param name="strErrCode">Code of error</param>
        /// <param name="strErrMsg">Message of error</param>
        /// <param name="iPlcTimeOut">response waiting time</param>
        /// <returns>condition of writing</returns>
        bool PlcWrite(string strVar, string strWriteValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000);
        /// <summary>
        /// Get the PLC data(LEGACY)
        /// </summary>
        /// <param name="strVar">plc variable name(ex]%DB6420)</param>
        /// <param name="strRtnValue">return value(2digit hexa code)</param>
        /// <param name="strErrCode">Code of error</param>
        /// <param name="strErrMsg">Message of error</param>
        /// <param name="iPlcTimeOut">response waiting time</param>
        /// <returns>condition of reading</returns>
        bool PlcRead(string strVar, ref string strRtnValue, ref string strErrCode, ref string strErrMsg, int iPlcTimeOut = 3000);
        /// <summary>
        /// Convert string to PLC DATA STRUCTURE(LEGACY)
        /// </summary>
        /// <param name="strReadBuf">original value</param>
        /// <param name="buf">converted value</param>
        /// <returns>Conversion condition</returns>        
        #endregion
    }
}
