<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListBoxSr.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.ListBoxSr" EnableViewState="true" %>

<%@ Register Src="Searcher.ascx" TagName="Searcher" TagPrefix="uc" %>

<div id="divListBoxSr" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="lbInput"></asp:Label>
        <span id="spanRequired" runat="server" visible="False">*</span>
    </div>
    <div class="input">
        <asp:ListBox ID="lbInput" runat="server" OnSelectedIndexChanged="LbInputOnSelectedIndexChanged"/>
    </div>
    <div id="divLinkButtons" runat="server" class="list-box-buttons">
        <asp:LinkButton ID="lbtnNew" clientIdAttr="LbExtLbtnNew" runat="server" Text="New" OnClick="BtnNewOnClick" Visible="True"/>
        <asp:LinkButton ID="lbtnAdd" clientIdAttr="LbExtLbtnAdd" runat="server" Text="Add" OnClick="BtnAddOnClick" Visible="True"/>
        <asp:LinkButton ID="lbtnEdit" clientIdAttr="LbExtLbtnEdit" runat="server" Text="Edit" OnClick="BtnEditOnClick" Visible="True"/>
        <asp:LinkButton ID="lbtnRemove" clientIdAttr="LbExtLbtnRemove" runat="server" Text="Remove" OnClick="BtnRemoveOnClick" Visible="True"/>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
    <uc:Searcher ID="searcher" runat="server" />
</div>

