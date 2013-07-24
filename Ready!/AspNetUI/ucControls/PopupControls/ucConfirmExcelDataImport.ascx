<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucConfirmExcelDataImport.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucConfirmExcelDataImport" %>

<div id="PopupControls_ConfirmExcelDataImport_Container" runat="server" class="modal">

    <div id="PopupControls_ConfirmExcelDataImport_ContainerContent" class="modal_container_small">
        
        <div class="modal_title_bar">
            <div id="divHeader">Confirm excel data import</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnCancel_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <asp:GridView ID="gvData" runat="server" SkinID="ExcelDataImportValidator" />
            <div class="center">
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click" UseSubmitBehavior="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>