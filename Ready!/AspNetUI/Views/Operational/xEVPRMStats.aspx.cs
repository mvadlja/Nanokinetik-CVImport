using System;
using System.Threading;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.ServiceProcess;
using System.Drawing;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using CommonComponents;

namespace AspNetUI.Views
{
    public partial class xEVPRMStats : DefaultPage
    {
        #region Class members
        string _serviceName;
        string _serviceDomainName;
        string _serviceImpersonationUsername;
        string _serviceImpersonationPassword;

        ServiceController _svcController;

        IService_log_PKOperations _serviceLogOperations;
        IXevprm_message_PKOperations _xevprmMessageOperations;
        #endregion

        #region Form handlers

        // Iniitialize
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnToggleService.Click += btnToggleService_Click;

            _serviceName = ConfigurationManager.AppSettings["EVGatewayServiceName"];
            _serviceDomainName = ConfigurationManager.AppSettings["serviceDomainName"];
            _serviceImpersonationUsername = ConfigurationManager.AppSettings["serviceImpersonationUsername"];
            _serviceImpersonationPassword = ConfigurationManager.AppSettings["serviceImpersonationPassword"];
            _serviceLogOperations = new Service_log_PKDAL();
            _xevprmMessageOperations = new Xevprm_message_PKDAL();
            _svcController = new ServiceController(_serviceName);

            BindForm();
        }

        public void btnRefresh_OnClick(object sender, EventArgs e)
        {
            BindForm();
        }

        // Bind necessary controls
        private void BindForm()
        {
            if (pnlService.EmbedVisibilityPermissions(Permission.AS2ServiceShowStatus))
            {
                if (serviceExists(_serviceName, _serviceDomainName))
                {
                    setServiceStatus(_svcController.Status);

                    btnToggleService.EmbedPermissions(Permission.AS2ServiceStartStop);

                    if (Thread.CurrentPrincipal.Identity.Name == "adminuser" && _svcController.Status == ServiceControllerStatus.Running)
                    {
                        divServiceProcessingCommands.Visible = true;
                    }
                }
                else
                {
                    ctlServiceStatus.Text = string.Format("The service {0} can't be found!", _serviceName);
                    ctlServiceStatus.Visible = true;
                    btnToggleService.Disable();
                }
            }

            DataSet xEvprmStats = _xevprmMessageOperations.GetXevprmStatistic();

            if (xEvprmStats != null && xEvprmStats.Tables.Count > 0)
            {
                DataRow xEvprmStatsRow = xEvprmStats.Tables[0].Rows[0];
                ctlNumberOfSentMessages.InnerText = xEvprmStatsRow["sent_messages"].ToString();
                ctlSentMessageWaitingACK.InnerText = xEvprmStatsRow["ack_pending"].ToString();
                ctlNumberOfReceivedMessage.InnerText = xEvprmStatsRow["received_messages"].ToString();
                ctlPendingProcessing.InnerText = xEvprmStatsRow["process_pending"].ToString();
                ctlReceivedACKSuccess.InnerText = xEvprmStatsRow["ACK01"].ToString();
                ctlReceivedACKPartialy.InnerText = xEvprmStatsRow["ACK02"].ToString();
                ctlReceivedACKError.InnerText = xEvprmStatsRow["ACK03"].ToString();
                ctlLastProcessingTime.InnerText = !string.IsNullOrWhiteSpace(xEvprmStatsRow["lastProcessingTime"].ToString()) ? xEvprmStatsRow["lastProcessingTime"].ToString() : "-";
            }

            try
            {
                if (File.Exists(ConfigurationManager.AppSettings["ServiceConfigFile"]))
                {
                    string configFile = string.Empty;
                    configFile = File.ReadAllText(ConfigurationManager.AppSettings["ServiceConfigFile"]);

                    Match intervalMatch = Regex.Match(configFile, "<add\\s+key=\"Interval\"\\s*value=\"(\\d*)\"\\s*/>", RegexOptions.None);
                    if (intervalMatch.Success && !string.IsNullOrWhiteSpace(intervalMatch.Groups[1].Value))
                    {
                        ctlPollingInterval.InnerText = intervalMatch.Groups[1].Value + " sec";
                    }
                    else
                    {
                        ctlPollingInterval.InnerText = "-";
                    }
                }
            }
            catch (Exception)
            {

            }

            if (File.Exists(ConfigurationManager.AppSettings["AS2HandlerInboundLogFullFilePath"]))
            {
                var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
                if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) hlLogFileDownload.NavigateUrl = "~/Views/Business/FileDownload.ashx?file=AS2LogInbound";
               
                hlLogFileDownload.Text = "Log.txt";
            }
            else hlEmailNotificationLogFileDownload.Text = Constant.ControlDefault.LbPrvText;

