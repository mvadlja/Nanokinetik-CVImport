using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IControlBehavior
    {
        bool AutoPostback { get; set; }
        event EventHandler<ValueChangedEventArgs> InputValueChanged;
    }
}
