using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static YMES.FX.MainForm.Base.Common;

namespace YMES.FX.MainForm.Base
{
    [ToolboxItem(true)]
    [TypeConverter(typeof(MainFormTitlesConverter))]
    public class MainFormDesign : MainFormComponent
    {
        private const string CN_CATEGORY = "_YMES.Appearance";
        private Image m_LogoImg = Properties.Resources.PB_LOGO;
        private Icon m_AppIcon;

        private MsgType m_MsgTypeText = new MsgType("WARNING", "NOTICE", "ERROR", "TRACE");
        private DB.Base.XMLConfConnEle m_DBXmlElementName = new DB.Base.XMLConfConnEle("DB_CONNECTION", "DBSVR", "DBUID", "DBPWD", "DBSERVICE", "DBPORT");
        
        private Common.DateFormatEnum m_TIT_DateFormat = Common.DateFormatEnum.YYYYMMDD;
        private DateTime m_CurrentTime = DateTime.Now;
        private IMainFormDesign m_Parent;
        private string m_TIT_Plant;
        private Font m_TIT_Plant_FONT;

        private string m_TIT_Line;
        private Font m_TIT_Line_FONT;

        private string m_TIT_Station;
        private Font m_TIT_Station_FONT;

        private string m_TIT_Result;
        private Font m_TIT_Result_FONT;

        private string m_TIT_WorkStandard;
        private Font m_TIT_WorkStandard_FONT;

        private string m_TIT_Exit;
        private Font m_TIT_Exit_FONT;

        private string m_TIT_Config;
        private Font m_TIT_Config_FONT;

        private string m_DuplicatedRunMsg = "Duplicated Application";
        private string m_DuplicatedRunTitle = "Violated Run";
        private string m_Exit_Dlg_Title= "Exit of Program";
        private string m_Exit_Dlg_Contents= "Do you want to exit program?";

        [System.ComponentModel.Category(CN_CATEGORY)]
        public DB.Base.XMLConfConnEle DBXmlElementName
        {
            get { return m_DBXmlElementName; }
            set { m_DBXmlElementName = value; }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string Exit_Dlg_Title
        {
            get { return m_Exit_Dlg_Title; }
            set { m_Exit_Dlg_Title = value; }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string DuplicatedRunMsg
        {
            get { return m_DuplicatedRunMsg; }
            set { m_DuplicatedRunMsg = value; }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string DuplicatedRunTitle
        {
            get { return m_DuplicatedRunTitle; }
            set { m_DuplicatedRunTitle = value; }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string Exit_Dlg_Contents
        {
            get { return m_Exit_Dlg_Contents; }
            set { m_Exit_Dlg_Contents = value; }
        }

        [Browsable(false)]
        public IMainFormDesign Parent
        {
            get 
            { 
                if(ContainerControl!= null)
                {
                    if(ContainerControl is IMainFormDesign)
                    {
                        m_Parent = (IMainFormDesign)ContainerControl;
                        return (IMainFormDesign)ContainerControl;
                    }
                }
                return null;
                 
            }
            set 
            {
                if (ContainerControl != null)
                {
                    if (ContainerControl is IMainFormDesign)
                    {
                        ((IMainFormDesign)ContainerControl).Plant_CTL.Text = m_TIT_Plant;
                        ((IMainFormDesign)ContainerControl).Plant_CTL.Font = m_TIT_Plant_FONT;
                        ((IMainFormDesign)ContainerControl).Line_CTL.Text = m_TIT_Line;
                        ((IMainFormDesign)ContainerControl).Line_CTL.Font = m_TIT_Line_FONT;
                        ((IMainFormDesign)ContainerControl).Station_CTL.Text = m_TIT_Station;
                        ((IMainFormDesign)ContainerControl).Station_CTL.Font = m_TIT_Station_FONT;
                        ((IMainFormDesign)ContainerControl).WorkStandard_CTL.Text = m_TIT_WorkStandard;
                        ((IMainFormDesign)ContainerControl).WorkStandard_CTL.Font = m_TIT_WorkStandard_FONT;
                        ((IMainFormDesign)ContainerControl).Result_CTL.Text = m_TIT_Result;
                        ((IMainFormDesign)ContainerControl).Result_CTL.Font = m_TIT_Result_FONT;
                        ((IMainFormDesign)ContainerControl).Config_CTL.Text = m_TIT_Config;
                        ((IMainFormDesign)ContainerControl).Config_CTL.Font = m_TIT_Config_FONT;
                        ((IMainFormDesign)ContainerControl).LogoImg.Image = m_LogoImg;
                    }
                }
            }
        }
        public MainFormDesign()
        {
            m_TIT_Plant = "PLANT";
            m_TIT_Plant_FONT = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);

            m_TIT_Line = "This is Production Line";
            m_TIT_Line_FONT = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold);

            m_TIT_Station = "This is WorkStation";
            m_TIT_Station_FONT = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);

            m_TIT_Result = "Result";
            m_TIT_Result_FONT = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);

            m_TIT_Config = "Config";
            m_TIT_Config_FONT = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);

