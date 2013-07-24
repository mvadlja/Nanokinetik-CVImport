using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Timers;
using System.Runtime.Remoting.Messaging;
namespace SandozTaskScheduler
{
    public partial class MATaskScheduler : ServiceBase
    {

        private System.Timers.Timer serviceLifeCycleTimer;
        private System.Timers.Timer archivationTimer;

        delegate void StarterMethod();
        private bool workflowInProgress = false;
        private bool archivationInProgress = false;

        public MATaskScheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //service Timer initialization
            string interval = System.Configuration.ConfigurationManager.AppSettings["Interval"];

            int intervalMinutes = 0;
            if (!int.TryParse(interval, out intervalMinutes))
            {
                intervalMinutes = 60;
            }

            serviceLifeCycleTimer = new System.Timers.Timer(1000 * 60 * intervalMinutes);
            serviceLifeCycleTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            serviceLifeCycleTimer.Enabled = true;


            //archivation timer initialization
            string archivationInterval = System.Configuration.ConfigurationManager.AppSettings["ArchivationInterval"];
            int archivationIntervalInt = 0;
            if (!int.TryParse(archivationInterval, out archivationIntervalInt))
            {
                archivationIntervalInt = -1;
            }

            string archivationHoursString = System.Configuration.ConfigurationManager.AppSettings["ArchivationHours"];
            List<int> archivationHours = new List<int>();
            if (!String.IsNullOrEmpty(archivationHoursString))
            {
                String[] hourParts = archivationHoursString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String hour in hourParts) {
                    int hr;
                    if (Int32.TryParse(hour, out hr) && hr>=0 && hr<24 && !archivationHours.Contains(hr)) {
                        archivationHours.Add(hr);
                    }
                }
            }

            //hourly mode
            if (archivationHours.Count != 0 || archivationIntervalInt == -1)
            {
                if (archivationHours.Count == 0) archivationHours.Add(0);
                Workflow.ArcivationHours = archivationHours;
                archivationTimer = new System.Timers.Timer(1000 * 60 * 1);
                archivationTimer.Elapsed += new ElapsedEventHandler(archivationTimer_Elapsed);
                archivationTimer.Enabled = true;
            }
            //interval mode
            else 
            {
                Workflow.ArcivationHours = null;
                archivationTimer = new System.Timers.Timer(1000 * 60*60  *archivationIntervalInt);
                archivationTimer.Elapsed += new ElapsedEventHandler(archivationTimer_Elapsed);
                archivationTimer.Enabled = true;
            }

            
        }

        protected override void OnStop()
        {
            serviceLifeCycleTimer.Enabled = false;
            archivationTimer.Enabled = false;
        }

        protected override void OnContinue()
        {
            serviceLifeCycleTimer.Enabled = true;
            archivationTimer.Enabled = true;
        }

        protected override void OnShutdown()
        {
            serviceLifeCycleTimer.Enabled = false;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.workflowInProgress) return;
            StarterMethod startJob = new StarterMethod(Workflow.Start);
            startJob.BeginInvoke(new AsyncCallback(ExecutionEnded), null);
            this.workflowInProgress = true;
        }

        private void ExecutionEnded(IAsyncResult ar)
        {
            StarterMethod startJob = (StarterMethod)((AsyncResult)ar).AsyncDelegate;
            startJob.EndInvoke(ar);
            this.workflowInProgress = false;
        }

        void archivationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.archivationInProgress) return;
            StarterMethod startJob = new StarterMethod(Workflow.StartArchivation);
            startJob.BeginInvoke(new AsyncCallback(ArchivationEnded), null);
            this.archivationInProgress = true;
        }

        private void ArchivationEnded(IAsyncResult ar)
        {
            StarterMethod startJob = (StarterMethod)((AsyncResult)ar).AsyncDelegate;
            startJob.EndInvoke(ar);
            this.archivationInProgress = false;
        }

    }
}
