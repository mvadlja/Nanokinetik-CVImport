using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUIFramework
{
    public abstract class FormHolder : Page
    {
        public FormHolder() : base() { }

        private List<UserControl> _allPageForms = null;

        #region Properties

        public IMasterPageOperationalSupport MasterPage
        {
            get { return this.Master as IMasterPageOperationalSupport; }
        }

        // IMasterPageOperationalSupport special properties
        public List<XmlLocation> TopMenuLocations
        {
            get { return MasterPage.GetType().GetProperty("TopMenuLocations").GetValue(MasterPage, null) as List<XmlLocation>; }
        }

        public List<XmlLocation> LeftMenuLocations
        {
            get { return MasterPage.GetType().GetProperty("LeftMenuLocations").GetValue(MasterPage, null) as List<XmlLocation>; }
        }


        // Local properties
        public string InitialForm
        {
            get { return ViewState["InitialUserControls_" + this.ID] == null ? null : ViewState["InitialUserControls_" + this.ID].ToString(); }
            set { ViewState["InitialUserControls_" + this.ID] = value; }
        }

        public List<UserControl> AllForms
        {
            get 
            {
                if (_allPageForms == null)
                {
                    _allPageForms = new List<UserControl>();

                    foreach (Control c in Page.Controls)
                    {
                        FindAllPageForms(c, ref _allPageForms);
                    }
                }

                return _allPageForms;                     
            }
        }

        #endregion

        #region ASP.NET events

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Initialize services
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Handler for forms context menu items click
            MasterPage.ContextMenu.OnContextMenuItemClick += new EventHandler<ContextMenuEventArgs>(OnContextMenuItemClick);

            // Attaching events for list form
            // Example: Applications_list1.OnListItemSelected += new EventHandler<FormListEventArgs>(Applications_list1_OnListItemSelected);
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                // Select default form on page load
                if (InitialForm != null)
                {
                    MasterPage.ViewStateController.SelectedForm = InitialForm;
                }

                ShowSelectedForm();
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
        }

        #endregion


        /// <summary>
        /// Context menu click handler for this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void OnContextMenuItemClick(object sender, ContextMenuEventArgs e);

        /// <summary>
        /// Shows selected form
        /// </summary>
        public abstract void ShowSelectedForm();

        /// <summary>
        /// Retreives all forms on page
        /// </summary>
        /// <param name="c"></param>
        /// <param name="foundForms"></param>
        private void FindAllPageForms(Control c, ref List<UserControl> foundForms)
        {
            if ((c is IFormList || c is IFormDetails) && c is UserControl)
            {
                foundForms.Add(c as UserControl);
            }
            else
            {
                // Recursion
                foreach (Control cChild in c.Controls)
                {
                    FindAllPageForms(cChild, ref foundForms);
                }
            }
        }
    }
}
