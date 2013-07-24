<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateBox_CT.ascx.cs" Inherits="AspNetUI.Support.DateBox_CT" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<table id="tableCanvas" runat="server" cellpadding="2" cellspacing="0">
    <tr>
        <td id="tdLabel" runat="server" style="text-align: right">
            <asp:Label ID="ctlLabel" runat="server" Text="Label" />            
        </td>
        <td style="width: 8px">
            <asp:Label ID="ctlMark" runat="server" Text="*" style="color: Red;" Visible="false" />
        </td>
        <td id="tdControl" runat="server" style="white-space:nowrap;">
            <asp:TextBox ID="ctlInput" runat="server" MaxLength="10" Width="70px" ontextchanged="ctlInput_TextChanged" />
            <asp:Image ID="ctlInputImg" runat="server" ImageUrl="~/Images/calendar.png" style="vertical-align: middle;"/>
            <%--<asp:LinkButton ID="ctlInputImg" runat="server" Text="" Width="24px" >
                <asp:Image ID="calendarImage" runat="server" ImageUrl="~/Images/calendar.png" style="vertical-align: middle;"/>
            </asp:LinkButton>--%>
            <asp:CalendarExtender ID="ctlInput_CalendarExtender" FirstDayOfWeek="Monday" runat="server" Format="yyyy-MM-dd" Enabled="True" PopupButtonID="ctlInputImg" TargetControlID="ctlInput">
            </asp:CalendarExtender>
        </td>
        <td id="tdInputUnitsLabel" runat="server" visible="false">
            <asp:Label ID="ctlInputUnitsLabel" runat="server" />
        </td>
        <td style="width: 24px" runat="server" id="descriptionHolder" visible="false">
            <asp:Image ID="ctlDescription" runat="server" Width="16" ImageUrl="~/Images/info.png" style="padding: 2px;" />
        </td>
        <td style="width: 24px" runat="server" id="alertHolder" visible="false">
            <asp:Image ID="ctlAlert" runat="server" Width="20" ImageUrl="~/Images/alert.png" style="padding: 2px;" />
        </td>
        <td style="width: 24px" runat="server" id="reminderHolder" visible="false">
            <asp:LinkButton ID="lnkSetReminder" runat="server" CssClass="boldText" OnClick="lnkSetReminderClick" Text="" Width="24px">
                <asp:Image ID="imgSetReminder" runat="server" ImageUrl="~/Images/alarm.png" />
            </asp:LinkButton>
        </td>
    </tr>
</table>