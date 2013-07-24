<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormQPPV.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormQPPV" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_NAME">
        <div class="modal_title_bar" style="text-align:center;">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <customControls:TextBox_CT runat="server" ID="ctlQPPV" ControlLabel="QPPV code:"
                    LabelColumnWidth="150px" ControlInputWidth="300px" IsMandatory="true" ControlEmptyErrorMessage="QPPV code can't be empty." />
                <%-- <asp:Label runat="server" Text="Public search" /><asp:CheckBox runat="server" ID="cbxPublic" />--%>
               
            </asp:Panel>
            <div class="center" style="margin-top:20px;">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_Click"
                    UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px"
                    OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
