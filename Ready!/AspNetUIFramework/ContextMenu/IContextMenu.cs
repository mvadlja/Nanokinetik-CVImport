using System;

namespace AspNetUIFramework
{
    public interface IContextMenu
    {
        event EventHandler<ContextMenuEventArgs> OnContextMenuItemClick;

        void SetContextMenuItemsEnabled(ContextMenuItem[] contextMenuItems);
        void SetContextMenuItemsDisabled(ContextMenuItem[] contextMenuItems);
        void SetContextMenuItemsVisible(ContextMenuItem[] contextMenuItems);
    }
}
