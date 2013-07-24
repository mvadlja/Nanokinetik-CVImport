<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ASForm_details.ascx.cs" Inherits="AspNetUI.Views.ASForm_details" %>
<!-- Operational controls -->
<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>
<!-- Custom input controls -->
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" TagName="TextArea_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/ucControls/PopupControls/ucSearcherSUBST.ascx" TagName="ucSearcherSUBST" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSUBTRN.ascx" TagName="ucPopupFormSUBTRN" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormINTCOD.ascx" TagName="ucPopupFormINTCOD" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormPREEVCODE.ascx" TagName="ucPopupFormPREEVCODE" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSUBALS.ascx" TagName="ucPopupFormSUBALS" TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSUBATT.ascx" TagName="ucPopupFormSUBATT" TagPrefix="uc1" %>
<asp:Panel ID="pnlDataDetails" runat="server" class="preventableClose">
    <br />
    <div style="margin-left: 27px;">
    </div>
    <div style="margin-left: 27px;">
        <asp:Panel ID="pnlSearchEVCODE" runat="server">
            <table cellpadding="2" cellspacing="0">
                <tr>
                    <td align="right" style="width: 150px;">
                        <asp:Label ID="Label6" Text="EVCODE:" runat="server"></asp:Label>
                    </td>
                    <td width="6px" />
                    <td>
                        <uc1:ucSearcherSUBST ID="EVCODESearcher" runat="server" />
                        <operational:SearcherDisplay ID="EVCODESearcherDisplay" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <customControls:TextArea_CT ID="ctlSubstanceName" runat="server" ControlInputWidth="330px" MaxLength="2000" LabelColumnWidth="150px" ControlLabel="Substance name:" IsMandatory="true" ControlErrorMessage=""
            ControlEmptyErrorMessage="Substance name can't be emtpy" />
        <customControls:TextBox_CT ID="ctlCasNumber" runat="server" ControlInputWidth="330px" MaxLength="15" LabelColumnWidth="150px" ControlLabel="CAS number:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
        <customControls:TextBox_CT ID="ctlMolecularFormula" runat="server" ControlInputWidth="330px" LabelColumnWidth="150px" ControlLabel="Molecular formula:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
        <customControls:DropDownList_CT ID="ctlClass" runat="server" ControlInputWidth="336px" LabelColumnWidth="150px" ControlLabel="Substance class:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
        <customControls:TextArea_CT ID="ctlCbd" runat="server" ControlInputWidth="330px" LabelColumnWidth="150px" MaxLength="20000" ControlLabel="CBD:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
    </div>
    <br />
