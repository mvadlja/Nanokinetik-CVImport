using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.Net;
using Ready.Model;
using AspNetUIFramework;
using System.Globalization;

namespace AspNetUI.Views.Business
{
    public partial class ActivityTimeReport : System.Web.UI.Page
    {
        IOrganization_PKOperations _organizationOperations;
        IPerson_PKOperations _personOperations;
        
        protected void Page_Init(object sender, EventArgs e)
        {
            _personOperations = new Person_PKDAL();
            _organizationOperations = new Organization_PKDAL();

            ////////// ReportViewer control setup
            if (!IsPostBack)
            {
                rvMain.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerURL"].ToString());
                rvMain.ProcessingMode = ProcessingMode.Remote;
                rvMain.ServerReport.ReportServerCredentials = new CustomReportServerCredentials();
            }

            // Fix
            //if (Request.Browser.Browser == "IE") rvMain.AsyncRendering = true;
            //else rvMain.AsyncRendering = false;
            rvMain.AsyncRendering = false;
            rvMain.SizeToReportContent = true;
            //rvMain.Height = Unit.Percentage(100);
            //rvMain.Width = Unit.Percentage(100);
            DateTime currentTime = DateTime.Now;
            rvMain.ServerReport.DisplayName = String.Format("{0}_{1}_{2}_ActivityTimeReport", currentTime.Year % 100, currentTime.Month, currentTime.Day);
            ////////// END ReportViewer control setup
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindClient();
                BindPerson();

                DateTime to = DateTime.Now;
                DateTime from = new DateTime(to.Year, to.Month, 1);

                ctlDateFrom.ControlValue = from.ToString("dd.MM.yyyy");
                ctlDateTo.ControlValue = to.ToString("dd.MM.yyyy");
            }
        }

        void BindPerson()
        {
            List<Person_PK> items = _personOperations.GetEntities();// 

            items.Sort((c1, c2) =>
            {
                return c1.name.CompareTo(c2.name);
            });

            foreach (Person_PK item in items)
            {
                item.name += " " + item.familyname;
            }


            //items.Insert(0, new Person_PK { person_PK = -1, name = "-- Choose --" });

            ctlPerson.SourceValueProperty = "person_PK";
            ctlPerson.SourceTextExpression = "name";
            ctlPerson.FillControl<Person_PK>(items);

            //ctlPerson.ControlValue = "-1";
        }

        void BindClient()
        {
            List<Organization_PK> items = _organizationOperations.GetEntities();// 

            items.Sort((c1, c2) =>
            {
                return c1.name_org.CompareTo(c2.name_org);
            });
                        

            //items.Insert(0, new Organization_PK { organization_PK = -1, name_org = "-- Choose --" });

            ctlClient.SourceValueProperty = "organization_PK";
            ctlClient.SourceTextExpression = "name_org";
            ctlClient.FillControl<Organization_PK>(items);

            //ctlClient.ControlValue = "-1";
        }

        public void btnCreateClick(object sender, EventArgs e)
        {
            CultureInfo cultureInfo = new CultureInfo("hr-HR");
            
            int clientID = ValidationHelper.IsValidInt(ctlClient.ControlValue.ToString()) ? Convert.ToInt32(ctlClient.ControlValue) : -1;
            int personID = ValidationHelper.IsValidInt(ctlPerson.ControlValue.ToString()) ? Convert.ToInt32(ctlPerson.ControlValue) : -1;
            DateTime dateFrom = ValidationHelper.IsValidDateTime(ctlDateFrom.ControlValue.ToString(), cultureInfo) ? Convert.ToDateTime(ctlDateFrom.ControlValue, cultureInfo) : DateTime.Now;
            DateTime dateTo = ValidationHelper.IsValidDateTime(ctlDateTo.ControlValue.ToString(), cultureInfo) ? Convert.ToDateTime(ctlDateTo.ControlValue, cultureInfo) : DateTime.Now;
            rvMain.ServerReport.ReportPath = "/NanoKinetik/01_activity_time_report";
            List<ReportParameter> setParameters = new List<ReportParameter>();

            DateTime currentTime = DateTime.Now;
            rvMain.ServerReport.DisplayName = String.Format("{0}_{1}_{2}_ActivityTimeReport", currentTime.Year % 100, currentTime.Month, currentTime.Day);

            setParameters.Add(new ReportParameter("client", clientID.ToString()));
            setParameters.Add(new ReportParameter("person", personID.ToString()));
            setParameters.Add(new ReportParameter("date_from", dateFrom.ToString("dd.MM.yyyy")));
            setParameters.Add(new ReportParameter("date_to", dateTo.ToString("dd.MM.yyyy")));
            //setParameters.Add(new ReportParameter("date_from", dateFrom.ToString("MM-dd-yyyy") ));
            //setParameters.Add(new ReportParameter("date_to", dateTo.ToString("MM-dd-yyyy")));

            rvMain.ShowParameterPrompts = false;
            rvMain.ShowToolBar = true;
            rvMain.ShowZoomControl = false;
            rvMain.ShowExportControls = true;
            rvMain.ShowPrintButton = false;

            rvMain.ServerReport.SetParameters(setParameters);

            rvMain.ServerReport.Refresh();
        }
    }
}


[Serializable]
public class CustomReportServerCredentials : IReportServerCredentials
{
    #region IReportServerCredentials Members

    public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string password, out string authority)
    {
        authCookie = null;
        userName = null;
        password = null;
        authority = null;

        return false;
    }

    public System.Security.Principal.WindowsIdentity ImpersonationUser
    {
        get
        {
            return null;
        }
    }

    public System.Net.ICredentials NetworkCredentials
    {
        get
        {
            return new NetworkCredential(ConfigurationManager.AppSettings["ReportServerCredentialsUsername"], ConfigurationManager.AppSettings["ReportServerCredentialsPassword"] /*ConfigurationManager.AppSettings["ReportServerCredentialsDomain"]*/);
        }
    }

    #endregion
}