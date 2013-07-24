<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormGE.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormGE" %>

<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>
<%@ Register Src="~/ucControls/PopupControls/ucSearcher.ascx" TagName="ucSearcher" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormRS.ascx" TagName="ucPopupFormRS" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_GE">
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
                            <asp:Panel ID="pnlGeneElement" runat="server" Visible="true">
                                <br />
                                <customControls:DropDownList_CT ID="ddlGeneElementType" runat="server" LabelColumnWidth="150px"
                                    MaxLength="12" ControlInputWidth="332px" ControlLabel="Gene element type: " IsMandatory="true"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="Gene element type can't be empty" />
                                <asp:Panel ID="pnlSearchEVCODE" runat="server">
                                    <table cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td align="right" style="width: 150px;">
                                                <asp:Label ID="Label1" Text="Substance ID:" runat="server"></asp:Label>
                                            </td>
                                            <td width="7px" />
                                            <td>
                                                <uc1:ucSearcher ID="EVCODESearcher" runat="server" />
                                                <operational:SearcherDisplay ID="EVCODESearcherDisplay" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <customControls:TextBox_CT ID="tbxGeneElementName" runat="server" LabelColumnWidth="150px"
                                    MaxLength="2500" ControlInputWidth="327px" ControlLabel="Gene element name: "
                                    IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Gene element name can't be empty" />
                                <asp:Panel ID="pnlRs" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="160px" align="right">
                                                        <asp:Label ID="Label" runat="server" Text="Reference source: " IsMandatory="true" Visible="true"/>
                                                        <asp:Label ID="asterix" runat="server" Text="*" ForeColor="Red" />
                                                    </td>
                                            <td valign="top" rowspan="3">
                                                <div class="marginLeft_10">
                                                    <customControls:ListBox_CT ID="ctlRs" runat="server" ControlMultipleValueSelection="true"
                                                        LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlRsInputValueChanged"/>
                                                </div>
                                            </td>
                                            <td valign="middle">
                                                <asp:Button ID="btnAddRs" runat="server" Text="Add" Width="75px" onclick="btnAddRsOnClick"/>
                                                <uc1:ucPopupFormRS ID="UcPopupFormRs" runat="server" ViewStateMode="Enabled" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td valign="middle">
                                                <asp:Button ID="btnEditRs" runat="server" Text="Edit" Width="75px" Enabled="false" onclick="btnEditRsOnClick"  />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td valign="middle">
                                                <asp:Button ID="btnRemoveRs" runat="server" Text="Remove" Width="75px" Enabled="false" onclick="btnRemoveRsOnClick" />
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
