using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script;
using AspNetUIFramework;
using System.Collections;

namespace AspNetUI.Support
{
    public partial class ListBoxAER_CT : System.Web.UI.UserControl, IControlCommon, IControlList
    {
        private string _errorMessage = String.Empty;
        private string _emptyErrorMessage = String.Empty;
        private string _controlValueFormat = String.Empty;
        private ControlState _controlState = ControlState.ReadyForAction;
        private string _bindingPath = String.Empty;
        private string _sourceTextExpression = String.Empty;
        private Style _buttonsStyle;

        public event EventHandler<ValueChangedEventArgs> InputValueChanged;
        public event EventHandler<EventArgs> OnAddClick;
        public event EventHandler<EventArgs> OnEditClick;
        public event EventHandler<EventArgs> OnRemoveClick;

        public object Data
        {
            get { return (object)ViewState["ListBoxAER_CT_Data"]; }
            set { ViewState["ListBoxAER_CT_Data"] = value; }
        }

        public List<T> GetDataList<T>()
        {
            if (ViewState["ListBoxAER_CT_Data"] != null && ViewState["ListBoxAER_CT_Data"] is List<T>)
                return (List<T>)ViewState["ListBoxAER_CT_Data"];
            else {
                ViewState["ListBoxAER_CT_Data"] = new List<T>();
                return (List<T>)ViewState["ListBoxAER_CT_Data"];
            }
                
        }

        public void SetDataList<T>(List<T> value)
        {
            ViewState["ListBoxAER_CT_Data"] = value;
        }

        public void ClearDataList()
        {
            ViewState["ListBoxAER_CT_Data"] = null;
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

        private void SetDefaultControlState()
        {
            //this.Visible = true;
            ctlInput.Enabled = true;
            //ctlAlert.Visible = false;
            if (string.IsNullOrEmpty(ctlLabel.Style["color"]) || !ctlLabel.Style["color"].Contains("!important"))
            {
                ctlLabel.Style["color"] = "";
            }
            ctlInput.Style["border"] = "";
            ctlInput.Style["padding"] = "";

            ctlInput.ToolTip = String.Empty;
            //ctlAlert.ToolTip = String.Empty;
            ctlInput.Attributes.Remove("disabled");

            //btnAdd.ApplyStyle(_buttonsStyle);
            //btnEdit.ApplyStyle(_buttonsStyle);
            //btnRemove.ApplyStyle(_buttonsStyle);

            lbtnAdd.ApplyStyle(_buttonsStyle);
            lbtnEdit.ApplyStyle(_buttonsStyle);
            lbtnRemove.ApplyStyle(_buttonsStyle);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string associatedJS = divScript.InnerText;
            divScript.Visible = false;

            ClientScriptManager mgr = Page.ClientScript;
            Type cstype = this.GetType();
            if (!mgr.IsStartupScriptRegistered(cstype, "initScript"))
            {

                ScriptManager.RegisterStartupScript(this.Page, cstype, "initScript", "listBoxAER_initializeOnPostback(null,null);", true);
            }

            if (!mgr.IsClientScriptBlockRegistered(cstype, "associatedJS"))
            {

                ScriptManager.RegisterClientScriptBlock(this, cstype, "associatedJS", associatedJS, false);
            }
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
                foreach (string value in this.ControlValue as List<string>)
                {
                    textValues += ctlInput.Items.FindByValue(value).Text + ", ";
                }
                if (!string.IsNullOrWhiteSpace(textValues))
                    textValues = textValues.Remove(textValues.Length - 2);
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
                        //ctlAlert.ToolTip = ControlErrorMessage;
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

        public Style ButtonsStyle
        {
            get { return _buttonsStyle; }
            set { _buttonsStyle = value; }
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

        #endregion


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
       
        }

        #region Event Handlers
        protected void ctlInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InputValueChanged != null) InputValueChanged(this, new ValueChangedEventArgs(ctlInput.Text));

            //btnRemove.Enabled = false;
            //btnEdit.Enabled = false;
            lbtnRemove.Enabled = false;
            lbtnEdit.Enabled = false;

            int numSelectedItems = GetListSelectedItems().Count;
            if (numSelectedItems > 0)
            {
                //btnRemove.Enabled = true;
                lbtnRemove.Enabled = true;

                if (numSelectedItems == 1)
                {
                    //btnEdit.Enabled = true;
                    lbtnEdit.Enabled = true;
                }
            }
        }

