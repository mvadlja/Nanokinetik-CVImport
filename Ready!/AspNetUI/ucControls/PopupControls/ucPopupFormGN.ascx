<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucPopupFormGN.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucPopupFormGN" %>

<%@ Register Src="~/ucControls/PopupControls/ucPopupFormRS.ascx" TagName="ucPopupFormRS" TagPrefix="uc1" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_GN">
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
                            <asp:Panel ID="pnlGene" runat="server" Visible="true">
                                <br />
                                <customControls:DropDownList_CT ID="ddlGeneSequenceOrigin" runat="server" LabelColumnWidth="150px"
                                    MaxLength="4000" ControlInputWidth="333px" ControlLabel="Gene sequence origin: " IsMandatory="false"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                <customControls:DropDownList_CT ID="ddlGeneId" runat="server" LabelColumnWidth="150px" OnInputValueChanged="ddlGeneValueChanged" AutoPostback="true"
                                     ControlInputWidth="333px" ControlLabel="Gene ID: " IsMandatory="false"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="" />
                                <customControls:DropDownList_CT ID="ddlGeneName" runat="server" LabelColumnWidth="150px" OnInputValueChanged="ddlGeneValueChanged" AutoPostback="true"
                                    ControlInputWidth="333px" ControlLabel="Gene name: " IsMandatory="true"
                                    ControlErrorMessage="" ControlEmptyErrorMessage="Gene name can't be empty" />
                                <asp:Panel ID="pnlRs" runat="server">
                                     <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="160px" align="right">
                                                        <asp:Label ID="Label_CT2" runat="server" Text="Reference source:" IsMandatory="true" Visible="true"/>
                                                        <asp:Label ID="asterix" runat="server" ForeColor="Red">*</asp:Label>
                                                    </td>
                                                    <td valign="top" align="left" rowspan="3">
                                                    <div class="marginLeft_10">
                                                        <customControls:ListBox_CT ID="ctlRs" runat="server" ControlMultipleValueSelection="true"
                                                            LabelColumnWidth="0" ControlInputWidth="200px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlRsInputValueChanged"
                                                             /></div>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Button ID="btnAddRs" runat="server" Text="Add" Width="75px" OnClick="btnAddRsOnClick"/>
                                                        <uc1:ucPopupFormRS ID="UcPopupFormRs" runat="server" ViewStateMode="Enabled" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Button ID="btnEditRs" runat="server" Text="Edit" Width="75px" OnClick="btnEditRsOnClick"
                                                            Enabled="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Button ID="btnRemoveRs" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveRsOnClick"
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
