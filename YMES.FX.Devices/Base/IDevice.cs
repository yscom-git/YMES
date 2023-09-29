using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.Devices.Base
{
    public interface IDevice
    {
        bool OpenDevice(Dictionary<string, object> param);
        bool IsConnected();
        bool CloseDevice();
    }
}
