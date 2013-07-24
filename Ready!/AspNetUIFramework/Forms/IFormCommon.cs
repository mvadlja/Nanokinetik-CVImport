using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IFormCommon
    {
        void ShowForm(string arg);
        void HideForm(string arg);
        void ClearForm(string arg);
        void FillDataDefinitions(string arg);
        void BindForm(object id, string arg);
        bool ValidateForm(string arg);
    }
}
