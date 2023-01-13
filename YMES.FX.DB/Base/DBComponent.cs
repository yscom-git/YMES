using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.DB.Base
{
    [ToolboxItem(false)]
    public partial class DBComponent : Component
    {
        private const string CN_COMMANDTY = "@COMMAND_TYPE";
        private Dictionary<string, string> m_DicEmptyForSP = new Dictionary<string, string>();
        public Dictionary<string, string> DicEmptyForSP
        {
            get
            {
                m_DicEmptyForSP.Clear();
                m_DicEmptyForSP.Add(CN_COMMANDTY, "SP");
                return m_DicEmptyForSP;
            }
        }
        protected Dictionary<string, string> GetReservedParam(Dictionary<string, string> param)
        {
            if(param.Count ==1)
            {
                if(param.ContainsKey(CN_COMMANDTY))
                {
                    return new Dictionary<string, string>();
                }
            }
            return param;

        }
        protected Dictionary<string, object> GetReservedParam(Dictionary<string, object> param)
        {
            if (param.Count == 1)
            {
                if (param.ContainsKey(CN_COMMANDTY))
                {
                    return new Dictionary<string, object>();
                }
            }
            return param;

        }
        protected Dictionary<string, ArrayList> GetReservedParam(Dictionary<string, ArrayList> param)
        {
            if (param.Count == 1)
            {
                if (param.ContainsKey(CN_COMMANDTY))
                {
                    return new Dictionary<string, ArrayList>();
                }
            }
            return param;

        }
        private string m_XMLConfigPath = "";
        public string XMLConfigPath
        {
            get { return m_XMLConfigPath; }
            set { m_XMLConfigPath = value; }
        }
        public DBComponent()
        {
            InitializeComponent();
        }
        public DBComponent(string xmlPath)
        {
            InitializeComponent();
            m_XMLConfigPath = xmlPath;
        }
        public DBComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
