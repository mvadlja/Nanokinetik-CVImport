<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RadioButtonYn.ascx.cs" Inherits=" AspNetUI.Views.Shared.UserControl.RadioButtonYn" EnableViewState="true" %>

<div ID="divRadioButtonYn" runat="server" class="row">
    <div class="label vertical-align-bottom">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="rbYn"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input radio-buttons-vertical">
        <asp:RadioButtonList ID="rbYn" runat="Server" RepeatLayout="Flow" RepeatDirection="Horizontal">
            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
            <asp:ListItem Text="No" Value="No"></asp:ListItem> 
        </asp:RadioButtonList>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>


