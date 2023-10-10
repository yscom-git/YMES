using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.DB.Base
{
    [TypeConverter(typeof(XMLConfConnEleConverter))]
    public class XMLConfConnEle
    {
        public XMLConfConnEle(string dbKind, string dbServer, string dbID, string dbPWD, string dbSID, string dbPORT)
        {
            this.m_dbKind = dbKind;
            this.m_dbServer = dbServer;
            this.m_dbID = dbID;
            this.m_dbPWD = dbPWD;
            this.m_dbSID = dbSID;
            this.m_dbPORT = dbPORT;
        }
        public XMLConfConnEle()
        {

            this.m_dbKind = "DB_CONNECTION";
            this.m_dbServer = "DBSVR";
            this.m_dbID = "DBUID";
            this.m_dbPWD = "DBPWD";
            this.m_dbSID = "DBSERVICE";
            this.m_dbPORT = "DBPORT";

        }
        private string m_dbKind="";
        private string m_dbServer = "";
        private string m_dbID = "";
        private string m_dbPWD = "";
        private string m_dbSID = "";
        private string m_dbPORT = "";

        [Browsable(true)]
        [NotifyParentProperty(true)]
        [DefaultValue(null)]
        public string dbKind
        {
            get
            {
                return m_dbKind;
            }
            set
            {
                m_dbKind = value;
            }
        }
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [DefaultValue(null)]
        public string dbServer
        {
            get
            {
                return m_dbServer;
            }
            set
            {
                m_dbServer = value;
            }
        }
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [DefaultValue(null)]
        public string dbID
        {
            get
            {
                return m_dbID;
            }
            set
            {
                m_dbID = value;
            }
        }
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [DefaultValue(null)]
        public string dbPWD
        {
            get
            {
                return m_dbPWD;
            }
            set
            {
                m_dbPWD = value;
            }
        }
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [DefaultValue(null)]
        public string dbSID
        {
            get
            {
                return m_dbSID;
            }
            set
            {
                m_dbSID = value;
            }
        }
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [DefaultValue(null)]
        public string dbPORT
        {
            get
            {
                return m_dbPORT;
            }
            set
            {
                m_dbPORT = value;
            }
        }

    }

    public class XMLConfConnEleConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(XMLConfConnEle))
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
            if (destinationType == typeof(System.String) && value is XMLConfConnEle)
            {

                XMLConfConnEle m = (XMLConfConnEle)value;
                return "" + m.dbKind +
                    "," + m.dbServer +
                    "," + m.dbID +
                    "," + m.dbPWD +
                    "," + m.dbSID +
                    "," + m.dbPORT;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    XMLConfConnEle m = new XMLConfConnEle();
                    string[] sARR = ((string)value).Split(',');
                    for (int i = 0; i < sARR.Length; i++)
                    {
                        if (i == 0)
                        {
                            m.dbKind = sARR[i];
                        }
                        else if (i == 1)
                        {
                            m.dbServer = sARR[i];
                        }
                        else if (i == 2)
                        {
                            m.dbID = sARR[i];
                        }
                        else if (i == 3)
                        {
                            m.dbPWD = sARR[i];
                        }
                        else if (i == 4)
                        {
                            m.dbSID = sARR[i];
                        }
                        else if (i == 5)
                        {
                            m.dbPORT = sARR[i];
                        }

                    }


                    return m;
                }
                catch
                {
                    throw new ArgumentException("Can not convert '" + (string)value + "' to type XMLConfConnEle");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
