<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ModalPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.ModalPopup" %>

<div id="mpContainer" runat="server" class="modal">
    <div id="mpYContainerMain" class="modal_container">
        <div id="mpHeader" runat="server" class="modal_title_bar">
            <asp:Panel runat="server" ID="pnlHeader"></asp:Panel>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_Click" />
        </div>
        <div id="mpBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlContent" runat="server" />

            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" OnClick="btnOk_Click" CssClass="disableLeaveThisPage"/>
                <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_Click" />
                <asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnNo_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </div>
    </div>
</div>