</asp:Panel>
<div style="margin-left: 30px;">
    <asp:Panel ID="pnlSubstanceTranslation" runat="server" Visible="true">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td width="150" style='table-layout: fixed; vertical-align: top; padding-top5px;' align="right">
                    <asp:Label ID="lblSubstanceTranslation" runat="server">Substance translation:</asp:Label>
                </td>
                <td valign="top" rowspan="3">
                    <div style="margin-left: -3px">
                        <customControls:ListBox_CT ID="ctlSubstanceTranslation" runat="server" ControlMultipleValueSelection="true" LabelColumnWidth="0px" ControlInputWidth="336px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlSubstanceTranslationListInputValueChanged" />
                    </div>
                </td>
                <td width="30px" align="center">
                    <div style="margin-left: -70px; margin-top: 10px;">
                        <asp:LinkButton CssClass="boldText" ID="btnAddSubTranslation" runat="server" Text="Add" Width="75px" OnClick="btnAddSubTranslationOnClick" />
                        <uc1:ucPopupFormSUBTRN ID="ucPopupFormSUBTRN1" runat="server" ViewStateMode="Enabled" />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnEditSubTranslation" runat="server" Text="Edit" Width="75px" Enabled="false" OnClick="btnEditSubTranslationOnClick" /><br />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnRemoveSubTranslation" runat="server" Text="Remove" Width="75px" Enabled="false" OnClick="btnRemoveSubTranslationOnClick" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlSubstanceAliases" runat="server" Visible="true">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td width="150" style='table-layout: fixed; vertical-align: top; padding-top5px;' align="right">
                    <asp:Label ID="Label1" runat="server">Substance aliases:</asp:Label>
                </td>
                <td valign="top" rowspan="3">
                    <div style="margin-left: -3px">
                        <customControls:ListBox_CT ID="ctlSubstanceAliases" runat="server" ControlMultipleValueSelection="true" LabelColumnWidth="0px" ControlInputWidth="336px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlSubstanceAliasesListInputValueChanged" />
                    </div>
                </td>
                <td width="30px" align="center">
                    <div style="margin-left: -70px; margin-top: 10px;">
                        <asp:LinkButton CssClass="boldText" ID="btnAddSubAliases" runat="server" Text="Add" Width="75px" OnClick="btnAddSubAliasesOnClick" />
                        <uc1:ucPopupFormSUBALS ID="UcPopupFormSUBALS" runat="server" ViewStateMode="Enabled" />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnEditSubAliases" runat="server" Text="Edit" Width="75px" Enabled="false" OnClick="btnEditSubAliasesOnClick" /><br />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnRemoveSubAliases" runat="server" Text="Remove" Width="75px" Enabled="false" OnClick="btnRemoveSubAliasesOnClick" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlInternationalCodes" runat="server" Visible="true">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td width="150" style='table-layout: fixed; vertical-align: top; padding-top5px;' align="right">
                    <asp:Label ID="Label2" runat="server">International codes:</asp:Label>
                </td>
                <td valign="top" rowspan="3">
                    <div style="margin-left: -3px">
                        <customControls:ListBox_CT ID="ctlInternationalCodes" runat="server" ControlMultipleValueSelection="true" LabelColumnWidth="0px" ControlInputWidth="336px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlInternationalCodesListInputValueChanged" />
                    </div>
                </td>
                <td width="30px" align="center">
                    <div style="margin-left: -70px; margin-top: 10px;">
                        <asp:LinkButton CssClass="boldText" ID="btnAddInternationalCodes" runat="server" Text="Add" Width="75px" OnClick="btnAddInternationalCodesOnClick" />
                        <uc1:ucPopupFormINTCOD ID="UcPopupFormINTCOD" runat="server" ViewStateMode="Enabled" />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnEditInternationalCodes" runat="server" Text="Edit" Width="75px" Enabled="false" OnClick="btnEditInternationalCodesOnClick" /><br />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnRemoveInternationalCodes" runat="server" Text="Remove" Width="75px" Enabled="false" OnClick="btnRemoveInternationalCodesOnClick" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlPreviousEvcode" runat="server" Visible="true">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td width="150" style='table-layout: fixed; vertical-align: top; padding-top5px;' align="right">
                    <asp:Label ID="Label3" runat="server">Previous EVCODEs:</asp:Label>
                </td>
                <td valign="top" rowspan="3">
                    <div style="margin-left: -3px">
                        <customControls:ListBox_CT ID="ctlPreviousEvcode" runat="server" ControlMultipleValueSelection="true" LabelColumnWidth="0px" ControlInputWidth="336px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlPreviousEvcodeListInputValueChanged" />
                    </div>
                </td>
                <td width="30px" align="center">
                    <div style="margin-left: -70px; margin-top: 10px;">
                        <asp:LinkButton CssClass="boldText" ID="btnAddPreviousEvcode" runat="server" Text="Add" Width="75px" OnClick="btnAddPreviousEvcodeOnClick" />
                        <uc1:ucPopupFormPREEVCODE ID="UcPopupFormPREEVCODE" runat="server" ViewStateMode="Enabled" />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnEditPreviousEvcode" runat="server" Text="Edit" Width="75px" Enabled="false" OnClick="btnEditPreviousEvcodeOnClick" /><br />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnRemovePreviousEvcode" runat="server" Text="Remove" Width="75px" Enabled="false" OnClick="btnRemovePreviousEvcodeOnClick" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlSubstanceSsi" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td width="150" style='table-layout: fixed; vertical-align: top; padding-top5px;' align="right">
                    <asp:Label ID="Label4" runat="server">Substance SSI:</asp:Label>
                </td>
                <td valign="top" rowspan="3">
                    <div style="margin-left: -3px">
                        <customControls:ListBox_CT ID="ctlSubstanceSsi" runat="server" ControlMultipleValueSelection="true" LabelColumnWidth="0px" ControlInputWidth="307px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlSubstanceSsiListInputValueChanged" />
                    </div>
                </td>
                <td width="30px" align="center">
                    <div style="margin-left: -70px">
                        <asp:LinkButton CssClass="boldText" ID="btnAddSubstanceSsi" runat="server" Text="Add" Width="75px" OnClick="btnAddSubstanceSsiOnClick" />
                        <%--<uc1:ucPopupFormTRG ID="UcPopupFormTRG" runat="server" ViewStateMode="Enabled" />--%><br />
                        <asp:LinkButton CssClass="boldText" ID="btnEditSubstanceSsi" runat="server" Text="Edit" Width="75px" Enabled="false" OnClick="btnEditSubstanceSsiOnClick" /><br />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnRemoveSubstanceSsi" runat="server" Text="Remove" Width="75px" Enabled="false" OnClick="btnRemoveSubstanceSsiOnClick" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlSubAttachment" runat="server" Visible="true">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td width="150" style='table-layout: fixed; vertical-align: top; padding-top5px;' align="right">
                    <asp:Label ID="Label5" runat="server">Substance attachment:</asp:Label>
                </td>
                <td valign="top" rowspan="3">
                    <div style="margin-left: -3px">
                        <customControls:ListBox_CT ID="ctlSubAttachment" runat="server" ControlMultipleValueSelection="true" LabelColumnWidth="0px" ControlInputWidth="336px" ControlLabel="" AutoPostback="true" OnInputValueChanged="ctlSubAttachmentListInputValueChanged" />
                    </div>
                </td>
                <td width="30px" align="center">
                    <div style="margin-left: -70px; margin-top: 10px;">
                        <asp:LinkButton CssClass="boldText" ID="btnAddSubAttachment" runat="server" Text="Add" Width="75px" OnClick="btnAddSubAttachmentOnClick" />
                        <uc1:ucPopupFormSUBATT ID="UcPopupFormSUBATT" runat="server" ViewStateMode="Enabled" />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnEditSubAttachment" runat="server" Text="Edit" Width="75px" Enabled="false" OnClick="btnEditSubAttachmentOnClick" /><br />
                        <br />
                        <asp:LinkButton CssClass="boldText" ID="btnRemoveSubAttachment" runat="server" Text="Remove" Width="75px" Enabled="false" OnClick="btnRemoveSubAttachmentOnClick" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
<div style="margin-left: 27px;">
    <customControls:TextArea_CT ID="ctlComment" runat="server" ControlInputWidth="330px" LabelColumnWidth="150px" ControlLabel="Comments:" IsMandatory="false" ControlErrorMessage="" MaxLength="500" ControlEmptyErrorMessage="" />
</div>
<br />
<div class="bottomControlsHolder" valign="center">
    <asp:LinkButton ID="lbtCancel" Visible="true" runat="server" CssClass="button Cancel" CommandArgument="Cancel" CommandName="EventType" OnClick="btnCancelOnClick" Text=" Cancel" />
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button Save" OnClick="btnSaveOnClick"></asp:Button>
</div>
