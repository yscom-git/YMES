using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YMES.FX.DB;
using YMES.FX.MainForm.Base;

namespace YMES.FX.MainForm
{
    public partial class BaseMainForm : Form, IMainFormDesign
    {
        private YMES.FX.DB.CommonHelper m_DBHelper = null;
        private string m_XMLConfigFile = @".\Config.xml";
        private const string CN_CATEGORY = "_YMES";
        private Point m_MouseMovePoint = new Point();
        private bool m_bMoveMouse = false;

        public delegate void BeepBar(object sender, bool bBeep);
        public event BeepBar OnBeepBar = null;
        private bool m_AllowDuplicatedRun = false;
        private BaseContainer m_ChildBC = null;
        private DataTable m_XMLConfigDT = null;
        
        
        [Category(CN_CATEGORY)]
        public bool AllowDuplicatedRun
        {
            get { return m_AllowDuplicatedRun; }
            set { m_AllowDuplicatedRun = value; }
        }
        [Category(CN_CATEGORY)]
        public string XMLConfigFile
        {
            get { return m_XMLConfigFile; }
            set { m_XMLConfigFile = value; }
        }
        [Browsable(false)]
        public YMES.FX.DB.CommonHelper DBHelper
        {
            get { return m_DBHelper; }
        }
        [Browsable(false)]
        public BaseContainer ChildBC
        {
            get { return m_ChildBC; }
            set 
            { 
                m_ChildBC = value;
                ContainsBC(m_ChildBC);
            }
        }
        [Browsable(false)]
        public Label Plant_CTL
        {
            get { return lbl_Bizcd; }
            set { lbl_Bizcd = value; }
        }
        [Browsable(false)]
        public Label Line_CTL
        {
            get { return lbl_MainTitle; }
            set { lbl_MainTitle = value; }
        }
        [Browsable(false)]
        public Label Station_CTL
        {
            get { return lbl_SubTitle; }
            set { lbl_SubTitle = value; }
        }
        [Browsable(false)]
        public Button Result_CTL
        {
            get { return Btn_ProcResult; }
            set { Btn_ProcResult = value; }
        }
        [Browsable(false)]
        public Label Date_CTL
        {
            get { return lbl_Date; }
            set { lbl_Date = value; }
        }
        [Browsable(false)]
        public Label Time_CTL
        {
            get { return lbl_Time; }
            set { lbl_Time = value; }
        }
        [Browsable(false)]
        public Label MsgTitle_CTL
        {
            get { return lbl_MsgTitle; }
            set { lbl_MsgTitle = value; }
        }
        [Browsable(false)]
        public Label StatusMsg
        {
            get { return lbl_Msg; }
            set { lbl_Msg = value; }
        }

        [Browsable(false)]
        public Button WorkStandard_CTL
        {
            get { return btn_WorkStandard; }
            set { btn_WorkStandard = value; }
        }
        [Browsable(false)]
        public Button Exit_CTL
        {
            get { return btn_Exit; }
            set { btn_Exit = value; }
        }
        [Browsable(false)]
        public Button Config_CTL
        {
            get { return Btn_Config; }
            set { Btn_Config = value; }
        }
        [Browsable(false)]
        public PictureBox LogoImg
        {
            get
            {
                return pb_Logo;
            }
            set
            {

                pb_Logo = value;
                pb_Logo.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        [Browsable(false)]
        public Icon AppIcon
        {
            get
            {
                return this.Icon;
            }
            set
            {

                this.Icon = value;
            }
        }
        /// <summary>
        /// Get XML Value from XML File
        /// </summary>
        /// <param name="eleName">Element Name</param>
        /// <returns>XML Value</returns>
        public string GetXMLConfig(string eleName)
        {
            if (m_XMLConfigDT != null)
            {
                if (m_XMLConfigDT.Rows.Count > 0)
                {
                    if (m_XMLConfigDT.Columns.Contains(eleName))
                    {
                        return m_XMLConfigDT.Rows[0][eleName].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            return "";
        }

        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode == false)
            {
                OpenInitialConfig();
                if (ConnectDB() == false)
                {
                    StatusBarMsg(Common.MsgTypeEnum.Error, "DB Connection Error", System.Reflection.MethodBase.GetCurrentMethod().Name, true);
                }
                CheckDuplicatedRun(AllowDuplicatedRun);
                TmrTimeBase.Start();
            }

            base.OnLoad(e);

        }
        private bool ConnectDB()
        {
            try
            {
                string strDBType = GetXMLConfig("DB_CONNECTION").ToUpper();
                if (string.IsNullOrEmpty(strDBType))
                {
                    throw new Exception("Config file error:" + m_XMLConfigFile);
                }
                switch (strDBType)
                {
                    case "ORACLE":
                        m_DBHelper = new CommonHelper(DB.Base.DBKindEnum.Oracle);
                        break;
                    case "MSSQL":
                        m_DBHelper = new CommonHelper(DB.Base.DBKindEnum.MSSQL);
                        break;
                    case "WCF":
                        m_DBHelper = new CommonHelper(DB.Base.DBKindEnum.WCF);
                        break;
                    case "ACCESS":
                        m_DBHelper = new CommonHelper(DB.Base.DBKindEnum.ACCESS);
                        break;
                    default:
                        throw new Exception("DB Type Error");
                }
                m_DBHelper.XMLConfConnEle = MainFrmDesign.DBXmlElementName;
                return m_DBHelper.Open(m_XMLConfigFile);
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
            }
            return false;
        }
        protected virtual void OpenInitialConfig()
        {
            try
            {
                if (File.Exists(XMLConfigFile))
                {
                    m_XMLConfigDT = Base.Common.GetXml2DT(XMLConfigFile);
                }
                else
                {
                    m_XMLConfigDT = new DataTable();
                }
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
            }
        }
        private void CheckDuplicatedRun(bool allowDupRun)
        {
            if (allowDupRun == false)
            {
                bool isNew = true;
                System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out isNew);
                if (isNew == false)
                {    // Duplicated Run
                    FrmMsgBox(Common.MsgTypeEnum.Error, MainFrmDesign.DuplicatedRunMsg, MainFrmDesign.DuplicatedRunTitle, MsgBox.MsgBtnEnum.OK, true, true);
                    Application.Exit();
                    return;
                }
            }
        }
        private void ContainsBC(BaseContainer bc)
        {
            if (bc != null)
            {
                bc.Dock = DockStyle.Fill;
                this.Pan_Body.Controls.Clear();
                if (this.Pan_Body.Controls.Contains(bc) == false)
                {
                    this.Pan_Body.Controls.Add(bc);

                }
            }

        }
        public BaseMainForm()
        {

            InitializeComponent();
        }

        protected virtual void OnCloseBtn(object sender, EventArgs e)
        {
            if (FrmMsgBox(Common.MsgTypeEnum.Warnning, MainFrmDesign.Exit_Dlg_Contents, MainFrmDesign.Exit_Dlg_Title, MsgBox.MsgBtnEnum.YesNo) == DialogResult.Yes)
            {
                Close();
            }
        }
        protected virtual void OnMaxWindow(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.btn_WinMax.BackColor = SystemColors.Control;
            }
            else
            {


                this.WindowState = FormWindowState.Normal;
                this.btn_WinMax.BackColor = Color.Salmon;
                if (this.Width > Screen.AllScreens[0].Bounds.Width)
                {
                    this.Width = Screen.AllScreens[0].Bounds.Width;
                }
            }
        }
        protected virtual void OnMinWindow(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        protected virtual void OnWorkStandard(object sender, EventArgs e)
        {

        }
        protected virtual void OnResult(object sender, EventArgs e)
        {

        }
        protected virtual void OnConfig(object sender, EventArgs e)
        {

        }
        private void lbl_MainTitle_DoubleClick(object sender, EventArgs e)
        {
            OnMaxWindow(sender, e);
        }


        private void lbl_MainTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                m_MouseMovePoint = new Point(e.X, e.Y);
                m_bMoveMouse = true;
            }
        }

