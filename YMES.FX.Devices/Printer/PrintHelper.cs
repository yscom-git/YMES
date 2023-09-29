using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.Devices.Printer
{
    public partial class PrintHelper : Component, Base.IDevice
    {
        public enum PrintLanguageEnum
        {
            NONE,
            ZPL,
            IPL

        }
        public enum ConnectionTypeEnum
        {
            NONE,
            TCP,
            USB,
            SERIAL
        }
        private Base.IDevice m_prtDevice;
        public PrintHelper()
        {
            InitializeComponent();
        }

        public PrintHelper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public bool CloseDevice()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public bool OpenDevice(Dictionary<string, object> param)
        {
            ConnectionTypeEnum type = ConnectionTypeEnum.NONE;
            if(param.ContainsKey("CONNECT_TYPE"))
            {
                type = (ConnectionTypeEnum)param["CONNECT_TYPE"];
            }
            else
            {
                throw new Exception("Device Connect Type Error:CONNECT_TYPE");
            }
            PrintLanguageEnum lang = PrintLanguageEnum.NONE;
            if(param.ContainsKey("PRT_LANG"))
            {
                lang = (PrintLanguageEnum)param["PRT_LANG"];
            }
            else
            {
                throw new Exception("Printer Language Error:PRT_LANG");
            }

            switch(type)
            {
                case ConnectionTypeEnum.USB:
                    m_prtDevice = new ZebraUSB();
                    break;
            }

            return true;
        }
    }
}
