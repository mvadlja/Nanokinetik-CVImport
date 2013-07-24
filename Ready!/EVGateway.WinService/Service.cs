using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using System.Configuration;
using CommonComponents;

namespace EVGateway.WinService
{
    public partial class Service : ServiceBase
    {
        #region Declarations

        private static bool _inProcess;
        private static bool _emailNotificationsEnabled;

        private System.Timers.Timer _timer;

        private Workflow.Workflow _workflow;

        private static int _receivedMessageProcessingDelay;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public Service()
        {
            _inProcess = false;
            _emailNotificationsEnabled = true;

            InitializeComponent();
        }

        #endregion

        #region Service life cycle methods

        protected override void OnStart(string[] args)
        {
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(1000);
            //}

            _workflow = new Workflow.Workflow();

            int interval = GetConfigTimePeriod("Interval", TimeUnit.Seconds, TimeUnit.Miliseconds, 1800);

            _workflow.XevprmMessageSubmissionDelay = GetConfigTimePeriod("XevprmMessageSubmissionDelay", 30);
            _receivedMessageProcessingDelay = GetConfigTimePeriod("ReceivedMessageProcessingDelay", TimeUnit.Seconds, TimeUnit.Miliseconds, 60);

            Log.AS2Service.LogEvent("Service has been started");

            if (!args.Any() || args[0] != "DisableEmailNotifications")
            {
                SendEmailNotifications(EmailOperations.EventType.As2ServiceStart);
            }

            _inProcess = false;
            _emailNotificationsEnabled = true;

            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;

            Log.AS2Service.LogEvent("Service has been stopped");

            if (_emailNotificationsEnabled)
            {
                SendEmailNotifications(EmailOperations.EventType.As2ServiceStop);
            }

            _inProcess = false;
        }

        protected override void OnContinue()
        {
            _timer.Enabled = true;
        }

        protected override void OnShutdown()
        {
            _timer.Enabled = false;
        }

        protected override void OnCustomCommand(int command)
        {
            base.OnCustomCommand(command);

            if (command == 150) //Stop service
            {
                _emailNotificationsEnabled = false;

                this.Stop();

                _emailNotificationsEnabled = true;
            }

            try
            {

                if (command == 201)
                {
                    Log.AS2Service.LogEvent("Command received: Proccess xevprm messages ready for submission.");

                    if (!_inProcess)
                    {
                        Log.AS2Service.LogEvent("Command aborted: Proccess xevprm messages ready for submission. Service is working.");
                        return;
                    }

                    _inProcess = true;

                    try
                    {

                        _workflow.ProccessXevprmMessagesReadyForSubmission("OnCommand");
                    }
                    catch (Exception ex)
                    {
                        _inProcess = false;

                        Log.AS2Service.LogError(ex, "Processing error!");
                    }

                    _inProcess = false;
                }
                else if (command == 202)
                {
                    Log.AS2Service.LogEvent("Command received: Proccess received messages.");

                    if (!_inProcess)
                    {
                        Log.AS2Service.LogEvent("Command aborted: Proccess received messages. Service is working.");
                        return;
                    }

                    _inProcess = true;

                    try
                    {
                        _workflow.ProcessReceivedMessages("OnCommand");
                    }
                    catch (Exception ex)
                    {
                        _inProcess = false;

                        Log.AS2Service.LogError(ex, "Processing error!");
                    }

                    _inProcess = false;
                }
                else if (command == 203)
                {
                    Log.AS2Service.LogEvent("Command received: Proccess xevprm messages ready for MDN submission.");

                    if (!_inProcess)
                    {
                        Log.AS2Service.LogEvent("Command aborted: Proccess xevprm messages ready for MDN submission. Service is working.");
                        return;
                    }

                    _inProcess = true;

                    try
                    {
                        _workflow.ProcessXevprmMessagesReadyForMDNSubmission("OnCommand");
                    }
                    catch (Exception ex)
                    {
                        _inProcess = false;

                        Log.AS2Service.LogError(ex, "Processing error!");
                    }

                    _inProcess = false;
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Service tasks

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_inProcess) return;

                _inProcess = true;

                Log.AS2Service.LogEvent("Processing iteration started");

                _workflow.ProccessXevprmMessagesReadyForSubmission(null);

                Thread.Sleep(_receivedMessageProcessingDelay);

                _workflow.ProcessReceivedMessages(null);

                _workflow.ProcessXevprmMessagesReadyForMDNSubmission(null);

                Log.AS2Service.LogEvent("Processing iteration finished");

                _inProcess = false;
            }
            catch (Exception ex)
            {
                _inProcess = false;

                Log.AS2Service.LogError(ex, "Processing error!");
            }
        }
        #endregion