            if (File.Exists(ConfigurationManager.AppSettings["LogFilePath"] + ConfigurationManager.AppSettings["EmailNotificationLogFileName"]))
            {
                var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
                if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) hlEmailNotificationLogFileDownload.NavigateUrl = "~/Views/Business/FileDownload.ashx?file=EmailNotificationLog";

                hlEmailNotificationLogFileDownload.Text = "EmailNotificationLog.txt";
            }
            else hlEmailNotificationLogFileDownload.Text = Constant.ControlDefault.LbPrvText;
        }

        #endregion

        #region Service managment

        #region Helper handlers

        private void setServiceStatus(ServiceControllerStatus serviceStatus)
        {
            string lastChange = "";
            DateTime? logTime = _serviceLogOperations.TimeOfLastChange();
            if (logTime != null)
                lastChange = logTime.ToString();
            switch (serviceStatus)
            {
                case ServiceControllerStatus.Stopped:
                case ServiceControllerStatus.StopPending:
                    btnToggleService.Text = "Start service";
                    ctlServiceStatus.Text = string.Format("Service is stopped at {0}", lastChange);
                    ctlServiceStatus.ForeColor = Color.Red;
                    break;
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                    btnToggleService.Text = "Stop service";
                    ctlServiceStatus.Text = string.Format("Service is running since {0}", lastChange);
                    ctlServiceStatus.ForeColor = Color.Green;
                    break;
            }
        }

        // Service toggle button
        void btnToggleService_Click(object sender, EventArgs e)
        {
            if (!SecurityHelper.IsPermitted(Permission.AS2ServiceStartStop)) return;

            if (btnToggleService.Text.Trim().ToLower().Contains("start"))
            {
                startService();
            }
            else
            {
                stopService();
            }

            if (Thread.CurrentPrincipal.Identity.Name == "adminuser" && _svcController.Status == ServiceControllerStatus.Running)
            {
                divServiceProcessingCommands.Visible = true;
            }
            else
            {
                divServiceProcessingCommands.Visible = false;
            }
        }

        bool serviceExists(string serviceName, string serviceDomainName)
        {
            ServiceController[] services = ServiceController.GetServices(serviceDomainName);
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);
            return service != null;
        }

        #endregion

        #region Impersonation

        private void startService()
        {
            try
            {
                // need to impersonate with the user having appropriate rights to start the service
                Impersonate objImpersonate = new Impersonate(_serviceDomainName, _serviceImpersonationUsername, _serviceImpersonationPassword);
                if (objImpersonate.impersonateValidUser())
                {
                    StartWindowService();
                    objImpersonate.undoImpersonation();
                }
                else
                {
                    divServiceError.Visible = true;
                    serviceError.Text = "User doesn't have sufficient permissions to start windows service: " + _serviceName;
                }
            }
            catch (Exception Ex)
            {
                divServiceError.Visible = true;
                serviceError.Text = "Service cannot be started.\nContact your system administrator."; // + " || " + Ex.StackTrace + " || " + Ex.Message;
            }
        }

        private void stopService()
        {
            try
            {
                // need to impersonate with the user having appropriate rights to stop the service
                Impersonate objImpersonate = new Impersonate(_serviceDomainName, _serviceImpersonationUsername, _serviceImpersonationPassword);
                if (objImpersonate.impersonateValidUser())
                {
                    StopWindowService();
                    objImpersonate.undoImpersonation();
                }
                else
                {
                    divServiceError.Visible = true;
                    serviceError.Text = "User doesn't have sufficient permissions to stop windows service: " + _serviceName;
                }
            }
            catch (Exception Ex)
            {
                divServiceError.Visible = true;
                serviceError.Text = "Service cannot be stopped.\nContact your system administrator."; // + " || " + Ex.StackTrace + " || " + Ex.Message;
            }
        }

        #endregion

        #region Start/Stop windows service

        private void StartWindowService()
        {
            if (_svcController != null)
            {
                try
                {
                    if (_svcController.Status != ServiceControllerStatus.Running && _svcController.Status != ServiceControllerStatus.StartPending)
                    {
                        string [] args = new string[] {"DisableEmailNotifications"};
                        _svcController.Start(args);

                        _svcController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                        btnToggleService.Text = "Stop service";
                        divServiceError.Visible = false;
                        setServiceStatus(_svcController.Status);

                        SendEmailNotifications(EmailOperations.EventType.As2ServiceStart);
                    }
                }
                catch (Exception Ex)
                {
                    divServiceError.Visible = true;
                    serviceError.Text = "Service cannot be started.\nContact your system administrator."; // + " || " + Ex.StackTrace + " || " + Ex.Message;
                }
            }
        }

        private void StopWindowService()
        {
            if (_svcController != null)
            {
                try
                {
                    if (_svcController.Status == ServiceControllerStatus.Running && _svcController.CanStop)
                    {
                        //svcController.Stop();

                        _svcController.ExecuteCommand(150);//Stop service

                        _svcController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                        btnToggleService.Text = "Start service";
                        divServiceError.Visible = false;
                        setServiceStatus(_svcController.Status);

                        SendEmailNotifications(EmailOperations.EventType.As2ServiceStop);
                    }
                }
                catch (Exception Ex)
                {
                    divServiceError.Visible = true;
                    serviceError.Text = "Service cannot be stopped.\nContact your system administrator."; // + "|| " + Ex.StackTrace + " || " + Ex.Message;
                }
            }
        }

        protected void btnXevprmSubmission_OnClick(object sender, EventArgs e)
        {
            if (_svcController == null) return;

            try
            {
                if (_svcController.Status == ServiceControllerStatus.Running)
                {
                    _svcController.ExecuteCommand(201);
                    MasterPage.ModalPopup.ShowModalPopup("Message", "<div style='text-align:center'>Command sent: Proccess xevprm messages ready for submission.<br/><br/></div>");
                }
                else
                {
                    MasterPage.ModalPopup.ShowModalPopup("Error", "<div style='text-align:center'>Service is not running.<br/><br/></div>");
                }
            }
            catch (Exception ex)
            {
                MasterPage.ModalPopup.ShowModalPopup("Error", string.Format("<div style='text-align:center'>Exception: {0}<br/>StackTrace: {1}<br/><br/></div>", ex.Message, ex.StackTrace));
            }
        }

        protected void btnReceivedMessageProcessing_OnClick(object sender, EventArgs e)
        {
            if (_svcController == null) return;

            try
            {
                if (_svcController.Status == ServiceControllerStatus.Running)
                {
                    _svcController.ExecuteCommand(202);
                    MasterPage.ModalPopup.ShowModalPopup("Message", "<div style='text-align:center'>Command sent: Proccess received messages.<br/><br/></div>");
                }
                else
                {
                    MasterPage.ModalPopup.ShowModalPopup("Error", "<div style='text-align:center'>Service is not running.<br/><br/></div>");
                }
            }
            catch (Exception ex)
            {
                MasterPage.ModalPopup.ShowModalPopup("Error", string.Format("<div style='text-align:center'>Exception: {0}<br/>StackTrace: {1}<br/><br/></div>", ex.Message, ex.StackTrace));
            }
        }

        protected void btnMDNSubmission_OnClick(object sender, EventArgs e)
        {
            if (_svcController == null) return;

            try
            {
                if (_svcController.Status == ServiceControllerStatus.Running)
                {
                    _svcController.ExecuteCommand(203);
                    MasterPage.ModalPopup.ShowModalPopup("Message", "<div style='text-align:center'>Command sent: Proccess xevprm messages ready for MDN submission.<br/><br/></div>");
                }
                else
                {
                    MasterPage.ModalPopup.ShowModalPopup("Error", "<div style='text-align:center'>Service is not running.<br/><br/></div>");
                }
            }
            catch (Exception ex)
            {
                MasterPage.ModalPopup.ShowModalPopup("Error", string.Format("<div style='text-align:center'>Exception: {0}<br/>StackTrace: {1}<br/><br/></div>", ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #endregion

        #region Email notifications

        void SendEmailNotifications(EmailOperations.EventType eventType)
        {
            try
            {
                IUSEROperations _userOperations = new USERDAL();
                IPerson_PKOperations _person_PKOperations = new Person_PKDAL();

                USER user = _userOperations.GetUserByUsername(System.Threading.Thread.CurrentPrincipal.Identity.Name);
                Person_PK person = user != null ? _person_PKOperations.GetEntity(user.Person_FK) : null;

                if (person != null)
                {
                    string userFullName = string.Empty;

                    if (!string.IsNullOrWhiteSpace(person.name))
                    {
                        userFullName += person.name.Trim() + " ";
                    }

                    if (!string.IsNullOrWhiteSpace(person.familyname))
                    {
                        userFullName += person.familyname.Trim();
                    }

                    userFullName = userFullName.Trim();

                    var operationResult = EmailOperations.SendEmailNotifications(eventType, EmailOperations.ByWhom.User, userFullName);

                    if (!operationResult.IsSuccess)
                    {
                        SetLogFileName();
                        LogOperations.LogError(LogOperations.LogType.As2ServiceEmailNotificationsLog, operationResult.Exception, operationResult.Description);
                    }
                }
                else
                {
                    var operationResult = EmailOperations.SendEmailNotifications(eventType, EmailOperations.ByWhom.User);

                    if (!operationResult.IsSuccess)
                    {
                        SetLogFileName();
                        LogOperations.LogError(LogOperations.LogType.As2ServiceEmailNotificationsLog, operationResult.Exception, operationResult.Description);
                    }
                }
                
            }
            catch (Exception ex)
            {
                SetLogFileName();
                LogOperations.LogError(LogOperations.LogType.As2ServiceEmailNotificationsLog, ex, "Error at sending email notifications: ");
            }
        }

        private void SetLogFileName()
        {
            string logsFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            string emailNotificationLogFileName = ConfigurationManager.AppSettings["EmailNotificationLogFileName"];

            if (Directory.Exists(logsFilePath))
            {
                LogOperations.As2ServiceEmailNotificationsLogFile = logsFilePath;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(logsFilePath);
                    LogOperations.As2ServiceEmailNotificationsLogFile = logsFilePath;
                }
                catch (Exception ex)
                {
                    logsFilePath = "C:\\";
                    LogOperations.As2ServiceEmailNotificationsLogFile = logsFilePath;
                }
            }

            if (!string.IsNullOrWhiteSpace(LogOperations.As2ServiceEmailNotificationsLogFile))
            {
                LogOperations.As2ServiceEmailNotificationsLogFile += emailNotificationLogFileName;
            }
            else
            {
                LogOperations.As2ServiceEmailNotificationsLogFile += "EmailNotificationLog.txt";
            }
        }

        #endregion
    }
}