using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YMES.FX.MainForm
{
    public partial class BaseContainer : UserControl
    {
        protected FX.DB.CommonHelper DBHelper
        {
            get
            {
                if(this.ParentForm !=null)
                {
                    if(ParentForm is BaseMainForm)
                    {
                        return ((BaseMainForm)ParentForm).DBHelper;
                    }
                }
                return null;
            }
        }
        public BaseContainer()
        {
            InitializeComponent();
        }

        #region <<DBHelper
        public DataTable ExcuteQuery(string query, Dictionary<string, string> param)
        {
            return DBHelper.ExecuteQuery(query, param);
        }
        public int ExecuteNonQuery(string query, Dictionary<string, string> param)
        {
            return DBHelper.ExecuteNonQuery(query, param);
        }
        public int ExecuteNonQuery(string query, Dictionary<string, object> param)
        {
            return DBHelper.ExecuteNonQuery(query, param);
        }
        #endregion
    }
}
