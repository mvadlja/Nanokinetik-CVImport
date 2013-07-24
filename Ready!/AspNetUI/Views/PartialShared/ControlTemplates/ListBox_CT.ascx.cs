using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using System.Collections;
using System.Drawing;

namespace AspNetUI.Support
{
    public partial class ListBox_CT : System.Web.UI.UserControl, IControlCommon, IControlList, IControlSpecialDisable
    {
        private string _errorMessage = String.Empty;
        private string _emptyErrorMessage = String.Empty;
        private string _controlValueFormat = String.Empty;
        private ControlState _controlState = ControlState.ReadyForAction;
        private string _bindingPath = String.Empty;
        private string _sourceTextExpression = String.Empty;
        private string _changedJS;
        private string _clientID_CT;

        public string ClientID_CT
        {
            get { return _clientID_CT; }
            set { _clientID_CT = value; }
        }


        public string ChangedJS
        {
            get { return _changedJS; }
            set { _changedJS = value; }
        }


        public int ControlVisibleRows
        {
            get { return ctlInput.Rows; }
            set { ctlInput.Rows = value; }
        }

        public bool ControlMultipleValueSelection
        {
            get { return ctlInput.SelectionMode == ListSelectionMode.Single ? false : true; }
            set { ctlInput.SelectionMode = value == true ? ListSelectionMode.Multiple : ListSelectionMode.Single; }
        }

        #region IControlData

