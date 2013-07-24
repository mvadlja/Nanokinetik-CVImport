using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views
{
    public partial class SSIFView : FormHolder
    {
        MasterMain m = null;
        
        // View initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            m = (MasterMain)Page.Master;
        }

        // Context menu click handler for this form
        public override void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {

            switch (e.EventType)
            {
                // Back
                case ContextMenuEventTypes.Back:
                    XmlLocation parentLocation = AspNetUIFramework.LocationManager.Instance.GetLocationByName(m.CurrentLocation.ParentLocationID, AspNetUIFramework.CacheManager.Instance.AppLocations);

                    Response.Redirect(parentLocation.LocationUrl);

                    break;

                case ContextMenuEventTypes.Save:
                   
                        SSIForm.Save();
                    

                    //Response.Redirect(parentLocation.LocationUrl);

                    break;
                default:

                    break;
            }
        }

        // Displays correct form 
        public override void ShowSelectedForm()
        {
            if (Request.QueryString["f"] != null)
            {
                switch (Request.QueryString["f"].ToString())
                {
                    case "dn": // details new view
                       
                        SSIFForm_details1.ShowForm("");
                        SSIFForm_details1.BindForm(null, "");                       
                        break;
                }
            }
        }


    }
}