<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListBoxExt.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.ListBoxExt" EnableViewState="true" %>

<div id="divListBoxExt" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="lbInput"></asp:Label>
        <span id="spanRequired" runat="server" visible="False">*</span>
    </div>
    <div class="input">
        <asp:ListBox ID="lbInput" runat="server" OnSelectedIndexChanged="LbInputOnSelectedIndexChanged"/>
    </div>
    <div id="AERLinkButtons" runat="server" class="list-box-buttons">
        <asp:LinkButton ID="lbtnNew" clientIdAttr="LbExtLbtnNew" runat="server" Text="New" OnClick="BtnNewOnClick" Visible="False"/>
        <asp:LinkButton ID="lbtnAdd" clientIdAttr="LbExtLbtnAdd" runat="server" Text="Add" OnClick="BtnAddOnClick" Visible="False"/>
        <asp:LinkButton ID="lbtnEdit" clientIdAttr="LbExtLbtnEdit" runat="server" Text="Edit" OnClick="BtnEditOnClick" Visible="False"/>
        <asp:LinkButton ID="lbtnRemove" clientIdAttr="LbExtLbtnRemove" runat="server" Text="Remove" CssClass="remove" OnClick="BtnRemoveOnClick" Visible="False"/>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>