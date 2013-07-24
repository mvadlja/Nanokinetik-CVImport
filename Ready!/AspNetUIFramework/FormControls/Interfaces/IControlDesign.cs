using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AspNetUIFramework
{
    public interface IControlDesign
    {
        string TotalControlWidth { get; set; }
        string LabelColumnWidth { get; set; }
        string ControlInputWidth { get; set; }
        string ControlLabelAlign { get; set; }
        bool FontItalic { get; set; }
        bool FontBold { get; set; }
        Color FontColor { get; set; }
        bool FontValueBold { get; set; }
    }
}
