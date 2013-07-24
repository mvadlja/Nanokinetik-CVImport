<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="IndicationsPopup.ascx.cs" Inherits=" AspNetUI.Views.Shared.UserControl.Popup.IndicationsPopup" %>

<%@ Register Src="../TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>

<div id="PopupControls_Struct_Container" runat="server" class="modal">
    <div id="PopupControls_Struct_ContainerContent" class="modal_container">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" class="center">MEDDRA</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <div class="meddra-popup">
                <div class="width-60px">
                    <uc:DropDownList ID="ddlMeddraVersion" runat="server" Label="Version:" TextWidth="60px" LabelWidth="60px" Required="True" />
                </div>
                <div class="width-80px">
                    <uc:DropDownList ID="ddlMeddraLevel" runat="server" Label="Level:" TextWidth="80px" LabelWidth="80px" Required="True" />
                </div>
                <div class="width-80px">
                    <uc:TextBox ID="txtMeddraCode" runat="server" Label="Code:" TextWidth="80px" LabelWidth="80px" MaxLength="2000" Required="True" />
                </div>
                <div class="width-200px">
                    <uc:TextBox ID="txtMeddraTerm" runat="server" Label="Term:" TextWidth="200px" LabelWidth="200px" MaxLength="2000" />
                </div>
            </div>
            <div class="center" style="margin-top: 30px">
                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOk_OnClick" UseSubmitBehavior="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
