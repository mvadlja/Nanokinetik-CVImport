using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;

namespace AspNetUIFramework
{
    public interface IMasterPageOperationalSupport
    {
        // Returns business service by interface type
        XmlLocation CurrentLocation { get; set; }
        AppUser CurrentUser { get; }
        RightTypes RightTypeOfCurrentUserForThisPage { get; set; }
        IViewStateController ViewStateController { get; }
        IContextMenu ContextMenu { get; }
        IModalPopup MessageModalPopup { get; }
        IModalPopup ConfirmModalPopup { get; }
        void HandleClassicException(Exception ex);
        void HandleAjaxException(Exception ex);
    }
}
