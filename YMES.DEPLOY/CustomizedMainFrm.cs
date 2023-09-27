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
            base.OnLoad(e);
            StartBC();
        }
        private void StartBC()
        {
            string pgmClassName = "";
            if (GetXMLConfig("DEBUG_CLIENT").Contains("@"))
            {
                pgmClassName = Application.ProductName + "." + GetXMLConfig("DEBUG_CLIENT").Substring(1);
            }
            ChildBC = (YMES.FX.MainForm.BaseContainer)Activator.CreateInstance(Type.GetType(pgmClassName));
        }
    }
}
