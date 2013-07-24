<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="QuickLinksPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.QuickLinksPopup" %>
<%@ Register Src="../TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../CheckBox.ascx" TagName="CheckBox" TagPrefix="uc" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_NAME">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" class="center">Save Quick link</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%" CssClass="quick-links-popup">
                <br />
                <uc:TextBox ID="txtQuickLinkName" runat="server" Label="Quick link name:" MaxLength="2000" Required="True" />
                <uc:CheckBox ID="chcIsPublic" runat="server" Label="Public:" />
            </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOk_OnClick" UseSubmitBehavior="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
