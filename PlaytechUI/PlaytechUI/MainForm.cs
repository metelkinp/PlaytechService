using System;
using System.ServiceProcess;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PlaytechUI
{
    public partial class MainForm : Form
    {
        private string projectDir;
        private bool hasAnotherPath = false;
        private bool afterInit = false;

        public MainForm()
        {
            InitializeComponent();

            projectDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @".."));
            scriptsFolderInput.Text = projectDir;

            EnableBrowseFolder(false);

            InitializeHandlers();

            afterInit = true;
        }

        private void EnableBrowseFolder(bool enable)
        {
            scriptsFolderInput.Enabled = enable;
            browseButton.Enabled = enable;
        }

        private void InitializeHandlers()
        {
            CheckService(gameServiceController, gameRButton);

            if (gameServiceController.Status.Equals(ServiceControllerStatus.Running))
                ChangeStatus(String.Format("{0} is running", gameServiceController.DisplayName));

            gameRButton.CheckedChanged += new EventHandler(this.gameRButton_CheckedChanged);
            learnRButton.CheckedChanged += new EventHandler(this.learnRButton_CheckedChanged);
            stopRButton.CheckedChanged += new EventHandler(this.stopRButton_CheckedChanged);

            timer.Tick += new EventHandler(CheckServices);
            timer.Start();
        }

        private void browseButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.scriptsFolderInput.Text = fbd.SelectedPath;
                hasAnotherPath = true;
            }
        }

        private void stopRButton_CheckedChanged(object sender, EventArgs e)
        {
            if (afterInit)
            {
                try
                {
                    StopService(gameServiceController);
                    StopService(learningServiceController);
                    ChangeStatus("All services was stopped");
                    EnableBrowseFolder(true);
                }
                catch (Exception ex)
                {
                    ChangeStatus("ERROR: Can't stop services. Try again");
                    stopRButton.Checked = false;
                }
            }
        }

        private void gameRButton_CheckedChanged(object sender, EventArgs e)
        {
            if (afterInit)
            {
                try
                {
                    StopService(learningServiceController);

                    StartService(gameServiceController, gameRButton);
                }
                catch (Exception ex)
                {
                    ChangeStatus(String.Format("ERROR: Can't stop {0}. Try again", learningServiceController.DisplayName));
                    gameRButton.Checked = false;
                }
            }
        }

        private void learnRButton_CheckedChanged(object sender, EventArgs e)
        {
            if (afterInit)
            {
                try
                {
                    StopService(gameServiceController);

                    StartService(learningServiceController, learnRButton);
                }
                catch (Exception ex)
                {
                    ChangeStatus(String.Format("ERROR: Can't stop {0}. Try again", gameServiceController.DisplayName));
                    learnRButton.Checked = false;
                }
            }
        }

        private void ChangeStatus(string message)
        {
            statusBar.Panels[0].Text = "   " + message;
        }

        private void StopService(ServiceController service)
        {
            service.Refresh();

            if (service.Status.Equals(ServiceControllerStatus.Running))
            {
                ChangeStatus(String.Format("Stopping {0} ...", service.DisplayName));

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
            }
        }

        private void StartService(ServiceController service, RadioButton target)
        {
            service.Refresh();

            if (!service.Status.Equals(ServiceControllerStatus.Running))
            {
                try
                {
                    ChangeStatus(String.Format("Running {0} ...", service.DisplayName));

                    if (hasAnotherPath)
                    {
                        string serviceName = service.Equals(gameServiceController) ? "game.py" : "learn.py";
                        string path = Path.GetFullPath(Path.Combine(scriptsFolderInput.Text, serviceName));

                        string[] args = new string[] { @path };

                        service.Start(args);
                    } else
                    {
                        service.Start();
                    }

                    service.WaitForStatus(ServiceControllerStatus.Running);

                    ChangeStatus(String.Format("{0} is running", service.DisplayName));

                    EnableBrowseFolder(false);
                }
                catch (Exception ex)
                {
                    ChangeStatus(String.Format("ERROR: Can't run {0}. Try again", service.DisplayName));
                    target.Checked = false;
                }
            }

        }

        private void CheckServices(object mObject, EventArgs args)
        {
            if (afterInit)
            {
                CheckService(gameServiceController, gameRButton);
                CheckService(learningServiceController, learnRButton);
            }
        }

        private void CheckService(ServiceController service, RadioButton target)
        {
            service.Refresh();
            if (target.Checked == true && !service.Status.Equals(ServiceControllerStatus.Running))
            {
                target.Checked = false;
                ChangeStatus(String.Format("{0} was stopped", service.DisplayName));
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            timer.Stop();
            StopService(gameServiceController);
            StopService(learningServiceController);
            base.OnClosed(e);
        }

    }

}

