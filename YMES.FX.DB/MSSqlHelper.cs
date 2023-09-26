using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using YMES.FX.DB.Base;
using System.IO;

namespace YMES.FX.DB
{
    [ToolboxItem(true)]
    public class MSSqlHelper : DBComponent, IDBBase
    {
        public event BackgroundRCV OnBackgroundRCV = null;

        public event BackgroundPR OnBackgroundPR = null;
        private string m_OutRefCurString = "OUT_CURSOR";
        private string m_DBSVR = "";
        private string m_DBUID = "";
        private string m_DBPWD = "";
        private string m_DBNM = "";
        private DBOpenEnum m_DBOpenTY = DBOpenEnum.XML;
        private bool m_IsAsynBusy = false;

        private bool m_IsDBTrace = false;

        public bool IsDBTrace
        {
            get { return m_IsDBTrace; }
            set { m_IsDBTrace = value; }
        }

        public DBOpenEnum DBOpenTY
        {
            get
            {
                return m_DBOpenTY;
            }
            set
            {
                m_DBOpenTY = value;
            }
        }
        public int ExecuteNonQuery(string query, Dictionary<string, object> param)
        {
            return -1;
        }
        public bool IsAsynBusy
        {
            get { return m_IsAsynBusy; }
        }

        public string DBSVR
        {
            get { return m_DBSVR; }
        }
        public string DBUID
        {
            get { return m_DBUID; }
        }
        public string DBPWD
        {
            get { return m_DBPWD; }
        }
        public string DBNM
        {
            get { return m_DBNM; }
        }


