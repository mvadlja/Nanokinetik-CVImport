<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormTRG.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormTRG" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormRS.ascx" TagName="ucPopupFormRS" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>

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
                            <asp:Panel ID="pnlTarget" runat="server" Visible="true">
                                <br />
                                <customControls:DropDownList_CT ID="ddlTargetGeneId" runat="server" LabelColumnWidth="150px"
                                    ControlInputWidth="300px" ControlLabel="Target gene ID: " IsMandatory="false"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="Gene element type can't be empty" AutoPostback="true" OnInputValueChanged="TargetNameValueChanged"/>
                                
                                <customControls:DropDownList_CT ID="ddlTargetGeneName" runat="server" LabelColumnWidth="150px"
                                     ControlInputWidth="300px" ControlLabel="Target gene name: "
                                    IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Target gene name can't be empty" AutoPostback="true" OnInputValueChanged="TargetNameValueChanged"/>

                                    <customControls:DropDownList_CT ID="ddlInteractionType" runat="server" LabelColumnWidth="150px"
                                    ControlInputWidth="300px" ControlLabel="Interaction type: " IsMandatory="true"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="Interation type can't be empty" />

                                    <customControls:DropDownList_CT ID="ddlTargetOrganismType" runat="server" LabelColumnWidth="150px"
                                    ControlInputWidth="300px" ControlLabel="Target organism type: " IsMandatory="false"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="" />

                                    <customControls:DropDownList_CT ID="ddlTargetType" runat="server" LabelColumnWidth="150px"
                                    ControlInputWidth="300px" ControlLabel="Target type: " IsMandatory="false"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="" />
                               
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
             <asp:Panel ID="pnlRefSource" runat="server">
                                    <table cellpadding="0" cellspacing="0" style="margin-left:12px">
                                        <tr>
                                            <td width="150" style='table-layout:fixed' align="right">
                                            <div style="margin-right:8px">
                                                <asp:Label ID="lblRefSourceTitle" runat="server">Reference source:</asp:Label>
                                                <asp:Label ID="lblRefSourceReq" runat="server" ForeColor="Red">*</asp:Label>
                                            </div>
                                            </td>
                                            <td valign="top" rowspan = "3">
                                                <div class="marginLeft_10">
                                                    <customControls:ListBox_CT ID="ctlRefSources"  runat="server" ControlMultipleValueSelection = "true" LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlRefSourcesListInputValueChanged"/> 
                                                </div></td>
                                            <td valign="middle"><asp:Button ID="btnAddRefSource" runat="server" Text="Add" Width="75px" OnClick="btnAddRefSourceOnClick"/>
                                            <uc1:ucPopupFormRS ID="RefSourcesPopupForm" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle"><asp:Button ID="btnEditRefSource" runat="server" Text="Edit" Width="75px" OnClick="btnEditRefSourceOnClick"/></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle"><asp:Button ID="btnRemoveRefSources" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveRefSourcesOnClick"/></td>
                                        </tr>
                                    </table>
                                    <br />
                                </asp:Panel>
            <div class="center">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_Click"
                    UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px"
                    OnClick="btnCancel_Click" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>
