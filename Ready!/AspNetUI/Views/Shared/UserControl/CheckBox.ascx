<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CheckBox.ascx.cs" Inherits=" AspNetUI.Views.Shared.UserControl.CheckBox" EnableViewState="true" %>

<div id="divCheckBox" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="cbInput"></asp:Label>
        <span id="spanRequired" runat="server" visible="False">*</span>
    </div>
    <div class="input">
        <asp:CheckBox ID="cbInput" runat="server" Checked="false" />
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>


