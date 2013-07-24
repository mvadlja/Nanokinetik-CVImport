using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public class ValueChangedEventArgs : EventArgs
    {
        public object NewValue { get; set; }
        public object[] NewValues { get; set; }

        public ValueChangedEventArgs() { }
        public ValueChangedEventArgs(object newValue)
        {
            this.NewValue = newValue;
        }
        public ValueChangedEventArgs(object[] newValues)
        {
            this.NewValues = newValues;
        }
    }
}
