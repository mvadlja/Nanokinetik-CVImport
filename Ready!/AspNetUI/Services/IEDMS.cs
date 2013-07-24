using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AspNetUI.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEDMS" in both code and config file together.
    [ServiceContract]
    public interface IEDMS2
    {
        [OperationContract]
        void DoWork();
    }
}