        public object ControlValue
        {
            get
            {
                List<string> tempList = new List<string>();

                foreach (ListItem li in ctlInput.Items)
                {
                    if (li.Selected) tempList.Add(li.Value);
                }

                return tempList;
            }
            set
            {
                if (value is IList)
                {
                    IList values = value as IList;

                    foreach (object val in values)
                    {
                        foreach (ListItem li in ctlInput.Items)
                        {
                            if (SerializeValue(val).ToString() == li.Value.ToString())
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (ListItem li in ctlInput.Items)
                    {
                        if (SerializeValue(value).ToString() == li.Value.ToString())
                        {
                            li.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        public string ControlValueFormat
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ControlLabel
        {
            get { return ctlLabel.Text; }
            set
            {
                ctlLabel.Text = value;

                if (String.IsNullOrEmpty(value)) ctlLabel.Visible = false;
                else ctlLabel.Visible = true;
            }
        }

        public string ControlInputUnitsLabel
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ControlDescriptor
        {
            get { return ctlDescription.ToolTip; }
            set
            {
                ctlDescription.ToolTip = value;

                if (String.IsNullOrEmpty(value)) ctlDescription.Visible = false;
                else ctlDescription.Visible = true;
            }
        }

        public string ControlErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public string ControlEmptyErrorMessage
        {
            get { return _emptyErrorMessage; }
            set { _emptyErrorMessage = value; }
        }

        public bool IsMandatory
        {
            get { return ctlMark.Visible; }
            set { ctlMark.Visible = value; }
        }

        public int MaxLength
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ControlTextValue
        {
            get 
            { 
                string textValues = string.Empty;
                foreach ( string value in this.ControlValue as List<string> )
                {
                    textValues += ctlInput.Items.FindByValue( value ).Text + ", ";
                }
                if ( !string.IsNullOrWhiteSpace( textValues ) )
                    textValues = textValues.Remove( textValues.Length - 2 );
                return textValues;
            }
        }
        #endregion

        #region IControlStates

        public ControlState CurrentControlState
        {
            get { return _controlState; }
            set
            {
                _controlState = value;
                SetDefaultControlState();

                switch (_controlState)
                {
                    case ControlState.IAmInvalid:
                        //ctlAlert.Visible = true;
                        ctlLabel.Style["color"] = "#ff0000";
                        ctlInput.Style["border"] = "1px solid #ff0000!important";
                        ctlInput.Style["padding"] = "0px";
                        ctlInput.ToolTip = ControlErrorMessage;
                        ctlAlert.ToolTip = ControlErrorMessage;
                        break;
                    case ControlState.YouCantChangeMe:
                        ctlInput.Enabled = false;
                        ctlInput.Attributes.Add("disabled", "");
                        string classes = ctlInput.Attributes["class"] ?? "";
                        classes += "diabledListBox";
                        ctlInput.Attributes.Add("class", classes);
                        break;
                    case ControlState.YouCantSeeMe:
                        this.Visible = false;
                        break;
                    case ControlState.ReadyForAction:
                    case ControlState.DontKnowActually:
                    default:
                        // Who cares?
                        break;
                }
            }
        }

        public bool ControlSupportsState(ControlState state)
        {
            if (state == ControlState.DontKnowActually
                || state == ControlState.IAmInvalid
                || state == ControlState.ReadyForAction
                || state == ControlState.YouCantChangeMe
                || state == ControlState.YouCantSeeMe)
                return true;
            else return false;
        }

        #endregion

        #region IControlDesign

        public string TotalControlWidth
        {
            get { return tableCanvas.Width; }
            set { tableCanvas.Width = value; }
        }

        public string LabelColumnWidth
        {
            get { return tdLabel.Width; }
            set { tdLabel.Width = value; }
        }

        public string ControlInputWidth
        {
            get { return ctlInput.Width.ToString(); }
            set { ctlInput.Width = Unit.Parse(value); }
        }

        public string ControlLabelAlign
        {
            get { return tdLabel.Style["text-align"]; }
            set { tdLabel.Style["text-align"] = value; }
        }

        public bool FontItalic
        {
            get { return ctlLabel.Font.Italic; }
            set { ctlLabel.Font.Italic = value; }
        }

        public bool FontBold
        {
            get { return ctlLabel.Font.Bold; }
            set { ctlLabel.Font.Bold = value; }
        }

        public bool FontValueBold
        {
            get { return ctlInput.Font.Bold; }
            set { ctlInput.Font.Bold = value; }
        }

        public bool FontUnderline
        {
            get { return ctlLabel.Font.Underline; }
            set { ctlLabel.Font.Underline = value; }
        }

        public Color FontColor
        {
            get { return ctlLabel.ForeColor; }
            set { ctlLabel.ForeColor = value; }
        }

        public Unit Height
        {
            set { ctlInput.Height = value; }
        }



        #endregion

        #region IControlBinder

        public string BindingPath
        {
            get { return _bindingPath; }
            set { _bindingPath = value; }
        }

        public string SerializeValue(object input)
        {
            string value = String.Empty;

            if (input != null)
            {
                value = input.ToString();
            }

            return value;
        }

        public object DeserializeValue(string input)
        {
            return input;
        }

        #endregion

        #region IControlBehavior

        public bool AutoPostback
        {
            get { return ctlInput.AutoPostBack; }
            set { ctlInput.AutoPostBack = value; }
        }

        public event EventHandler<ValueChangedEventArgs> InputValueChanged;

        #endregion

        #region IControlList

        public string SourceValueProperty
        {
            get { return ctlInput.DataValueField; }
            set { ctlInput.DataValueField = value; }
        }

        /// <summary>
        /// Example: "{0}. {1}...||PropertyA,PropertyB" or simply property name
        /// </summary>
        public string SourceTextExpression
        {
            get { return _sourceTextExpression; }
            set { _sourceTextExpression = value; }
        }

        public void FillControl<T>(IList<T> source)
        {
            object text = null;
            object value = null;

            ctlInput.SelectedIndex = -1;
            ctlInput.Items.Clear();

            if (source != null)
            {
                if (!String.IsNullOrEmpty(SourceTextExpression) && !String.IsNullOrEmpty(SourceValueProperty))
                {
                    foreach (T item in source)
                    {
                        value = item.GetType().GetProperty(SourceValueProperty).GetValue(item, null);

                        if (!SourceTextExpression.Contains("{"))
                        {
                            text = item.GetType().GetProperty(SourceTextExpression).GetValue(item, null);
                        }
                        else
                        {
                            string[] templateParts = SourceTextExpression.Split(new string[] { "||" }, StringSplitOptions.None);
                            string[] templateProperties = templateParts[1].Split(new string[] { "," }, StringSplitOptions.None);
                            string template = templateParts[0];
                            object tempValue = String.Empty;

                            for (int i = 0; i < templateProperties.Length; i++)
                            {
                                tempValue = item.GetType().GetProperty(templateProperties[i]).GetValue(item, null);
                                template = template.Replace("{" + i + "}", tempValue.ToString());
                            }

                            text = template;
                        }
                        if (text == null) text = "";
                        ctlInput.Items.Add(new ListItem(text.ToString(), value.ToString()));
                    }
                }
                else
                {
                    foreach (T item in source)
                    {
                        ctlInput.Items.Add(new ListItem(item.ToString(), item.ToString()));
                    }
                }
            }
        }
        
        public ListItemCollection ControlBoundItems
        {
            get
            {
                return ctlInput.Items;
            }
        }

        public void SortItemsByText(bool preserveSelection = false)
        {
            ListItemCollection items = ctlInput.Items;
            
            List<ListItem> itemsList = new List<ListItem>(items.Count);
            foreach (ListItem item in items)
            {
                item.Selected = preserveSelection;
                itemsList.Add(item);
            }

            itemsList.Sort(delegate(ListItem item1, ListItem item2)
            {
                return item1.Text.CompareTo(item2.Text);
            });

            ctlInput.Items.Clear();
            foreach (ListItem item in itemsList)
            {
                ctlInput.Items.Add(item);
            }
        }

        public void SortItemsByValue(bool preserveSelection = false)
        {
            ListItemCollection items = ctlInput.Items;

            List<ListItem> itemsList = new List<ListItem>(items.Count);
            foreach (ListItem item in items)
            {
                item.Selected = preserveSelection;
                itemsList.Add(item);
            }

            itemsList.Sort(delegate(ListItem item1, ListItem item2)
            {
                return item1.Value.CompareTo(item2.Value);
            });

            ctlInput.Items.Clear();
            foreach (ListItem item in itemsList)
            {
                ctlInput.Items.Add(item);
            }
        }

        public void MoveSelectedItemsTo(ListBox_CT listBox)
        {
            List<ListItem> itemsToMove = new List<ListItem>();
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    itemsToMove.Add(ctlInput.Items[i]);
                }
            }

            for (int i = 0; i < itemsToMove.Count; i++)
            {
                ctlInput.Items.Remove(itemsToMove[i]);
                listBox.ControlBoundItems.Add(itemsToMove[i]);
            }
        }

        public void CopySelectedItemsTo(ListBox_CT listBox)
        {
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    listBox.ControlBoundItems.Add(ctlInput.Items[i]);
                }
            }
        }

        #endregion


        private void SetDefaultControlState()
        {
            //this.Visible = true;
            ctlInput.Enabled = true;
            ctlAlert.Visible = false;
            if (string.IsNullOrEmpty(ctlLabel.Style["color"]) || !ctlLabel.Style["color"].Contains("!important"))
            {
                ctlLabel.Style["color"] = "";
            }

            ctlInput.Style["border"] = "";
            ctlInput.Style["padding"] = "";

            ctlInput.ToolTip = String.Empty;
            ctlAlert.ToolTip = String.Empty;
            ctlInput.Attributes.Remove("disabled");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlInput.Attributes["onchange"] = this.ChangedJS;
            ctlInput.Attributes.Add("clientID_CT", this.ClientID_CT != null ? this.ClientID_CT : "undefined");
        }

        protected void ctlInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InputValueChanged != null) InputValueChanged(this, new ValueChangedEventArgs(ctlInput.Text));
        }

        public void DisableControl()
        {
            CurrentControlState = ControlState.YouCantChangeMe;
        }

        public void EnableControl()
        {
            CurrentControlState = ControlState.ReadyForAction;
        }
    }
}