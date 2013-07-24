<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TextBoxSr.ascx.cs" Inherits=" AspNetUI.Views.Shared.UserControl.TextBoxSr" EnableViewState="true" %>

<%@ Register Src="Searcher.ascx" TagName="Searcher" TagPrefix="uc" %>

<div ID="divTextBoxSr" runat="server" class="row">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtInput"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="input">
        <asp:TextBox ID="txtInput" runat="server" Enabled="False"></asp:TextBox>
        <asp:HiddenField ID="selectedValue" runat="server"/>
    </div>
    <div class="inputSelect">
       <asp:LinkButton ID="lbtnSelect" runat="server" Text="Select" OnClick="lbtnSelect_OnClick" CssClass="boldText"></asp:LinkButton>
    </div>
    <span id="spanSrSeparator" runat="server" CssClass="boldText"> | </span>
    <div class="inputRemove">
       <asp:LinkButton ID="lbtnRemove" runat="server" Text="Remove" OnClick="lbtnRemove_OnClick" CssClass="boldText"></asp:LinkButton>
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
    <uc:Searcher ID="searcher" runat="server" />
</div>

