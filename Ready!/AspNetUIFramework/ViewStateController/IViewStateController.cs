using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IViewStateController
    {
        void AddSelectedEntityID(object id);
        bool CheckIfEntityExists(int index);
        void RemoveAllEntityIDs();
        void RemoveLastSelectedEntityID();
        int SelectedEntitiesCount { get; }
        object[] SelectedEntityIDs { get; }
        string SelectedForm { get; set; }
    }
}
