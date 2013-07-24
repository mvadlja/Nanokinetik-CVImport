using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace AspNetUIFramework
{
    public interface IControlList
    {
        string SourceValueProperty { get; set; }
        string SourceTextExpression { get; set; }
        void FillControl<T>(IList<T> source);
        ListItemCollection ControlBoundItems { get; }
    }
}
