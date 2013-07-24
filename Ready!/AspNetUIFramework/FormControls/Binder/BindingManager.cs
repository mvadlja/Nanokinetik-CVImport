using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace AspNetUIFramework
{
    public class BindingManager
    {
        private Panel _bindingRoot;

        #region Properties

        public Panel BindingRoot
        {
            get { return _bindingRoot; }
        }

        #endregion

        public BindingManager(Panel bindingRoot)
        {
            _bindingRoot = bindingRoot;
        }

        public void DataBind(object bindingObject)
        {
            if (_bindingRoot != null)
            {
                foreach (Control c in _bindingRoot.Controls)
                {
                    BindRecursively(c, bindingObject);
                }
            }
        }

        public TObj FillObjectFromBoundForm<TObj>(TObj initialState) where TObj : class, new()
        {
            if (initialState == null)
            {
                initialState = new TObj();
            }



            return initialState;
        }

        #region Helpers

        private void BindRecursively(Control c, object bindingObject)
        {
            // Skipping bindingRoot panel
            if (c is Panel && ((c as Panel).Attributes["bindingRoot"] != null && (c as Panel).Attributes["bindingRoot"].ToLower() == "true"))
            {
                // Do nothing, skipp this control because it is binding root
            }
            else if (c is IControlCommon)
            {
                IControlCommon tempC = c as IControlCommon;

                if (!String.IsNullOrEmpty(tempC.BindingPath))
                {
                    object valueToBind = ExtractValueFromBindingObjectPath(bindingObject, tempC.BindingPath);
                    tempC.ControlValue = tempC.SerializeValue(valueToBind);
                }
            }

            // Continuing recursion
            if (c.Controls != null)
            {
                foreach (Control innerControl in c.Controls)
                {
                    BindRecursively(innerControl, bindingObject);
                }
            }
        }

        // TODO: implement indexer support!
        private object ExtractValueFromBindingObjectPath(object bindingObject, string path)
        {
            object value = bindingObject;
            string[] pathParts = path.Split(new string[] { "." }, StringSplitOptions.None);

            foreach (string part in pathParts)
            {
                value = value.GetType().GetProperty(part).GetValue(value, null);
            }

            return value;
        }

        private object FillObjectRecursively(object initialState)
        {



            return initialState;
        }

        #endregion


    }
}
