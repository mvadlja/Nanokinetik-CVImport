using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public class ItemSelectedEventArgs : EventArgs
    {
        public string SelectedItemID { get; set; }

        public ItemSelectedEventArgs(string selectedItemID)
        {
            this.SelectedItemID = selectedItemID;
        }
    }
}
