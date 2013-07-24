<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormSave.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormSave" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_GE">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <div class="center">
                    <customControls:TextBox_CT runat="server" ID="ctlName" ControlLabel="Name:" LabelColumnWidth="150px"
                        IsMandatory="true" ControlInputWidth="300px" ControlEmptyErrorMessage="Name can't be empty" />
                </div>
            </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="Save" Width="75px" SkinID="blue" OnClick="btnOk_Click"
                    UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px"
                    OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
