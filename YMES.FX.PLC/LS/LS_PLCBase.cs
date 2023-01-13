using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.PLC.LS
{
    public class LS_PLCBase
    {
        private string m_BaseAddr = "";
        private string m_DefaultBitType = "DX";
        private string m_DefaultWordType = "DW";
        private string m_DefaultByteType = "DB";

        public enum PlcModelEnum
        {
            XGT,
            XGB,
            MK
        }
        private PlcModelEnum m_PLCModel = PlcModelEnum.XGT;
        private int m_SendTimeout = 2000;
        private int m_ReceiveTimeout = 2000;

        public string GetAbsAddr(string dataType, string baseAddr, int seq)
        {
            if (dataType.Length == 2)
            {
                dataType = "%" + dataType;
            }
            int nAddr = Convert.ToInt32(baseAddr);
            switch (dataType.Substring(2, 1))
            {
                case "B":
                    nAddr = (nAddr * 2) + seq;
                    break;
                case "W":
                    nAddr = (nAddr) + seq;
                    break;
                case "X":
                    if (m_PLCModel == PlcModelEnum.XGT)
                    {
                        nAddr = (nAddr * 16) + seq;
                    }
                    break;

            }
            return dataType + string.Format("{0:0000}", nAddr);
        }

        private int m_SizeOfRcvBuffer = 500;

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
        /// prefix of memory variable name(D, M ..)
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public string DefaultBitType
        {
            get { return m_DefaultBitType; }
            set { m_DefaultBitType = value; }
        }
        /// <summary>
        /// prefix of memory variable name(D, M ..)
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public string DefaultByteType
        {
            get { return m_DefaultByteType; }
            set { m_DefaultByteType = value; }
        }
        /// <summary>
        /// prefix of memory variable name(D, M ..)
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public string DefaultWordType
        {
            get { return m_DefaultWordType; }
            set { m_DefaultWordType = value; }
        }
        /// <summary>
        /// Base address of memory(without prefix)
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public string BaseAddr
        {
            get { return m_BaseAddr; }
            set { m_BaseAddr = value; }
        }
        /// <summary>
        /// Type of PLC
        /// </summary>
        [System.ComponentModel.Category("_PLC_SETTING")]
        public PlcModelEnum PLCModel
        {
            get { return m_PLCModel; }
            set { m_PLCModel = value; }
        }
    }
}
