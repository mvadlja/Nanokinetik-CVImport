using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class ListBoxAu : System.Web.UI.UserControl, IDoubleClick, ILastChange, ICustomEnabled, IArticle57Relevant, IXevprmValidationError
    {
        #region Declarations

        bool _allowDoubleClick;
        public EventHandler<EventArgs> OnAssignClick;
        public EventHandler<EventArgs> OnUnassignClick;

        public EventHandler<FormEventArgs<List<ListItem>>> OnItemsAssigned;
        public EventHandler<FormEventArgs<List<ListItem>>> OnItemsUnAssigned;

        #endregion

        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivListBoxAu
        {
            get { return divListBoxAu; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.ListBox LbInputFrom
        {
            get { return lbInputFrom; }
        }

        public System.Web.UI.WebControls.ListBox LbInputTo
        {
            get { return lbInputTo; }
        }

        public Label LblError
        {
            get { return lblError; }
        }

        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public string TextFrom
        {
            get { return lbInputFrom.Text ?? string.Empty; }
            set { lbInputFrom.Text = value; }
        }

        public string TextTo
        {
            get { return lbInputTo.Text ?? string.Empty; }
            set { lbInputTo.Text = value; }
        }

        public bool Required
        {
            get { return spanRequired.Visible; }
            set { spanRequired.Visible = value; }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public Unit TextFromWidth
        {
            get { return lbInputFrom.Width; }
            set { lbInputFrom.Width = value; }
        }

        public Unit TextToWidth
        {
            get { return lbInputTo.Width; }
            set { lbInputTo.Width = value; }
        }

        public int VisibleRowsFrom
        {
            get { return lbInputFrom.Rows; }
            set { lbInputFrom.Rows = value; }
        }

        public int VisibleRowsTo
        {
            get { return lbInputTo.Rows; }
            set { lbInputTo.Rows = value; }
        }

        public ListSelectionMode SelectionMode
        {
            get { return lbInputFrom.SelectionMode; }
            set
            {
                lbInputFrom.SelectionMode = value;
                lbInputTo.SelectionMode = value;
            }
        }

        public bool AllowDoubleClick
        {
            get
            {
                return lbInputFrom.Attributes["lbAuAllowDblClick"] == "true" ||
                       lbInputTo.Attributes["lbAuAllowDblClick"] == "true";
            }
            set
            {
                if (value)
                {
                    lbInputFrom.Attributes.Add("lbAuAllowDblClick", "true");
                    lbInputTo.Attributes.Add("lbAuAllowDblClick", "true");
                    _allowDoubleClick = true;
                }
                else
                {
                    lbInputFrom.Attributes.Add("lbAuAllowDblClick", "false");
                    lbInputTo.Attributes.Add("lbAuAllowDblClick", "false");
                    _allowDoubleClick = false;
                }
            }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public List<int> OldValue
        {
            get { return ViewState["OldValue"] != null ? (List<int>)ViewState["OldValue"] : null; }
            set { ViewState["OldValue"] = value; }
        }

        public bool IsModified
        {
            get
            {
                if (OldValue == null) return true;
                var oldValuesPk = OldValue;
                var newValuesPk = lbInputFrom.Items.Cast<ListItem>().Select(newValue => newValue != null ? Convert.ToInt32(newValue.Value) : 0).OrderBy(newValue => newValue).ToList();

                return !ListOperations.ListsEquals(oldValuesPk, newValuesPk);
            }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = lbInputFrom.Items.Cast<ListItem>().Select(newValue => newValue != null ? Convert.ToInt32(newValue.Value) : 0).OrderBy(newValue => newValue).ToList();
            }

            HandleDoubleClick();
        }

        #endregion

        private void HandleDoubleClick()
        {
            var postBackControlName = Page.Request.Params["__EVENTTARGET"];
            if (!string.IsNullOrWhiteSpace(postBackControlName) && postBackControlName.Contains("_")) postBackControlName = postBackControlName.Substring(0, postBackControlName.LastIndexOf("_", StringComparison.Ordinal));

            var controlNameClient = ClientID;

            if (postBackControlName != controlNameClient) return;

            // Double Click Event Handlers
            if (_allowDoubleClick &&
                Request.Params["lbInputFromDblClick"] != null &&
                Request.Params["lbInputFromDblClick"].Replace(",", "") == "doubleclicked")
            {
                btnAssign_OnClick(null, null);
            }

            if (_allowDoubleClick &&
                Request.Params["lbInputToDblClick"] != null &&
                Request.Params["lbInputToDblClick"].Replace(",", "") == "doubleclicked")
            {
                btnUnassign_OnClick(null, null);
            }
        }

        #region Support methods

        public void Fill<T>(IList<T> source, System.Web.UI.WebControls.ListBox destionation, string sourceTextExpression, string valueProperty)
        {
            destionation.SelectedIndex = -1;
            destionation.Items.Clear();

            if (source == null) return;

            if (!String.IsNullOrEmpty(sourceTextExpression) && !String.IsNullOrEmpty(valueProperty))
            {
                foreach (var item in source)
                {
                    var value = item.GetType().GetProperty(valueProperty).GetValue(item, null);

                    object text;
                    if (!sourceTextExpression.Contains("{"))
                    {
                        text = item.GetType().GetProperty(sourceTextExpression).GetValue(item, null);
                    }
                    else
                    {
                        var templateParts = sourceTextExpression.Split(new[] { "||" }, StringSplitOptions.None);
                        var templateProperties = templateParts[1].Split(new[] { "," }, StringSplitOptions.None);
                        var template = templateParts[0];

                        for (var i = 0; i < templateProperties.Length; i++)
                        {
                            var tempValue = item.GetType().GetProperty(templateProperties[i]).GetValue(item, null);
                            template = template.Replace("{" + i + "}", tempValue.ToString());
                        }

                        text = template;
                    }

                    var textString = Convert.ToString(text);

                    if (string.IsNullOrWhiteSpace(textString))
                    {
                        textString = Constant.UnknownValue;
                    }

                    var valueString = Convert.ToString(value);

                    destionation.Items.Add(new ListItem(textString, valueString));
                }
            }
            else
            {
                foreach (var item in source)
                {
                    destionation.Items.Add(new ListItem(Convert.ToString(item), Convert.ToString(item)));
                }
            }
        }

        public void Clear()
        {
            lbInputFrom.Items.Clear();
            lbInputTo.Items.Clear();
        }

        #region Event handlers

        public void btnAssign_OnClick(object sender, EventArgs e)
        {
            var selectedItemList = lbInputFrom.GetSelectedItems();

            if (OnAssignClick != null) OnAssignClick(sender, e);
            else
            {
                lbInputFrom.MoveSelectedItemsTo(lbInputTo);
                lbInputTo.SortItemsByText();

                if (OnItemsAssigned != null)
                {
                    OnItemsAssigned(sender, new FormEventArgs<List<ListItem>>(selectedItemList));
                }
            }
        }

        public void btnUnassign_OnClick(object sender, EventArgs e)
        {
            var selectedItemList = lbInputTo.GetSelectedItems();

            if (OnUnassignClick != null) OnUnassignClick(sender, e);
            else
            {
                lbInputTo.MoveSelectedItemsTo(lbInputFrom);
                lbInputFrom.SortItemsByText();

                if (OnItemsUnAssigned != null)
                {
                    OnItemsUnAssigned(sender, new FormEventArgs<List<ListItem>>(selectedItemList));
                }
            }
        }

        #endregion

        #endregion

        #region ICustomEnabled

        public void Enable(List<WebControl> controlList = null)
        {
            if (controlList != null && controlList.Any()) controlList.ForEach(c => c.Enabled = true);
            else
        {
            btnAssign.Enabled = true;
            btnUnassign.Enabled = true;
        }
        }

        public void Disable(List<WebControl> controlList = null)
        {
            if (controlList != null && controlList.Any()) controlList.ForEach(c => c.Enabled = false);
            else
        {
            btnAssign.Enabled = false;
            btnUnassign.Enabled = false;

            StyleHelper.Disable(btnAssign);
            StyleHelper.Disable(btnUnassign);
        }
        }

        #endregion
    }
}