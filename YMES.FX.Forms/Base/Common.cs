using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace YMES.FX.MainForm.Base
{
    public class Common
    {
        public enum MsgTypeEnum
        {
            None,
            Error,
            Warnning,
            Alarm,
            Trace
        }
        public enum DateFormatEnum
        {
            YYYYMMDD,
            DDMMYYYY,
            MMDDYYYY
        }
        public struct TitST
        {
            public string title;
            public Font font;
        }
        public static Form GetForm(string frmName)
        {
            foreach(Form frm in Application.OpenForms)
            {
                if(frm.Name == frmName)
                {
                    return frm;
                }
            }
            return null;
        }

        public static DataTable GetXml2DT(string path)
        {

            try
            {
                if (File.Exists(path))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(path);
                    if (ds.Tables.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            for (int col = 0; col < ds.Tables[0].Columns.Count; col++)
                            {
                                ds.Tables[0].Rows[row][col] = ds.Tables[0].Rows[row][col].ToString().Trim();
                            }
                        }
                    }
                    return ds.Tables[0];
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            [MarshalAs(UnmanagedType.U2)]
            public short wYear;
            [MarshalAs(UnmanagedType.U2)]
            public short wMonth;
            [MarshalAs(UnmanagedType.U2)]
            public short wDayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public short wDay;
            [MarshalAs(UnmanagedType.U2)]
            public short wHour;
            [MarshalAs(UnmanagedType.U2)]
            public short wMinute;
            [MarshalAs(UnmanagedType.U2)]
            public short wSecond;
            [MarshalAs(UnmanagedType.U2)]
            public short wMilliseconds;

            public void Init()
            {
                wYear = 0;
                wMonth = 0;
                wDayOfWeek = 0;
                wDay = 0;
                wHour = 0;
                wMinute = 0;
                wSecond = 0;
                wMilliseconds = 0;
            }
        }
        #region API Call
        //API - 시스템 시간 가져오기
        [DllImport("kernel32.dll", SetLastError = true)]
        public extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);

        //API - 시스템 시간 설정
        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        public extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);
        #endregion

    }
}
