<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearcherDisplay.ascx.cs" Inherits="AspNetUI.Support.SearcherDisplay" %>
<asp:Panel ID="pnlSearcher" runat="server">
    <table cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:TextBox ID="txtSearcherData" runat="server" ReadOnly="true" Width="330px" Visible="true" style="background-color:#F4F4F4;"></asp:TextBox>
            </td>
            <td>
                <asp:LinkButton ID="lbtSearch" runat="server" Text="Select" CssClass="boldText" OnClick="lbtSearch_Click"/>
            </td>
            <td>
                <asp:LinkButton ID="lbtClear" runat="server" Text="| Remove" CssClass="boldText" OnClick="lbtClear_Click" Visible="true" Width="60px"/>
            </td>
        </tr>
    </table>
</asp:Panel>