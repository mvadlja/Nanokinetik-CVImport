<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ListBoxAu.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.ListBoxAu" EnableViewState="true" %>

<div ID="divListBoxAu" runat="server" class="row list-box-au">
    <div class="label">
        <asp:Label ID="lblName" runat="server" AssociatedControlID="lbInputFrom"></asp:Label>
        <span id="spanRequired" runat="server" Visible="False">*</span>
    </div>
    <div class="list1">
        <asp:ListBox ID="lbInputFrom" runat="server" />
        <input type="hidden" name="lbInputFromDblClick" />
    </div>
    <div class="buttons">
        <asp:Button ID="btnAssign" runat="server" Text=">" OnClick="btnAssign_OnClick" />
        <br/>
        <asp:Button ID="btnUnassign" runat="server" Text="<" OnClick="btnUnassign_OnClick" />
    </div> 
    <div class="list2">
        <asp:ListBox ID="lbInputTo" runat="server" />
        <input type="hidden" name="lbInputToDblClick" />
    </div>
    <div class="error">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
</div>