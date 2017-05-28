using System;
using System.ServiceProcess;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public MainForm()
        {
            InitializeComponent();

            timer.Tick += new EventHandler(CheckServices);
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.scriptsFolderInput.Text = fbd.SelectedPath;
            }
        }

        private void stopRButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                StopService(gameServiceController);
                StopService(learningServiceController);
                ChangeStatus("All services was stopped");
            }
            catch (Exception ex)
            {
                ChangeStatus("ERROR: Can't stop services. Try again");
                stopRButton.Checked = false;
            }
        }

        private void gameRButton_CheckedChanged(object sender, EventArgs e)
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

        private void learnRButton_CheckedChanged(object sender, EventArgs e)
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

        private void ChangeStatus(string message)
        {
            statusBar.Panels[0].Text = "    " + message;
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

                    string path = scriptsFolderInput.Text;
                    path += service.Equals(gameServiceController) ? "\\game.py" : "\\learn.py";

                    string[] args = new string[] {@path};

                    service.Start(args);
                    service.WaitForStatus(ServiceControllerStatus.Running);

                    ChangeStatus(String.Format("{0} is running", service.DisplayName));
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
            CheckService(gameServiceController, gameRButton);
            CheckService(learningServiceController, learnRButton);
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

