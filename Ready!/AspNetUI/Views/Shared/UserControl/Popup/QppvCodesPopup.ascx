<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="QppvCodesPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.QppvCodesPopup" %>
<%@ Register Src="../TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_NAME">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" class="text-align-center">QPPV code</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%" CssClass="quick-links-popup">
                <br />
                <uc:TextBox ID="txtQppvCode" runat="server" Label="QPPV code:" MaxLength="2000" Required="True" TextWidth="200" /> 
                <br />
            </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOk_OnClick" UseSubmitBehavior="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
