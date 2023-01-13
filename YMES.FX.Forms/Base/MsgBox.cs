using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YMES.FX.MainForm.Base
{
    public partial class MsgBox : Form
    {
        public delegate void Confirm(object sender, DialogResult result);
        public event Confirm OnConfirm = null;


        private bool m_MiniSize = false;
        private Point m_MouseMovePoint = new Point();
        private bool m_bMoveMouse = false;

        private MsgType m_MsgTypeText = new MsgType("WARNING", "NOTICE", "ERROR", "TRACE");
        [System.ComponentModel.Category("_YMES.Appearance")]
        public MsgType MsgTypeText
        {
            get { return m_MsgTypeText; }
            set { m_MsgTypeText = value; }
        }

        public enum MsgBtnEnum
        {
            OK,
            YesNo
        }
        public MsgBox()
        {
            InitializeComponent();
        }
        public MsgBox(MsgType cfgMsg)
        {
            InitializeComponent();
           
            if(DesignMode == false)
            {
                MsgTypeText = cfgMsg;
                
            }
        }
        public MsgBox(string title, string message)
        {
            InitializeComponent();
            this.Lbl_Title.Text = title;
            this.Lbl_Msg.Text = message;
        }
        public DialogResult ShowDlg(Common.MsgTypeEnum msgty, string msg, string title, MsgBtnEnum btnty = MsgBtnEnum.OK, bool modal = false)
        {

            if (m_MiniSize)
            {
                this.Size = new Size(490, 174);
                label1.Visible = true;
                label1.BringToFront();
            }
            else
            {
                this.Size = new Size(619, 174);
                label1.Visible = false;
            }
            switch (msgty)
            {
                case Common.MsgTypeEnum.Alarm:
                    Lbl_Icon.BackColor = Color.Blue;
                    Lbl_Icon.ForeColor = Color.White;
                    Lbl_Icon.Text = this.MsgTypeText.MSG_ALARM;
                    break;
                case Common.MsgTypeEnum.Error:
                    Lbl_Icon.BackColor = Color.Red;
                    Lbl_Icon.ForeColor = Color.Yellow;
                    Lbl_Icon.Text = this.MsgTypeText.MSG_ERROR;
                    break;
                case Common.MsgTypeEnum.Warnning:
                    Lbl_Icon.BackColor = Color.Yellow;
                    Lbl_Icon.ForeColor = Color.Black;
                    Lbl_Icon.Text = this.MsgTypeText.MSG_WARN;
                    break;
                case Common.MsgTypeEnum.Trace:
                    Lbl_Icon.Text = this.MsgTypeText.MSG_TRACE;
                    break;
            }
            Btn_OK.Visible = false;
            Btn_Yes.Visible = false;
            Btn_No.Visible = false;

            switch (btnty)
            {
                case MsgBtnEnum.OK:
                    Btn_OK.Visible = true;
                    break;
                case MsgBtnEnum.YesNo:
                    Btn_Yes.Visible = true;
                    Btn_No.Visible = true;
                    break;
            }
            Lbl_Msg.Text = msg;
            Lbl_Title.Text = title;
            if (modal)
            {
                this.TopMost = true;
                this.BringToFront();
                if (this.ParentForm != null)
                {
                    this.StartPosition = FormStartPosition.Manual;
                    this.Location = new Point(this.ParentForm.Location.X, this.ParentForm.Height / 2);
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                return this.ShowDialog(this.ParentForm != null ? this.ParentForm : this.Parent);

            }
            else
            {
                this.TopMost = true;
                this.BringToFront();
                if (this.Parent != null)
                {
                    this.Show(Parent);
                }
                else
                {
                    this.Show();
                }
                this.Location = new Point(400, 380);
                return DialogResult.OK;
            }
        }

        private void Lbl_Title_MouseDown(object sender, MouseEventArgs e)
        {
            m_MouseMovePoint = new Point(e.X, e.Y);
            m_bMoveMouse = true;
        }

        private void Lbl_Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_bMoveMouse)
            {
                Point pt = new Point(this.Left + e.X - m_MouseMovePoint.X, this.Top + e.Y - m_MouseMovePoint.Y);
                this.Location = pt;
            }
        }
        private void Lbl_Title_MouseUp(object sender, MouseEventArgs e)
        {
            m_bMoveMouse = false;
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            Close();
            if (OnConfirm != null)
            {
                OnConfirm(this, this.DialogResult);
            }
        }

        private void Btn_Yes_Click(object sender, EventArgs e)
        {
            Close();
            if (OnConfirm != null)
            {
                OnConfirm(this, this.DialogResult);
            }
        }

        private void Btn_No_Click(object sender, EventArgs e)
        {
            Close();
            if (OnConfirm != null)
            {
                OnConfirm(this, this.DialogResult);
            }
        }

    }
}
