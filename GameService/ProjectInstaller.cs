using System;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace CVDGameService
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

            string dir = Assembly.GetExecutingAssembly().Location;

            string scriptPath = Path.GetFullPath(Path.Combine(dir, @"..\..\game.py"));

            string rootDir = Path.GetFullPath(Path.Combine(dir, @"..\.."));


            Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\" \"" + pythonPath + "\" \"" + rootDir + "\" \"" + scriptPath + "\"";

            base.OnBeforeInstall(savedState);
        }
    }
}
