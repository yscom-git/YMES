using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YMES.FX.Controls.Base;

namespace YMES.FX.Controls
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Label))]
    public class YBitLabel : System.Windows.Forms.Label, IYControls
    {
        private bool m_Value = false;
        private Color m_OnForeColor = Color.Black;
        private Color m_OnBGColor = Color.Lime;
        private Color m_OffForeColor = Color.White;
        private Color m_OffBGColor = Color.Black;

        private string m_Key = string.Empty;
        
        [Category(Common.CN_CATEGORY_APP)]
        public Color OnForeColor
        {
            get { return m_OnForeColor; }
            set { m_OnForeColor = value; }
        }
        [Category(Common.CN_CATEGORY_APP)]
        public Color OnBGColor
        {
            get { return m_OnBGColor; }
            set { m_OnBGColor = value; }
        }
        [Category(Common.CN_CATEGORY_APP)]
        public Color OffForeColor
        {
            get { return m_OffForeColor; }
            set { m_OffForeColor = value; }
        }
        [Category(Common.CN_CATEGORY_APP)]
        public Color OffBGColor
        {
            get { return m_OffBGColor; }
            set { m_OffBGColor = value; }
        }

        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        public void ClearValue()
        {
            this.m_Value = false;
        }

        public object GetValue()
        {
            return this.m_Value;
        }

        public void RefreshCtl()
        {
            if(this.m_Value)
            {
                this.ForeColor = OnForeColor;
                this.BackColor = OnBGColor;
            }
            else
            {
                this.ForeColor = OffForeColor;
                this.BackColor = OffBGColor;
            }
        }

        public void SetValue(object val)
        {
            if (val is string)
            {
                val = false;
                switch (val.ToString().Replace(" ", "").ToUpper())
                {
                    case "TRUE":
                    case "T":
                    case "OK":
                    case "1":
                    case "Y":
                    case "YES":
                        m_Value = true;
                        break;
                }
            }
            else if (val is Boolean)
            {
                m_Value = (Boolean)val;
            }
            else
            {
                m_Value = false;
            }
            RefreshCtl();
        }
        
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.AutoSize = false;
            this.TextAlign = ContentAlignment.MiddleCenter;
            RefreshCtl();
        }
    }
}
