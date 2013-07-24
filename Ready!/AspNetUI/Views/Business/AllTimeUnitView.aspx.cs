using System;
using AspNetUIFramework;

namespace AspNetUI.Views.Business
{
    public partial class AllTimeUnitViewOld : Shared.Template.DefaultPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PageType = PageType.List;
            LoadFormVariables();
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.SetContextMenuItemsVisible(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New Entry") });
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }
        }

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                default:
                    break;
            }
        }
    }
}