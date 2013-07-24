<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextArea_CT.ascx.cs" Inherits="AspNetUI.Support.TextArea_CT" %>

<table id="tableCanvas" runat="server" cellpadding="2" cellspacing="0">
    <tr>
        <td id="tdLabel" runat="server" style="text-align: right; vertical-align: top">
            <asp:Label ID="ctlLabel" runat="server" Text="Label" />            
        </td>
        <td style="width: 8px; vertical-align: top">
            <asp:Label ID="ctlMark" runat="server" Text="*" style="color: Red;" Visible="false" />
        </td>
        <td id="tdControl" runat="server">
            <asp:TextBox ID="ctlInput" TextMode="MultiLine" Rows="5" runat="server" 
                ontextchanged="ctlInput_TextChanged" />
        </td>
        <td id="tdInputUnitsLabel" runat="server" visible="false">
            <asp:Label ID="ctlInputUnitsLabel" runat="server" />
        </td>
        <td style="width: 24px; vertical-align: top">
            <asp:Image ID="ctlDescription" runat="server" Width="16" ImageUrl="~/Images/info.png" style="padding: 2px;" Visible="false" />
        </td>
        <td style="width: 24px; vertical-align: top">
            <asp:Image ID="ctlAlert" runat="server" Width="20" ImageUrl="~/Images/alert.png" style="padding: 2px;" Visible="false" />
        </td>
    </tr>
</table>