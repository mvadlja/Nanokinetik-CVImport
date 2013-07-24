using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data;
using AspNetUI.Views.Shared.Template;
using Microsoft.Reporting.WebForms;
using Ready.Model;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.Business
{
    public partial class CustomerTimeReport : DefaultPage
    {
        #region Declarations

        IOrganization_PKOperations _organizationOperations;
        IPerson_PKOperations _personOperations;
        IProject_PKOperations _projectOperations;
        ITime_unit_PKOperations _timeUnitOperations;
        IUSEROperations _userOperations;

        #endregion

        #region Page methods

        /// <summary>
        /// Page Init method
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Sender object arguments</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PageType = PageType.Other;

            if (!SecurityHelper.IsPermittedAny(new List<Permission> { Permission.CreateCustomerTimeReport, Permission.CreateCustomerTimeReportMy })) btnCreateReport.Disable();

            _personOperations = new Person_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _projectOperations = new Project_PKDAL();
            _timeUnitOperations = new Time_unit_PKDAL();
            _userOperations = new USERDAL();

            if (!IsPostBack)
            {
                rvMain.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerURL"]);
                rvMain.ProcessingMode = ProcessingMode.Remote;
                rvMain.ServerReport.ReportServerCredentials = new CustomReportServerCredentials();
            }
            rvMain.AsyncRendering = false;

            rvMain.SizeToReportContent = true;

            DateTime currentTime = DateTime.Now;
            rvMain.ServerReport.DisplayName = String.Format("{0}{1}{2}CustomerTimeReport", currentTime.Year % 100, currentTime.Month, currentTime.Day);
        }

        /// <summary>
        /// Page Load method
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Sender object arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                BindPerson();
                BindCustomer();
                BindProject();

                DateTime to = DateTime.Now;
                DateTime from = new DateTime(to.Year, to.Month, 1);

                ctlDateFrom.ControlValue = from.ToString("dd.MM.yyyy");
                ctlDateTo.ControlValue = to.ToString("dd.MM.yyyy");

                string[] userRoles = SessionManager.Instance.CurrentUser.Roles;

                // Disable report calculation for other users unless current user has Office role
                //if (!userRoles.Contains("Office"))
                //{
                if (!SecurityHelper.IsPermitted(Permission.CreateCustomerTimeReport))
                {
                    disableResponsibleUser();
                }
                else
                {
                    // Disable Billing rate calculation for other users if current user has noBillingRate role 
                    //if (userRoles.Contains("noBillingRateCalculationForOthers"))
                    //{
                    if (!SecurityHelper.IsPermitted(Permission.CalculateBillingRate))
                    {
                        disableResponsibleUser();
                    }
                }
            }
        }

        #endregion

        #region Support methods

        #region Helper methods

        /// <summary>
        /// Disables choose option for Responsible user.
        /// Sets current user linked person to Responsible user default value
        /// </summary>
        void disableResponsibleUser()
        {
            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                ctlPerson.ControlValue = user.Person_FK;
            }
            ctlPerson.CurrentControlState = ControlState.YouCantChangeMe;
        }

        #endregion

        #region Controls binding methods

        /// <summary>
        /// Binds data for Responsible user drop down list. 
        /// Formats drop down list display values
        /// </summary>
        void BindPerson()
        {
            List<Person_PK> items = _personOperations.GetEntities();

            items.Sort((c1, c2) =>
            {
                return c1.name.CompareTo(c2.name);
            });

            foreach (Person_PK item in items)
            {
                item.name += " " + item.familyname;
            }

            ctlPerson.SourceValueProperty = "person_PK";
            ctlPerson.SourceTextExpression = "name";
            ctlPerson.FillControl<Person_PK>(items);
        }

        /// <summary>
        /// Binds data for Customer drop down list.
        /// Removes first item in the list
        /// </summary>
        void BindCustomer()
        {
            List<Organization_PK> items = _organizationOperations.GetEntities();

            items.Sort((c1, c2) =>
            {
                return c1.name_org.CompareTo(c2.name_org);
            });

            ctlCustomer.SourceValueProperty = "organization_PK";
            ctlCustomer.SourceTextExpression = "name_org";
            ctlCustomer.FillControl<Organization_PK>(items);

            if (ctlCustomer.ControlBoundItems.Count > 0)
            {
                ctlCustomer.ControlBoundItems.RemoveAt(0);
            }
        }

        /// <summary>
        /// Binds data for Project drop down list.
        /// </summary>
        void BindProject()
        {
            List<Project_PK> items = _projectOperations.GetEntities();

            items.Sort((p1, p2) =>
            {
                return p1.name.CompareTo(p2.name);
            });

            ctlProject.SourceValueProperty = "project_PK";
            ctlProject.SourceTextExpression = "name";
            ctlProject.FillControl<Project_PK>(items);
        }

        #endregion

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles report generation
        /// </summary>
        /// <param name="sender">Sender button</param>
        /// <param name="e">Sender button arguments</param>
        public void btnCreateReportClick(object sender, EventArgs e)
        {
            if (!SecurityHelper.IsPermittedAny(new List<Permission> { Permission.CreateCustomerTimeReport, Permission.CreateCustomerTimeReportMy })) return;

            int projectID = ValidationHelper.IsValidInt(ctlProject.ControlValue.ToString()) ? Convert.ToInt32(ctlProject.ControlValue) : -1;
            DataSet connectedClients = null;
            if (projectID != -1)
            {
                connectedClients = _timeUnitOperations.GetConnectedClients(projectID);
            }

            if (connectedClients != null && connectedClients.Tables.Count > 0 && connectedClients.Tables[0].Rows.Count > 1)
            {
                if (MasterPage != null)
                {
                    MasterPage.ModalPopup.ShowModalPopup("Error!", "<div style='text-align:center'>Please select a project which is linked to activities related only to products from the same client.<br/><br/></div>");
                }

                return;
            }

            CultureInfo cultureInfo = new CultureInfo("hr-HR");

            int customerID = ValidationHelper.IsValidInt(ctlCustomer.ControlValue.ToString()) ? Convert.ToInt32(ctlCustomer.ControlValue) : -1;
            int personID = ValidationHelper.IsValidInt(ctlPerson.ControlValue.ToString()) ? Convert.ToInt32(ctlPerson.ControlValue) : -1;

            DateTime dateFrom = ValidationHelper.IsValidDateTime(ctlDateFrom.ControlValue.ToString(), cultureInfo) ? Convert.ToDateTime(ctlDateFrom.ControlValue, cultureInfo) : DateTime.Now;
            DateTime dateTo = ValidationHelper.IsValidDateTime(ctlDateTo.ControlValue.ToString(), cultureInfo) ? Convert.ToDateTime(ctlDateTo.ControlValue, cultureInfo) : DateTime.Now;

            rvMain.ServerReport.ReportPath = ConfigurationManager.AppSettings["CustomerTimeReportName"];
            List<ReportParameter> setParameters = new List<ReportParameter>();

            DateTime currentTime = DateTime.Now;
            rvMain.ServerReport.DisplayName = String.Format("{0}{1}{2}CustomerTimeReport", currentTime.Year % 100, currentTime.Month, currentTime.Day);

            setParameters.Add(new ReportParameter("client", customerID.ToString()));
            setParameters.Add(new ReportParameter("person", personID.ToString()));
            setParameters.Add(new ReportParameter("date_from", dateFrom.ToString("dd.MM.yyyy")));
            setParameters.Add(new ReportParameter("date_to", dateTo.ToString("dd.MM.yyyy")));
            setParameters.Add(new ReportParameter("show_auth_numbers", authNumYesBtn.Checked ? "true" : "false"));
            setParameters.Add(new ReportParameter("project_PK", projectID.ToString()));
            setParameters.Add(new ReportParameter("calculateBillingRate", rbBillingRateYes.Checked ? "true" : "false"));

            rvMain.ShowParameterPrompts = false;
            rvMain.ShowToolBar = true;
            rvMain.ShowPageNavigationControls = false;
            rvMain.ShowExportControls = true;
            rvMain.ShowRefreshButton = false;
            rvMain.ShowZoomControl = false;
            rvMain.ShowPrintButton = false;

            rvMain.ServerReport.SetParameters(setParameters);

            rvMain.ServerReport.Refresh();
        }

        /// <summary>
        /// Handles Customer control state initiated by Billing rate checked option
        /// </summary>
        /// <param name="sender">Sender radio button</param>
        /// <param name="e">Sender radio button arguments</param>
        protected void rbBillingRateYes_CheckedChanged(object sender, EventArgs e)
        {
            ctlCustomer.ControlValue = -1;
            ctlCustomer.CurrentControlState = ControlState.YouCantChangeMe;
            // Disable Billing rate calculation for other users if current user has noBillingRate role 
            if (!SecurityHelper.IsPermitted(Permission.CalculateBillingRate) && rbBillingRateYes.Checked)
            {
                disableResponsibleUser();
            }
        }

        /// <summary>
        /// Handles Customer control state initiated by Billing rate checked option
        /// </summary>
        /// <param name="sender">Sender radio button</param>
        /// <param name="e">Sender radio button arguments</param>
        protected void rbBillingRateNo_CheckedChanged(object sender, EventArgs e)
        {
            ctlCustomer.CurrentControlState = ControlState.ReadyForAction;
            if (!SecurityHelper.IsPermitted(Permission.CreateCustomerTimeReport))
            {
                disableResponsibleUser();
            }
            else
            {
                // Disable Billing rate calculation for other users if current user has noBillingRate role 
                if (!SecurityHelper.IsPermitted(Permission.CalculateBillingRate) && !rbBillingRateYes.Checked)
                {
                    disableResponsibleUser();
                }
                else ctlPerson.CurrentControlState = ControlState.ReadyForAction;
            }
        }

        #endregion
    }
}

