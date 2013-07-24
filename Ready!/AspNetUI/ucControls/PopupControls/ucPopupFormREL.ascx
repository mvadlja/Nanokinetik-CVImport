<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormREL.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormREL" %>

<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>
<%@ Register Src="~/ucControls/PopupControls/ucSearcher.ascx" TagName="ucSearcher" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormRS.ascx" TagName="ucPopupFormRS" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormAMOUNT.ascx" TagName="ucPopupFormAMT" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_REL">
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
                            <asp:Panel ID="pnlSubstanceRelationship" runat="server" Visible="true">
                                <br />
                                <customControls:DropDownList_CT ID="ddlRelationship" runat="server" EnableViewState="true"
                                    Visible="true" LabelColumnWidth="150px" ControlInputWidth="306px" ControlLabel="Relationship: "
                                    IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Relationship can't be empty" />
                                <customControls:DropDownList_CT ID="ddlInteractionType" runat="server" EnableViewState="true"
                                    Visible="true" LabelColumnWidth="150px" ControlInputWidth="306px" ControlLabel="Interaction type: "
                                    IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                <asp:Panel ID="pnlSearchEVCODE" runat="server">
                                    <table cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td align="right" style="width: 150px;">
                                                <asp:Label ID="Label1" Text="Substance ID:" runat="server"></asp:Label>
                                            </td>
                                            <td width="6px" />
                                            <td>
                                                <uc1:ucSearcher ID="EVCODESearcherRel" runat="server" />
                                                <operational:SearcherDisplay ID="EVCODESearcherDisplayRel" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <customControls:TextBox_CT ID="tbxSubstanceName" runat="server" LabelColumnWidth="150px"
                                    MaxLength="4000" ControlInputWidth="300px" ControlLabel="Substance name: " IsMandatory="true"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="Substance name can't be empty" />
                                <customControls:DropDownList_CT ID="ddlAmountType" runat="server" LabelColumnWidth="150px"
                                    ControlInputWidth="306px" ControlLabel="Amount type: " IsMandatory="false"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                <asp:Panel ID="pnlAmount" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="160" style='table-layout: fixed' align="right">
                                                    <asp:Label ID="lblAmountTitle" runat="server">Amount:</asp:Label>
                                                    <asp:Label ID="lblAmountReq" runat="server" ForeColor="Red">*</asp:Label>
                                                </td>
                                                <td valign="top" rowspan="3">
                                                    <div class="marginLeft_10">
                                                        <customControls:ListBox_CT ID="ctlAmt" runat="server" ControlMultipleValueSelection="true"
                                                            LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlAmtInputValueChanged"
                                                             />
                                                    </div>
                                                </td>
                                                <td valign="middle">
                                                    <asp:Button ID="btnAddAmt" runat="server" Text="Add" Width="75px" CssClass="marginLeft_27" OnClick="btnAddAmtOnClick"/>
                                                    <uc1:ucPopupFormAMT ID="UcPopupFormAmt" runat="server" ViewStateMode="Enabled" />      
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td valign="middle">
                                                        <asp:Button ID="btnEditAmt" runat="server" Text="Edit" Width="75px" CssClass="marginLeft_27" OnClick="btnEditAmtOnClick" Enabled="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td valign="middle">
                                                        <asp:Button ID="btnRemoveAmt" runat="server" Text="Remove" Width="75px" CssClass="marginLeft_27" OnClick="btnRemoveAmtOnClick" Enabled="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp
                                                </td>
                                            </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlRs" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="160" style='table-layout:fixed' align="right"><asp:Label ID="lblRefSourceTitle" runat="server">Reference source:</asp:Label>
                                                <asp:Label ID="lblRefSourceReq" runat="server" ForeColor="Red">*</asp:Label></td>
                                                <td valign="top" rowspan="3"> 
                                                    <div class="marginLeft_10">
                                                        <customControls:ListBox_CT ID="ctlRs" runat="server" ControlMultipleValueSelection="true"
                                                            LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlRsInputValueChanged"
                                                             />
                                                    </div>
                                                </td>
                                                <td valign="middle">
                                                    <asp:Button ID="btnAddRs" runat="server" Text="Add" Width="75px" CssClass="marginLeft_27" OnClick="btnAddRsOnClick"/>
                                                    <uc1:ucPopupFormRS ID="UcPopupFormRs" runat="server" ViewStateMode="Enabled" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td valign="middle">
                                                    <asp:Button ID="btnEditRs" runat="server" Text="Edit" Width="75px" CssClass="marginLeft_27" OnClick="btnEditRsOnClick"
                                                        Enabled="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td valign="middle">
                                                    <asp:Button ID="btnRemoveRs" runat="server" Text="Remove" Width="75px" CssClass="marginLeft_27" OnClick="btnRemoveRsOnClick"
                                                        Enabled="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp
                                                </td>
                                            </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
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