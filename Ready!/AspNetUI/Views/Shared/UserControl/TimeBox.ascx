<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TimeBox.ascx.cs" Inherits=" AspNetUI.Views.Shared.UserControl.TimeBox" EnableViewState="true" %>

<div ID="divTimeBox" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtInputHours"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input inputTime">
        <asp:TextBox ID="txtInputHours" runat="server"></asp:TextBox>
        <span id="spanHours" runat="server" Visible="True">Hours</span>
        <asp:TextBox ID="txtInputMinutes" runat="server"></asp:TextBox>
        <span id="spanMinutes" runat="server" Visible="True">Mins</span>
        <asp:HiddenField ID="hashValue" runat="server"/>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>


