<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateRangeFilterPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.DateRangeFilterPopup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<div style="display: none">
    <asp:TextBox ID="txtInput" runat="server"></asp:TextBox>
    <act:CalendarExtender ID="ceInput" FirstDayOfWeek="Monday" runat="server" Format="dd.MM.yyyy" Enabled="True" TargetControlID="txtInput"></act:CalendarExtender>
</div>
