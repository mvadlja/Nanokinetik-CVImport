using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IControlStates
    {
        ControlState CurrentControlState { get; set; }
        bool ControlSupportsState(ControlState state);
    }
}
