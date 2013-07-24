using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IControlData
    {
        object ControlValue { get; set; }
        string ControlValueFormat { get; set; }
        string ControlLabel { get; set; }
        string ControlInputUnitsLabel { get; set; }
        string ControlDescriptor { get; set; }
        string ControlErrorMessage { get; set; }
        string ControlEmptyErrorMessage { get; set; }
        bool IsMandatory { get; set; }
        int MaxLength { get; set; }
        // added
        string ControlTextValue { get; }
    }
}
