using System;
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
