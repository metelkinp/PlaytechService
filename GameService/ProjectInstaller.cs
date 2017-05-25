using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace GameService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            string pythonPath = "python.exe";
            string scriptPath = "C:\\playtech_card_recognintion\\game.py";

            Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\" \"" + pythonPath + "\" \"" + scriptPath + "\"";

            base.OnBeforeInstall(savedState);
        }
    }
}
