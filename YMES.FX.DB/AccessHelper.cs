using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMES.FX.DB.Base;
using System.ComponentModel;

namespace YMES.FX.DB
{
    [ToolboxItem(true)]
    public class AccessHelper : DBComponent, IDBBase
    {
        private string m_strDB = "";
        private OleDbConnection m_DBConn = null;
        private bool m_IsOpen = false;
        private bool m_AutoCloseMode = false;

        public bool AutoCloseMode
        {
            get { return m_AutoCloseMode; }
            set { m_AutoCloseMode = value; }
        }
        public bool IsOpen
        {
            get { return m_IsOpen; }
            set { m_IsOpen = value; }
        }

        string IDBBase.OutRefCurString
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        string IDBBase.DBSVR
        {
            get
            {
                return m_strDB;
            }
        }

        string IDBBase.DBUID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IDBBase.DBPWD
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IDBBase.DBNM
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IDBBase.IsDBTrace
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        bool IDBBase.IsAsynBusy
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        DBOpenEnum IDBBase.DBOpenTY
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public AccessHelper(string strDB)
        {
            m_strDB = strDB;
        }
        public AccessHelper()
        {

        }
        public bool OpenDB()
        {
            try
            {
                m_IsOpen = false;
                if (string.IsNullOrEmpty(m_strDB))
                {
                    Debug.WriteLine("DB Path error!");
                    return false;

                }
                else if (System.IO.File.Exists(m_strDB) == false)
                {
                    Debug.WriteLine("There is no DB File");
                    return false;

                }
                string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_strDB;
                m_DBConn = new OleDbConnection(connStr);
                m_DBConn.Open();
                m_IsOpen = true;
                return true;
            }
            catch
            {
                return false;
            }

        }
        public DataTable ExcuteQuery(string query)
        {
            DataTable dt = new DataTable();

            try
            {
                if (m_IsOpen == false)
                {
                    OpenDB();
                }
                OleDbDataAdapter adp = new OleDbDataAdapter(query, m_DBConn);
                adp.Fill(dt);
                if (AutoCloseMode)
                {
                    CloseDB();
                }
                return dt;
            }
            catch (Exception eLog)
            {
                Debug.WriteLine(eLog.ToString());
                return null;
            }

        }
        public bool ExcuteNonQuery(string query)
        {
            try
            {
                if (m_IsOpen == false)
                {
                    OpenDB();
                }
                OleDbCommand com = new OleDbCommand(query, m_DBConn);
                com.ExecuteNonQuery();
                if (AutoCloseMode)
                {
                    CloseDB();
                }
                return true;
            }
            catch (Exception eLog)
            {
                Debug.WriteLine(eLog.ToString());
                return false;
            }


        }
        public int ExcuteNonQuery(string query, Dictionary<string, object> param)
        {
            return -1;
        }
        public void CloseDB()
        {
            m_IsOpen = false;
            m_DBConn.Close();
        }

        bool IDBBase.Open(string path)
        {
            m_strDB = path;
            return OpenDB();
        }

        bool IDBBase.Open(string svr, string uid, string pwd, string dbnm)
        {
            m_strDB = svr;
            return OpenDB();
        }

        bool IDBBase.Close()
        {
            CloseDB();
            return true;
        }

        bool IDBBase.IsOpen()
        {
            return m_IsOpen;
        }

        DataTable IDBBase.ExcuteQuery(string query, Dictionary<string, string> param)
        {
            return ExcuteQuery(query);
        }

        int IDBBase.ExcuteNonQuery(string query, Dictionary<string, string> param)
        {
            try
            {
                ExcuteNonQuery(query);

            }
            catch
            {
                return 0;
            }
            return 1;
        }

        int IDBBase.BulkInsert(string toTable, ref DataTable sendData, bool bAppend)
        {
            throw new NotImplementedException();
        }

        void IDBBase.AsyncExcute(object sender, DBQueryTypeEnum qt, string query, Dictionary<string, string> param)
        {
            throw new NotImplementedException();
        }


        public bool IsOciConnect
        {
            get { return false; }
        }

        /*
        public event BackgroundRCV OnBackgroundRCV;

        public event BackgroundPR OnBackgroundPR;
        */

        bool IDBBase.AsynBusy(object key)
        {
            return false;
        }

        bool IDBBase.IsOciConnect
        {
            get { throw new NotImplementedException(); }
        }

        event BackgroundRCV IDBBase.OnBackgroundRCV
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event BackgroundPR IDBBase.OnBackgroundPR
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }
    }
}
