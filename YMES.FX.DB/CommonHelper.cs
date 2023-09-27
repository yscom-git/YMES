using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMES.FX.DB.Base;

namespace YMES.FX.DB
{
    [ToolboxItem(true)]
    public  class CommonHelper : DBComponent, IDBBase
    {
        private IDBBase m_DB;

        public CommonHelper()
        {
            AssignDBType(m_DBKind);
        }
        public CommonHelper(DBKindEnum ty)
        {
            AssignDBType(ty);


        }
        private void AssignDBType(DBKindEnum ty)
        {
            m_DBKind = ty;
            switch (ty)
            {
                case DBKindEnum.Oracle:
                    m_DB = new OracleHelper();
                    break;
                case DBKindEnum.MSSQL:
                    m_DB = new MSSqlHelper();
                    break;
                case DBKindEnum.WCF:
                    m_DB = new WCFHelper();
                    break;
                case DBKindEnum.ACCESS:
                    m_DB = new AccessHelper();
                    break;
            }
        }
        private DBKindEnum m_DBKind = DBKindEnum.Oracle;
        public DBKindEnum DBKind
        {
            get { return m_DBKind; }
            set { m_DBKind = value; }
        }
        public string OutRefCurString
        {
            get
            {
                return m_DB.OutRefCurString;
            }
            set
            {
                m_DB.OutRefCurString = value;
            }
        }
            

        public string DBSVR
        {
            get { return m_DB.DBSVR; }
        }
        

        public string DBUID
        {
            get { return m_DB.DBUID; }
        }
        

        public string DBPWD
        {
            get { return m_DB.DBPWD;}
        }
        

        public string DBNM
        {
            get { return m_DB.DBNM;}
        }
        

        public bool IsDBTrace
        {
            get { return m_DB.IsDBTrace; }
            set { m_DB.IsDBTrace = value; }
        }

        public bool IsAsynBusy
        {
            get { return m_DB.IsAsynBusy; }
        }

        public bool IsOciConnect
        {
            get { return m_DB.IsOciConnect; }
        }

        public DBOpenEnum DBOpenTY 
        {
            get
            {
                return m_DB.DBOpenTY;
            }
            set
            {
                m_DB.DBOpenTY = value;
            }
        }

        public event BackgroundRCV OnBackgroundRCV;
        public event BackgroundPR OnBackgroundPR;

        public bool AsynBusy(object key)
        {
            return m_DB.AsynBusy(key);
        }

        public void AsyncExecute(object sender, DBQueryTypeEnum qt, string query, Dictionary<string, string> param)
        {
            m_DB.AsyncExecute(sender, qt, query, param);
        }

        public int BulkInsert(string toTable, ref DataTable sendData, bool bAppend = true)
        {
            return m_DB.BulkInsert(toTable, ref sendData, bAppend);
        }

        public bool Close()
        {
            return m_DB.Close();
        }

        public int ExecuteNonQuery(string query, Dictionary<string, string> param = null)
        {
            return m_DB.ExecuteNonQuery(query, param);
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> param)
        {
            return m_DB.ExecuteNonQuery(query, param);
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, string> param = null)
        {
            return m_DB.ExecuteQuery(query, param);
        }

        public bool IsOpen()
        {
            return m_DB.IsOpen();
        }

        public bool Open(string path = "")
        {
            return m_DB.Open(path);
        }

        public bool Open(string svr, string uid, string pwd, string dbnm)
        {
            return m_DB.Open(svr, uid, pwd, dbnm);
        }
    }
}
