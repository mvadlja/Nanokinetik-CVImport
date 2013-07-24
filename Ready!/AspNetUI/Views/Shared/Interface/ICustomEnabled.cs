using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.Shared.Interface
{
    public interface ICustomEnabled
    {
        void Enable(List<WebControl> controlList = null);
        void Disable(List<WebControl> controlList = null);
    }
}