        public bool Close()
        {
            try
            {
                m_Conn.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public string OutRefCurString
        {
            get { return m_OutRefCurString; }
            set { m_OutRefCurString = value; }
        }
        private System.Data.SqlClient.SqlConnection m_Conn = null;
        public bool Open(string svr, string uid, string pwd, string dbnm)
        {
            try
            {

                m_DBSVR = svr;
                m_DBUID = uid;
                m_DBPWD = pwd;
                m_DBNM = dbnm;

                m_Conn = new SqlConnection(string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", m_DBSVR, m_DBNM, m_DBUID, m_DBPWD));
                m_Conn.Open();

                return true;

            }
            catch
            {
                return false;

            }
        }
        public bool Open(string path = "")
        {
            try
            {

                if (string.IsNullOrEmpty(path))
                {
                    path = XMLConfigPath;
                    if (DBOpenTY == DBOpenEnum.Args)
                    {
                        return Open(m_DBSVR, m_DBUID, m_DBPWD, m_DBNM);
                    }
                }
                if (File.Exists(path))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(path);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            m_DBSVR = ds.Tables[0].Columns.Contains("DBNAME") ? ds.Tables[0].Rows[0]["DBNAME"].ToString() : "";
                            m_DBUID = ds.Tables[0].Columns.Contains("DBUID") ? ds.Tables[0].Rows[0]["DBUID"].ToString() : "";
                            m_DBPWD = ds.Tables[0].Columns.Contains("DBPWD") ? ds.Tables[0].Rows[0]["DBPWD"].ToString() : "";
                            m_DBNM = ds.Tables[0].Columns.Contains("DBSERVICE") ? ds.Tables[0].Rows[0]["DBSERVICE"].ToString() : "";

                            m_Conn = new SqlConnection(string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", m_DBSVR, m_DBNM, m_DBUID, m_DBPWD));
                            m_Conn.Open();

                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;

            }
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, string> param = null)
        {

            try
            {
                if (m_Conn == null || m_Conn.State != ConnectionState.Open)
                {
                    if (!Open())
                    {
                        throw new Exception("DB is closed.");
                    }
                }
                DateTime now = DateTime.Now;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = m_Conn;
                if (param!=null)
                {
                    param = GetReservedParam(param);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, string> pair in param)
                    {
                        SqlParameter sqlParam = new SqlParameter(pair.Key, pair.Value);
                        cmd.Parameters.Add(sqlParam);
                    }
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                TimeSpan span = DateTime.Now - now;
                if (IsDBTrace)
                {
                    int cnt = 0;
                    if (dt != null)
                    {
                        cnt = dt.Rows.Count;
                    }
                    Util.WriteTxtLog(query + ":Delay[" + span.TotalMilliseconds.ToString() + "]" + ":Count[" + cnt.ToString() + "]");
                }
                return dt;

            }
            catch (Exception eLog)
            {
                throw new Exception(eLog.Message);
            }
        }

        public int ExecuteNonQuery(string query, Dictionary<string, string> param = null)
        {
            SqlTransaction tran = null;
            try
            {
                DateTime now = DateTime.Now;
                if (m_Conn == null || m_Conn.State != ConnectionState.Open)
                {
                    if (!Open())
                    {
                        throw new Exception("DB is closed.");
                    }
                }
                tran = m_Conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = m_Conn;
                cmd.Transaction = tran;
                if (param != null)
                {
                    param = GetReservedParam(param);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, string> pair in param)
                    {
                        SqlParameter sqlParam = new SqlParameter(pair.Key, pair.Value);
                        cmd.Parameters.Add(sqlParam);
                    }
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }
                int nRet = cmd.ExecuteNonQuery();

                tran.Commit();

                TimeSpan span = DateTime.Now - now;
                if (IsDBTrace)
                {
                    Util.WriteTxtLog(query + ":Delay[" + span.TotalMilliseconds.ToString() + "]" + ":Count[" + nRet.ToString() + "]");
                }
                return nRet;

            }
            catch (Exception eLog)
            {
                tran.Rollback();
                throw new Exception(eLog.Message);
            }
        }


        public bool IsOpen()
        {
            if (m_Conn.State == ConnectionState.Closed)
            {
                return false;
            }
            return true;
        }

        public int BulkInsert(string toTable, ref DataTable sendData, bool bAppend = true)
        {
            SqlTransaction tran = null;
            int nRet = 0;
            try
            {
                if (m_Conn == null || m_Conn.State != ConnectionState.Open)
                {
                    if (!Open())
                    {
                        throw new Exception("DB is closed.");
                    }
                }
                tran = m_Conn.BeginTransaction();
                SqlBulkCopy bulk = new SqlBulkCopy(m_Conn, SqlBulkCopyOptions.Default, tran);
                bulk.DestinationTableName = toTable;

                bulk.WriteToServer(sendData);
                nRet = sendData.Rows.Count;
                tran.Commit();
            }
            catch (Exception eLog)
            {
                tran.Rollback();
                nRet = -1;
                throw new Exception(eLog.Message);

            }
            return nRet;
        }


        public void AsyncExecute(object sender, DBQueryTypeEnum qt, string query, Dictionary<string, string> param)
        {

            AsyncDBST rst = new AsyncDBST();
            rst.query = "";
            rst.param = new Dictionary<string, string>();
            rst.dt = new DataTable();

            BackgroundWorker backWorker = new BackgroundWorker();
            backWorker.DoWork += new DoWorkEventHandler(ExecuteBackground_DoWork);
            backWorker.ProgressChanged += new ProgressChangedEventHandler(ExecuteBackground_ProgressChanged);
            backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteBackground_RunWorkerCompleted);
            backWorker.WorkerReportsProgress = true;
            backWorker.WorkerSupportsCancellation = true;

            if (!backWorker.IsBusy)
            {
                rst.param = param;
                rst.query = query;
                rst.qt = qt;
                m_IsAsynBusy = false;
                backWorker.RunWorkerAsync(rst);
            }
            else
            {
                m_IsAsynBusy = true;
            }

        }
        private void ExecuteBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AsyncDBST rst = (AsyncDBST)e.Result;
            if (e.Error != null)
            {

            }
            else if (e.Cancelled)
            {//취소함
                if (OnBackgroundRCV != null)
                {
                    OnBackgroundRCV(rst.sender, rst.query, rst.param, null);
                }
            }
            else
            {

                if (OnBackgroundRCV != null)
                {
                    OnBackgroundRCV(rst.sender, rst.query, rst.param, rst.dt);
                }
            }
        }

        private void ExecuteBackground_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (OnBackgroundPR != null)
            {
                OnBackgroundPR(sender, e);

            }
        }

        private void ExecuteBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            AsyncDBST rst = new AsyncDBST();
            rst = (AsyncDBST)e.Argument;
            try
            {


                if (!string.IsNullOrEmpty(rst.query))
                {
                    if (rst.qt == DBQueryTypeEnum.Get)
                    {
                        rst.dt = ExecuteQuery(rst.query, rst.param);
                    }
                    else if (rst.qt == DBQueryTypeEnum.Set)
                    {
                        ExecuteNonQuery(rst.query, rst.param);
                    }
                }
                e.Result = rst;
            }
            catch (Exception eLog)
            {
                throw new Exception("[Query:" + rst.query + "]" + "[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
            }

        }
        public bool IsOciConnect
        {
            get { return false; }
        }
        bool IDBBase.AsynBusy(object key)
        {
            return false;
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
