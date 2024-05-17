using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YMES.FX.MainForm.Base;

namespace YMES.FX.MainForm
{
    public partial class BaseContainer : UserControl
    {
        public delegate void BaseFormLoad(object sender, EventArgs e);
        public event BaseFormLoad OnAfterBaseFormLoad = null;

        public virtual void AfterBaseFormLoad(EventArgs e)
        {
            if(OnAfterBaseFormLoad!= null)
            {
                OnAfterBaseFormLoad(this, e);
            }
        }
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
        protected FX.MainForm.BaseMainForm PBaseFrm
        {
            get
            {
                if (this.ParentForm != null)
                {
                    if (ParentForm is BaseMainForm)
                    {
                        return (FX.MainForm.BaseMainForm)ParentForm;
                    }
                }
                return null;

            }
        }
        public BaseContainer()
        {
            InitializeComponent();
        }
        #region <<BaseFrom Control
        /// <summary>
        /// Write Message in StatusBar
        /// </summary>
        /// <param name="msgType">Type of Message</param>
        /// <param name="msg">Message</param>
        /// <param name="callMethodName">Called Method Name</param>
        /// <param name="beep">use the Alram Horn</param>
        /// <param name="logWrite">Trace the Log data</param>
        public void StatusBarMsg(Common.MsgTypeEnum msgType, string msg, string callMethodName = "", bool beep = false, bool logWrite = true)
        {
            PBaseFrm.StatusBarMsg(msgType, msg, callMethodName, beep, logWrite);
        }
        #endregion
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
