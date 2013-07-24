<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucConfirmAction.ascx.cs" Inherits="AspNetUI.Support.ucConfirmAction" %>

<div id="PopupControls_ConfirmInput_Container" runat="server" class="modal">
    <div id="PopupControls_ConfirmInput_ContainerContent" class="modal_container_small">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server"></div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnCancel_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <br />
            <div style="text-align: center" id="divMessage" runat="server" />
            <br />
            <div class="center">
                <asp:Button ID="btnYes" SkinID="blue" runat="server" Text="Yes" Width="50px" OnClick="btnYes_Click" UseSubmitBehavior="false" />
                <span style="width: 15px;">&nbsp</span>
                <asp:Button ID="btnNo" SkinID="blue" runat="server" Text="No" Width="50px" OnClick="btnNo_Click" UseSubmitBehavior="false" />
                <span runat="server" visible="false" id="cancelButtonContainer">
                    <span style="width:15px;">&nbsp</span>
                    <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="60px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
                </span>
            </div>
        </div>
    </div>
</div>
