using System;
using System.Collections.Generic;
using System.Configuration;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using Microsoft.Reporting.WebForms;
using System.Globalization;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.Business
{
    public partial class RegulatoryActivityReport : DefaultPage
    {
        IOrganization_PKOperations _organizationOperations;
        IType_PKOperations _typeOperations;
        ICountry_PKOperations _countryOperations;
        IProduct_PKOperations _productOperations;
        ISubstance_PKOperations _substanceOperations;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!SecurityHelper.IsPermitted(Permission.CreateRegulatoryTimeReport)) btnCreate.Disable();

            _organizationOperations = new Organization_PKDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _productOperations = new Product_PKDAL();
            _substanceOperations = new Substance_PKDAL();

            ctlProducts.OnListItemSelected += ctlProducts_OnListItemSelected;
            ctlProductSearcherDisplay.OnSearchClick += ctlProductSearcherDisplay_OnSearchClick;
            ctlProductSearcherDisplay.OnRemoveClick += ctlProductSearcherDisplay_OnRemoveClick;

            actIngrSearcher.OnListItemSelected += actIngrSearcher_OnListItemSelected;
            actIngrSearcherDisplay1.OnSearchClick += actIngrSearcherDisplay1_OnSearchClick;
            actIngrSearcherDisplay1.OnRemoveClick += actIngrSearcherDisplay1_OnRemoveClick;
            ////////// ReportViewer control setup
            if (!IsPostBack)
            {
                rvMain.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerURL"]);
                rvMain.ProcessingMode = ProcessingMode.Remote;
                rvMain.ServerReport.ReportServerCredentials = new CustomReportServerCredentials();
            }

            // Fix
            //if (Request.Browser.Browser == "IE") rvMain.AsyncRendering = true;
            //else rvMain.AsyncRendering = false;
            rvMain.AsyncRendering = false;

            rvMain.SizeToReportContent = true;
            DateTime currentTime = DateTime.Now;
            rvMain.ServerReport.DisplayName = (Uri.EscapeDataString(String.Format("{0}{1}{2}RegulatoryActivityReport", currentTime.Year % 100, currentTime.Month, currentTime.Day)));
            
            //rvMain.Height = Unit.Percentage(100);
            //rvMain.Width = Unit.Percentage(100);
            ////////// END ReportViewer control setup
        }

        void actIngrSearcherDisplay1_OnRemoveClick(object sender, EventArgs e)
        {
            actIngrSearcherDisplay1.EnableSearcher(true);
        }

        void actIngrSearcherDisplay1_OnSearchClick(object sender, EventArgs e)
        {
            actIngrSearcher.ShowModalSearcher("SubName");
        }

        void actIngrSearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
                Substance_PK sub = _substanceOperations.GetEntity(e.DataItemID);
                if (sub != null)
                    actIngrSearcherDisplay1.SetSelectedObject(sub.substance_PK, sub.substance_name);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                BindClient();
                BindRegulatoryStatus();
                BindCountry();
                BindType();
                BindSubstance();

                //DateTime to = DateTime.Now;
                //DateTime from = new DateTime(to.Year, to.Month, 1);

                //ctlSubmissionDateFrom.ControlValue = from.ToString("dd.MM.yyyy");
                //ctlSubmissionDateTo.ControlValue = to.ToString("dd.MM.yyyy");
                //ctlApprovalDateFrom.ControlValue = from.ToString("dd.MM.yyyy");
                //ctlApprovalDateTo.ControlValue = to.ToString("dd.MM.yyyy");
            }
        }

        void ctlProducts_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Product_PK product = _productOperations.GetEntity(e.DataItemID);

            if (product != null)
                ctlProductSearcherDisplay.SetSelectedObject(product.product_PK, product.name);
        }

        void ctlProductSearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        {
            ctlProductSearcherDisplay.EnableSearcher(true);
        }

        void ctlProductSearcherDisplay_OnSearchClick(object sender, EventArgs e)
        {
            ctlProducts.ShowModalSearcher("Products");
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

            if (ctlClient.ControlBoundItems.Count > 0)
            {
                ctlClient.ControlBoundItems.RemoveAt(0);
            }

            //ctlClient.ControlValue = "-1";
        }

        void BindRegulatoryStatus()
        {
            List<Type_PK> items = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.RegulatoryStatus);

            items.Sort((c1, c2) =>
            {
                return c1.name.CompareTo(c2.name);
            });

            ctlRegulatoryStatus.SourceValueProperty = "type_PK";
            ctlRegulatoryStatus.SourceTextExpression = "name";
            ctlRegulatoryStatus.FillControl(items);
        }


        void BindCountry()
        {
            List<Country_PK> countries = _countryOperations.GetEntities();

            countries.Sort((c1, c2) =>
            {
                return c1.abbreviation.CompareTo(c2.abbreviation);
            });

            ctlCountry.SourceValueProperty = "abbreviation";
            ctlCountry.SourceTextExpression = "abbreviation";
            ctlCountry.FillControl<Country_PK>(countries);
        }

        void BindSubstance()
        {
            //List<Substance_PK> substances = _substanceOperations.GetEntities();

            //substances.Sort((c1, c2) =>
            //{
            //    return c1.substance_name.CompareTo(c2.substance_name);
            //});

            //ctlActiveIngredient.SourceValueProperty = "substance_PK";
            //ctlActiveIngredient.SourceTextExpression = "substance_name";
            //ctlActiveIngredient.FillControl<Substance_PK>(substances);
        }

        void BindType()
        {
            List<Type_PK> items = _typeOperations.GetTypesForDDL("A");

            items.Sort((c1, c2) =>
            {
                return c1.name.CompareTo(c2.name);
            });

            ctlActivityType.SourceValueProperty = "name";
            ctlActivityType.SourceTextExpression = "name";
            ctlActivityType.FillControl<Type_PK>(items);
        }


        public void btnCreateClick(object sender, EventArgs e)
        {
            if (!SecurityHelper.IsPermitted(Permission.CreateRegulatoryTimeReport)) return;

            CultureInfo cultureInfo = new CultureInfo("hr-HR");

            int clientPK = ValidationHelper.IsValidInt(ctlClient.ControlValue.ToString()) ? Convert.ToInt32(ctlClient.ControlValue) : -1;
            int productFK = ctlProductSearcherDisplay.SelectedObject != null ? Convert.ToInt32(ctlProductSearcherDisplay.SelectedObject) : -1;
            //int countryPK = ValidationHelper.IsValidInt(ctlCountry.ControlValue.ToString()) ? Convert.ToInt32(ctlCountry.ControlValue) : -1;
            string country = String.IsNullOrEmpty(ctlCountry.ControlValue.ToString()) ? "All" : ctlCountry.ControlValue.ToString();
            string type = String.IsNullOrEmpty(ctlActivityType.ControlValue.ToString()) ? "All" : ctlActivityType.ControlValue.ToString();
            int regulatoryStatusPK = ValidationHelper.IsValidInt(ctlRegulatoryStatus.ControlValue.ToString()) ? Convert.ToInt32(ctlRegulatoryStatus.ControlValue) : -1;
            //int typePK = ValidationHelper.IsValidInt(ctlActivityType.ControlValue.ToString()) ? Convert.ToInt32(ctlActivityType.ControlValue) : -1;
            int substancePK = actIngrSearcherDisplay1.SelectedObject != null && ValidationHelper.IsValidInt(actIngrSearcherDisplay1.SelectedObject.ToString()) ? Convert.ToInt32(actIngrSearcherDisplay1.SelectedObject) : -1;

            DateTime? submissionDateFrom = ValidationHelper.IsValidDateTime(ctlSubmissionDateFrom.ControlValue.ToString(), cultureInfo) ? (DateTime?)Convert.ToDateTime(ctlSubmissionDateFrom.ControlValue, cultureInfo) : null;
            DateTime? submissionDateTo = ValidationHelper.IsValidDateTime(ctlSubmissionDateTo.ControlValue.ToString(), cultureInfo) ? (DateTime?)Convert.ToDateTime(ctlSubmissionDateTo.ControlValue, cultureInfo) : null;
            DateTime? approvalDateFrom = ValidationHelper.IsValidDateTime(ctlApprovalDateFrom.ControlValue.ToString(), cultureInfo) ? (DateTime?)Convert.ToDateTime(ctlApprovalDateFrom.ControlValue, cultureInfo) : null;
            DateTime? approvalDateTo = ValidationHelper.IsValidDateTime(ctlApprovalDateTo.ControlValue.ToString(), cultureInfo) ? (DateTime?)Convert.ToDateTime(ctlApprovalDateTo.ControlValue, cultureInfo) : null;

            //rvMain.ServerReport.ReportPath = "/NanoKinetik/03_regulatory_activity_report";
            rvMain.ServerReport.ReportPath = ConfigurationManager.AppSettings["RegulatoryActivityReportName"];
            List<ReportParameter> setParameters = new List<ReportParameter>();

            //foreach(ReportParameterInfo rpi in rvMain.ServerReport.GetParameters())
            //{
            //}

            setParameters.Add(new ReportParameter("organization_pk", clientPK.ToString()));
            setParameters.Add(new ReportParameter("product_PK", productFK.ToString()));
            setParameters.Add(new ReportParameter("country", country.ToString()));
            setParameters.Add(new ReportParameter("regulatoryStatus", regulatoryStatusPK.ToString()));
            setParameters.Add(new ReportParameter("type", type.ToString()));
            setParameters.Add(new ReportParameter("substance", substancePK.ToString()));

            setParameters.Add(new ReportParameter("submission_date_from", submissionDateFrom.HasValue ? ((DateTime)submissionDateFrom).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("submission_date_to", submissionDateTo.HasValue ? ((DateTime)submissionDateTo).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("approval_date_from", approvalDateFrom.HasValue ? ((DateTime)approvalDateFrom).ToString("dd.MM.yyyy") : null));
            setParameters.Add(new ReportParameter("approval_date_to", approvalDateTo.HasValue ? ((DateTime)approvalDateTo).ToString("dd.MM.yyyy") : null));

            rvMain.ShowParameterPrompts = false;
            rvMain.ShowToolBar = true;
            rvMain.ShowPrintButton = false;
            rvMain.ShowZoomControl = false;
            rvMain.ShowExportControls = true;
            rvMain.ShowPrintButton = false;

            rvMain.ServerReport.SetParameters(setParameters);
            rvMain.ServerReport.Refresh();
        }
    }
}


