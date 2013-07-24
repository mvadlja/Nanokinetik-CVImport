<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="PartnerPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.PartnerPopup" %>

<%@ Register Src="../DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_RS" style="width: 700px">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server">Partner</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content" style="width: 700px">
            <uc:DropDownList ID="ddlPartner" runat="server" Label="Partner:" LabelWidth="170px" TextWidth="433px" Required="True" />
            <uc:DropDownList ID="ddlPartnerType" runat="server" Label="Partner type:" LabelWidth="170px" TextWidth="433px" Required="False" />
            <div class="center" style="margin-top: 12px;">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_OnClick" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_OnClick" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
