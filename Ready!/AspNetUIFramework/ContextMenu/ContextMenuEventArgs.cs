using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    [Serializable()]
    public class ContextMenuEventArgs : EventArgs
    {
        private ContextMenuEventTypes _eventType;

        public ContextMenuEventTypes EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        public ContextMenuEventArgs(ContextMenuEventTypes eventType)
            : base()
        {
            this.EventType = eventType;
        }
    }
}
