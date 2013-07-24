using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Support
{
    public partial class SearcherDisplay : UserControl
    {
        public event EventHandler<EventArgs> OnSearchClick;
        public event EventHandler<EventArgs> OnRemoveClick;

        private string _label;
        private bool _visibleSearcherData;


        public bool VisibleSearcherData
        {
            get { return _visibleSearcherData; }
            set { _visibleSearcherData = value; SetVisibleSearcherDataView(value); }
        }

        public object SelectedObject
        {
            get { return ViewState["Searcher_SelectedObject_" + this.ID]; }
            protected set { ViewState["Searcher_SelectedObject_" + this.ID] = value; }
        }

        // added for Input confirmation
        public string PrintableLabelValue
        {
            get { return _label; }
            set { _label = value; }
        }
        // added for Input confirmation
        public string PrintableTextValue
        {
            get { return txtSearcherData.Text; }
        }

        public string ControlDescriptor
        {
            get { return txtSearcherData.ToolTip; }
            set
            {
                txtSearcherData.ToolTip = value;

                if (String.IsNullOrEmpty(value)) txtSearcherData.Visible = false;
                else txtSearcherData.Visible = true;
            }
        }

        public void HighlightTextbox()
        {
            txtSearcherData.Attributes.Add("style", "border-color:red");
        }

        public void SetSelectedObject(object selectedObject, string objectDisplayData)
        {
            SelectedObject = selectedObject;
            txtSearcherData.Text = objectDisplayData;
        }

        public void ClearSelectedObject()
        {
            SelectedObject = null;
            txtSearcherData.Text = String.Empty;

            if (OnRemoveClick != null)
                OnRemoveClick(this, new EventArgs());
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            if (OnSearchClick != null) OnSearchClick(this, e);
        }

        protected void lbtClear_Click(object sender, EventArgs e)
        {
            ClearSelectedObject();
        }

        public void EnableSearcher(bool enable)
        {
            lbtClear.Enabled = enable;
            lbtSearch.Enabled = enable;
        }

        public void SetVisibleSearcherDataView(bool isVisible)
        {
            txtSearcherData.Visible = isVisible;
            lbtClear.Visible = isVisible;

            if (isVisible)
            {
                lbtSearch.Text = "Select";
            }
            else
            {
                lbtSearch.Text = "Add";
            }
        }
    }
}