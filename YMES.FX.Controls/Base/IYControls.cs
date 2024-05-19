using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.Controls.Base
{
    public interface IYControls
    {
        string Key { get; set; }
        void SetValue(object val);
        object GetValue();
        void ClearValue();
        void RefreshCtl();
    }
}
