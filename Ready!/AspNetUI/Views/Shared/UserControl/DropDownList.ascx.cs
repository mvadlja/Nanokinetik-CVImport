using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class DropDownList : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
    {
        #region Declarations

        public event EventHandler<EventArgs> SelectedIndexChanged;

        #endregion

        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivTextBox
        {
            get { return divDropDownList; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.DropDownList DdlInput
        {
            get { return ddlInput; }
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

        public string Text
        {
            get { return ddlInput.SelectedItem.Text ?? string.Empty; }
            set { if (ddlInput.SelectedItem != null) ddlInput.SelectedItem.Text = value; }
        }

        public ListItem SelectedItem
        {
            get { return ddlInput.SelectedItem; }
        }

        public int SelectedIndex
        {
            get { return ddlInput.SelectedIndex; }
            set { ddlInput.SelectedIndex = value; }
        }

        public object SelectedValue
        {
            get { return ddlInput.SelectedValue; }
            set
            {
                if (ddlInput.Items.FindByValue(Convert.ToString(value)) != null)
                {
                    ddlInput.SelectedValue = Convert.ToString(value);
                }
                else if (ddlInput.Items.FindByValue(string.Empty) != null)
                {
                    ddlInput.SelectedValue = string.Empty;    
                }
            }
        }

        public object SelectedText
        {
            get { return ddlInput.SelectedItem.Text; }
            set
            {
                var selected = ddlInput.Items.FindByText(Convert.ToString(value)) ?? 
                               ddlInput.Items.FindByText(Constant.UnknownValue) ?? 
                               ddlInput.Items.FindByText(string.Empty);

                if (selected != null)
                {
                    ddlInput.SelectedValue = selected.Value;
                }
            }
        }

        public int? SelectedId
        {
            get
            {
                int id;
                if (int.TryParse(ddlInput.Text, out id))
                {
                    return id;
                }
                return null;
            }
            set
            {
                if (ddlInput.Items.FindByValue(Convert.ToString(value)) != null)
                {
                    ddlInput.SelectedValue = Convert.ToString(value);
                }
                else if (ddlInput.Items.FindByValue(string.Empty) != null)
                {
                    ddlInput.SelectedValue = string.Empty;
                }
            }
        }

        public bool Required
        {
            get { return spanRequired.Visible; }
            set { spanRequired.Visible = value; }
        }

        public bool Enabled
        {
            get { return ddlInput.Enabled; }
            set { ddlInput.Enabled = value; }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public Unit TextWidth
        {
            get { return ddlInput.Width; }
            set { ddlInput.Width = value; }
        }

        public bool AutoPostback
        {
            get { return ddlInput.AutoPostBack; }
            set { ddlInput.AutoPostBack = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public int? OldValue
        {
            get { return ViewState["OldValue"] != null && ValidationHelper.IsValidInt(ViewState["OldValue"].ToString()) ? (int?)ViewState["OldValue"] : null; }
            set { ViewState["OldValue"] = value; }
        }

        public bool IsModified
        {
            get { return OldValue != SelectedId; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = SelectedId;
            }
        }

        #endregion

        #region Support methods

        public void Fill<T>(IList<T> source, string sourceTextExpression, string valueProperty, bool displayDefaultValue = true)
        {
            ddlInput.SelectedIndex = -1;
            ddlInput.Items.Clear();

            if (displayDefaultValue == true)
            {
                ddlInput.Items.Insert(0, new ListItem(Constant.ControlDefault.DdlText, Constant.ControlDefault.DdlValue));
            }
            
            if (source == null) return;

            if (!String.IsNullOrEmpty(sourceTextExpression) && !String.IsNullOrEmpty(valueProperty))
            {
                foreach (var item in source)
                {
                    var value = item.GetType().GetProperty(valueProperty).GetValue(item, null);

                    object text = null;
                    if (!sourceTextExpression.Contains("{"))
                    {
                        text = item.GetType().GetProperty(sourceTextExpression).GetValue(item, null);
                    }
                    else
                    {
                        var templateParts = sourceTextExpression.Split(new string[] { "||" }, StringSplitOptions.None);
                        var templateProperties = templateParts[1].Split(new string[] { "," }, StringSplitOptions.None);
                        var template = templateParts[0];

                        for (var i = 0; i < templateProperties.Length; i++)
                        {
                            var tempValue = item.GetType().GetProperty(templateProperties[i]).GetValue(item, null);
                            template = template.Replace("{" + i + "}", (tempValue != null ? tempValue.ToString() : String.Empty));
                        }

                        text = template;
                    }

                    var textString = Convert.ToString(text);

                    if (string.IsNullOrWhiteSpace(textString))
                    {
                        textString = Constant.UnknownValue;
                    }

                    var valueString = Convert.ToString(value);

                    ddlInput.Items.Add(new ListItem(textString, valueString));
                }
            }
            else
            {
                foreach (var item in source)
                {
                    var textString = Convert.ToString(item);

                    if (string.IsNullOrWhiteSpace(textString))
                    {
                        textString = Constant.UnknownValue;
                    }

                    var valueString = Convert.ToString(item);

                    ddlInput.Items.Add(new ListItem(textString, valueString));
                }
            }
        }

        #region List items sorting

        public void SortItemsByText(SortType sortType = SortType.Asc, bool preserveSelection = false, bool displayDefaultValue = true)
        {
            var items = ddlInput.Items;

            var itemsList = new List<ListItem>(items.Count);

            foreach (ListItem item in items)
            {
                item.Selected = preserveSelection;
                if (item.Value != Constant.ControlDefault.DdlValue && item.Text != Constant.ControlDefault.DdlText)
                {
                    itemsList.Add(item);
                }
            }

            if (sortType == SortType.Asc)
            {
                itemsList.Sort((item1, item2) => item1.Text.CompareTo(item2.Text));
            }
            else
            {
                itemsList.Sort((item1, item2) => item1.Text.CompareTo(item2.Text) * (-1));
            }

            ddlInput.Items.Clear();

            if (displayDefaultValue == true)
            {
                ddlInput.Items.Insert(0, new ListItem(Constant.ControlDefault.DdlText, Constant.ControlDefault.DdlValue));
            }

            foreach (var item in itemsList)
            {
                ddlInput.Items.Add(item);
            }
        }

        public void SortItemsByValue(SortType sortType = SortType.Asc, bool preserveSelection = false)
        {
            var items = ddlInput.Items;

            var itemsList = new List<ListItem>(items.Count);

            foreach (ListItem item in items)
            {
                item.Selected = preserveSelection;
                if (item.Value != Constant.ControlDefault.DdlValue && item.Text != Constant.ControlDefault.DdlText)
                {
                    itemsList.Add(item);
                }
            }

            if (sortType == SortType.Asc)
            {
                itemsList.Sort((item1, item2) => item1.Value.CompareTo(item2.Value));
            }
            else
            {
                itemsList.Sort((item1, item2) => item1.Value.CompareTo(item2.Value) * (-1));
            }

            ddlInput.Items.Clear();

            ddlInput.Items.Insert(0, new ListItem(Constant.ControlDefault.DdlText, Constant.ControlDefault.DdlValue));

            foreach (var item in itemsList)
            {
                ddlInput.Items.Add(item);
            }
        }

        #endregion

        #endregion

        protected void ddlInput_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(sender, e);
            }

        }
    }
}