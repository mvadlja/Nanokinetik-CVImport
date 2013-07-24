<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPopupFormSC.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormSC" %>

<%@ Register Src="~/ucControls/PopupControls/ucPopupFormRS.ascx" TagName="ucPopupFormRS" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" tagname="TextArea_CT" tagprefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" tagname="ListBox_CT" tagprefix="customControls" %>


<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_SC">
        
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
                            <asp:Panel ID="pnlSubCode" runat="server" Visible="true" >
                                <br />
                                <customControls:TextBox_CT ID="ctlSubCode" runat="server" LabelColumnWidth="150px" MaxLength="12" ControlInputWidth="195px" ControlLabel="Code: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Code can't be empty" />
                                <customControls:DropDownList_CT ID="ctlSubCodeSystem" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Code system: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Code system can't be empty" AutoPostback="true" />
                                <customControls:DropDownList_CT ID="ctlSubCodeSystemID" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Code system ID: " IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage=""  AutoPostback="true" />
                                <customControls:DropDownList_CT ID="ctlSubCodeSystemStatus" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Code system status: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Code system status can't be empty" />
                                <customControls:DateBox_CT ID="ctlSubCodeSystemChangeDate" runat="server" LabelColumnWidth="150px" ControlLabel="Code system change date:" ControlValueFormat="dd.MM.yyyy" IsMandatory="false" ControlEmptyErrorMessage="" ControlErrorMessage="Change date is not a valid date." />
                                <customControls:TextArea_CT ID="ctlSubCodeComment" runat="server" LabelColumnWidth="150px" MaxLength="2500" ControlInputWidth="324px" ControlLabel="Comment: " IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                <br />
                                <asp:Panel ID="pnlRefSource" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="160" style='table-layout:fixed' align="right">
                                            <div style="margin-right:-10px;">
                                                <asp:Label ID="lblRefSourceTitle" runat="server">Reference sources: </asp:Label>
                                                <asp:Label ID="lblRefSourceReq" runat="server" ForeColor="Red">*</asp:Label>
                                            </div>
                                            </td>
                                            <td valign="top" rowspan = "3">
                                                <div class="marginLeft_10" style="margin-left:8px">
                                                    <customControls:ListBox_CT ID="ctlRefSources"  runat="server" ControlMultipleValueSelection = "true" LabelColumnWidth="0px" ControlInputWidth="296px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlRefSourcesListInputValueChanged"/>
                                                </div>
                                             </td>
                                            <td valign="middle">
                                                <div class="marginLeft_3">
                                                    <asp:Button ID="btnAddRefSource" runat="server" Text="Add" Width="75px" OnClick="btnAddRefSourceOnClick"/>
                                                </div>
                                            <uc1:ucPopupFormRS ID="RefSourcesPopupForm" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle">
                                                <div class="marginLeft_3">
                                                    <asp:Button ID="btnEditRefSource" runat="server" Text="Edit" Width="75px" OnClick="btnEditRefSourceOnClick"/>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle">
                                                <div class="marginLeft_3">
                                                    <asp:Button ID="btnRemoveRefSources" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveRefSourcesOnClick"/>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </asp:Panel>
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