using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetUIFramework;

namespace AspNetUI.Support
{
    public partial class ViewStateController : System.Web.UI.UserControl, IViewStateController
    {
        #region IViewStateController Members

        // Adding object to view state collection
        public void AddSelectedEntityID(object id)
        {
            object[] tempArray;

            if (SelectedEntityIDs != null)
            {
                tempArray = new object[SelectedEntityIDs.Length + 1];

                for (int i = 0; i < SelectedEntityIDs.Length; i++)
                {
                    tempArray[i] = SelectedEntityIDs[i];
                }

                tempArray[tempArray.Length - 1] = id;
            }
            else
            {
                tempArray = new object[] { id };
            }

            SelectedEntityIDs = tempArray;
        }

        public bool CheckIfEntityExists(int index)
        {
            try
            {
                object entity = SelectedEntityIDs[index];
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Removing all objects from view state collection
        public void RemoveAllEntityIDs()
        {
            SelectedEntityIDs = null;
        }

        // Removing object from view state collection
        public void RemoveLastSelectedEntityID()
        {
            object[] tempArray;

            if (SelectedEntityIDs != null)
            {
                if (SelectedEntityIDs.Length == 1)
                {
                    SelectedEntityIDs = null;
                }
                else
                {
                    tempArray = new object[SelectedEntityIDs.Length - 1];

                    for (int i = 0; i < (SelectedEntityIDs.Length - 1); i++)
                    {
                        tempArray[i] = SelectedEntityIDs[i];
                    }

                    SelectedEntityIDs = tempArray;
                }
            }
        }

        public int SelectedEntitiesCount
        {
            get { return SelectedEntityIDs == null ? 0 : SelectedEntityIDs.Length; }
        }

        public object[] SelectedEntityIDs
        {
            get { return ViewState["SelectedObjectIDs"] == null ? null : (object[])ViewState["SelectedObjectIDs"]; }
            private set { ViewState["SelectedObjectIDs"] = value; }
        }

        public string SelectedForm
        {
            get { return ViewState["SelectedForm"] == null ? String.Empty : ViewState["SelectedForm"].ToString(); }
            set { ViewState["SelectedForm"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}