<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="MessageErrorPopup.ascx.cs" Inherits="AspNetUI.Support.MessageErrorPopup" %>
<div id="messageModalPopupContainer" runat="server" class="modal">
    <div id="messageModalPopupContainerContent" class="modal_container_validation_error">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <div id="divMessage" runat="server" class="center" style="max-height:350px;overflow:auto;">&nbsp</div>
            <div id="divButtons" runat="server" class="center" style="margin-top:10px">
                <asp:Button ID="btnOk" runat="server" Text="Close" OnClick="btnOk_Click" UseSubmitBehavior="false" />
                <asp:Button ID="btnValidate" runat="server" Text="Validate" OnClick="btnValidate_Click" UseSubmitBehavior="false" Visible="false" />
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExportToExcel" UseSubmitBehavior="false" Visible="false" />
            </div>
        </div>
    </div>
</div>