<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="ManufacturerPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.ManufacturerPopup" %>

<%@ Register Src="../DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_RS" style="width: 700px">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server">Manufacturer</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content" style="width: 700px">
            <uc:DropDownList ID="ddlManufacturer" runat="server" Label="Manufacturer:" LabelWidth="170px" TextWidth="433px" Required="True" />
            <uc:DropDownList ID="ddlManufacturerType" runat="server" Label="Manufacturer type:" LabelWidth="170px" TextWidth="433px" Required="False" AutoPostback="True"/>
            <uc:TextBoxSr ID="txtSrActiveSubstance" runat="server" Label="Active substance:" LabelWidth="170px" TextWidth="330px" SearchType="Substance" Visible="False" />
            <div class="center" style="margin-top: 12px;">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_OnClick" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_OnClick" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
