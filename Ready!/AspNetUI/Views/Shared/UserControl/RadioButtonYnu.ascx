<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RadioButtonYnu.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.RadioButtonYnu" EnableViewState="true" %>

<div ID="divRadioButtonYnu" runat="server" class="row">
    <div class="label radio-boxes vertical-align-bottom"">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="rbYnu"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input radio-buttons-vertical">
        <asp:RadioButtonList ID="rbYnu" runat="Server" RepeatLayout="Flow" RepeatDirection="Horizontal">
            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
            <asp:ListItem Text="No" Value="No"></asp:ListItem> 
            <asp:ListItem Text="Unknown" Value="NULL" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>


