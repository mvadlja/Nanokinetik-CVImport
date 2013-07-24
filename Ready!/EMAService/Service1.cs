using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Ready.Model;
using System.Configuration;
using System.IO;
using System.Timers;
using System.Text.RegularExpressions;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Net;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
namespace EMAService
{
    public partial class EMAService : ServiceBase
    {
        #region Declarations

        private static bool _inProcess;

        private System.Timers.Timer _timer;

        private Workflow workflow;

        #endregion

        #region Properties

        public static bool InProcess
        {
            get { return _inProcess; }
            set { _inProcess = value; }
        }

        public System.Timers.Timer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

     
        #endregion

//        private IService_log_PKOperations _serviceLogOperations;
        
    


        #region Constructors

        public EMAService()
        {
       
            InitializeComponent();
            
            InProcess = false;

            workflow = new Workflow();
 
        }

        #endregion

        #region Service life cycle methods

        protected override void OnStart(string[] args)
        {
            string intervalConfig = ConfigurationManager.AppSettings["Interval"];

            int interval = 0;

            if (int.TryParse(intervalConfig, out interval))
            {
                if (interval > 0)
                {
                    interval *= 1000;
                }
                else
                {
                    interval = 1800000;
                }
            }
            else
            {
                interval = 1800000;
            }

           

            Timer = new System.Timers.Timer(interval);
            Timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            Timer.Enabled = true;

            Workflow.LogEvent("Service has been started");

            InProcess = false;
        }

        protected override void OnStop()
        {
            Timer.Enabled = false;

            Workflow.LogEvent("Service has been stopped");

            InProcess = false;
        }

        protected override void OnContinue()
        {
            Timer.Enabled = true;
        }

        protected override void OnShutdown()
        {
            Timer.Enabled = false;
        }

        #endregion

        #region Service tasks

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!InProcess)
                {
                    InProcess = true;

                    Workflow.LogEvent("Processing iteration started");

                    workflow.Start();

                    Workflow.LogEvent("Processing iteration finished");

                    InProcess = false;
                }
            }
            catch (Exception ex)
            {
                InProcess = false;

                Workflow.LogError(ex, "Error at processing iteration!");
            }
        }
        #endregion

        
       
    }
}
