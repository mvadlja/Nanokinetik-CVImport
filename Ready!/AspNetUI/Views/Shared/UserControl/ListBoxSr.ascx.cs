using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class ListBoxSr : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
    {
        #region Declarations

        private enum ActionType
        {
            New,
            Add,
            Edit,
            Remove
        }

        public event EventHandler<EventArgs> OnNewClick;
        public event EventHandler<EventArgs> OnAddClick;
        public event EventHandler<EventArgs> OnEditClick;
        public event EventHandler<EventArgs> OnRemoveClick;

        public object Data
        {
            get { return ViewState["ListBoxExtData"]; }
            set { ViewState["ListBoxExtData"] = value; }
        }

        private string _actions;

        private List<ActionType> _actionTypes;

        private SearchType _searchType;
 
        #endregion

        #region Properties

        public Searcher Searcher
        {
            get { return searcher; }
        }

        public System.Web.UI.HtmlControls.HtmlGenericControl DivListBoxSr
        {
            get { return divListBoxSr; }
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

        public bool IsModified
        {
            get
            {
                if (OldValue == null) return true;
                var oldValuesPk = OldValue;
                var newValuesPk = lbInput.Items.Cast<ListItem>().Select(newValue => newValue != null ? Convert.ToInt32(newValue.Value) : 0).OrderBy(newValue => newValue).ToList();

                return !ListOperations.ListsEquals(oldValuesPk, newValuesPk);
            }
        }

        /// <summary>
        /// Available actions seperated by comma
        /// </summary>
        public string Actions
        {
            get { return _actions; }
            set 
            { 
                _actions = value;

                SetAvailableActions();
            }
        }

        private List<ActionType> ActionTypes
        {
            get { return _actionTypes ?? new List<ActionType>(); }
            set { _actionTypes = value; }
        }

        public SearchType SearchType
        {
            get { return _searchType; }
            set { _searchType = value; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = lbInput.Items.Cast<ListItem>().Select(newValue => newValue != null ? Convert.ToInt32(newValue.Value) : 0).OrderBy(newValue => newValue).ToList();
            }
        }

        #endregion

        #region Support methods

        private void SetAvailableActions()
        {
            ActionTypes.Clear();

            lbtnNew.Visible = false;
            lbtnAdd.Visible = false;
            lbtnEdit.Visible = false;
            lbtnRemove.Visible = false;

            if (string.IsNullOrWhiteSpace(_actions)) return;

            foreach (var action in _actions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (action.ToLower().Trim() == ActionType.New.ToString().ToLower())
                {
                    ActionTypes.Add(ActionType.New);
                    lbtnNew.Visible = true;
                }
                else if (action.ToLower().Trim() == ActionType.Add.ToString().ToLower())
                {
                    ActionTypes.Add(ActionType.Add);
                    lbtnAdd.Visible = true;
                }
                else if (action.ToLower().Trim() == ActionType.Edit.ToString().ToLower())
                {
                    ActionTypes.Add(ActionType.Edit);
                    lbtnEdit.Visible = true;
                }
                else if (action.ToLower().Trim() == ActionType.Remove.ToString().ToLower())
                {
                    ActionTypes.Add(ActionType.Remove);
                    lbtnRemove.Visible = true;
                }
            }
        }

        public List<T> GetDataList<T>()
        {
            if (ViewState["ListBoxExtData"] is List<T>)
                return (List<T>)ViewState["ListBoxExtData"];

            ViewState["ListBoxExtData"] = new List<T>();
            return (List<T>)ViewState["ListBoxExtData"];
        }

        public void SetDataList<T>(List<T> value)
        {
            ViewState["ListBoxExtData"] = value;
        }

        public void ClearDataList()
        {
            ViewState["ListBoxExtData"] = null;
        }

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
                        var templateParts = sourceTextExpression.Split(new[] { "||" }, StringSplitOptions.None);
                        var templateProperties = templateParts[1].Split(new[] { "," }, StringSplitOptions.None);
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

        public void RemoveSelected<T>(string property = null)
        {
            RemoveSelectedFromLbData<T>(property);
            lbInput.RemoveSelected();
        }

        public void RemoveSelectedFromLbData<T>(string property)
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
                    List<int?> selectedIDs = lbInput.GetValidSelectedIds();
                    dataList.RemoveAll(m => selectedIDs.Contains((int?)m.GetType().GetProperty(property).GetValue(m, null)));
                }
                else if (objectPropertyType == typeof(string))
                {
                    List<string> selectedValues = lbInput.GetSelectedValues();
                    dataList.RemoveAll(m => selectedValues.Contains((string)m.GetType().GetProperty(property).GetValue(m, null)));
                }
            }
            else
            {
                if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
                {
                    List<int?> selectedIDs = lbInput.GetValidSelectedIds();
                    dataList.RemoveAll(m => selectedIDs.Contains((int)(object)m));
                }
                else if (typeof(T) == typeof(string))
                {
                    List<string> selectedValues = lbInput.GetSelectedValues();
                    dataList.RemoveAll(m => selectedValues.Contains((string)(object)m));
                }

            }
        }

        #endregion

        #region IOMethods
        /// <summary>
        /// Get list of objects that exists in database but not in ListBox hidden data, compared by provided property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemsInDb"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public List<T> GetDbItemsToDelete<T>(List<T> itemsInDb, string property = null)
        {
            var itemsToDelete = new List<T>();
            var dataList = GetDataList<T>();

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
                    foreach (var item in itemsInDb)
                    {
                        var itemValue = item.GetType().GetProperty(property).GetValue(item, null);

                        if (itemValue == null) continue;

                        if (!dataList.Exists(m => (int?)m.GetType().GetProperty(property).GetValue(m, null) == (int)itemValue))
                        {
                            itemsToDelete.Add(item);
                        }
                    }
                }
                else if (objectPropertyType == typeof(string))
                {
                    foreach (var item in itemsInDb)
                    {
                        var itemValue = Convert.ToString(item.GetType().GetProperty(property).GetValue(item, null));

                        if (string.IsNullOrWhiteSpace(itemValue)) continue;

                        if (!dataList.Exists(m => Convert.ToString(m.GetType().GetProperty(property).GetValue(m, null)) == itemValue))
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
                    foreach (var item in itemsInDb)
                    {
                        if ((object)item == null) continue;

                        if (!dataList.Exists(m => (int?)(object)m == (int)(object)item))
                        {
                            itemsToDelete.Add(item);
                        }
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    foreach (var item in itemsInDb)
                    {
                        if (string.IsNullOrWhiteSpace(Convert.ToString(item))) continue;

                        if (!dataList.Exists(m => (Convert.ToString(m) == Convert.ToString(item))))
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
        /// <param name="itemsInDb"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public List<int> GetDbItemsIdsToDelete<T>(List<T> itemsInDb, string property = null)
        {
            var itemsToDelete = new List<int>();
            var dataList = GetDataList<T>();

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
                    foreach (var item in itemsInDb)
                    {
                        var itemValue = item.GetType().GetProperty(property).GetValue(item, null);

                        if (itemValue == null) continue;

                        if (!dataList.Exists(m => (int?)m.GetType().GetProperty(property).GetValue(m, null) == (int)itemValue))
                        {
                            itemsToDelete.Add((int)itemValue);
                        }
                    }
                }
            }
            else
            {
                if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
                {
                    foreach (var item in itemsInDb)
                    {
                        if ((object)item == null) continue;

                        if (!dataList.Exists(m => (int?)(object)m == (int)(object)item))
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
        /// <param name="itemsInDb"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public List<string> GetDbItemsStrIdsToDelete<T>(List<T> itemsInDb, string property = null)
        {
            var itemsToDelete = new List<string>();
            var dataList = GetDataList<T>();

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
                    foreach (var item in itemsInDb)
                    {
                        var itemValue = Convert.ToString(item.GetType().GetProperty(property).GetValue(item, null));

                        if (string.IsNullOrWhiteSpace(itemValue)) continue;

                        if (!dataList.Exists(m => Convert.ToString(m.GetType().GetProperty(property).GetValue(m, null)) == itemValue))
                        {
                            itemsToDelete.Add(itemValue);
                        }
                    }
                }
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    foreach (var item in itemsInDb)
                    {
                        var itemValue = Convert.ToString(item);

                        if (string.IsNullOrWhiteSpace(itemValue)) continue;

                        if (!dataList.Exists(m => (Convert.ToString(m) == itemValue)))
                        {
                            itemsToDelete.Add(itemValue);
                        }
                    }
                }

            }

            return itemsToDelete;
        }
        /// <summary>
        /// Get list of objects where object property in case of int is less then 0 and in case of string starts with "-"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="preserveObjectId"></param>
        /// <returns></returns>
        internal List<T> GetNewDataListItems<T>(string property, bool preserveObjectId = false)
        {
            var itemsToSave = new List<T>();
            var dataList = GetDataList<T>();

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
                        var itemValue = item.GetType().GetProperty(property).GetValue(item, null);

                        if (itemValue == null || (int)itemValue >= 0) continue;

                        if (!preserveObjectId)
                        {
                            item.GetType().GetProperty(property).SetValue(item, null, null);
                        }

                        itemsToSave.Add(item);
                    }
                }
                else if (objectPropertyType == typeof(string))
                {
                    foreach (var item in dataList)
                    {
                        var itemValue = Convert.ToString(item.GetType().GetProperty(property).GetValue(item, null));

                        if (string.IsNullOrWhiteSpace(itemValue)) continue;

                        if (itemValue.StartsWith("-"))
                        {
                            if (!preserveObjectId)
                            {
                                item.GetType().GetProperty(property).SetValue(item, null, null);
                            }

                            itemsToSave.Add(item);
                        }
                    }
                }
            }
            else
            {
                if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
                {
                    foreach (var item in dataList)
                    {
                        if ((object)item != null && (int)(object)item < 0)
                        {
                            itemsToSave.Add(item);
                        }
                    }
                }
                else if (typeof(T) == typeof(string))
                {
                    foreach (var item in dataList)
                    {
                        var itemValue = Convert.ToString(item);

                        if (itemValue.StartsWith("-"))
                        {
                            itemsToSave.Add(item);
                        }
                    }
                }
            }
            return itemsToSave;
        }
        #endregion

        #endregion

        #region Event Handlers

        protected void LbInputOnSelectedIndexChanged(object sender, EventArgs e)
        {
            lbtnRemove.Enabled = false;
            lbtnEdit.Enabled = false;

            var numSelectedItems = lbInput.GetNumberOfSelectedItems();

            if (numSelectedItems <= 0) return;

            lbtnRemove.Enabled = true;

            if (numSelectedItems == 1)
            {
                lbtnEdit.Enabled = true;
            }
        }

        protected void BtnNewOnClick(object sender, EventArgs e)
        {
            if (OnNewClick != null) OnNewClick(sender, e);
        }

        protected void BtnAddOnClick(object sender, EventArgs e)
        {
            if (OnAddClick != null)
            {
                OnAddClick(sender, e);
            }
            else
            {
                searcher.ShowModalSearcher(SearchType, SelectMode.Multiple);
            }
        }

        protected void BtnEditOnClick(object sender, EventArgs e)
        {
            if (OnEditClick != null) OnEditClick(sender, e);
        }

        protected void BtnRemoveOnClick(object sender, EventArgs e)
        {
            if (OnRemoveClick != null)
            {
                OnRemoveClick(sender, e);
            }
            else
            {
                lbInput.RemoveSelected();
            }
        }
        #endregion
    }
}