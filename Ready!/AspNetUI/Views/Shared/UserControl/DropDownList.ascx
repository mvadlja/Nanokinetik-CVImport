<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="DropDownList.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.DropDownList" EnableViewState="true" %>

<div ID="divDropDownList" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="ddlInput"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input">
        <asp:DropDownList ID="ddlInput" runat="server" OnSelectedIndexChanged="ddlInput_OnSelectedIndexChanged"></asp:DropDownList>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>