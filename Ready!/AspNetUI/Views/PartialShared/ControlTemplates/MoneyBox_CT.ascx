<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MoneyBox_CT.ascx.cs" Inherits="AspNetUI.Support.MoneyBox_CT" %>

<table id="tableCanvas" runat="server" cellpadding="2" cellspacing="0">
    <tr>
        <td id="tdLabel" runat="server" style="text-align: right">
            <asp:Label ID="ctlLabel" runat="server" Text="Label" />            
        </td>
        <td style="width: 8px">
            <asp:Label ID="ctlMark" runat="server" Text="*" style="color: Red;" Visible="false" />
        </td>
        <td id="tdControl" runat="server">
            <asp:TextBox ID="ctlInput" runat="server" ontextchanged="ctlInput_TextChanged" style="text-align: right" />
        </td>
        <td id="tdInputUnitsLabel" runat="server" visible="true">
            <asp:Label ID="ctlInputUnitsLabel" runat="server" Text="kn" />
        </td>
        <td style="width: 24px">
            <asp:Image ID="ctlDescription" runat="server" Width="16" ImageUrl="~/Images/info.png" style="padding: 2px;" Visible="true" ToolTip="Examle: 1000,00" />
        </td>
        <td style="width: 24px">
            <asp:Image ID="ctlAlert" runat="server" Width="20" ImageUrl="~/Images/alert.png" style="padding: 2px;" Visible="false" />
        </td>
    </tr>
</table>