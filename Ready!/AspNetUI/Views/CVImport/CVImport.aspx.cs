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
using AspNetUI.Model.CVImport;
using SpreadsheetLight;
using System.Text;
using System.Web.UI;
using Ready.Model.Business;

namespace AspNetUI.Views.CVImport
{
    public partial class CVImport : FormPage
    {
        [Serializable]
        private class CheckedData
        {
            public int? ID { get; set; }
            public string EVCode { get; set; }
            public string Value { get; set; }
            public string Action { get; set; }
        }

        private bool IsGridShown
        {
            get { return pnlGrid.Visible; }
        }

        private List<CheckedData> ListCheckedData
        {
            get { return (List<CheckedData>)ViewState["ListCheckedData"]; }
            set { ViewState["ListCheckedData"] = value; }
        }


        #region Page events
        protected void Page_Init(object sender, EventArgs e)
        {
            grid.OnPageChanged += grid_OnPageChanged;
            grid.OnPerPageChanged += grid_OnPerPageChanged;
            grid.OnFilterChanged += grid_OnFilterChanged;

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
                //new ListItem("Anatomical Therapeutic Chemical(ATC)", "ATC"),
                new ListItem("Authorisation Procedure", "AP"),
                new ListItem("Authorisation Status", "AS"),
                //new ListItem("Concentration Types", "CT"),
                new ListItem("Medical Devices", "MD"),
                new ListItem("Pharmaceutical Dose Forms", "PDF"),
                new ListItem("Routes of Administration", "RA"),
                new ListItem("Substances", "SB"),
                //new ListItem("Units of Measurement", "UM"),
                //new ListItem("Units of Presentation", "UP"),
            };

            ddlCVType.Fill(list, "Text", "Value");
            ddlCVType.SortItemsByText();
        }

        private void BindGrid()
        {
            CVImportDAL cvImportDAL = new CVImportDAL();

            var filters = grid.GetFilters();

            string actionName = filters["Action"];

            int totalRecordsCount = 0;
            DataSet data = cvImportDAL.GetComparedData(
                                SessionManager.Instance.CurrentUser.UserID,
                                actionName,
                                grid.CurrentPage,
                                grid.PageSize,
                                out totalRecordsCount);

            grid.TotalRecords = totalRecordsCount;

            grid.DataSource = data;
            grid.DataBind();
            FillActionFilter();

        }

        private void FillActionFilter()
        {
            var actionList = new List<ListItem>
                                            {
                                                new ListItem(""), 
                                                new ListItem("Insert"), 
                                                new ListItem("Update"), 
                                                new ListItem("Delete"), 
                                                //new ListItem("No Action")
                                            };
            grid.SetComboList("Action", actionList);
        }

        #endregion

