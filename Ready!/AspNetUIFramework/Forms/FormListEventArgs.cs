using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public class FormListEventArgs : EventArgs
    {
        private object _dataItemID;

        public object DataItemID
        {
            get { return _dataItemID; }
            set { _dataItemID = value; }
        }

        public FormListEventArgs() { }
        public FormListEventArgs(object dataItemID)
        {
            this.DataItemID = dataItemID;
        }
    }
}
