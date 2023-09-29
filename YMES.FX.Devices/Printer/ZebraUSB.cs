using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.Devices.Printer
{
    public class ZebraUSB : Base.IDevice
    {
        bool m_bOpen = false;
        IntPtr m_hPrinter = IntPtr.Zero;
        DOCINFOA m_di = new DOCINFOA();
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        /// <summary>
        /// Open the device
        /// </summary>
        /// <param name="param">PRINT_NAME, DOC_NAME/Zebra Label, DATA_TYPE/RAW</param>
        /// <returns>Result of Opening</returns>
        public bool OpenDevice(Dictionary<string, object> param)
        {
            if(param.ContainsKey("DOC_NAME")== false)
            {
                param["DOC_NAME"] = "Zebra Label";
            }
            if (param.ContainsKey("DATA_TYPE") == false)
            {
                param["DATA_TYPE"] = "RAW";
            }
            m_di.pDocName = param["DOC_NAME"].ToString();
            m_di.pDataType = param["DATA_TYPE"].ToString();
            m_bOpen = OpenPrinter(param["PRINT_NAME"].ToString().Normalize(), out m_hPrinter, IntPtr.Zero);
            return m_bOpen;

        }
        public bool IsConnected()
        {
            return m_bOpen;
        }
        public bool CloseDevice()
        {
            if (m_hPrinter != null)
            {
                ClosePrinter(m_hPrinter);
            }
            return true;
        }
        public bool SendBytesToPrinter(string data)
        {
            return SendBytesToPrinter(data, data.Length);
        }
        private bool SendBytesToPrinter(string data, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;

            bool bSuccess = false;




            IntPtr buf = Marshal.StringToCoTaskMemAnsi(data);
            StartDocPrinter(m_hPrinter, 1, m_di);
            StartPagePrinter(m_hPrinter);
            bSuccess = WritePrinter(m_hPrinter, buf, dwCount, out dwWritten);
            EndPagePrinter(m_hPrinter);
            EndDocPrinter(m_hPrinter);
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
                throw new Win32Exception(dwError);
            }
            return bSuccess;
        }
    }
}
