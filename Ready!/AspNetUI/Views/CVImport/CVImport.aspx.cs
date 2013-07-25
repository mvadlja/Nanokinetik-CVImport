using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using AspNetUI.Model.XlsImport;
using SpreadsheetLight;
using System.Text;
using System.Web.UI;
using Ready.Model.Business;

namespace AspNetUI.Views.CVImport
{
    public partial class CVImport : System.Web.UI.Page
    {
        #region Page events
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdlCVType();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpdatePanel updatePanel = Page.Master.FindControl("upCommon") as UpdatePanel;
            UpdatePanelControlTrigger trigger = new PostBackTrigger();
            trigger.ControlID = btnImport.UniqueID;
            updatePanel.Triggers.Add(trigger);
        }

        #endregion

        #region Binders
        private void BindDdlCVType()
        {
            var list = new List<ListItem>
            {
                new ListItem("Authorisation procedure", "AP"),
                new ListItem("Authorisation status", "AS"),
            };

            ddlCVType.Fill(list, "Text", "Value");
            ddlCVType.SortItemsByText();
        }

        #endregion

        #region Event handlers
        public void btnImport_OnClick(object sender, EventArgs e)
        {

        }

        public void btnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ActivityView/List.aspx?EntityContext=Activity");
        }

        protected void ddlCVType_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnDdlCVTypeChange();
        }

        private void OnDdlCVTypeChange()
        {
            if (ddlCVType.SelectedValue != null ||
                !String.IsNullOrEmpty(ddlCVType.SelectedValue.ToString()))
            {
                fileUpload.Enabled = true;
            }
            else
            {
                fileUpload.Enabled = false;
            }
        }
        #endregion
    }
}