        #region Email notifications

        private void SendEmailNotifications(EmailOperations.EventType eventType)
        {
            var operationResult = EmailOperations.SendEmailNotifications(eventType, EmailOperations.ByWhom.System);

            if (!operationResult.IsSuccess)
            {
                Log.AS2ServiceEmailNotifications.LogError(operationResult.Exception, operationResult.Description);
            }
        }

        #endregion

        private enum TimeUnit
        {
            Days,
            Hours,
            Minutes,
            Seconds,
            Miliseconds
        }

        private static int GetConfigTimePeriod(string key, int @default)
        {
            return GetConfigTimePeriod(key, TimeUnit.Seconds, TimeUnit.Seconds, @default);
        }

        private static int GetConfigTimePeriod(string key, TimeUnit from, TimeUnit to, int @default, bool ceiling = true)
        {
            int value;
            bool isSuccess = false;

            var timeSpan = new TimeSpan();

            string intervalConfig = ConfigurationManager.AppSettings[key];

            if (int.TryParse(intervalConfig, out value))
            {
                if (value > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    value = @default;
                }
            }
            else
            {
                value = @default;
            }

            if (isSuccess && from != to)
            {
                switch (from)
                {
                    case TimeUnit.Days:
                        timeSpan = TimeSpan.FromDays(value);
                        break;
                    case TimeUnit.Hours:
                        timeSpan = TimeSpan.FromHours(value);
                        break;
                    case TimeUnit.Minutes:
                        timeSpan = TimeSpan.FromMinutes(value);
                        break;
                    case TimeUnit.Seconds:
                        timeSpan = TimeSpan.FromSeconds(value);
                        break;
                    case TimeUnit.Miliseconds:
                        timeSpan = TimeSpan.FromMilliseconds(value);
                        break;
                }

                var time = DateTime.Now - DateTime.Now.AddSeconds(5);

                switch (to)
                {
                    case TimeUnit.Days:
                        if (Math.Ceiling(timeSpan.TotalDays) > (double)int.MaxValue) value = int.MaxValue;
                        else value = ceiling ? (int)Math.Ceiling(timeSpan.TotalDays) : (int)Math.Floor(timeSpan.TotalDays);
                        break;
                    case TimeUnit.Hours:
                        if (Math.Ceiling(timeSpan.TotalHours) > (double)int.MaxValue) value = int.MaxValue;
                        else value = ceiling ? (int)Math.Ceiling(timeSpan.TotalHours) : (int)Math.Floor(timeSpan.TotalHours);
                        break;
                    case TimeUnit.Minutes:
                        if (Math.Ceiling(timeSpan.TotalMinutes) > (double)int.MaxValue) value = int.MaxValue;
                        else value = ceiling ? (int)Math.Ceiling(timeSpan.TotalMinutes) : (int)Math.Floor(timeSpan.TotalMinutes);
                        break;
                    case TimeUnit.Seconds:
                        if (Math.Ceiling(timeSpan.TotalSeconds) > (double)int.MaxValue) value = int.MaxValue;
                        else value = ceiling ? (int)Math.Ceiling(timeSpan.TotalSeconds) : (int)Math.Floor(timeSpan.TotalSeconds);
                        break;
                    case TimeUnit.Miliseconds:
                        if (Math.Ceiling(timeSpan.TotalMilliseconds) > (double)int.MaxValue) value = int.MaxValue;
                        else value = ceiling ? (int)Math.Ceiling(timeSpan.TotalMilliseconds) : (int)Math.Floor(timeSpan.TotalMilliseconds);
                        break;
                }
            }

            return value;
        }

    }
}
