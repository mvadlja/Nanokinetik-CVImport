<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="XevprmValidationErrorPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.XevprmValidationErrorPopup" %>
<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_validation_error">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server">Validation errors</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content" style="padding: 0px;">
            <div id="divValidationErrors" runat="server" style="max-height:350px;overflow:auto;">&nbsp</div>
            <div id="divButtons" runat="server" class="center" style="margin-top:10px; margin-bottom: 10px;">
                <asp:Button ID="btnValidate" runat="server" Text="Validate" OnClick="btnValidate_OnClick" UseSubmitBehavior="false" Visible="false" />
                <asp:Button ID="btnOk" runat="server" Text="Close" OnClick="btnOk_OnClick" UseSubmitBehavior="false" />
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExportToExcel_OnClick" UseSubmitBehavior="false" Visible="false" />
            </div>
        </div>
    </div>
</div>