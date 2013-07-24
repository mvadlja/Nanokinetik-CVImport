<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContextMenu.ascx.cs" Inherits="AspNetUI.Support.ContextMenu" %>

<div id="ContextMenuLayout" runat="server" class="controlButtons" visible="true">
        <asp:LinkButton ID="lbtBack" visible="false" runat="server" CssClass="button Back" CommandArgument="Back" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Back" />
        <asp:LinkButton ID="lbtNew" visible="false" runat="server" CssClass="button New" CommandArgument="New" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="New entry" />
        <asp:LinkButton ID="lbtCreateTemplate" visible="false" runat="server" CssClass="button CreateTemplate" CommandArgument="CreateTemplate" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Create template" />
        <asp:LinkButton ID="lbtCancel" visible="false" runat="server" CssClass="button Cancel" CommandArgument="Cancel" CommandName="EventType" OnClick="ContextMenuItem_Click" Text=" Cancel" />
        <asp:LinkButton ID="lbtSave" visible="false" runat="server" CssClass="button Save" CommandArgument="Save" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Save" />
        <asp:LinkButton ID="lbtCreateCalculation" visible="false" runat="server" CssClass="button CreateCalculation" CommandArgument="CreateCalculation" CommandName="EventType" OnClick="ContextMenuItem_Click" Text=" Create calculation" />
        <asp:LinkButton ID="lbtUpdateCalculation" visible="false" runat="server" CssClass="button UpdateCalculation" CommandArgument="UpdateCalculation" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Update calculation" />
        <asp:LinkButton ID="lbtEdit" visible="false" runat="server" CssClass="button Edit" CommandArgument="Edit" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Edit" />
        <asp:LinkButton ID="lbtDelete" visible="false" runat="server" CssClass="button Delete marginLeft" CommandArgument="Delete" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Delete" />
        <asp:LinkButton ID="lbtSaveAs" visible="false" runat="server" CssClass="button SaveAs marginLeft" CommandArgument="SaveAs" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Save As" />
        <asp:LinkButton ID="lbtPreviousItem" Visible="false" runat="server" CssClass="button PrevItem marginLeft" CommandArgument="PreviousItem" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Previous" />
        <asp:LinkButton ID="lbtNextItem" Visible="false" runat="server" CssClass="button NextItem" CommandArgument="NextItem" CommandName="EventType" OnClick="ContextMenuItem_Click" Text="Next" />
       <%-- <asp:LinkButton ID="LinkButton1" visible="false" runat="server" CssClass="button Delete marginLeft" CommandArgument="Delete" CommandName="EventType" OnClientClick="DisplayConfirmModalPopup(\'Warning!\', \'Are you sure that you want to delete this record?\', this, ''); return false;"  OnClick="ContextMenuItem_Click" Text="Delete" />
        --%>
        <span id="additionalContextMenuItems"></span>
</div>
