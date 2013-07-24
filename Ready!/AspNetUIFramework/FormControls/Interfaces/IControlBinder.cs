using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IControlBinder
    {
        string BindingPath { get; set; }
        string SerializeValue(object input);
        object DeserializeValue(string input);
    }
}
