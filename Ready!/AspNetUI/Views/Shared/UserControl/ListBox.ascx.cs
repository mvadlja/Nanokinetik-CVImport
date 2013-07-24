using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;
using System.Linq;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class ListBox : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivListBox
        {
            get { return divListBox; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.ListBox LbInput
        {
            get { return lbInput; }
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
            get { return lbInput.Text ?? string.Empty; }
            set { lbInput.Text = value; }
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

        public Unit TextWidth
        {
            get { return lbInput.Width; }
            set { lbInput.Width = value; }
        }

        public int VisibleRows
        {
            get { return lbInput.Rows; }
            set { lbInput.Rows = value; }
        }

        public Unit Height
        {
            get { return lbInput.Height; }
            set { lbInput.Height = value; }
        }

        public ListSelectionMode SelectionMode
        {
            get { return lbInput.SelectionMode; }
            set { lbInput.SelectionMode = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public List<int> OldValue
        {
            get { return ViewState["OldValue"] != null ? (List<int>)ViewState["OldValue"] : null; }
            set
            {
                ViewState["OldValue"] = value;
            }
        }

        public List<string> OldValueString
        {
            get { return ViewState["OldValueString"] != null ? (List<string>)ViewState["OldValueString"] : null; }
            set
            {
                ViewState["OldValueString"] = value;
            }
        }

        public bool IsModified
        {
            get
            {
                if (OldValue == null) return true;
                var oldValuesPk = OldValue;
                var newValuesPk = lbInput.Items.Cast<ListItem>().Where(newValue => newValue.Selected).Select(newValue => ValidationHelper.IsValidInt(newValue.Value) ? Convert.ToInt32(newValue.Value) : 0).OrderBy(newValue => newValue).ToList();
                var oldValuesString = OldValueString;
                var newValuesString = lbInput.Items.Cast<ListItem>().Where(newValue => newValue.Selected).Select(newValue => newValue.Text).OrderBy(newValue => newValue).ToList();

                return !ListOperations.ListsEquals(oldValuesPk, newValuesPk) || !ListOperations.ListsEquals(oldValuesString, newValuesString);
            }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = lbInput.Items.Cast<ListItem>().Where(newValue => newValue.Selected).Select(newValue => ValidationHelper.IsValidInt(newValue.Value) ? Convert.ToInt32(newValue.Value) : 0).OrderBy(newValue => newValue).ToList();
                OldValueString = lbInput.Items.Cast<ListItem>().Where(newValue => newValue.Selected).Select(newValue => newValue.Text).OrderBy(newValue => newValue).ToList();
            }
        }

        #endregion

        #region Support methods

        public void Fill<T>(IList<T> source, string sourceTextExpression, string valueProperty)
        {
            lbInput.SelectedIndex = -1;
            lbInput.Items.Clear();

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
                        var templateParts = sourceTextExpression.Split(new [] { "||" }, StringSplitOptions.None);
                        var templateProperties = templateParts[1].Split(new [] { "," }, StringSplitOptions.None);
                        var template = templateParts[0];

                        for (var i = 0; i < templateProperties.Length; i++)
                        {
                            var tempValue = item.GetType().GetProperty(templateProperties[i]).GetValue(item, null);
                            template = template.Replace("{" + i + "}", Convert.ToString(tempValue));
                        }

                        text = template;
                    }

                    var textString = Convert.ToString(text);

                    if (string.IsNullOrWhiteSpace(textString))
                    {
                        textString = Constant.UnknownValue;
                    }

                    var valueString = Convert.ToString(value);

                    lbInput.Items.Add(new ListItem(textString, valueString));
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

                    lbInput.Items.Add(new ListItem(textString, valueString));
                }
            }
        }

        public void Clear()
        {
            lbInput.Items.Clear();
        }

        #region Selected items operations

        public void MoveSelectedItemsTo(System.Web.UI.WebControls.ListBox destinationListBox)
        {
            var itemsToMove = new List<ListItem>();
            for (int i = 0; i < lbInput.Items.Count; i++)
            {
                if (lbInput.Items[i].Selected)
                {
                    itemsToMove.Add(lbInput.Items[i]);
                }
            }

            foreach (var listItem in itemsToMove)
            {
                lbInput.Items.Remove(listItem);
                destinationListBox.Items.Add(listItem);
            }
        }

        public void CopySelectedItemsTo(System.Web.UI.WebControls.ListBox destinationListBox)
        {
            for (int i = 0; i < lbInput.Items.Count; i++)
            {
                if (lbInput.Items[i].Selected)
                {
                    destinationListBox.Items.Add(lbInput.Items[i]);
                }
            }
        }

        public void UnselectAll()
        {
            for (var i = 0; i < lbInput.Items.Count; i++)
            {
                lbInput.Items[i].Selected = false;
            }
        }

        public List<string> GetSelectedValues()
        {
            var selectedValues = new List<string>();
            for (var i = 0; i < lbInput.Items.Count; i++)
            {
                if (lbInput.Items[i].Selected)
                {
                    selectedValues.Add(lbInput.Items[i].Value);
                }
            }
            return selectedValues;
        }

        public List<ListItem> GetSelectedItems()
        {
            var selectedItems = new List<ListItem>();
            for (var i = 0; i < lbInput.Items.Count; i++)
            {
                if (lbInput.Items[i].Selected)
                {
                    selectedItems.Add(lbInput.Items[i]);
                }
            }
            return selectedItems;
        }

        public string GetFirstSelectedValue()
        {
            for (var i = 0; i < lbInput.Items.Count; i++)
            {
                if (lbInput.Items[i].Selected)
                {
                    return lbInput.Items[i].Value;
                }
            }
            return null;
        }

        public int? GetFirstSelectedId()
        {
            for (var i = 0; i < lbInput.Items.Count; i++)
            {
                if (!lbInput.Items[i].Selected) continue;
                if (ValidationHelper.IsValidInt(lbInput.Items[i].Value))
                    return int.Parse(lbInput.Items[i].Value);
            }
            return null;
        }

        public ListItem GetFirstSelectedItem()
        {
            for (var i = 0; i < lbInput.Items.Count; i++)
            {
                if (lbInput.Items[i].Selected)
                {
                    return lbInput.Items[i];
                }
            }
            return null;
        }

        public List<string> GetSelectedText()
        {
            var selectedText = new List<string>();
            for (var i = 0; i < lbInput.Items.Count; i++)
            {
                if (lbInput.Items[i].Selected)
                {
                    selectedText.Add(lbInput.Items[i].Text);
                }
            }
            return selectedText;
        }

        public void RemoveSelected()
        {
            var itemsToRemove = new List<ListItem>();
            for (var i = 0; i < lbInput.Items.Count; i++)
            {
                if (lbInput.Items[i].Selected)
                {
                    itemsToRemove.Add(lbInput.Items[i]);
                }
            }

            foreach (var listItem in itemsToRemove)
            {
                lbInput.Items.Remove(listItem);
            }
        }

        #endregion

        #endregion
    }
}