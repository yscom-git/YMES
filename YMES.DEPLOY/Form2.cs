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
    public partial class Form2 : YMES.FX.MainForm.BaseMainForm
    {
        public Form2()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UserControl1 ctl = new UserControl1();
            this.AddPGM(ctl);
            //TEST
            //TEST2
        }
    }
}
