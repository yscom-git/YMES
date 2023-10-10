
using System.Collections.Generic;
using System.Data;

namespace YMES.FX.DB.Base
{
    
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
        bool Open(string svr, string uid, string pwd, string dbnm, string port);
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
        DataTable ExecuteQuery(string query, Dictionary<string, string> param=null);
        /// <summary>
        /// DML Query
        /// </summary>
        /// <param name="query">Query or (PKG)SP</param>
        /// <param name="param">Parameters</param>
        /// <returns>Affected Count</returns>
        int ExecuteNonQuery(string query, Dictionary<string, string> param=null);
        /// <summary>
        /// Bulk Insert Data
        /// </summary>
        /// <param name="toTable">Object Table Name</param>
        /// <param name="sendData">Insert DataTable</param>
        /// <param name="bAppend">Append</param>
        /// <returns>Affected Count</returns>
        int ExecuteNonQuery(string query, Dictionary<string, object> param);
        int BulkInsert(string toTable, ref DataTable sendData, bool bAppend = true);
        bool AsynBusy(object key);

        void AsyncExecute(object sender, DBQueryTypeEnum qt, string query, Dictionary<string, string> param);
       
        event BackgroundRCV OnBackgroundRCV;

        event BackgroundPR OnBackgroundPR;
    }
}