using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YMES.FX.Controls
{
    [ToolboxBitmap(typeof(TableLayoutPanel))]
    public partial class YTableLayout : TableLayoutPanel
    {
        public YTableLayout()
        {
            this.DoubleBuffered = true;
            
            InitializeComponent();
        }
    }
}
