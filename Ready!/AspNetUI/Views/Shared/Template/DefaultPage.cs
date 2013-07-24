using System;
using System.Globalization;
using System.Web.UI;
using AspNetUI.Support;

namespace AspNetUI.Views.Shared.Template
{
    public abstract class DefaultPage : Page
    {
        #region Properties

        public Default MasterPage;
        public MasterMain MasterPageOld;
        public PageType PageType { get; set; }
        public EntityContext EntityContext { get; set; }
        public CultureInfo CultureInfoHr { get; set; }
        public string From { get; set; }
        public EntityContext RefererEntityContext
        {
            get { return LocationManager.ResolveEntityContext(MasterPage.RefererLocation); }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (MasterPage == null)
            {
                LoadFormVariables();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        #endregion

        #region Page virtual methods

        public virtual void LoadFormVariables()
        {
            MasterPage = (Default)Page.Master;

            CultureInfoHr = new CultureInfo("hr-HR");
            From = Request.QueryString["From"] ?? Request.QueryString["from"];

            if (MasterPage != null) EntityContext = MasterPage.EntityContext;
        }

        public virtual object SaveForm(object arg)
        {
            MasterPage.OneTimePermissionToken = Permission.View;
            return null;
        }

        public virtual object DeleteItem(object arg)
        {
            return null;
        }

        public virtual bool SecurityPageSpecific()
        {
            return false;
        }

        #endregion
        
        #region Support methods

      
        #endregion
    }
}