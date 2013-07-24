<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormMEDDR.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormMEDDR" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>

<div id="PopupControls_Struct_Container" runat="server" class="modal">
    <div id="PopupControls_Struct_ContainerContent" class="modal_container">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <br />
            <table style="text-align:center;padding:0px;margin:0 auto;width:350px;">
                <tr>
                    <td width="60px" style="font-weight:bold">Version: <span style='color:Red'>*</span></td>
                    <td width="80px" style="font-weight:bold">Level: <span style='color:Red'>*</span></td>
                    <td width="80px" style="font-weight:bold">Code: <span style='color:Red'>*</span></td>
                    <td style="font-weight:bold">Term: </td>
                </tr>
                <tr class="popupFormRow">
                    <td>
                        <customControls:DropDownList_CT ID="ctlmeddraversion" runat="server" LabelColumnWidth="60px"
                            MaxLength="6" ControlInputWidth="60px" ControlLabel=""
                            IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Version can't be empty." />
                    </td>
                    <td style="width:80px">
                        <customControls:DropDownList_CT ID="ctlmeddralevel" runat="server" LabelColumnWidth="80px"
                            MaxLength="10" ControlInputWidth="80px" ControlLabel="" 
                            IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Level can't be empty." />
                    </td>
                    <td>
                        <customControls:TextBox_CT ID="ctlmeddracode" runat="server" LabelColumnWidth="80px" ControlLabelAlign="above"
                            MaxLength="10" ControlInputWidth="80px" ControlLabel=""
                            IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Code can't be empty." />
                    </td>
                    <td>
                        <customControls:TextBox_CT ID="ctlmeddraterm" runat="server" LabelColumnWidth="200px"
                            MaxLength="256" ControlInputWidth="200px" ControlLabel=""
                            IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="Code can't be empty." />
                    </td>
                </tr>
            </table>

                <div class="center" style="margin-top:30px">
                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" UseSubmitBehavior="false"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
            
        </div>
    </div>
</div>