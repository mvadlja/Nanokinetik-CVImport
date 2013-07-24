<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ListBox.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.ListBox" EnableViewState="true" %>

<div ID="divListBox" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="lbInput"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input">
        <asp:ListBox ID="lbInput" runat="server" />
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>




            
