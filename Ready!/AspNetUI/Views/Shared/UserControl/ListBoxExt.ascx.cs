using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class ListBoxExt : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
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

        private string _actions;
        private List<ActionType> _actionTypes;
        private Type _type;

        #endregion

        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivListBoxExt
        {
            get { return divListBoxExt; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.ListBox LbInput
        {
            get { return lbInput; }
        }

        public LinkButton LbtnNew
        {
            get { return lbtnNew; }
        }

        public LinkButton LbtnAdd
        {
            get { return lbtnAdd; }
        }

        public LinkButton LbtnEdit
        {
            get { return lbtnEdit; }
        }

        public LinkButton LbtnRemove
        {
            get { return lbtnRemove; }
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

        public bool FontItalic
        {
            get { return lblName.Font.Italic; }
            set { lblName.Font.Italic = value; }
        }

        public bool FontUnderline
        {
            get { return lblName.Font.Underline; }
            set { lblName.Font.Underline = value; }
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

        public bool AutoPostBack
        {
            get { return lbInput.AutoPostBack; }
            set { lbInput.AutoPostBack = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
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
                var newValuesPk = lbInput.Items.Cast<ListItem>().Select(newValue => ValidationHelper.IsValidInt(newValue.Value) ? Convert.ToInt32(newValue.Value) : 0).OrderBy(newValue => newValue).ToList();
                var oldValuesString = OldValueString;
                var newValuesString = lbInput.Items.Cast<ListItem>().Select(newValue => newValue.Text).OrderBy(newValue => newValue).ToList();

                return !ListOperations.ListsEquals(oldValuesPk, newValuesPk) || !ListOperations.ListsEquals(oldValuesString, newValuesString);
            }
        }

        private List<ActionType> ActionTypes
        {
            get { return _actionTypes ?? new List<ActionType>(); }
            set { _actionTypes = value; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = lbInput.Items.Cast<ListItem>().Select(newValue => ValidationHelper.IsValidInt(newValue.Value) ? Convert.ToInt32(newValue.Value) : 0).OrderBy(newValue => newValue).ToList();
                OldValueString = lbInput.Items.Cast<ListItem>().Select(newValue => newValue.Text).OrderBy(newValue => newValue).ToList();
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

        public List<T> GetDataEntities<T>()
        {
            if (ViewState["ListBoxExtData"] is Dictionary<string, T>)
            {
                return (from item in ViewState["ListBoxExtData"] as Dictionary<string, T> select item.Value).ToList();
            }

            return new List<T>();
        }

        public Dictionary<string, T> GetData<T>()
        {
            if (ViewState["ListBoxExtData"] is Dictionary<string, T>)
            {
                return ViewState["ListBoxExtData"] as Dictionary<string, T>;
            }

            return new Dictionary<string, T>();
        }

        public void SetData<T>(Dictionary<string, T> value)
        {
            ViewState["ListBoxExtData"] = value;
            _type = typeof(T);
        }

        public bool AddEntityToData<T>(string listItemValue, T entity)
        {
            if (string.IsNullOrWhiteSpace(listItemValue) || entity == null) return false;

            _type = typeof(T);

            var data = GetData<T>();

            if (data.Keys.All(key => key != listItemValue))
            {
                data.Add(listItemValue, entity);

                ViewState["ListBoxExtData"] = data;
                return true;
            }

            return false;
        }

        public bool UpdateEntityFromData<T>(string listItemValue, T entity)
        {
            if (string.IsNullOrWhiteSpace(listItemValue)) return false;

            if (RemoveEntityFromData<T>(listItemValue))
            {
                AddEntityToData(listItemValue, entity);

                return true;
            }

            return false;
        }

        public bool AddOrUpdateEntityFromData<T>(string listItemValue, T entity)
        {
            if (string.IsNullOrWhiteSpace(listItemValue)) return false;

            RemoveEntityFromData<T>(listItemValue);
            if (AddEntityToData(listItemValue, entity))
            {
                return true;
            }

            return false;
        }

        public bool RemoveEntityFromData<T>(string listItemValue)
        {
            if (string.IsNullOrWhiteSpace(listItemValue)) return false;

            var data = GetData<T>();

            if (data.ContainsKey(listItemValue))
            {
                data.Remove(listItemValue);

                ViewState["ListBoxExtData"] = data;
                return true;
            }

            return false;
        }

        public object GetEntityFromData<T>(string listItemValue)
        {
            if (string.IsNullOrWhiteSpace(listItemValue)) return null;

            var data = GetData<T>();

            if (data.ContainsKey(listItemValue))
            {
                return data[listItemValue];
            }

            return null;
        }

        public object GetFirstSelectedEntityFromData<T>()
        {
            var listItemValue = lbInput.GetFirstSelectedValue();

            if (string.IsNullOrWhiteSpace(listItemValue)) return null;

            var data = GetData<T>();

            if (data.ContainsKey(listItemValue))
            {
                return data[listItemValue];
            }

            return null;
        }

        public int GetNextIdForNewEntity<T>()
        {
            var minId = -1;

            foreach (var key in GetData<T>().Keys)
            {
                int id;

                if (!int.TryParse(key, out id)) continue;

                if (id <= minId)
                    minId = id - 1;
            }

            return minId;
        }

        public void ClearData()
        {
            ViewState["ListBoxExtData"] = null;
        }

        public void Fill<T>(IList<T> source, string sourceTextExpression, string valueProperty, bool fillData = false, string defaultEmptyValue = Constant.UnknownValue)
        {
            Clear();

            _type = typeof(T);

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
                        textString = defaultEmptyValue;
                    }

                    var valueString = Convert.ToString(value);

                    if (lbInput.Items.FindByValue(valueString) != null) continue;

                    lbInput.Items.Add(new ListItem(textString, valueString));

                    if (fillData)
                    {
                        AddEntityToData(valueString, item);
                    }
                }
            }
            else
            {
                foreach (var item in source)
                {
                    var textString = Convert.ToString(item);

                    if (string.IsNullOrWhiteSpace(textString))
                    {
                        textString = defaultEmptyValue;
                    }

                    var valueString = Convert.ToString(item);

                    if (lbInput.Items.FindByValue(valueString) != null) continue;

                    lbInput.Items.Add(new ListItem(textString, valueString));

                    if (fillData)
                    {
                        AddEntityToData(valueString, item);
                    }
                }
            }
        }

        public void FillData<T>(ListItemCollection listItemCollection, List<T> entityList, string valueProperty)
        {
            Clear();

            _type = typeof(T);

            if (listItemCollection == null || entityList == null || string.IsNullOrEmpty(valueProperty)) return;

            foreach (ListItem listItem in listItemCollection)
            {
                T entityMatch = entityList.First(entity => Convert.ToString(entity.GetType().GetProperty(valueProperty).GetValue(listItem, null)) == listItem.Value);

                if (entityMatch == null) continue;

                AddEntityToData(listItem.Value, entityMatch);
            }
        }

        public void Clear()
        {
            lbInput.Items.Clear();
            ClearData();
        }

        #region Selected items operations

        public void RemoveSelected<T>(string property = null)
        {
            lbInput.GetSelectedItems().ForEach(item => RemoveEntityFromData<T>(item.Value));
            lbInput.RemoveSelected();
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
            var data = GetData<T>();

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

                foreach (var item in itemsInDb)
                {
                    var itemValue = Convert.ToString(item.GetType().GetProperty(property).GetValue(item, null));

                    if (string.IsNullOrWhiteSpace(itemValue)) continue;

                    if (!data.ContainsKey(itemValue))
                    {
                        itemsToDelete.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in itemsInDb)
                {
                    var itemValue = Convert.ToString(item);

                    if (string.IsNullOrWhiteSpace(itemValue)) continue;

                    if (!data.ContainsKey(itemValue))
                    {
                        itemsToDelete.Add(item);
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
            var data = GetData<T>();

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

                foreach (var item in itemsInDb)
                {
                    var itemValue = Convert.ToString(item.GetType().GetProperty(property).GetValue(item, null));

                    if (string.IsNullOrWhiteSpace(itemValue)) continue;

                    if (!data.ContainsKey(itemValue))
                    {
                        itemsToDelete.Add(int.Parse(itemValue));
                    }
                }
            }
            else
            {
                foreach (var item in itemsInDb)
                {
                    var itemValue = Convert.ToString(item);

                    if (string.IsNullOrWhiteSpace(itemValue)) continue;

                    if (!data.ContainsKey(itemValue))
                    {
                        itemsToDelete.Add(int.Parse(itemValue));
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
            var data = GetData<T>();

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

                foreach (var item in itemsInDb)
                {
                    var itemValue = Convert.ToString(item.GetType().GetProperty(property).GetValue(item, null));

                    if (string.IsNullOrWhiteSpace(itemValue)) continue;

                    if (!data.ContainsKey(itemValue))
                    {
                        itemsToDelete.Add(itemValue);
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

                        if (!data.ContainsKey(itemValue))
                        {
                            itemsToDelete.Add(itemValue);
                        }
                    }
                }

            }

            return itemsToDelete;
        }

        public List<T> GetNewDataListItems<T>()
        {
            var data = GetData<T>();

            return (from keyValuePair in data where keyValuePair.Key.StartsWith("-") select keyValuePair.Value).ToList();
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
            if (OnAddClick != null) OnAddClick(sender, e);
        }

        protected void BtnEditOnClick(object sender, EventArgs e)
        {
            if (OnEditClick != null) OnEditClick(sender, e);
        }

        protected void BtnRemoveOnClick(object sender, EventArgs e)
        {
            if (OnRemoveClick != null) OnRemoveClick(sender, e);
        }

        #endregion
    }
}