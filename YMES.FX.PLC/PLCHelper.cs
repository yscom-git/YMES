using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMES.FX.PLC.Base;
using YMES.FX.PLC.LS;

namespace YMES.FX.PLC
{
    public class PLCHelper
    {
        public delegate void PlcSeqSignal(object sender, int pos, bool bVal, Common.PLC_DeviceArray[] currentBuffer);
        public event PlcSeqSignal OnPlcSeqSignal;
        private System.Windows.Forms.Control m_ParentControl = null;
        private IPLC m_PLC = null;
        private Dictionary<string, string> m_DicVars = new Dictionary<string, string>();
        private int m_SizeOfBlock = 160;

        private Common.PLC_DeviceArray[] m_sMachPlcBuffer = null;
        private Common.PLC_DeviceArray[] m_sPreMachPlcBuffer = null;
        private bool m_LoopRead = true;
        public enum PlcTypeEnum
        {
            OMRON_FINS_ETHERNET,
            LS_CNET,
            LS_FENET,
            MITSUBISHI_MELSEC_ETHERNET
        }


        public PLCHelper(System.Windows.Forms.Control parentControl, PlcTypeEnum plcType, Dictionary<string, string> param)
        {
            m_ParentControl = parentControl;
            m_DicVars = param;
            if (param == null || param.Count <= 0)
            {
                throw new Exception("This needs parameters for initialization.");
            }
            switch (plcType)
            {
                case PlcTypeEnum.LS_CNET:
                    m_PLC = new LS.CNET();
                    break;
                case PlcTypeEnum.LS_FENET:
                    m_PLC = new LS.FENET();
                    break;
                case PlcTypeEnum.OMRON_FINS_ETHERNET:
                    m_PLC = new Omron.FINS_ETH();
                    break;
                case PlcTypeEnum.MITSUBISHI_MELSEC_ETHERNET:
                    m_PLC = new Mitsubishi.MELSEC_ETH();
                    break;
            }
        }
        private void InitVars(int blockSize)
        {
            m_sMachPlcBuffer = new Common.PLC_DeviceArray[blockSize];
            m_sPreMachPlcBuffer = new Common.PLC_DeviceArray[blockSize];


            for (int iLoop1 = 0; iLoop1 < blockSize; ++iLoop1)
            {
                m_sPreMachPlcBuffer[iLoop1].iWordValue = 0;
                m_sMachPlcBuffer[iLoop1].iWordValue = 0;

                m_sPreMachPlcBuffer[iLoop1].iBitValue = new short[16];
                m_sMachPlcBuffer[iLoop1].iBitValue = new short[16];
                for (int iLoop2 = 0; iLoop2 < 16; ++iLoop2)
                {
                    m_sPreMachPlcBuffer[iLoop1].iBitValue[iLoop2] = 0;
                    m_sMachPlcBuffer[iLoop1].iBitValue[iLoop2] = 0;
                }
            }

        }
        public void Start()
        {
            m_PLC.Start(m_DicVars);
            m_SizeOfBlock = Convert.ToInt32(GetVars("@BLOCK_SIZE"));
            InitVars(m_SizeOfBlock);
            System.Threading.ThreadPool.QueueUserWorkItem(PlcBlockRead, 50);
        }
        public void Close()
        {
            m_LoopRead = false;
            m_PLC.Close();
        }
        public string GetVars(string key)
        {
            if (m_DicVars.ContainsKey(key))
            {
                return m_DicVars[key];
            }
            return string.Empty;
        }
        public bool WriteBit(string baseAddr, int seq, bool val, string dataType)
        {
            return m_PLC.WriteBit(baseAddr, seq, val, dataType);
        }
        public bool WriteBit(int seq, bool val, string dataType)
        {
            return WriteBit(GetVars("@PLCBASEADDR"), seq, val, dataType);
        }
        public bool WriteWord(string baseAddr, int seq, string val, string dataType)
        {

            return m_PLC.WriteWord(dataType, baseAddr, seq, val);
        }
        public bool WriteWord(int seq, string val, string dataType)
        {
            return WriteWord(GetVars("@PLCBASEADDR"), seq, val, dataType);
        }
        public bool ReadBit(string baseAddr, int seq, string dataType)
        {
            return m_PLC.ReadBit(baseAddr, seq, dataType);
        }
        public bool ReadBit(int seq, string dataType)
        {
            return ReadBit(GetVars("@PLCBASEADDR"), seq, dataType);
        }
        public bool Read(string strVar, out string strRtnValue)
        {
            strRtnValue = "";
            string strErrCode = "";
            string strErrMsg = "";
            return m_PLC.PlcRead(strVar, ref strRtnValue, ref strErrCode, ref strErrMsg, 1000);
        }
        public bool Read(string baseAddr, int seq, string dataType, out string strRtnValue)
        {
            strRtnValue = "";
            string strVar = m_PLC.GetAbsAddr(dataType, baseAddr, seq);
            return Read(strVar, out strRtnValue);
        }
        public bool Read(int seq, string dataType, out string strRtnValue)
        {
            strRtnValue = "";
            return Read(GetVars("@PLCBASEADDR"), seq, dataType, out strRtnValue);
        }
        public string GetBlockAddr()
        {
            return m_PLC.GetAbsAddr(GetVars("@BLOCK_TYPE"), GetVars("@PLCBASEADDR"), 0);
        }
        private void PLCProcess(int iLoop1, int iLoop2, bool bValue, Common.PLC_DeviceArray[] currentBuffer)
        {
            try
            {
                if (iLoop1 == -1 && iLoop2 == -1)
                {
                    OnPlcSeqSignal(this, -1, bValue, currentBuffer);
                }
                else
                {
                    int nDiff = (16 * iLoop1) + iLoop2;
                    if (OnPlcSeqSignal != null)
                    {
                        OnPlcSeqSignal(this, nDiff, bValue, currentBuffer);
                    }
                }
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }
        }
        private void PlcBlockRead(object arg)
        {

            string strErrCode = "";
            string strErrMsg = "";
            string strPlcBuffer = "";
            int iLoop1 = 0;
            int iLoop2 = 0;

            try
            {

                string baseVar = GetVars("@PLCBASEADDR");

                DateTime chkAliveDT = new DateTime();
                string strDeviceAddr = GetBlockAddr();
                int tick = 100;
                if (arg is int)
                {
                    tick = (int)arg;
                }
                chkAliveDT = DateTime.Now;
                do
                {

                    try
                    {


                        strErrCode = "";
                        strErrMsg = "";
                        strPlcBuffer = "";
                        if (m_PLC.Connected == true)
                        {   //Normal Condition

                            if (m_PLC.PlcReadBlock(strDeviceAddr, m_SizeOfBlock, ref strPlcBuffer, ref strErrCode, ref strErrMsg, 2000))
                            {
                                m_PLC.SplitValue(strPlcBuffer, ref m_sMachPlcBuffer);


                                for (iLoop1 = 0; iLoop1 < m_SizeOfBlock; iLoop1++)
                                {
                                    //16점을 읽어서 처리함
                                    for (iLoop2 = 0; iLoop2 <= 0x0F; iLoop2++)
                                    {

                                        if (m_sMachPlcBuffer[iLoop1].iBitValue[iLoop2] != m_sPreMachPlcBuffer[iLoop1].iBitValue[iLoop2])
                                        {   //PLC Event 발생 시 처리

                                            //이전 처리 버퍼에 현재 상태를 저장 함
                                            m_sPreMachPlcBuffer[iLoop1].iBitValue[iLoop2] = m_sMachPlcBuffer[iLoop1].iBitValue[iLoop2];

                                            //비트값 임시 저장
                                            int iBitValue = m_sMachPlcBuffer[iLoop1].iBitValue[iLoop2];
                                            m_ParentControl.Invoke(new System.Windows.Forms.MethodInvoker(
                                            delegate ()
                                            {
                                                PLCProcess(iLoop1, iLoop2, iBitValue == 1 ? true : false, m_sMachPlcBuffer);
                                            }));


                                        }
                                    }
                                }
                            }
                        }
                        else
                        {   //OFF
                            m_ParentControl.Invoke(new System.Windows.Forms.MethodInvoker(
                            delegate ()
                            {
                                PLCProcess(-1, -1, false, m_sPreMachPlcBuffer);
                            }));
                        }

                    }
                    catch (Exception innerLog)
                    {
                        System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "/INNER" + "]" + innerLog.Message.ToString());
                    }
                    finally
                    {

                        System.Threading.Thread.Sleep(tick);

                    }
                } while (m_LoopRead);
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message.ToString());
            }

        }
    }
}
