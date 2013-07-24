using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IFormDetails : IFormCommon
    {
        object SaveForm(object id, string arg);
    }
}
