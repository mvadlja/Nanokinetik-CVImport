<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TextBox.ascx.cs" Inherits=" AspNetUI.Views.Shared.UserControl.TextBox" EnableViewState="true" %>

<div ID="divTextBox" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtInput"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input">
        <asp:TextBox ID="txtInput" runat="server"></asp:TextBox>
        <asp:HiddenField ID="hashValue" runat="server"/>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>


