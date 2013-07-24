using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using AspNetUI.Views;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Support
{
    public static class ExtensionMethods
    {
        #region WebControl

        /// <summary>
        /// Adds "cssClassToAdd" class to a control Attributes "class" collection.
        /// Duplicates will be ignored and will not be added multiple times.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cssClassToAdd"></param>
        public static void AddCssClass(this WebControl control, string cssClassToAdd)
        {
            var controlCssAttribute = control.Attributes["class"] ?? control.CssClass;

            if (controlCssAttribute == null)
            {
                if (control.Attributes["class"] != null) control.Attributes.Add("class", cssClassToAdd);
                else if (control.CssClass != null) control.CssClass = cssClassToAdd;
            }
            else
            {
                if (control.Attributes["class"] != null) control.Attributes.Add("class", String.Join(" ", control.Attributes["class"].Split(' ').Except(new[] { "", cssClassToAdd }).Concat(new[] { cssClassToAdd }).ToArray()));
                else if (control.CssClass != null) control.CssClass = String.Join(" ", control.CssClass.Split(' ').Except(new[] { "", cssClassToAdd }).Concat(new[] { cssClassToAdd }).ToArray());
            }
        }

        /// <summary>
        /// Removes exact "cssClassToRemove" class from control Attributes "class" collection 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cssClassToRemove"></param>
        public static void RemoveCssClass(this WebControl control, string cssClassToRemove)
        {
            var controlCssAttribute = control.Attributes["class"] ?? control.CssClass;

            if (controlCssAttribute == null || !controlCssAttribute.Any()) return;

            if (control.Attributes["class"] != null) control.Attributes.Add("class", String.Join(" ", controlCssAttribute.Split(' ').Where(c => c != cssClassToRemove).ToArray()));
            else if (control.CssClass != null) control.CssClass = String.Join(" ", controlCssAttribute.Split(' ').Where(c => c != cssClassToRemove).ToArray());
        }

        /// <summary>
        /// Removes exact "cssClassesToRemove" class from control Attributes "class" collection 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cssClassesToRemove"></param>
        public static void RemoveCssClasses(this WebControl control, List<string> cssClassesToRemove)
        {
            cssClassesToRemove.ForEach(control.RemoveCssClass);
        }

        /// <summary>
        /// Removes all css classes from Attributes "class" collection that contains "cssClassToRemove" class name 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cssClassToRemove"></param>
        public static void RemoveCssClassContains(this WebControl control, string cssClassToRemove)
        {
            var controlCssAttribute = control.Attributes["class"] ?? control.CssClass;

            if (controlCssAttribute == null || !controlCssAttribute.Any()) return;

            if (control.Attributes["class"] != null) control.Attributes.Add("class", String.Join(" ", controlCssAttribute.Split(' ').Where(c => !c.Contains(cssClassToRemove)).ToArray()));
            else if (control.CssClass != null) control.CssClass = String.Join(" ", controlCssAttribute.Split(' ').Where(c => !c.Contains(cssClassToRemove)).ToArray());
        }

        #endregion

        #region Product

        /// <summary>
        /// Gets product name formatted as: <product.name> + " (" <product.product_number> ")"
        /// </summary>
        /// <param name="product"></param>
        /// <param name="defaultEmptyValue"></param>
        /// <returns></returns>
        public static string GetNameFormatted(this Product_PK product, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var nameFormatted = string.Empty;

            if (product == null) return defaultEmptyValue;

            if (!string.IsNullOrWhiteSpace(product.name) && !string.IsNullOrWhiteSpace(product.product_number))
            {
                nameFormatted = product.name + " (" + product.product_number + ")";
            }
            else if (!string.IsNullOrWhiteSpace(product.name))
            {
                nameFormatted = product.name;
            }
            else if (!string.IsNullOrWhiteSpace(product.product_number))
            {
                nameFormatted = "(" + product.product_number + ")";
            }

            return nameFormatted;
        }

        #endregion

        #region Activity

        /// <summary>
        /// Gets activity name formatted as: <activity.name> + " (" <activity.procedure_number> ")"
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="defaultEmptyValue"></param>
        /// <returns></returns>
        public static string GetNameFormatted(this Activity_PK activity, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var nameFormatted = string.Empty;

            if (activity == null) return defaultEmptyValue;

            if (!string.IsNullOrWhiteSpace(activity.name) && !string.IsNullOrWhiteSpace(activity.procedure_number))
            {
                nameFormatted = activity.name + " (" + activity.procedure_number + ")";
            }
            else if (!string.IsNullOrWhiteSpace(activity.name))
            {
                nameFormatted = activity.name;
            }
            else if (!string.IsNullOrWhiteSpace(activity.procedure_number))
            {
                nameFormatted = "(" + activity.procedure_number + ")";
            }

            return nameFormatted;
        }

        #endregion

        #region Person

        /// <summary>
        /// Gets QPPV person formatted display text as <qppvPerson.FullName> + " (" <qppvCode.qppv_code> ")"
        /// </summary>
        /// <param name="qppvCodeFk"></param>
        /// <param name="defaultEmptyValue"> </param>
        /// <returns></returns>
        public static string GetQppvNameFormatted(this Person_PK qppvPerson, int? qppvCodeFk, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var qppvCodeOperations = new Qppv_code_PKDAL();

            var qppvCode = qppvCodeFk.HasValue ? qppvCodeOperations.GetEntity(qppvCodeFk) : null;
            if (qppvCode == null) return defaultEmptyValue;

            var nameFormatted = String.Empty;

            if (qppvPerson != null && qppvCode.person_FK != null && !string.IsNullOrWhiteSpace(qppvCode.qppv_code))
            {
                nameFormatted = qppvPerson.FullName + " (" + qppvCode.qppv_code + ")";
            }
            else if (qppvPerson != null && !string.IsNullOrWhiteSpace(qppvPerson.FullName))
            {
                nameFormatted = qppvPerson.FullName;
            }
            else if (!string.IsNullOrWhiteSpace(qppvCode.qppv_code))
            {
                nameFormatted = " (" + qppvCode + ")";
            }

            return nameFormatted.Trim();
        }

        #endregion

        #region Organisation

        // Manufacturer
        public static string GetNameFormatted(this Org_in_type_for_manufacturer manufacturer, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var nameFormatted = string.Empty;

            if (manufacturer == null) return defaultEmptyValue;

            nameFormatted = !string.IsNullOrWhiteSpace(manufacturer.ManufacturerName) ? manufacturer.ManufacturerName : string.Empty;

            if (manufacturer.ManufacturerTypeName == "Active substance")
            {
                nameFormatted += !string.IsNullOrWhiteSpace(manufacturer.SubstanceName) ? " (Active Substance=<" + manufacturer.SubstanceName + ">)" : " (Active Substance)";
            }
            else
            {
                nameFormatted += !string.IsNullOrWhiteSpace(manufacturer.ManufacturerTypeName) ? " (" + manufacturer.ManufacturerTypeName + ")" : string.Empty;
            }

            return nameFormatted;
        }

        // Partner
        public static string GetNameFormatted(this Org_in_type_for_partner partner, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var nameFormatted = string.Empty;
            if (partner == null) return defaultEmptyValue;

            nameFormatted = !string.IsNullOrWhiteSpace(partner.PartnerName) ? partner.PartnerName : string.Empty;
            nameFormatted += !string.IsNullOrWhiteSpace(partner.PartnerTypeName) ? " (" + partner.PartnerTypeName + ")" : string.Empty;

            return nameFormatted;
        }

        #endregion

        #region ATC

        public static string GetNameFormatted(this Atc_PK atc, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var nameFormatted = string.Empty;
            if (atc == null) return defaultEmptyValue;

            nameFormatted = !string.IsNullOrWhiteSpace(atc.atccode) ? atc.atccode : Constant.UnknownValue;
            nameFormatted += !string.IsNullOrWhiteSpace(atc.name) ? " (" + atc.name + ")" : string.Empty;

            return nameFormatted;
        }

        public static string GetAtcNameFormatted(Atc_PK atc, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var nameFormatted = string.Empty;
            if (atc == null) return defaultEmptyValue;

            nameFormatted = !string.IsNullOrWhiteSpace(atc.atccode) ? atc.atccode : Constant.UnknownValue;
            nameFormatted += !string.IsNullOrWhiteSpace(atc.name) ? " (" + atc.name + ")" : string.Empty;

            return nameFormatted;
        }

        #endregion

        #region Country

        public static string GetNameFormatted(this Country_PK country, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var nameFormatted = string.Empty;

            if (country == null) return defaultEmptyValue;

            if (!string.IsNullOrWhiteSpace(country.name) && !string.IsNullOrWhiteSpace(country.abbreviation))
            {
                nameFormatted = country.abbreviation + " - " + country.name;
            }
            else if (!string.IsNullOrWhiteSpace(country.name))
            {
                nameFormatted = country.name;
            }
            else if (!string.IsNullOrWhiteSpace(country.abbreviation))
            {
                nameFormatted = country.abbreviation;
            }

            return nameFormatted;
        }

        #endregion

        #region IList<T>

        public static bool SortByField<T>(this List<T> list, string sortingField, SortType sortType = SortType.Asc)
        {
            if (!list.Any()) return false;
            var firstItem = list.First();

            var propertyInfo = firstItem.GetType().GetProperty(sortingField);
            if (propertyInfo == null) return false;
            
            if (sortType == SortType.Asc)
            {
                list.Sort((obj1, obj2) => Comparer.Default.Compare(propertyInfo.GetValue(obj1, null), propertyInfo.GetValue(obj2, null)));
            }
            else
            {
                list.Sort((obj1, obj2) => Comparer.Default.Compare(propertyInfo.GetValue(obj2, null), propertyInfo.GetValue(obj1, null)));
            }

            return true;
        }

        public static List<T> SortByField<T, TProperty>(this List<T> list, Expression<Func<T, TProperty>> propertyLambda, SortType sortType = SortType.Asc)
        {
            if (!list.Any()) return list;

            if (sortType == SortType.Asc)
            {
                list.Sort((obj1, obj2) => Comparer<TProperty>.Default.Compare(propertyLambda.Compile()(obj1), propertyLambda.Compile()(obj2)));
            }
            else
            {
                list.Sort((obj1, obj2) => Comparer<TProperty>.Default.Compare(propertyLambda.Compile()(obj2), propertyLambda.Compile()(obj1)));
            }

            return list;
        }
        #endregion

        #region T[]

        internal static bool In<T>(this T entity, params T[] entities)
        {
            if (entities == null || entities.Count() == 0) return false;

            if (entities.Contains(entity)) return true;
            return false;
        }

        internal static bool NotIn<T>(this T entity, params T[] entities)
        {
            if (entities == null || entities.Count() == 0) return true;

            if (entities.Contains(entity)) return false;
            return true;
        }

        #endregion

        #region ListBox

        public static void SortItemsByText(this ListBox listBox, SortType sortType = SortType.Asc, bool preserveSelection = false)
        {
            var items = listBox.Items;

            var itemsList = new List<ListItem>(items.Count);
            foreach (ListItem item in items)
            {
                item.Selected = preserveSelection;
                itemsList.Add(item);
            }

            if (sortType == SortType.Asc)
            {
                itemsList.Sort((item1, item2) => item1.Text.CompareTo(item2.Text));
            }
            else
            {
                itemsList.Sort((item1, item2) => item1.Text.CompareTo(item2.Text) * (-1));
            }

            listBox.Items.Clear();
            foreach (var item in itemsList)
            {
                listBox.Items.Add(item);
            }
        }

        public static void SortItemsByValue(this ListBox listBox, SortType sortType = SortType.Asc, bool preserveSelection = false)
        {
            var items = listBox.Items;

            var itemsList = new List<ListItem>(items.Count);
            foreach (ListItem item in items)
            {
                item.Selected = preserveSelection;
                itemsList.Add(item);
            }

            if (sortType == SortType.Asc)
            {
                itemsList.Sort((item1, item2) => item1.Value.CompareTo(item2.Value));
            }
            else
            {
                itemsList.Sort((item1, item2) => item1.Value.CompareTo(item2.Value) * (-1));
            }

            listBox.Items.Clear();
            foreach (var item in itemsList)
            {
                listBox.Items.Add(item);
            }
        }

        public static void MoveSelectedItemsTo(this ListBox sourceListBox, ListBox destinationListBox)
        {
            var itemsToMove = new List<ListItem>();
            for (int i = 0; i < sourceListBox.Items.Count; i++)
            {
                if (sourceListBox.Items[i].Selected)
                {
                    itemsToMove.Add(sourceListBox.Items[i]);
                }
            }

            foreach (var listItem in itemsToMove)
            {
                sourceListBox.Items.Remove(listItem);
                destinationListBox.Items.Add(listItem);
            }
        }

        public static void CopySelectedItemsTo(this ListBox sourceListBox, ListBox destinationListBox)
        {
            for (int i = 0; i < sourceListBox.Items.Count; i++)
            {
                if (sourceListBox.Items[i].Selected)
                {
                    destinationListBox.Items.Add(sourceListBox.Items[i]);
                }
            }
        }

        public static void RemoveSelected(this ListBox listBox)
        {
            var itemsToRemove = new List<ListItem>();
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].Selected)
                {
                    itemsToRemove.Add(listBox.Items[i]);
                }
            }

            foreach (var listItem in itemsToRemove)
            {
                listBox.Items.Remove(listItem);
            }
        }

        public static void UnselectAll(this ListBox listBox)
        {
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                listBox.Items[i].Selected = false;
            }
        }

        public static List<string> GetSelectedValues(this ListBox listBox)
        {
            var selectedValues = new List<string>();
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].Selected)
                {
                    selectedValues.Add(listBox.Items[i].Value);
                }
            }
            return selectedValues;
        }

        public static List<ListItem> GetSelectedItems(this ListBox listBox)
        {
            var selectedItems = new List<ListItem>();
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].Selected)
                {
                    selectedItems.Add(listBox.Items[i]);
                }
            }
            return selectedItems;
        }

        public static string GetFirstSelectedValue(this ListBox listBox)
        {
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].Selected)
                {
                    return listBox.Items[i].Value;
                }
            }
            return null;
        }

        public static int? GetFirstSelectedId(this ListBox listBox)
        {
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (!listBox.Items[i].Selected) continue;
                if (ValidationHelper.IsValidInt(listBox.Items[i].Value))
                    return int.Parse(listBox.Items[i].Value);
            }
            return null;
        }

        public static List<int?> GetValidSelectedIds(this ListBox listBox)
        {
            var selectedIds = new List<int?>();
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].Selected)
                {
                    if (ValidationHelper.IsValidInt(listBox.Items[i].Value))
                        selectedIds.Add(int.Parse(listBox.Items[i].Value));
                }
            }
            return selectedIds;
        }

        public static ListItem GetFirstSelectedItem(this ListBox listBox)
        {
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].Selected)
                {
                    return listBox.Items[i];
                }
            }
            return null;
        }

        public static List<string> GetSelectedText(this ListBox listBox)
        {
            var selectedText = new List<string>();
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].Selected)
                {
                    selectedText.Add(listBox.Items[i].Text);
                }
            }
            return selectedText;
        }

        public static int GetNumberOfSelectedItems(this ListBox listBox)
        {
            var numSelectedItems = 0;
            for (var i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.Items[i].Selected)
                {
                    numSelectedItems++;
                }
            }
            return numSelectedItems;
        }

        #endregion

        #region ListItemCollection

        /// <summary>
        /// Gets sorted list of ListItemCollection by provided field and sort type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listItemCollection">Source list item collection</param>
        /// <param name="sourceList">Source list of entities to compare with list item collection</param>
        /// <param name="entityPk">Primary key of entities source list</param>
        /// <param name="sortingField">Sorting field</param>
        /// <param name="sortType">Sort type. Default value is Descending</param>
        /// <returns>Sorted list</returns>
        public static IList<T> GetSortedListByField<T>(this ListItemCollection listItemCollection, IList<T> sourceList, string entityPk, string sortingField, SortType sortType = SortType.Asc)
        {
            var sortedList = new List<T>();

            foreach (var item in sourceList)
            {
                var listItemToAdd = listItemCollection.FindByValue(Convert.ToString(item.GetType().GetProperty(entityPk).GetValue(item, null)));
                if (listItemToAdd != null)
                {
                    sortedList.Add(item);
                }
            }

            sortedList.SortByField(sortingField, sortType);

            return sortedList;
        }

        /// <summary>
        /// Gets sorted list of ListItemCollection by provided field and sort type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEntityPk"> </typeparam>
        /// <typeparam name="TSortingField"> </typeparam>
        /// <param name="listItemCollection">Source list item collection</param>
        /// <param name="sourceList">Source list of entities to compare with list item collection</param>
        /// <param name="entityPk">Primary key of entities source list</param>
        /// <param name="sortingField">Sorting field</param>
        /// <param name="sortType">Sort type. Default value is Descending</param>
        /// <returns>Sorted list</returns>
        public static IList<T> GetSortedListByField<T, TEntityPk, TSortingField>(this ListItemCollection listItemCollection, IList<T> sourceList, Expression<Func<T, TEntityPk>> entityPk, Expression<Func<T, TSortingField>> sortingField, SortType sortType = SortType.Asc)
        {
            var sortedList = new List<T>();

            foreach (var item in sourceList)
            {
                var listItemToAdd = listItemCollection.FindByValue(Convert.ToString(entityPk.Compile()(item)));
                if (listItemToAdd != null)
                {
                    sortedList.Add(item);
                }
            }

            sortedList.SortByField(sortingField, sortType);

            return sortedList;
        }

        #endregion

        #region Views.Shared.UserControl.DropDownList

        /// <summary>
        /// Checks if drop dow list has selected value
        /// </summary>
        /// <param name="dropDownList">Drop down list</param>
        /// <returns>Constant.DropDownList.Selected value if true, string.Empty if false</returns>
        public static string HasValue(this Views.Shared.UserControl.DropDownList dropDownList)
        {
            return Convert.ToString(dropDownList.SelectedValue) != Constant.ControlDefault.DdlValue ? Constant.DropDownList.Value : string.Empty;
        }

        #endregion

        #region Views.Shared.UserControl.ListBoxSr

        /// <summary>
        /// Checks if list box select/remove has selected value
        /// </summary>
        /// <param name="listBoxSr">List box select/remove</param>
        /// <returns>Constant.ListBoxSr.Selected value if true, string.Empty if false</returns>
        public static string HasValue(this Views.Shared.UserControl.ListBoxSr listBoxSr)
        {
            return listBoxSr.LbInput.Items.Count > 0 ? Constant.ListBoxSr.Value : string.Empty;
        }

        #endregion

        #region Views.Shared.UserControl.ListBoxExt

        /// <summary>
        /// Checks if list box extended has selected value
        /// </summary>
        /// <param name="listBoxExt">List box extended</param>
        /// <returns>Constant.ListBoxExt.Selected value if true, string.Empty if false</returns>
        public static string HasValue(this Views.Shared.UserControl.ListBoxExt listBoxExt)
        {
            return listBoxExt.LbInput.Items.Count > 0 ? Constant.ListBoxExt.Value : string.Empty;
        }

        #endregion

        #region Views.Shared.UserControl.ListBox

        /// <summary>
        /// Checks if list box extended has selected value
        /// </summary>
        /// <param name="listBox">List box extended</param>
        /// <returns>Constant.ListBoxExt.Selected value if true, string.Empty if false</returns>
        public static string HasValue(this Views.Shared.UserControl.ListBox listBox)
        {
            return listBox.LbInput.GetFirstSelectedId() != null ? Constant.ListBox.Value : string.Empty;
        }

        #endregion

        #region Views.Shared.UserControl.DateTimeRangeBox

        public static void Clear(this Views.Shared.UserControl.DateTimeRangeBox dateTimeRangeBox)
        {
            dateTimeRangeBox.TextFrom = string.Empty;
            dateTimeRangeBox.TextTo = string.Empty;
        }

        #endregion

        #region Fill user controls

        public static string PropertyName<TSource, TProperty>(this TSource entity, Expression<Func<TProperty>> propertyLambda) where TSource : class
        {
            var member = (MemberExpression)propertyLambda.Body;

            return member.Member.Name;
        }

        public static TProperty PropertyValue<TSource, TProperty>(this TSource entity, Expression<Func<TProperty>> propertyLambda) where TSource : class
        {
            return propertyLambda.Compile()();
        }

        public static TProperty PropertyValue<TSource, TProperty>(this TSource entity, Expression<Func<TSource, TProperty>> propertyLambda) where TSource : class
        {
            return propertyLambda.Compile()(entity);
        }

        //DropDownList
        public static void Fill<T, TPropertyText, TPropertyValue>(this DropDownList dropDownList, IList<T> source, Expression<Func<T, TPropertyText>> textExpr, Expression<Func<T, TPropertyValue>> valueExpr, bool addDefault = true) where T : class
        {
            dropDownList.Items.Clear();

            if (addDefault)
            {
                dropDownList.Items.Add(new ListItem(Constant.ControlDefault.DdlText, Constant.ControlDefault.DdlValue));
            }

            foreach (var item in source)
            {
                var text = Convert.ToString(item.PropertyValue(textExpr));
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    dropDownList.Items.Add(new ListItem(text, value));
            }
        }

        public static void FillAdvanced<T, TPropertyValue>(this DropDownList dropDownList, IList<T> source, Func<T, string> textFunc, Expression<Func<T, TPropertyValue>> valueExpr, bool addDefault = true) where T : class
        {
            dropDownList.Items.Clear();

            if (addDefault)
            {
                dropDownList.Items.Add(new ListItem(Constant.ControlDefault.DdlText, Constant.ControlDefault.DdlValue));
            }

            foreach (var item in source)
            {
                var text = textFunc(item);
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    dropDownList.Items.Add(new ListItem(text, value));
            }
        }

        //ListBox
        public static void Fill<T, TPropertyText, TPropertyValue>(this ListBox listBox, IList<T> source, Expression<Func<T, TPropertyText>> textExpr, Expression<Func<T, TPropertyValue>> valueExpr) where T : class
        {
            listBox.Items.Clear();
            foreach (var item in source)
            {
                var text = Convert.ToString(item.PropertyValue(textExpr));
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    listBox.Items.Add(new ListItem(text, value));
            }
        }

        public static void FillAdvanced<T, TPropertyValue>(this ListBox listBox, IList<T> source, Func<T, string> textFunc, Expression<Func<T, TPropertyValue>> valueExpr) where T : class
        {
            listBox.Items.Clear();
            foreach (var item in source)
            {
                var text = textFunc(item);
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    listBox.Items.Add(new ListItem(text, value));
            }
        }

        //DropDownList
        public static void Fill<T, TPropertyText, TPropertyValue>(this Views.Shared.UserControl.DropDownList dropDownList, IList<T> source, Expression<Func<T, TPropertyText>> textExpr, Expression<Func<T, TPropertyValue>> valueExpr, bool addDefault = true) where T : class
        {
            dropDownList.DdlInput.Items.Clear();

            if (addDefault)
            {
                dropDownList.DdlInput.Items.Add(new ListItem(Constant.ControlDefault.DdlText, Constant.ControlDefault.DdlValue));
            }

            foreach (var item in source)
            {
                var text = Convert.ToString(item.PropertyValue(textExpr));
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    dropDownList.DdlInput.Items.Add(new ListItem(text, value));
            }
        }

        public static void FillAdvanced<T, TPropertyValue>(this Views.Shared.UserControl.DropDownList dropDownList, IList<T> source, Func<T, string> textFunc, Expression<Func<T, TPropertyValue>> valueExpr, bool addDefault = true) where T : class
        {
            dropDownList.DdlInput.Items.Clear();

            if (addDefault)
            {
                dropDownList.DdlInput.Items.Add(new ListItem(Constant.ControlDefault.DdlText, Constant.ControlDefault.DdlValue));
            }

            foreach (var item in source)
            {
                var text = textFunc(item);
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    dropDownList.DdlInput.Items.Add(new ListItem(text, value));
            }
        }

        //ListBoxExt
        public static void Fill<T, TPropertyText, TPropertyValue>(this Views.Shared.UserControl.ListBoxExt listBoxExt, IList<T> source, Expression<Func<T, TPropertyText>> textExpr, Expression<Func<T, TPropertyValue>> valueExpr, bool fillData = false) where T : class
        {
            listBoxExt.LbInput.Items.Clear();
            foreach (var item in source)
            {
                var text = Convert.ToString(item.PropertyValue(textExpr));
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                {
                    listBoxExt.LbInput.Items.Add(new ListItem(text, value));

                    if (fillData)
                    {
                        listBoxExt.AddEntityToData(value, item);
                    }
                }
            }
        }

        public static void FillAdvanced<T, TPropertyValue>(this Views.Shared.UserControl.ListBoxExt listBoxExt, IList<T> source, Func<T, string> textFunc, Expression<Func<T, TPropertyValue>> valueExpr, bool fillData = false) where T : class
        {
            listBoxExt.LbInput.Items.Clear();
            foreach (var item in source)
            {
                var text = textFunc(item);
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                {
                    listBoxExt.LbInput.Items.Add(new ListItem(text, value));

                    if (fillData)
                    {
                        listBoxExt.AddEntityToData(value, item);
                    }
                }
            }
        }

        //ListBoxSr
        public static void Fill<T, TPropertyText, TPropertyValue>(this Views.Shared.UserControl.ListBoxSr listBoxSr, IList<T> source, Expression<Func<T, TPropertyText>> textExpr, Expression<Func<T, TPropertyValue>> valueExpr) where T : class
        {
            listBoxSr.LbInput.Items.Clear();
            foreach (var item in source)
            {
                var text = Convert.ToString(item.PropertyValue(textExpr));
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    listBoxSr.LbInput.Items.Add(new ListItem(text, value));
            }
        }

        public static void FillAdvanced<T, TPropertyValue>(this Views.Shared.UserControl.ListBoxSr listBoxSr, IList<T> source, Func<T, string> textFunc, Expression<Func<T, TPropertyValue>> valueExpr) where T : class
        {
            listBoxSr.LbInput.Items.Clear();
            foreach (var item in source)
            {
                var text = textFunc(item);
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    listBoxSr.LbInput.Items.Add(new ListItem(text, value));
            }
        }

        //ListBoxAu
        public static void Fill<T, TPropertyText, TPropertyValue>(this Views.Shared.UserControl.ListBoxAu listBoxAu, ListBox listBox, IList<T> source, Expression<Func<T, TPropertyText>> textExpr, Expression<Func<T, TPropertyValue>> valueExpr) where T : class
        {
            listBox.Items.Clear();
            foreach (var item in source)
            {
                var text = Convert.ToString(item.PropertyValue(textExpr));
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    listBox.Items.Add(new ListItem(text, value));
            }
        }

        public static void FillAdvanced<T, TPropertyValue>(this Views.Shared.UserControl.ListBoxAu listBoxAu, ListBox listBox, IList<T> source, Func<T, string> textFunc, Expression<Func<T, TPropertyValue>> valueExpr) where T : class
        {
            listBox.Items.Clear();
            foreach (var item in source)
            {
                var text = textFunc(item);
                var value = Convert.ToString(item.PropertyValue(valueExpr));

                if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
                    listBox.Items.Add(new ListItem(text, value));
            }
        }
        #endregion
    }
}