<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUploadCtrl.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.FileUploadCtrl" %>

<div ID="divFileUploadCtrl" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="fileUpload"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input">
        <asp:FileUpload runat="server" ID="fileUpload" />
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>