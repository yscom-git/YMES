
using System.Collections.Generic;

using System.Data;
using System.ComponentModel;

namespace YMES.FX.DB.Base
{
    public delegate void BackgroundRCV(object sender, string query, Dictionary<string, string> param, DataTable dt);
    public delegate void BackgroundPR(object sender, ProgressChangedEventArgs e);
    /// <summary>
    /// DataBase I/F
    /// </summary>
    public interface IDBBase
    {

        /// <summary>
        /// DB Open
        /// </summary>
        /// <param name="path">XML Path</param>
        /// <returns>Is Open</returns>
        bool Open(string path = "");
        bool Open(string svr, string uid, string pwd, string dbnm);
        /// <summary>
        /// DB Close
        /// </summary>
        /// <returns></returns>
        bool Close();
        /// <summary>
        /// Is DB Open
        /// </summary>
        /// <returns>Open Status</returns>
        bool IsOpen();
        /// <summary>
        /// Data Return Query
        /// </summary>
        /// <param name="query">Query or (PKG)SP</param>
        /// <param name="param">Parameters</param>
        /// <returns>Data(DataTable)</returns>
        DataTable ExcuteQuery(string query, Dictionary<string, string> param=null);
        /// <summary>
        /// DML Query
        /// </summary>
        /// <param name="query">Query or (PKG)SP</param>
        /// <param name="param">Parameters</param>
        /// <returns>Affected Count</returns>
        int ExcuteNonQuery(string query, Dictionary<string, string> param=null);
        /// <summary>
        /// Bulk Insert Data
        /// </summary>
        /// <param name="toTable">Object Table Name</param>
        /// <param name="sendData">Insert DataTable</param>
        /// <param name="bAppend">Append</param>
        /// <returns>Affected Count</returns>
        int ExcuteNonQuery(string query, Dictionary<string, object> param);
        int BulkInsert(string toTable, ref DataTable sendData, bool bAppend = true);
        bool AsynBusy(object key);

        void AsyncExcute(object sender, DBQueryTypeEnum qt, string query, Dictionary<string, string> param);
       
        string XMLConfigPath { get; set; }
        string OutRefCurString { get; set; }
        string DBSVR { get; }
        string DBUID { get; }
        string DBPWD { get; }
        string DBNM { get; }
        bool IsDBTrace { get; set; }
        bool IsAsynBusy { get; }
        bool IsOciConnect { get; }
        DBOpenEnum DBOpenTY { get; set; }



        event BackgroundRCV OnBackgroundRCV;

        event BackgroundPR OnBackgroundPR;
    }
}