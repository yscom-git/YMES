
using System.Collections.Generic;

using System.Data;

namespace YMES.FX.DB.Base
{
    public enum DBOpenEnum
    {
        XML
            , Args
    }
    public enum DBQueryTypeEnum
    {
        Get
        , Set
    }
    public struct AsyncDBST
    {
        public string query;
        public Dictionary<string, string> param;
        public DataTable dt;
        public DBQueryTypeEnum qt;
        public object sender;
    }
    public enum DBKindEnum
    {
        Oracle
        , MSSQL
        , ACCESS
        ,WCF
    }
}