        #region Event handlers
        public void btnImport_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ImportExcelData())
                {
                    BindGrid();

                    pnlImport.Visible = false;
                    pnlGrid.Visible = true;

                    btnImport.Visible = false;
                    btnNext.Visible = true;

                    h1CVTitle.InnerText = ddlCVType.SelectedText.ToString();
                }
                else
                {
                    MasterPage.ModalPopup.ShowModalPopup("Error!", "Import unsuccessful.");
                }
            }
            catch (System.IO.FileFormatException ex)
            {
                MasterPage.ModalPopup.ShowModalPopup("File import error!", "Unable to read file. Try to save file as .xlsx and try again");
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ListCheckedData != null &&
               ListCheckedData.Count > 0)
            {
                int insertedRecords = ListCheckedData.FindAll(x => x.Action == "Insert").Count;
                int updatedRecords = ListCheckedData.FindAll(x => x.Action == "Update").Count;
                int deletedRecords = ListCheckedData.FindAll(x => x.Action == "Delete").Count;

                lblInsert.Text = insertedRecords.ToString();
                lblUpdate.Text = updatedRecords.ToString();
                lblDelete.Text = deletedRecords.ToString();

                pnlImport.Visible = false;
                pnlGrid.Visible = false;
                pnlSummary.Visible = true;

                btnApply.Visible = true;
                btnNext.Visible = false;
            }
            else
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", "Please choose at least on item.");
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (ApplyImport())
            {
                Response.Redirect("~/Views/CVImport/CVImport.aspx");
            }
            else
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", "An error occured on applying the import.");
            }
        }


        public void btnCancel_OnClick(object sender, EventArgs e)
        {
            if (btnImport.Visible == true)
            {
                Response.Redirect("~/Views/ActivityView/List.aspx?EntityContext=Activity");
            }
            else if (btnNext.Visible == true)
            {
                Response.Redirect("~/Views/CVImport/CVImport.aspx");
            }
            else if (btnApply.Visible == true)
            {
                btnNext.Visible = true;
                btnApply.Visible = false;

                pnlGrid.Visible = true;
                pnlSummary.Visible = false;
            }
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                int? id = checkBox.Attributes["ItemID"] == null ||
                          checkBox.Attributes["ItemID"] == "" ?
                            null :
                            (int?)Convert.ToInt32(checkBox.Attributes["ItemID"]);
                string evCode = checkBox.Attributes["EVCode"];
                string emaValue = checkBox.Attributes["EmaValue"];
                string action = checkBox.Attributes["Action"];

                CheckedData checkedData = new CheckedData { ID = id, EVCode = evCode, Value = emaValue, Action = action };

                if (ListCheckedData == null)
                {
                    ListCheckedData = new List<CheckedData>();
                }

                if (checkBox.Checked)
                {
                    ListCheckedData.Add(checkedData);
                }
                else
                {
                    CheckedData data;
                    if (id != null)
                    {
                        data = ListCheckedData.Find(x => x.ID == id);
                    }
                    else
                    {
                        data = ListCheckedData.Find(x => x.EVCode == evCode && x.Value == emaValue);
                    }

                    if (data != null)
                    {
                        ListCheckedData.Remove(data);
                    }
                }
            }
        }

        protected void grid_OnPageChanged(object sender, int newPage)
        {
            grid.CurrentPage = newPage;
            BindGrid();
        }

        protected void grid_OnPerPageChanged(int newValue)
        {
            grid.PageSize = newValue;
            BindGrid();
        }

        void grid_OnFilterChanged(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox checkBox = e.Row.Cells[0].FindControl("chkAction") as CheckBox;

                if (checkBox != null && ListCheckedData != null)
                {
                    int? id = checkBox.Attributes["ItemID"] == null ||
                              checkBox.Attributes["ItemID"] == "" ?
                                null :
                                (int?)Convert.ToInt32(checkBox.Attributes["ItemID"]);
                    string evCode = checkBox.Attributes["EVCode"];
                    string emaValue = checkBox.Attributes["EmaValue"];
                    string action = checkBox.Attributes["Action"];

                    if (action == "No action")
                    {
                        checkBox.Visible = false;
                    }
                    else
                    {
                        CheckedData data;
                        if (id != null)
                        {
                            data = ListCheckedData.Find(x => x.ID == id);
                        }
                        else
                        {
                            data = ListCheckedData.Find(x => x.EVCode == evCode && x.Value == emaValue);
                        }

                        if (data != null)
                        {
                            checkBox.Checked = true;
                        }
                    }
                }

            }
        }

        #endregion

        #region Excel Import

        private bool ImportExcelData()
        {
            bool isSuccessful = false;

            if (fileUpload.HasFile)
            {
                List<CVImportData> data = ReadExcel();

                if (data.Count > 0)
                {
                    string serializedData = SerializeCVImportData(data);
                    string cvName = ddlCVType.SelectedValue.ToString();

                    CVImportDAL cvImportDAL = new CVImportDAL();

                    isSuccessful = cvImportDAL.ImportInitialData(serializedData, cvName, SessionManager.Instance.CurrentUser.UserID);
                }
            }

            return isSuccessful;
        }

        private List<CVImportData> ReadExcel()
        {
            List<CVImportData> list = new List<CVImportData>();

            Stream stream = fileUpload.PostedFile.InputStream;

            using (SLDocument doc = new SLDocument(stream))
            {
                int rowIndex = 1;
                bool readStarted = false;

                while (true)
                {
                    string column1Value = doc.GetCellValueAsString(rowIndex, 1);
                    string column2Value = doc.GetCellValueAsString(rowIndex, 2);
                    string column3Value = doc.GetCellValueAsString(rowIndex, 3);
                    string column4Value = doc.GetCellValueAsString(rowIndex, 4);

                    if (readStarted)
                    {
                        if (!String.IsNullOrEmpty(column1Value))
                        {
                            list.Add(new CVImportData
                                        {
                                            Code = column1Value,
                                            Value_1 = column2Value,
                                            Value_2 = column3Value,
                                            Value_3 = column4Value
                                        });
                        }
                        else
                        {
                            break;
                        }
                    }


                    if (column1Value == "xEVMPD Code" &&
                        !readStarted)
                    {
                        readStarted = true;
                    }


                    if (rowIndex > 20 && !readStarted)
                    {
                        break;
                    }

                    rowIndex++;
                }
            }


            return list;
        }

        private string SerializeCVImportData(List<CVImportData> data)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Data>\n");

            foreach (CVImportData dataItem in data)
            {
                sb.Append("\t<Item ");
                XMLSerializationHelper.AppendIfNotNull(sb, "Code", dataItem.Code);
                XMLSerializationHelper.AppendIfNotNull(sb, "Value_1", dataItem.Value_1);
                XMLSerializationHelper.AppendIfNotNull(sb, "Value_2", dataItem.Value_2);
                XMLSerializationHelper.AppendIfNotNull(sb, "Value_3", dataItem.Value_3);
                sb.Append("/>\n");
            }
            sb.Append("</Data>\n");

            return sb.ToString();
        }


        #endregion

        #region Final import


        private bool ApplyImport()
        {
            bool isSuccessful = false;

            if (ListCheckedData != null ||
                ListCheckedData.Count > 0)
            {
                string serializedCheckedData = SerializeCheckedData(ListCheckedData);
                string cvName = ddlCVType.SelectedValue.ToString();

                CVImportDAL cvimportDAL = new CVImportDAL();

                isSuccessful = cvimportDAL.ApplyImportData(serializedCheckedData, cvName, SessionManager.Instance.CurrentUser.UserID);
            }

            return isSuccessful;
        }

        private string SerializeCheckedData(List<CheckedData> ListCheckedData)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Data>");

            foreach (CheckedData dataItem in ListCheckedData)
            {
                sb.Append("\t<Item ");
                XMLSerializationHelper.AppendIfNotNull(sb, "ID", dataItem.ID);
                XMLSerializationHelper.AppendIfNotNull(sb, "EVCode", dataItem.EVCode);
                XMLSerializationHelper.AppendIfNotNull(sb, "Value", dataItem.Value);
                sb.Append("/>\n");
            }

            sb.Append("</Data>");

            return sb.ToString();
        }

        #endregion


    }
}