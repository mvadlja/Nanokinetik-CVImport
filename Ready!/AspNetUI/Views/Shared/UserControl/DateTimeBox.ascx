<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="DateTimeBox.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.DateTimeBox" EnableViewState="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<div id="divDateBox" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtInput"></asp:Label>
        <span id="spanRequired" runat="server" visible="False">*</span>
    </div>
    <div class="input">
        <asp:TextBox ID="txtInput" runat="server" CssClass="date"></asp:TextBox>
        <asp:Image ID="imgDateTime" runat="server" ImageUrl="~/Images/calendar.png" />
        <asp:LinkButton ID="lnkSetReminder" runat="server" Text="" Width="13px" Visible="False" CssClass="reminder-icon-not-exists" />
        <act:CalendarExtender ID="ceInput" FirstDayOfWeek="Monday" runat="server" Format="dd.MM.yyyy" Enabled="True" PopupButtonID="imgDateTime" TargetControlID="txtInput">
        </act:CalendarExtender>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>
