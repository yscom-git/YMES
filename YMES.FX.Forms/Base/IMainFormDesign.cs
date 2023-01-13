using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YMES.FX.MainForm.Base
{
    public interface IMainFormDesign
    {
        Label Plant_CTL { get; set; }
        Label Line_CTL { get; set; }
        Label Station_CTL { get; set; }
        Label Date_CTL { get; set; }
        Label Time_CTL { get; set; }
        Label MsgTitle_CTL { get; set; }
        Button Result_CTL { get; set; }
        Button WorkStandard_CTL { get; set; }
        Button Exit_CTL { get; set; }
        Button Config_CTL { get; set; }

        PictureBox LogoImg { get; set; }
        
    }
}
