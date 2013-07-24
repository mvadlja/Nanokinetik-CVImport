using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    [Serializable()]
    public class ContextMenuItem
    {
        private ContextMenuEventTypes _eventType;
        private string _toolTip;

        public ContextMenuEventTypes EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        public string ToolTip
        {
            get { return _toolTip; }
            set { _toolTip = value; }
        }

        public ContextMenuItem() { }
        public ContextMenuItem(ContextMenuEventTypes eventType, string toolTip)
        {
            this.EventType = eventType;
            this.ToolTip = toolTip;
        }
    }
}
