using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.MainForm.Base
{
    [TypeConverter(typeof(MsgTypeConverter))]
    public class MsgType
    {
        public MsgType(string warn, string alarm, string error, string trace)
        {
            this.m_MSG_WARN = warn;
            this.m_MSG_ALARM = alarm;
            this.m_MSG_ERROR = error;
            this.m_MSG_TRACE = trace;
        }
        public MsgType()
        {
            this.m_MSG_WARN = "";
            this.m_MSG_ALARM = "";
            this.m_MSG_ERROR = "";
            this.m_MSG_TRACE = "";

        }
        private string m_MSG_WARN;
        private string m_MSG_ALARM;
        private string m_MSG_ERROR;
        private string m_MSG_TRACE;


        public string MSG_WARN
        {
            get
            {
                return m_MSG_WARN;
            }
            set
            {
                m_MSG_WARN = value;
            }
        }
        public string MSG_ALARM
        {
            get
            {
                return m_MSG_ALARM;
            }
            set
            {
                m_MSG_ALARM = value;
            }
        }
        public string MSG_ERROR
        {
            get
            {
                return m_MSG_ERROR;
            }
            set
            {
                m_MSG_ERROR = value;
            }
        }
        public string MSG_TRACE
        {
            get
            {
                return m_MSG_TRACE;
            }
            set
            {
                m_MSG_TRACE = value;
            }
        }


    }

    public class MsgTypeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MsgType))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is MsgType)
            {
                MsgType m = (MsgType)value;
                return "" + m.MSG_WARN +
                    "," + m.MSG_ALARM +
                    "," + m.MSG_ERROR +
                    "," + m.MSG_TRACE;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    MsgType m = new MsgType();
                    string[] sARR = ((string)value).Split(',');
                    for (int i = 0; i < sARR.Length; i++)
                    {
                        if (i == 0)
                        {
                            m.MSG_WARN = sARR[i];
                        }
                        else if (i == 1)
                        {
                            m.MSG_ALARM = sARR[i];
                        }
                        else if (i == 2)
                        {
                            m.MSG_ERROR = sARR[i];
                        }
                        else if (i == 3)
                        {
                            m.MSG_TRACE = sARR[i];
                        }

                    }


                    return m;
                }
                catch
                {
                    throw new ArgumentException("Can not convert '" + (string)value + "' to type MsgType");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
