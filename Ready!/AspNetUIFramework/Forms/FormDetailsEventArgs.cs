using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public class FormDetailsEventArgs : EventArgs
    {
        private object _dataItem;

        public object DataItem
        {
            get { return _dataItem; }
            set { _dataItem = value; }
        }

        public FormDetailsEventArgs() { }
        public FormDetailsEventArgs(object dataItem)
        {
            this.DataItem = dataItem;
        }
    }
}
