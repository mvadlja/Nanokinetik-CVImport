using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IFormList : IFormCommon
    {
        void Search(string arg);
        void DeleteItem(object id, string arg);
        void SetDefaultGVSettings();
        void ShowFormWithoutClearing();

        event EventHandler<FormListEventArgs> OnListItemSelected;
    }
}
