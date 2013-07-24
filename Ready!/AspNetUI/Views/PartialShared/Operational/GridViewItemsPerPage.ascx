<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewItemsPerPage.ascx.cs" Inherits="AspNetUI.Views.PartialShared.Operational.IPPGridViewPager" %>
<asp:Panel ID="GridItemsPerPage" runat="server" style="margin-top:-20px; height:20px; margin-left:5px;" >
    Per page: 
    <asp:LinkButton OnClick="itemPerPageChanged" runat="server" ID="PerPage15" style="margin-left:3px; text-decoration:none;" >
        15
    </asp:LinkButton>
     <asp:LinkButton ID="PerPage50" OnClick="itemPerPageChanged" runat="server" style="margin-left:3px; text-decoration:none;"  >
        50
    </asp:LinkButton>
     <asp:LinkButton ID="PerPage100" OnClick="itemPerPageChanged" runat="server" style="margin-left:3px; text-decoration:none;">
        100
    </asp:LinkButton>
</asp:Panel>