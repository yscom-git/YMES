using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YMES.FX.MainForm;

namespace YMES.DEPLOY
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.ParentForm is BaseMainForm)
            {
                ((BaseMainForm)this.ParentForm).FrmMsgBox(FX.MainForm.Base.Common.MsgTypeEnum.Alarm, "test", "test");
            }
        }
    }
}
