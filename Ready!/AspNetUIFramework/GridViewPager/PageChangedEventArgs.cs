using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    [Serializable]
    public class PageChangedEventArgs : EventArgs
    {
        private int _newPageIndex;

        public int NewPageIndex
        {
            get { return _newPageIndex; }
            set { _newPageIndex = value; }
        }

        public PageChangedEventArgs() { }
        public PageChangedEventArgs(int newPageIndex)
        {
            this.NewPageIndex = newPageIndex;
        }
    }
}
