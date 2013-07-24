<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormPROP.ascx.cs"
    Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormPROP" %>

<%@ Register Src="~/ucControls/PopupControls/ucPopupFormAMOUNT.ascx" TagName="ucPopupFormAMOUNT" TagPrefix="uc1" %>
<!-- Custom input controls -->

<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>
<%@ Register Src="~/ucControls/PopupControls/ucSearcher.ascx" TagName="ucSearcher" TagPrefix="uc1" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" tagname="ListBox_CT" tagprefix="customControls" %>


<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_PROP">
        
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
                            <asp:Panel ID="pnlProperty" runat="server" Visible="true" >
                                <br />
                                <customControls:DropDownList_CT ID="ctlPropertyName" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Property name: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Property name can't be empty" />
                                <customControls:DropDownList_CT ID="ctlPropertyType" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Property type: " IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Property type can't be empty" />
                                <customControls:DropDownList_CT ID="ctlAmountType" runat="server" LabelColumnWidth="150px" ControlInputWidth="200px" ControlLabel="Amount type: " IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                
                                <asp:Panel ID="pnlSearchName" runat="server">
                                <table cellpadding="2" cellspacing="0">
                                <tr>
                                    <td align="right" style="width: 150px;">
                                        <asp:Label ID="ctlSubstanceName" Text="Substance Name:" runat="server"></asp:Label>
                                    </td>
                                    <td width="6px" />
                                    <td>
                                        <uc1:ucSearcher ID="SUBSearcher" runat="server" />
                                        <operational:SearcherDisplay ID="SUBSearcherDisplay" runat="server" />
                                    </td>
                                </tr>
                                </table>
                                </asp:Panel>

                                <asp:Panel ID="pnlSearchEVCODE" runat="server">
                                <table cellpadding="2" cellspacing="0">
                                <tr>
                                    <td align="right" style="width: 150px;">
                                        <asp:Label ID="ctlSubstanceID" Text="Substance ID:" runat="server"></asp:Label>
                                    </td>
                                    <td width="8px" />
                                    <td>
                                        <asp:TextBox ID="EVCODESearcherDisplay" runat="server" Width="195px" ReadOnly="true" Text =""/>
                                    </td>
                                </tr>
                                </table>
                            </asp:Panel>

                                <%--<asp:Panel ID="pnlSearchEVCODE" runat="server">
                                <table cellpadding="2" cellspacing="0">
                                <tr>
                                    <td align="right" style="width: 160px;">
                                        <asp:Label ID="ctlSubstanceID" Text="Substance ID:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <uc1:ucSearcher ID="EVCODESearcher" runat="server" />
                                        <operational:SearcherDisplay ID="EVCODESearcherDisplay" runat="server" />
                                    </td>
                                </tr>
                                </table>
                            </asp:Panel>--%>
                                <br />
                                <asp:Panel ID="pnlAmount" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="160" style='table-layout:fixed' align="right"><asp:Label ID="lblAmountTitle" runat="server">Amount: </asp:Label>
                                            <asp:Label ID="lblAmountReq" runat="server" ForeColor="Red">*</asp:Label></td>
                                            <td valign="top" rowspan = "3">
                                                <div class="marginLeft_10">
                                                    <customControls:ListBox_CT ID="ctlAmount"  runat="server" ControlMultipleValueSelection = "true" LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlAmountListInputValueChanged"/>
                                                </div>
                                            </td>
                                            <td valign="middle"><asp:Button ID="btnAddAmount" runat="server" Text="Add" Width="75px" OnClick="btnAddAmountOnClick"/>
                                            <uc1:ucPopupFormAMOUNT ID="AmountPopupForm" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle"><asp:Button ID="btnEditAmount" runat="server" Text="Edit" Width="75px" OnClick="btnEditAmountOnClick"/></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td valign="middle"><asp:Button ID="btnRemoveAmount" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveAmountOnClick"/></td>
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