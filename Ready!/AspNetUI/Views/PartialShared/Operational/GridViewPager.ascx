<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewPager.ascx.cs"
    Inherits="AspNetUI.Support.GridViewPager" %>
<div id="Pagination">
    Page
    <asp:Label ID="txtJumpToPage" runat="server"></asp:Label>
    of
    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
    (<asp:Label ID="lblPagerStatus" runat="server" Text="items count"></asp:Label>
    items)&nbsp;&nbsp;&nbsp;
    <asp:ImageButton ID="ibPreviousPage" runat="server" ToolTip="Previous page" CommandArgument="ibPreviousPage"
        ImageAlign="AbsMiddle" ImageUrl="../GridViewPager_Resources/previous_page.png"
        OnClick="ibGoToPage_Click" />
    <asp:Label ID="pagesHolder" runat="server"></asp:Label>
    <asp:ImageButton ID="ibNextPage" runat="server" ToolTip="Next page" CommandArgument="ibNextPage"
        ImageAlign="AbsMiddle" ImageUrl="../GridViewPager_Resources/next_page.png" OnClick="ibGoToPage_Click" />
</div>