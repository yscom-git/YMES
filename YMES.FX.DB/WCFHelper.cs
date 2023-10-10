using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMES.FX.DB.Base;

namespace YMES.FX.DB
{
    [ToolboxItem(true)]
    public class WCFHelper : DBComponent, IDBBase
    {
        private BackgroundWorker m_backWorker = null;
        public event BackgroundRCV OnBackgroundRCV = null;

        public event BackgroundPR OnBackgroundPR = null;
        private WCF.Client m_WCF = null;
        private int m_resTime = 2000;
        private DBOpenEnum m_DBOpenTY = DBOpenEnum.XML;
        private bool m_bOpen = false;
        public int ResponseTime
        {
            get { return m_resTime; }
            set { m_resTime = value; }
        }
        public bool Open(string path = "")
        {
            try
            {

                if (string.IsNullOrEmpty(path))
                {
                    path = XMLConfigPath;
                }
                if (File.Exists(path))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(path);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string uri = ds.Tables[0].Columns.Contains(GetXMLName(XMLConfNameEnum.dbServer)) ? ds.Tables[0].Rows[0][GetXMLName(XMLConfNameEnum.dbServer)].ToString() : "";
                            string user = ds.Tables[0].Columns.Contains(GetXMLName(XMLConfNameEnum.dbID)) ? ds.Tables[0].Rows[0][GetXMLName(XMLConfNameEnum.dbID)].ToString() : "";

                            return OpenClient(uri, user);
                        }
                    }
                }
                return false;
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);

                return false;

            }
        }
        private bool OpenClient(string uri, string user)
        {
            try
            {
                m_DBConnInfor.SVR = uri;
                m_DBConnInfor.ID = user;
                string[] parURL = uri.Trim().ToLower().Replace("http://", "").Replace("https://", "").Split(':');
                if (parURL.Length > 0)
                {
                    m_DBConnInfor.SID = parURL[0];
                    m_DBConnInfor.PORT = parURL[1].Trim().Split('/')[0];
                    m_WCF = new WCF.Client(uri, m_DBConnInfor.SVR, m_DBConnInfor.PORT, m_resTime);
                }
                else
                {
                    throw new Exception("URL Error:" + uri);
                }
                m_bOpen = true;
                return true;
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
                return false;
            }
        }
        public int ExecuteNonQuery(string query, Dictionary<string, object> param)
        {
            return -1;
        }
        public bool Open(string svr, string uid, string pwd, string dbnm, string port)
        {
            try
            {
                return OpenClient(svr, uid);

            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);

                return false;

            }
        }
        public bool Open(string svr, string uid, string pwd, string dbnm)
        {
            return Open(svr, uid, pwd, dbnm, m_DBConnInfor.PORT);
        }

        public bool Close()
        {
            return true;
        }

        public bool IsOpen()
        {
            return m_WCF.IsConnected();
        }

        public System.Data.DataTable ExecuteQuery(string query, Dictionary<string, string> param = null)
        {
            try
            {
                if (m_bOpen == false)
                {
                    Open();
                }

                return m_WCF.ExecuteQuery(query, param);
            }
            catch (Exception eLog)
            {
                Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
                return null;
            }
        }

        public int ExecuteNonQuery(string query, Dictionary<string, string> param = null)
        {
            try
            {
                if (m_bOpen == false)
                {
                    Open();
                }

                return m_WCF.ExecuteNonQuery(query, param);
            }
            catch (Exception eLog)
            {
                Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
                return -1;
            }
        }

        public int BulkInsert(string toTable, ref System.Data.DataTable sendData, bool bAppend = true)
        {
            int nRet = -1;
            try
            {

                return nRet;

            }
            catch (Exception eLog)
            {

                Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
            }

            return nRet;
        }

        Dictionary<object, BackgroundWorker> m_dicBackWK = new Dictionary<object, BackgroundWorker>();
        public void AsyncExecute(DBQueryTypeEnum qt, string query, Dictionary<string, string> param)
        {
            AsyncExecute(null, qt, query, param);
        }
        public void AsyncExecute(object sender, DBQueryTypeEnum qt, string query, Dictionary<string, string> param)
        {
            try
            {

                AsyncDBST rst = new AsyncDBST();
                rst.query = "";
                rst.param = new Dictionary<string, string>();
                rst.dt = new DataTable();
                if (sender == null)
                {
                    if (m_backWorker == null)
                    {
                        m_backWorker = new BackgroundWorker();
                        m_backWorker.DoWork += new DoWorkEventHandler(ExecuteBackground_DoWork);
                        m_backWorker.ProgressChanged += new ProgressChangedEventHandler(ExecuteBackground_ProgressChanged);
                        m_backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteBackground_RunWorkerCompleted);
                        m_backWorker.WorkerReportsProgress = true;
                        m_backWorker.WorkerSupportsCancellation = true;
                    }

                    if (!m_backWorker.IsBusy)
                    {
                        rst.param = param;
                        rst.query = query;
                        rst.qt = qt;
                        rst.sender = sender;
                        m_backWorker.RunWorkerAsync(rst);
                        m_IsAsynBusy = false;
                    }
                    else
                    {
                        m_IsAsynBusy = true;
                    }
                }
                else
                {

                    if (m_dicBackWK.ContainsKey(sender) == false)
                    {
                        BackgroundWorker backWorker = new BackgroundWorker();
                        backWorker = new BackgroundWorker();
                        backWorker.DoWork += new DoWorkEventHandler(ExecuteBackground_DoWork);
                        backWorker.ProgressChanged += new ProgressChangedEventHandler(ExecuteBackground_ProgressChanged);
                        backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteBackground_RunWorkerCompleted);
                        backWorker.WorkerReportsProgress = true;
                        backWorker.WorkerSupportsCancellation = true;
                        m_dicBackWK.Add(sender, backWorker);
                    }

                    if (m_dicBackWK.ContainsKey(sender))
                    {

                        if (m_dicBackWK[sender].IsBusy == false)
                        {
                            rst.param = param;
                            rst.query = query;
                            rst.qt = qt;
                            rst.sender = sender;
                            m_dicBackWK[sender].RunWorkerAsync(rst);
                            m_IsAsynBusy = false;

                        }
                        else
                        {
                            m_IsAsynBusy = true;
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
            }

        }
        private void ExecuteBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                AsyncDBST rst;
                if (e.Error != null)
                {

                }
                else if (e.Cancelled)
                {//취소함
                    //OnBackgroundRCV(rst.query, rst.param, rst.dt);

                }
                else
                {
                    rst = (AsyncDBST)e.Result;
                    if (OnBackgroundRCV != null)
                    {
                        OnBackgroundRCV(rst.sender, rst.query, rst.param, rst.dt);
                    }
                }

            }
            catch (Exception eLog)
            {
                Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
            }
            finally
            {
                m_IsAsynBusy = false;
            }

        }

        private void ExecuteBackground_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (OnBackgroundPR != null)
                {
                    OnBackgroundPR(sender, e);

                }
            }
            catch (Exception eLog)
            {
                Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
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
                Debug.WriteLine("[Query:" + rst.query + "]" + "[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
            }
            finally
            {

            }

        }

       
      
        public bool AsynBusy(object key)
        {


            return false;
        }
    }
}