        private void lbl_MainTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_bMoveMouse)
            {
                Point pt = new Point(this.Left + e.X - m_MouseMovePoint.X, this.Top + e.Y - m_MouseMovePoint.Y);
                this.Location = pt;
            }
        }

        private void lbl_MainTitle_MouseUp(object sender, MouseEventArgs e)
        {
            m_bMoveMouse = false;
        }
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
            this.Invoke(new MethodInvoker(
            delegate ()
            {
                MainFrmDesign.StatusMsgTitle(msgType);

                if (msgType != Common.MsgTypeEnum.Trace)
                {
                    lbl_Msg.Text = msg;
                    if (beep)
                    {
                        SetBeep(beep);
                    }
                }

            }));
        }

        public DialogResult FrmMsgBox(Common.MsgTypeEnum msgType, string contents, string title, MsgBox.MsgBtnEnum btnty = MsgBox.MsgBtnEnum.OK, bool beep = false, bool bModal = false)
        {
            DialogResult rslt = DialogResult.OK;
            try
            {
                string msgName = "BaseMainForm_MSG";
                Form frm = Common.GetForm(msgName);
                if (frm != null)
                {
                    frm.Close();
                }


                MsgBox msg = new MsgBox(MainFrmDesign.MsgTypeText);

                msg.Name = "BaseMainForm_MSG";
                bool modal = bModal;
                if (btnty == MsgBox.MsgBtnEnum.YesNo)
                {
                    modal = true;
                }

                rslt = msg.ShowDlg(msgType, contents, title, btnty, modal);

                if (msgType != Common.MsgTypeEnum.Trace)
                {
                    if (beep)
                    {
                        SetBeep(beep);
                    }
                }
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
            }
            return rslt;
        }
        /// <summary>
        /// Use the Alram Horn
        /// </summary>
        /// <param name="bBeep">Run or Stop</param>
        public void SetBeep(bool bBeep = true)
        {
            if (OnBeepBar != null)
            {
                if (this.InvokeRequired == true)
                {
                    this.Invoke(new MethodInvoker(
                    delegate ()
                    {
                        OnBeepBar(this, bBeep);
                    }));
                }
                else
                {
                    OnBeepBar(this, bBeep);
                }
            }
        }

        private void lbl_WorkStatus_DoubleClick(object sender, EventArgs e)
        {
            if (Btn_Config.Visible == false)
            {
                Btn_Config.Visible = true;
            }
            else
            {
                Btn_Config.Visible = false;
            }
        }
        public void AddPGM(UserControl uc)
        {
            this.Pan_Body.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        private void TmrTimeBase_Tick(object sender, EventArgs e)
        {
            MainFrmDesign.SyncTIT_Date(DateTime.Now);
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
