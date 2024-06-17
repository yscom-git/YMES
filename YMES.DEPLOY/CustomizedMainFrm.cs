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
    public partial class CustomizedMainFrm : BaseMainForm
    {
        public CustomizedMainFrm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            RunBC = "";   //Call Start the Logic. "ex) MES.TagPrint"
            base.OnLoad(e);            
        }

    }
}
