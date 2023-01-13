using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YMES.FX.MainForm.Base
{
    public partial class MainFormComponent : Component
    {
        [Browsable(false)]
        public ContainerControl ContainerControl
        {
            get { return _containerControl; }
            set { _containerControl = value; }
        }
        private ContainerControl _containerControl = null;
        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;
                if (value == null)
                {
                    return;
                }

                IDesignerHost host = value.GetService(
                    typeof(IDesignerHost)) as IDesignerHost;
                if (host != null)
                {
                    IComponent componentHost = host.RootComponent;
                    if (componentHost is ContainerControl)
                    {
                        ContainerControl = componentHost as ContainerControl;
                    }
                }
            }
        }
        
        public MainFormComponent()
        {
            InitializeComponent();
        }

        public MainFormComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
