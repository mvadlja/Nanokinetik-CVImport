using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public class FormPopupEventArgs : EventArgs
    {
        private object _data;
        private string _action;

        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public FormPopupEventArgs() { }

        public FormPopupEventArgs(object data)
        {
            this.Data = data;
        }

        public FormPopupEventArgs(string action)
        {
            this.Action = action;
        }

        public FormPopupEventArgs(string action, object data)
        {
            this.Action = action;
            this.Data = data;
        }
    }
}
