<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPopupFormON.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormON" %>

<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" tagname="ListBox_CT" tagprefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_ON">
        
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
                            
                            <asp:Panel ID="pnlOfficialName" runat="server" Visible="true" >
                                <br />
                                <customControls:DropDownList_CT ID="ctlONameType" runat="server" LabelColumnWidth="110px" ControlInputWidth="360px" ControlLabel="Type: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Type can't be empty" AutoPostback="true" />
                                <customControls:DropDownList_CT ID="ctlONameStatus" runat="server" LabelColumnWidth="110px" ControlInputWidth="360px" ControlLabel="Status: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Status can't be empty" />
                                <customControls:DateBox_CT ID="ctlONameStatusChangeDate" runat="server" LabelColumnWidth="110px" ControlValueFormat="dd.MM.yyyy" ControlLabel="Status change date:" IsMandatory="false" ControlEmptyErrorMessage="" ControlErrorMessage="Status change date is not a valid date." />
                                
                                <customControls:ListBox_CT ID="ctlONDomains" runat="server" LabelColumnWidth="110px" ControlInputWidth="380px" ControlLabel="Domain:" IsMandatory="false" ControlMultipleValueSelection = "true" ControlVisibleRows="6" ControlErrorMessage="Domain is not a valid number." ControlEmptyErrorMessage="Domain must be selected." />
                                <customControls:ListBox_CT ID="ctlONJuristictions" runat="server" LabelColumnWidth="110px" ControlInputWidth="380px" ControlLabel="Jurisdiction:" IsMandatory="false" ControlMultipleValueSelection = "true" ControlVisibleRows="6"  ControlErrorMessage="Jurisdiction is not a valid number." ControlEmptyErrorMessage="Jurisdiction must be selected." />
                                
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" OnClick="btnOk_Click" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>

    </div>

</div>