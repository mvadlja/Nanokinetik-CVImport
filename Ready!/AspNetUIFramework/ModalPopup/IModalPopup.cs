using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetUIFramework
{
    public interface IModalPopup
    {
        string ModalPopupContainerBodyPadding { get; set; }
        string ModalPopupContainerHeight { get; set; }
        string ModalPopupContainerWidth { get; set; }
        void ShowModalPopup(string header, string message);
    }
}
