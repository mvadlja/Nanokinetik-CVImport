using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Timers;
using System.Runtime.Remoting.Messaging;


namespace ReadyScheduler
{
    public partial class ReadyScheduler : ServiceBase
    {
        private System.Timers.Timer timer;
        delegate void MyDelegate();

        public ReadyScheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string interval = ConfigurationManager.AppSettings["Interval"];
          
            int intervalMin = 0;
            if (!int.TryParse(interval, out intervalMin))
            {
                intervalMin = 60;
            }
            //svakih n minuta
            timer = new System.Timers.Timer(1000 * 60 * intervalMin);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }

        protected override void OnContinue()
        {
            timer.Enabled = true;
        }

        protected override void OnShutdown()
        {
            timer.Enabled = false;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //REMINDERS
            {
                MyDelegate X = new MyDelegate(CheckReminders);
                AsyncCallback cb = new AsyncCallback(EndTask);
                IAsyncResult ar = X.BeginInvoke(cb, null);
            }
        }

        private void CheckReminders()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateAutomaticReminders"]) &&
                ConfigurationManager.AppSettings["CreateAutomaticReminders"].Trim().ToLower() == "true")
            {
                global::ReadyScheduler.Tasks.RemindersTask.CreateAutomaticReminders();
            }
            
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CheckReminders"]) &&
                ConfigurationManager.AppSettings["CheckReminders"].Trim().ToLower() == "true")
            {
                global::ReadyScheduler.Tasks.RemindersTask.CheckReminders();
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteOldDismissedAutomaticReminders"]) &&
                ConfigurationManager.AppSettings["DeleteOldDismissedAutomaticReminders"].Trim().ToLower() == "true")
            {
                global::ReadyScheduler.Tasks.RemindersTask.DeleteOldDismissedAutomaticReminders();
            }
        }

        static void EndTask(IAsyncResult ar)
        {
            MyDelegate X = (MyDelegate)((AsyncResult)ar).AsyncDelegate;
            X.EndInvoke(ar);
        }
    }
}
