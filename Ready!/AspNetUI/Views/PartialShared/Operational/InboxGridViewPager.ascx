<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InboxGridViewPager.ascx.cs" Inherits="AspNetUI.Support.InboxGridViewPager" %>

<table cellpadding="0" cellspacing="0" id="MainTable" runat="server" class="pagerStyle">
    <tr>
        <td>
            <center>
                <table cellpadding="2" cellspacing="0" style="height: 24px">
                    <tr>
                        <td><asp:ImageButton ID="ibFirstPage" runat="server" ToolTip="First page" CommandArgument="ibFirstPage" ImageAlign="AbsMiddle" ImageUrl="../GridViewPager_Resources/paging_arrow_left_double.gif" OnClick="ibGoToPage_Click" /></td>
                        <td><asp:ImageButton ID="ibPreviousPage" runat="server" ToolTip="Previous page" CommandArgument="ibPreviousPage" ImageAlign="AbsMiddle" ImageUrl="../GridViewPager_Resources/paging_arrow_left.gif" OnClick="ibGoToPage_Click" /></td>
                        <td><asp:TextBox ID="txtCurrentPage" runat="server" Width="30px"  style="text-align: right; font-family: Verdana; font-size: 10px; border-width: 0px" MaxLength="4"></asp:TextBox></td>
                        <td><asp:ImageButton ID="ibNextPage" runat="server" ToolTip="Next page" CommandArgument="ibNextPage" ImageAlign="AbsMiddle" ImageUrl="../GridViewPager_Resources/paging_arrow_right.gif" OnClick="ibGoToPage_Click" /></td>
                    </tr>
                </table>
            </center>
        </td>
    </tr>
</table>
