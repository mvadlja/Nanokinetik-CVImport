<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormALSTRN.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormALSTRN" %>

<%@ Register src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" tagname="TextArea_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_ISO">
        
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
	                        <customControls:DropDownList_CT ID="ctlLanguage" runat="server" LabelColumnWidth="150px" ControlInputWidth="300px" ControlLabel="Language" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Language can't be empty."/>
                            <customControls:TextArea_CT ID="ctlTerm" runat="server" LabelColumnWidth="150px" ControlInputWidth="300px" ControlLabel="Term" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Term can't be empty." MaxLength="2000" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>