            m_TIT_Exit = "EXIT";
            m_TIT_Exit_FONT = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);

            m_TIT_WorkStandard = "WORK\r\nSTANDARD";
            m_TIT_WorkStandard_FONT = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold);
            m_LogoImg = Properties.Resources.PB_LOGO;
            

        }

        [System.ComponentModel.Category(CN_CATEGORY)]
        public Image LogoImg
        {
            get { return m_LogoImg; }
            set 
            { 
                m_LogoImg = value;
                if (Parent != null)
                {
                    Parent.LogoImg.Image = m_LogoImg;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public Icon AppIcon
        {
            get { return m_AppIcon; }
            set
            {
                m_AppIcon = value;
                if (Parent != null)
                {
                    Parent.AppIcon = m_AppIcon;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string TIT_Plant
        {
            get { return m_TIT_Plant; }
            set 
            { 
                m_TIT_Plant = value;
                if (Parent != null)
                {
                    Parent.Plant_CTL.Text = m_TIT_Plant;
                }
            }

        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public Font TIT_Plant_FONT
        {
            get { return m_TIT_Plant_FONT; }
            set 
            { 
                m_TIT_Plant_FONT = value;
                if (Parent != null)
                {
                    Parent.Plant_CTL.Font = m_TIT_Plant_FONT;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string TIT_Line
        {
            get { return m_TIT_Line; }
            set 
            { 
                m_TIT_Line = value;
                if (Parent != null)
                {
                    Parent.Line_CTL.Text = m_TIT_Line;
                }
            }

        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public Font TIT_Line_FONT
        {
            get { return m_TIT_Line_FONT; }
            set 
            { 
                m_TIT_Line_FONT = value;
                if (Parent != null)
                {
                    Parent.Line_CTL.Font = m_TIT_Line_FONT;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string TIT_Station
        {
            get { return m_TIT_Station; }
            set 
            { 
                m_TIT_Station = value;
                if (Parent != null)
                {
                    Parent.Station_CTL.Text = m_TIT_Station;
                }
            }

        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public Font TIT_Station_FONT
        {
            get { return m_TIT_Station_FONT; }
            set 
            { 
                m_TIT_Station_FONT = value;
                if (Parent != null)
                {
                    Parent.Station_CTL.Font = m_TIT_Station_FONT;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string TIT_WorkStandard
        {
            get { return m_TIT_WorkStandard; }
            set 
            {
                m_TIT_WorkStandard = value;
                if (Parent != null)
                {
                    Parent.WorkStandard_CTL.Text = m_TIT_WorkStandard;
                }
            }

        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public Font TIT_WorkStandard_FONT
        {
            get { return m_TIT_WorkStandard_FONT; }
            set
            {
                m_TIT_WorkStandard_FONT = value;
                if (Parent != null)
                {
                    Parent.WorkStandard_CTL.Font = m_TIT_WorkStandard_FONT;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string TIT_Result
        {
            get { return m_TIT_Result; }
            set
            {
                m_TIT_Result = value;
                if (Parent != null)
                {
                    Parent.Result_CTL.Text = m_TIT_Result;
                }
            }

        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public Font TIT_Result_FONT
        {
            get { return m_TIT_Result_FONT; }
            set 
            {
                m_TIT_Result_FONT = value;
                if (Parent != null)
                {
                    Parent.Result_CTL.Font = m_TIT_Result_FONT;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string TIT_Exit
        {
            get { return m_TIT_Exit; }
            set
            {
                m_TIT_Exit = value;
                if (Parent != null)
                {
                    Parent.Exit_CTL.Text = m_TIT_Exit;
                }
            }

        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public Font TIT_Exit_FONT
        {
            get { return m_TIT_Exit_FONT; }
            set
            {
                m_TIT_Exit_FONT = value;
                if (Parent != null)
                {
                    Parent.Exit_CTL.Font = m_TIT_Exit_FONT;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public string TIT_Config
        {
            get { return m_TIT_Config; }
            set
            {
                m_TIT_Config = value;
                if (Parent != null)
                {
                    Parent.Config_CTL.Text = m_TIT_Config;
                }
            }

        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public Font TIT_Config_FONT
        {
            get { return m_TIT_Config_FONT; }
            set
            {
                m_TIT_Config_FONT = value;
                if (Parent != null)
                {
                    Parent.Config_CTL.Font = m_TIT_Config_FONT;
                }
            }
        }
        [System.ComponentModel.Category(CN_CATEGORY)]
        public DateFormatEnum TIT_DateFormat
        {
            get { return m_TIT_DateFormat; }
            set
            {

                m_TIT_DateFormat = value;
                SyncTIT_Date(m_CurrentTime);
            }
        }
        
        [System.ComponentModel.Category(CN_CATEGORY)]
        public MsgType MsgTypeText
        {
            get { return m_MsgTypeText; }
            set { m_MsgTypeText = value; }
        }
        public void SyncTIT_Date(DateTime date)
        {
            m_CurrentTime = date;
            if (Parent != null)
            {
                switch (m_TIT_DateFormat)
                {
                    case Common.DateFormatEnum.YYYYMMDD:
                        Parent.Date_CTL.Text = date.ToString("yyyy-MM-dd");
                        Parent.Time_CTL.Text = date.ToString("HH:mm:ss");
                        break;
                    case Common.DateFormatEnum.DDMMYYYY:
                        Parent.Date_CTL.Text = date.ToString("dd-MM-yyyy");
                        Parent.Time_CTL.Text = date.ToString("HH:mm:ss");
                        break;
                    case Common.DateFormatEnum.MMDDYYYY:
                        Parent.Date_CTL.Text = date.ToString("MM-dd-yyyy");
                        Parent.Time_CTL.Text = date.ToString("HH:mm:ss");
                        break;
                }
            }
            
        }
        public void StatusMsgTitle(Common.MsgTypeEnum msgType)
        {
            if (m_Parent != null)
            {
                switch (msgType)
                {
                    case Common.MsgTypeEnum.Alarm:
                        Parent.MsgTitle_CTL.Text = this.MsgTypeText.MSG_ALARM;
                        Parent.MsgTitle_CTL.BackColor = Color.Blue;
                        Parent.MsgTitle_CTL.ForeColor = Color.White;
                        Parent.StatusMsg.ForeColor = Color.Blue;
                        break;
                    case Common.MsgTypeEnum.Error:
                        Parent.MsgTitle_CTL.Text = this.MsgTypeText.MSG_ERROR;
                        Parent.MsgTitle_CTL.BackColor = Color.Red;
                        Parent.MsgTitle_CTL.ForeColor = Color.White;
                        Parent.StatusMsg.ForeColor = Color.Red;
                        break;
                    case Common.MsgTypeEnum.Warnning:
                        Parent.MsgTitle_CTL.Text = this.MsgTypeText.MSG_WARN;
                        Parent.MsgTitle_CTL.BackColor = Color.Purple;
                        Parent.MsgTitle_CTL.ForeColor = Color.White;
                        Parent.StatusMsg.ForeColor = Color.Purple;
                        break;
                    case Common.MsgTypeEnum.Trace:
                        Parent.MsgTitle_CTL.Text = this.MsgTypeText.MSG_TRACE;
                        Parent.MsgTitle_CTL.BackColor = Color.Purple;
                        Parent.MsgTitle_CTL.ForeColor = Color.White;
                        Parent.StatusMsg.ForeColor = Color.Purple;
                        break;
                }
            }
        }

        private void InitializeComponent()
        {

        }
    }

    public class MainFormTitlesConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(MainFormDesign))
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
            if (destinationType == typeof(System.String) && value is MainFormDesign)
            {
                MainFormDesign m = (MainFormDesign)value;
                return "" + m.TIT_Plant;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    MainFormDesign m = new MainFormDesign();
                    string[] sARR = ((string)value).Split(',');
                    for (int i = 0; i < sARR.Length; i++)
                    {
                        if (i == 0)
                        {
                            m.TIT_Plant = sARR[i];
                        }

                    }


                    return m;
                }
                catch
                {
                    throw new ArgumentException("Can not convert '" + (string)value + "' to type MainFormTitles");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
