<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucConfirmInput.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucConfirmInput" %>

<div id="PopupControls_ConfirmInput_Container" runat="server" class="modal">

    <div id="PopupControls_ConfirmInput_ContainerContent" class="modal_container_small">
        
        <div class="modal_title_bar">
            <div id="divHeader">Confirm data</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnCancel_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <asp:Label ID="lblResult" Text="" runat="server" />
            <div class="center">
                <asp:Button ID="btnSave" SkinID="blue" runat="server" Text="Save" OnClick="btnSave_Click" UseSubmitBehavior="false" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>
