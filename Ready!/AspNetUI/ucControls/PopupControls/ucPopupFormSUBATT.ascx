﻿<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormSUBATT.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormSUBATT" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_ISO">
        
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" class="text-align-center" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content disable-table-hover">

            <asp:Panel ID="pnlFormContainer" runat="server" GroupingText="" Width="100%">
                <br />
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
	                        <customControls:TextBox_CT ID="ctlAttachmentCode" runat="server" LabelColumnWidth="150px" ControlInputWidth="300px" ControlLabel="Attachment code:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Attachment code can't be empty."/>
                            <customControls:TextBox_CT ID="ctlResolutionMode" runat="server" LabelColumnWidth="150px" ControlInputWidth="300px" ControlLabel="Resolution mode:" IsMandatory="true" ControlErrorMessage="Resolution mode is not valid number" ControlEmptyErrorMessage="Resolution mode can't be empty."  />
                            <customControls:TextBox_CT ID="ctlValidityDeclaration" runat="server" LabelColumnWidth="150px" ControlInputWidth="300px" ControlLabel="Validity Declaration:" IsMandatory="true" ControlErrorMessage="Validity declaration is not number" ControlEmptyErrorMessage="Validity declaration mode can't be empty."  />
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