        public void DisableControl()
        {
            CurrentControlState = ControlState.YouCantChangeMe;
        }

        public void EnableControl()
        {
            CurrentControlState = ControlState.ReadyForAction;
        }

        protected void btnAddOnClick(object sender, EventArgs e)
        {
            if (OnAddClick != null) OnAddClick(sender, e);
        }

        protected void btnEditOnClick(object sender, EventArgs e)
        {
            if (OnEditClick != null) OnEditClick(sender, e);
            //btnEdit.Enabled = false;
            //   lbtnEdit.Enabled = false;
        }

        protected void btnRemoveOnClick(object sender, EventArgs e)
        {
            if (OnRemoveClick != null) OnRemoveClick(sender, e);
            //btnRemove.Enabled = false;
            //  lbtnRemove.Enabled = false;
        }
        #endregion

        #region List items sorting
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
        #endregion

        #region Selected items operations
        /// <summary>
        /// Move selected list items to destinationListBox
        /// </summary>
        /// <param name="listBox"></param>
        public void MoveSelectedItemsTo(ListBox_CT destionationListBox)
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
                destionationListBox.ControlBoundItems.Add(itemsToMove[i]);
            }
        }
        /// <summary>
        /// Copy selected items to destionation ListBox
        /// </summary>
        /// <param name="listBox"></param>
        public void CopySelectedItemsTo(ListBox_CT destionationListBox)
        {
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    destionationListBox.ControlBoundItems.Add(ctlInput.Items[i]);
                }
            }
        }

        /// <summary>
        /// Unselect all ListBox items
        /// </summary>
        public void UnselectAll()
        {
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                ctlInput.Items[i].Selected = false;
            }
            //btnRemove.Enabled = false;
            //btnEdit.Enabled = false;
            //  lbtnRemove.Enabled = false;
            //    lbtnEdit.Enabled = false;
        }

        public List<int> GetListValidSelectedIDs()
        {
            List<int> selectedIDs = new List<int>();
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    if (ValidationHelper.IsValidInt(ctlInput.Items[i].Value))
                        selectedIDs.Add(int.Parse(ctlInput.Items[i].Value));
                }
            }
            return selectedIDs;
        }

        public List<string> GetListSelectedValues()
        {
            List<string> selectedValues = new List<string>();
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    selectedValues.Add(ctlInput.Items[i].Value);
                }
            }
            return selectedValues;
        }

        public List<ListItem> GetListSelectedItems()
        {
            List<ListItem> selectedItems = new List<ListItem>();
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    selectedItems.Add(ctlInput.Items[i]);
                }
            }
            return selectedItems;
        }

        public string GetFirstSelectedValue()
        {
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    return ctlInput.Items[i].Value;
                }
            }
            return null;
        }

        public int? GetFirstSelectedID()
        {
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    if (ValidationHelper.IsValidInt(ctlInput.Items[i].Value))
                        return int.Parse(ctlInput.Items[i].Value);
                    else
                        return null;
                }
            }
            return null;
        }

        public ListItem GetFirstSelectedItem()
        {
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    return ctlInput.Items[i];
                }
            }
            return null;
        }

        public List<string> GetListSelectedText()
        {
            List<string> selectedText = new List<string>();
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    selectedText.Add(ctlInput.Items[i].Text);
                }
            }
            return selectedText;
        }

        /// <summary>
        /// If T is custom type and property is provided removes selected items from ListBox and ListBox hidden data where ListBox item value matches custom type property value
        /// else removes selected items from LIstBox and ListBox hidden data where Listbox hidden data item value matches selected ListBox item value
        /// </summary> 
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        public void RemoveSelected<T>(string property = null)
        {
            RemoveSelectedOnlyFromData<T>(property);
            RemoveSelectedOnlyFromListBox();
        }

        /// <summary>
        /// If T is custom type and property is provided removes selected items from ListBox hidden data where ListBox item value matches custom type property value
        /// else removes selected items from ListBox hidden data where Listbox hidden data item value matches selected ListBox item value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        public void RemoveSelectedOnlyFromData<T>(string property)
        {
            List<T> dataList = GetDataList<T>();

            if (!string.IsNullOrWhiteSpace(property))
            {
                Type objectPropertyType = null;
                try
                {
                    objectPropertyType = typeof(T).GetProperty(property).PropertyType;
                }
                catch (NullReferenceException e)
                {

                    throw new Exception("Object doesn't contain property: " + property);
                }

                if (objectPropertyType == typeof(int?) || objectPropertyType == typeof(int))
                {
                    List<int> selectedIDs = GetListValidSelectedIDs();
                    dataList.RemoveAll(m => selectedIDs.Contains((int)m.GetType().GetProperty(property).GetValue(m, null)));
                }
                else if (objectPropertyType == typeof(string))
                {
                    List<string> selectedValues = GetListSelectedValues();
                    dataList.RemoveAll(m => selectedValues.Contains((string)m.GetType().GetProperty(property).GetValue(m, null)));
                }
            }
            else
            {
                if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
                {
                    List<int> selectedIDs = GetListValidSelectedIDs();
                    dataList.RemoveAll(m => selectedIDs.Contains((int)(object)m));
                }
                else if (typeof(T) == typeof(string))
                {
                    List<string> selectedValues = GetListSelectedValues();
                    dataList.RemoveAll(m => selectedValues.Contains((string)(object)m));
                }

            }
        }
        /// <summary>
        /// Removes selected ListBox items from ListBox
        /// </summary>
        public void RemoveSelectedOnlyFromListBox()
        {
            List<ListItem> itemsToRemove = new List<ListItem>();
            for (int i = 0; i < ctlInput.Items.Count; i++)
            {
                if (ctlInput.Items[i].Selected)
                {
                    itemsToRemove.Add(ctlInput.Items[i]);
                }
            }

            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                ctlInput.Items.Remove(itemsToRemove[i]);
            }
        }
        #endregion

        #region IOMethods
        /// <summary>
        /// Get list of objects that exists in database and not in ListBox hidden data compared by provided property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemsInDB"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public List<T> GetListToDelete<T>(List<T> itemsInDB, string property = null)
        {
            List<T> itemsToDelete = new List<T>();
            List<T> dataList = GetDataList<T>();

            if (!string.IsNullOrWhiteSpace(property))
            {
                Type objectPropertyType = null;
                try
                {
                    objectPropertyType = typeof(T).GetProperty(property).PropertyType;
                }
                catch (NullReferenceException e)
                {
                    throw new Exception("Object doesn't contain property: " + property);
                }

                if (objectPropertyType == typeof(int?) || objectPropertyType == typeof(int))
                {
                    foreach (var item in itemsInDB)
                    {
                        if (!dataList.Exists(m => (int)m.GetType().GetProperty(property).GetValue(m, null) == (int)item.GetType().GetProperty(property).GetValue(item, null)))
                        {
                            itemsToDelete.Add(item);
                        }
                    }
                }
                else if (objectPropertyType == typeof(string))
                {
                    foreach (var item in itemsInDB)
                    {
                        if (!dataList.Exists(m => (string)m.GetType().GetProperty(property).GetValue(m, null) == (string)item.GetType().GetProperty(property).GetValue(m, null)))
                        {
                            itemsToDelete.Add(item);
                        }
                    }
                }
            }
            else
            {
                if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
                {
                    foreach (var item in itemsInDB)
                    {
                        if (!dataList.Exists(m => (int)(object)m == (int)(object)item))
                        {
                            itemsToDelete.Add(item);
                        }
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    foreach (var item in itemsInDB)
                    {
                        if (!dataList.Exists(m => (string)(object)m == (string)(object)item))
                        {
                            itemsToDelete.Add(item);
                        }
                    }
                }

            }

            return itemsToDelete;
        }

        /// <summary>
        /// Get list of property values as integers that exists in database and not in ListBox hidden data compared by provided property where property is int
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemsInDB"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public List<int> GetListIDsToDelete<T>(List<T> itemsInDB, string property = null)
        {
            List<int> itemsToDelete = new List<int>();
            List<T> dataList = GetDataList<T>();

            if (!string.IsNullOrWhiteSpace(property))
            {
                Type objectPropertyType = null;
                try
                {
                    objectPropertyType = typeof(T).GetProperty(property).PropertyType;
                }
                catch (NullReferenceException e)
                {

                    throw new Exception("Object doesn't contain property: " + property);
                }

                if (objectPropertyType == typeof(int?) || objectPropertyType == typeof(int))
                {
                    foreach (var item in itemsInDB)
                    {
                        if (!dataList.Exists(m => (int)m.GetType().GetProperty(property).GetValue(m, null) == (int)item.GetType().GetProperty(property).GetValue(item, null)))
                        {
                            itemsToDelete.Add((int)item.GetType().GetProperty(property).GetValue(item, null));
                        }
                    }
                }
            }
            else
            {
                if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
                {
                    foreach (var item in itemsInDB)
                    {
                        if (!dataList.Exists(m => (int)(object)m == (int)(object)item))
                        {
                            itemsToDelete.Add((int)(object)item);
                        }
                    }
                }
            }

            return itemsToDelete;
        }
        /// <summary>
        /// Get list of property values as strings that exists in database and not in ListBox hidden data compared by provided property where property is string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemsInDB"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public List<string> GetListValuesToDelete<T>(List<T> itemsInDB, string property = null)
        {
            List<string> itemsToDelete = new List<string>();
            List<T> dataList = GetDataList<T>();

            if (!string.IsNullOrWhiteSpace(property))
            {
                Type objectPropertyType = null;
                try
                {
                    objectPropertyType = typeof(T).GetProperty(property).PropertyType;
                }
                catch (NullReferenceException e)
                {

                    throw new Exception("Object doesn't contain property: " + property);
                }

                if (objectPropertyType == typeof(string))
                {
                    foreach (var item in itemsInDB)
                    {
                        if (!dataList.Exists(m => (string)m.GetType().GetProperty(property).GetValue(m, null) == (string)item.GetType().GetProperty(property).GetValue(item, null)))
                        {
                            itemsToDelete.Add((string)item.GetType().GetProperty(property).GetValue(item, null).ToString());
                        }
                    }
                }
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    foreach (var item in itemsInDB)
                    {
                        if (!dataList.Exists(m => (string)(object)m == (string)(object)item))
                        {
                            itemsToDelete.Add((string)(object)item);
                        }
                    }
                }

            }

            return itemsToDelete;
        }
        /// <summary>
        /// Get list of objects where object property in case of int is less then 0 and in case of string begins with "-"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="preserveObjectID"></param>
        /// <returns></returns>
        internal List<T> GetNewDataListItems<T>(string property, bool preserveObjectID = false)
        {
            List<T> itemsToSave = new List<T>();
            List<T> dataList = GetDataList<T>();

            if (!string.IsNullOrWhiteSpace(property))
            {
                Type objectPropertyType = null;
                try
                {
                    objectPropertyType = typeof(T).GetProperty(property).PropertyType;
                }
                catch (NullReferenceException e)
                {
                    throw new Exception("Object doesn't contain property: " + property);
                }

                if (objectPropertyType == typeof(int?) || objectPropertyType == typeof(int))
                {
                    foreach (var item in dataList)
                    {
                        if ((int)item.GetType().GetProperty(property).GetValue(item, null) < 0)
                        {
                            if (!preserveObjectID)
                                item.GetType().GetProperty(property).SetValue(item, null, null);
                            itemsToSave.Add(item);
                        }
                        //else
                        //{
                        //    itemsToSave.Add(item);
                        //}
                    }
                }
                else if (objectPropertyType == typeof(string))
                {
                    foreach (var item in dataList)
                    {
                        if (item.GetType().GetProperty(property).GetValue(item, null).ToString().StartsWith("-"))
                        {
                            if (!preserveObjectID)
                                item.GetType().GetProperty(property).SetValue(item, null, null);
                            itemsToSave.Add(item);
                        }
                        //else
                        //{
                        //    itemsToSave.Add(item);
                        //}
                    }
                }
            }
            else
            {
                //return dataList;
                //if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
                //{
                //    foreach (var item in dataList)
                //    {
                //        if ((int)(object)item < 0)
                //        {
                //            itemsToSave.Add(item);
                //        }
                //    }
                //}
                //else if (typeof(T) == typeof(string))
                //{
                //    foreach (var item in dataList)
                //    {
                //        if (item.ToString().StartsWith("-"))
                //        {
                //            itemsToSave.Add(item);
                //        }
                //    }
                //}
            }
            return itemsToSave;
        }
        #endregion
    }
}