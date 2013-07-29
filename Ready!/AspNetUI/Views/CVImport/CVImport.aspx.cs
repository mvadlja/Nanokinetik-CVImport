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
    public partial class CVImport : System.Web.UI.Page
    {
        private class TempData
        {
            public int ID { get; set; }
            public string CurrentValue { get; set; }
            public string EmaValue { get; set; }
            public string EVCode { get; set; }
            public string Action { get; set; }
        }


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

        private void BindGrid()
        {
            List<TempData> data = new List<TempData>
            {
                new TempData{ID=1, CurrentValue="", EmaValue="Value1", EVCode="1", Action="Insert"},
                new TempData{ID=2, CurrentValue="", EmaValue="Value2", EVCode="2", Action="Insert"},
                new TempData{ID=3, CurrentValue="Value3", EmaValue="Value3--", EVCode="3", Action="Update"},
                new TempData{ID=4, CurrentValue="Value4", EmaValue="Value4--", EVCode="4", Action="Update"},
                new TempData{ID=5, CurrentValue="Value5", EmaValue="", EVCode="5", Action="Delete"},
                new TempData{ID=6, CurrentValue="Value5", EmaValue="", EVCode="5", Action="Delete"}
            };

            APGrid.DataSource = data;
            APGrid.DataBind();
            FillComboArticle57Relevant();

        }

        private void FillComboArticle57Relevant()
        {
            var actionList = new List<ListItem>
                                            {
                                                new ListItem(""), 
                                                new ListItem("Insert"), 
                                                new ListItem("Update"), 
                                                new ListItem("Delete")
                                            };
            APGrid.SetComboList("Action", actionList);
        }

        #endregion

        #region Event handlers
        public void btnImport_OnClick(object sender, EventArgs e)
        {
            ImportExcelData();
        }

        public void btnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ActivityView/List.aspx?EntityContext=Activity");
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                int id = Convert.ToInt32(checkBox.Attributes["ItemID"]);

                string nesto = "";
            }
        }
        #endregion

        #region Excel Import

        private DataSet ImportExcelData()
        {
            DataSet ds = null;
            if (fileUpload.HasFile)
            {
                List<CVImportData> data = ReadExcel();

                if (data.Count > 0)
                {
                    string serializedData = SerializeCVImportData(data);
                    string cvName = ddlCVType.SelectedValue.ToString();

                    CVImportDAL cvImportDAL = new CVImportDAL();

                    ds = cvImportDAL.ImportData(serializedData, cvName);
                }
            }
            
            return ds;
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

                    if (readStarted)
                    {
                        if (!String.IsNullOrEmpty(column1Value))
                        {
                            list.Add(new CVImportData { Code = column1Value, Value = column2Value });
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
                XMLSerializationHelper.AppendIfNotNull(sb, "Value", dataItem.Value);
                sb.Append("/>\n");
            }
            sb.Append("</Data>\n");

            return sb.ToString();
        }


        #endregion
    }
}