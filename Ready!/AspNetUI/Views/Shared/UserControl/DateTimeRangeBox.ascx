<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="DateTimeRangeBox.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.DateTimeRangeBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<div ID="divDateBox" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtInputFrom"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="inputFrom">
        From:
        <asp:TextBox ID="txtInputFrom" runat="server" CssClass="date"></asp:TextBox>
        <asp:Image ID="imgDateTimeFrom" runat="server" ImageUrl="~/Images/calendar.png" /> 
        <act:CalendarExtender ID="ceInputFrom" FirstDayOfWeek="Monday" runat="server" Format="dd.MM.yyyy" Enabled="True" PopupButtonID="imgDateTimeFrom" TargetControlID="txtInputFrom">
        </act:CalendarExtender>
    </div>
    <div class="inputTo">
        To:
        <asp:TextBox ID="txtInputTo" runat="server" CssClass="date"></asp:TextBox>
        <asp:Image ID="imgDateTimeTo" runat="server" ImageUrl="~/Images/calendar.png" /> 
        <act:CalendarExtender ID="ceInputTo" FirstDayOfWeek="Monday" runat="server" Format="dd.MM.yyyy" Enabled="True" PopupButtonID="imgDateTimeTo" TargetControlID="txtInputTo">
        </act:CalendarExtender>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>
