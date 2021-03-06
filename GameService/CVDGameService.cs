﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;

namespace CVDGameService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public long dwServiceType;
        public ServiceState dwCurrentState;
        public long dwControlsAccepted;
        public long dwWin32ExitCode;
        public long dwServiceSpecificExitCode;
        public long dwCheckPoint;
        public long dwWaitHint;
    }

    public partial class CVDGameService : ServiceBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private bool initialOwnership = true;
        private bool mutexWasCreated;

        private Mutex mutex;

        private string processExecutablePath;
        private string rootDir;
        private string scriptPath;

        private Process process;

        public CVDGameService(string[] args)
        {
            InitializeComponent();
            InitializeLog();

            if (args.Count() > 2)
            {
                processExecutablePath = @args[0];
                rootDir = @args[1];
                scriptPath = @args[2];
            }
        }

        protected override void OnStart(string[] args)
        {
            ChangeServiceStatus(ServiceState.SERVICE_START_PENDING, 10000L);
            
            //catch system mutex 
            CatchResource();

            //if OK we here
            eventLog.WriteEntry("Start game service", EventLogEntryType.Information);
            ChangeServiceStatus(ServiceState.SERVICE_RUNNING);

            //run main process
            string path = args.Count() > 0 ? @args[0] : "";
            RunProcess(path);
        }

        protected override void OnStop()
        {
            int timeout = 10000;
            var task = System.Threading.Tasks.Task.Factory.StartNew(() => StopProcess());

            while (!task.Wait(timeout))
                RequestAdditionalTime(timeout);

            eventLog.WriteEntry("Stop game service", EventLogEntryType.Information);
            ReleaseResource();
        }

        private void RunProcess(string path)
        {
            string pyPattern = @"^.*\.(py)$";
            string exePattern = @"^.*\.(exe)$";

            string scriptPath = ValidatePath(path, pyPattern) ? path : ValidatePath(this.scriptPath) ? this.scriptPath : null;
            string exePath = ValidatePath(processExecutablePath, exePattern) ? processExecutablePath : "python.exe";

            eventLog.WriteEntry(String.Format("Game process will running with arguments: {0} {1}", exePath, scriptPath), EventLogEntryType.Warning);

            //invalid arguments clause
            if (String.IsNullOrEmpty(scriptPath))
            {
                eventLog.WriteEntry(String.Format("Can't run game process, clause: {0}", "Invalid file paths"), EventLogEntryType.Error);
                this.Stop();
            }

            //normal flow
            process = new Process();

            try
            {
                process.StartInfo.FileName = exePath;
                process.StartInfo.Arguments = scriptPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

                string dir = ValidatePath(path, pyPattern) ? Path.GetFullPath(path).Replace("game.py", "") : ValidatePath(rootDir) ? rootDir : Path.GetFullPath(scriptPath).Replace("game.py", "");

                process.StartInfo.WorkingDirectory = dir;
                
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(process_Exited);

                process.Start();
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(String.Format("Can't run game process, clause: {0}", ex.Message), EventLogEntryType.Error);
                this.Stop();
            }

            eventLog.WriteEntry("Run game process", EventLogEntryType.Warning);

        }

        //handle exited event
        private void process_Exited(object sender, EventArgs e)
        {
            //in this method call StopProcess()
            this.Stop();
        }

        private void StopProcess()
        {
            //make sure process not null
            if (process != null)
            {
                //check if process still work and terminate them
                if (!process.HasExited)
                {
                    process.Kill();
                    process.WaitForExit();
                }

                EventLogEntryType type = process.ExitCode == 1 ? EventLogEntryType.Error : EventLogEntryType.Warning;

                eventLog.WriteEntry(String.Format("Game process was stopped, exit code: {0}", process.ExitCode), type);
            }
        }

        private bool ValidatePath(string path, string pattern = null)
        {
            bool pathNotNullOrEmpty = !String.IsNullOrEmpty(path);

            if (!String.IsNullOrEmpty(pattern))
                return pathNotNullOrEmpty && Regex.IsMatch(path, pattern);

            return pathNotNullOrEmpty;
        }

        private void InitializeLog()
        {
            eventLog = new EventLog();
            if (!EventLog.SourceExists("GameSource"))
            {
                EventLog.CreateEventSource("GameSource", "PlaytechLog");
            }
            eventLog.Source = "GameSource";
            eventLog.Log = "PlaytechLog";
        }

        private void ChangeServiceStatus(ServiceState state, long dwWaitHint = -1)
        {
            ServiceStatus status = new ServiceStatus();
            status.dwCurrentState = state;
            if (dwWaitHint > 0) status.dwWaitHint = dwWaitHint;
            SetServiceStatus(this.ServiceHandle, ref status);
        }

        private void CatchResource()
        {
            mutex = new Mutex(initialOwnership, "PlaytechServicesMutex", out mutexWasCreated);

            if (!(initialOwnership && mutexWasCreated))
            {
                eventLog.WriteEntry("Can't start game service, learning service must be stopped", EventLogEntryType.Error);
                this.Stop();
            }
        }

        private void ReleaseResource()
        {
            if (initialOwnership && mutexWasCreated)
                mutex.ReleaseMutex();
        }
    }
}
