<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="AlerterUserStatusPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.AlerterUserStatusPopup" %>

<%@ Register Src="../DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_RS" style="width: 700px">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server">Alert status</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content" style="width: 700px">
            <uc:DropDownList ID="ddlAlerterUserStatus" runat="server" Label="Status:" LabelWidth="170px" TextWidth="433px" />
            <div class="center" style="margin-top: 12px;">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_OnClick" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_OnClick" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
