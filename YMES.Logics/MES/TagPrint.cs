using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YMES.Logics.Base;

namespace YMES.Logics.MES
{
    public partial class TagPrint : Base.LocalizedContainer
    {
        public TagPrint()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DataTable dt = ExcuteQuery("SELECT sysdate DAT FROM DUAL", null) ;
            if(dt.Rows.Count>0)
            {        

            }
        }
    }
}
