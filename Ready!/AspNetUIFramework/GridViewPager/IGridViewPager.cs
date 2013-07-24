using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace AspNetUIFramework
{
    public interface IGridViewPager
    {
        int CurrentPage { get; set; }
        int RecordsPerPage { get; set; }
        int TotalRecordsCount { get; set; }
        string SortOrderBy { get; set; }
        bool SortReverseOrder { get; set; }
        event EventHandler<PageChangedEventArgs> PageChanged;
        void BindGridViewPager();
        bool Visible { get; set; }
        string AssociatedGridViewID { get; set; }
    }
}
