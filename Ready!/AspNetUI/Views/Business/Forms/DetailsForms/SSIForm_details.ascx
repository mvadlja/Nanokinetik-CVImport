<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SSIForm_details.ascx.cs"
    Inherits="AspNetUI.Views.SSIForm_details" %>
<!-- Operational controls -->
<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay"
    TagPrefix="operational" %>
<!-- Custom input controls -->
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT"
    TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" TagName="TextArea_CT"
    TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT"
    TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT"
    TagPrefix="customControls" %>
<%@ Register Src="~/ucControls/PopupControls/ucSearcher.ascx" TagName="ucSearcher"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSC.ascx" TagName="ucPopupFormSC"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSN.ascx" TagName="ucPopupFormSN"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSTRUCT.ascx" TagName="ucPopupFormSTRUCT"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormMOIETY.ascx" TagName="ucPopupFormMOIETY"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormPROP.ascx" TagName="ucPopupFormPROP"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormVER.ascx" TagName="ucPopupFormVER"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormSCLF.ascx" TagName="ucPopupFormSCLF"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormREL.ascx" TagName="ucPopupFormREL"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormGN.ascx" TagName="ucPopupFormGN"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormGE.ascx" TagName="ucPopupFormGE"
    TagPrefix="uc1" %>
<%@ Register Src="~/ucControls/PopupControls/ucPopupFormTRG.ascx" TagName="ucPopupFormTRG"
    TagPrefix="uc1" %>
<asp:Panel ID="Panel1" runat="server" GroupingText="Substance">
    <div style="margin-left: 27px;">

        <customControls:TextBox_CT ID="ctlname" runat="server" LabelColumnWidth="150px" MaxLength="50"
            ControlInputWidth="300px" ControlLabel="Name:" IsMandatory="true" ControlErrorMessage=""
            ControlEmptyErrorMessage="Name can't be empty." />
        <customControls:DropDownList_CT ID="ctlresponsible_user" runat="server" LabelColumnWidth="150px"
            ControlInputWidth="306px" ControlLabel="Responsible user:" IsMandatory="false"
            ControlErrorMessage="" ControlEmptyErrorMessage="" />
        <customControls:TextArea_CT ID="ctldescription" runat="server" LabelColumnWidth="150px"
            MaxLength="2000" ControlInputWidth="300px" ControlLabel="Description:" IsMandatory="false"
            ControlErrorMessage="" ControlEmptyErrorMessage="" />
        <customControls:TextArea_CT ID="ctlcomments" runat="server" LabelColumnWidth="150px"
            MaxLength="250" ControlInputWidth="300px" ControlLabel="Comment:" IsMandatory="false"
            ControlErrorMessage="" ControlEmptyErrorMessage="" />

    </div>
</asp:Panel>
<br />
<asp:Panel ID="pnlDataDetails" runat="server" CssClass="preventableClose">
    <div style="margin-left: 27px;">
        <customControls:DropDownList_CT ID="ctlLanguages" runat="server" LabelColumnWidth="161px"
            ControlInputWidth="306px" ControlLabel="Language:" IsMandatory="true" ControlErrorMessage=""
            ControlEmptyErrorMessage="Language can't be empty." />
    </div>
    <br />
    <%--<asp:Panel ID="pnlCommon" runat="server" GroupingText="Common" Visible="true" CssClass="pnlBorderBold">--%>
    <fieldset class="pnlBorderBold">
        <legend>Common</legend>
        <div style="margin-left: 27px;">
            <asp:Panel ID="pnlSearchEVCODE" runat="server">
                <table cellpadding="2" cellspacing="0">
                    <tr>
                        <td align="right" style="width: 150px;">
                            <asp:Label Text="Substance ID:" runat="server"></asp:Label>
                        </td>
                        <td width="6px" />
                        <td>
                            <uc1:ucSearcher ID="EVCODESearcher" runat="server" />
                            <operational:SearcherDisplay ID="EVCODESearcherDisplay" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div style="margin-left: 27px;">
            <customControls:DropDownList_CT ID="ctlSubstanceClass" runat="server" LabelColumnWidth="150px"
                AutoPostback="True" ControlInputWidth="306px" ControlLabel="Substance classification:"
                IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Substance classification can't be empty."
                OnInputValueChanged="ctlSubstanceClassInputValueChanged" />
        </div>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Panel ID="pnlSubName" runat="server" GroupingText="Substance name" Visible="true">
                        <br />
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="162" style='table-layout: fixed' align="right">
                                    <asp:Label ID="lblSubNameTitle" runat="server">Substance name: </asp:Label>
                                    <asp:Label ID="lblSubNameReq" runat="server" ForeColor="Red" Visible="true">*</asp:Label>
                                </td>
                                <td valign="top" rowspan="3">
                                    <customControls:ListBox_CT ID="ctlSubNames" runat="server" ControlMultipleValueSelection="true"
                                        LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                        ControlEmptyErrorMessage="Substance name can't be empty." OnInputValueChanged="ctlSubNamesListInputValueChanged" />
                                </td>
                                <td valign="middle">
                                    <asp:Button ID="btnAddSubName" runat="server" Text="Add" Width="75px" OnClick="btnAddSubNameOnClick" />
                                    <uc1:ucPopupFormSN ID="SubNamesPopupForm" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td valign="middle">
                                    <asp:Button ID="btnEditSubName" runat="server" Text="Edit" Width="75px" OnClick="btnEditSubNameOnClick"
                                        Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td valign="middle">
                                    <asp:Button ID="btnRemoveSubNames" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveSubNamesOnClick"
                                        Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlSubCodes" runat="server" GroupingText="Substance code" Visible="true">
                        <br />
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="150" style='table-layout: fixed' align="right">
                                    <asp:Label ID="lblSubCodesTitle" runat="server">Substance code:</asp:Label>
                                    <asp:Label ID="lblSubCodesReq" runat="server" ForeColor="Red" Visible="false">*</asp:Label>
                                </td>
                                <td valign="top" rowspan="3">
                                    <customControls:ListBox_CT ID="ctlSubCodes" runat="server" ControlMultipleValueSelection="true"
                                        LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                        OnInputValueChanged="ctlSubCodesListInputValueChanged" ControlEmptyErrorMessage="Substance code can't be empty." />
                                </td>
                                <td valign="middle">
                                    <asp:Button ID="btnAddSubCode" runat="server" Text="Add" Width="75px" OnClick="btnAddSubCodeOnClick" />
                                    <uc1:ucPopupFormSC ID="SubCodesPopupForm" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td valign="middle">
                                    <asp:Button ID="btnEditSubCode" runat="server" Text="Edit" Width="75px" OnClick="btnEditSubCodeOnClick"
                                        Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td valign="middle">
                                    <asp:Button ID="btnRemoveSubCodes" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveSubCodesOnClick"
                                        Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>

            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Panel ID="pnlVersion" runat="server" GroupingText="Version">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="162" style='table-layout: fixed' align="right">
                                    <asp:Label ID="Label3" runat="server">Version: </asp:Label>
                                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Visible="true">*</asp:Label>
                                </td>
                                <td valign="top" rowspan="3">
                                    <customControls:ListBox_CT ID="ctlVersion" runat="server" ControlMultipleValueSelection="true"
                                        LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                        ControlEmptyErrorMessage="Version can't be empty." OnInputValueChanged="ctlVersionListInputValueChanged" />
                                </td>
                                <td valign="middle">
                                    <div style="margin-left: 0px">
                                        <asp:Button ID="btnAddVersion" runat="server" Text="Add" Width="75px" OnClick="btnAddVerOnClick" />
                                        <uc1:ucPopupFormVER ID="UcPopupFormVER" runat="server" ViewStateMode="Enabled" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td valign="middle">
                                    <div style="margin-left: 0px">
                                        <asp:Button ID="btnEditVER" runat="server" Text="Edit" Width="75px" OnClick="btnEditVerOnClick"
                                            Enabled="false" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td valign="middle">
                                    <div style="margin-left: 0px">
                                        <asp:Button ID="btnDeleteVER" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveVerOnClick"
                                            Enabled="false" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlRefInfo" runat="server" GroupingText="Reference information">
            <div style="margin-left: 15px;">
                <customControls:TextArea_CT ID="tbxComment" runat="server" LabelColumnWidth="150px"
                    MaxLength="4000" ControlInputWidth="300px" ControlLabel="Comment:" IsMandatory="false" />
            </div>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlSubstanceClassificationPopUp" runat="server" GroupingText="Substance classification">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="150" style='table-layout: fixed' align="right">
                                        <asp:Label ID="lblSCLFTitle" runat="server">Substance classification: </asp:Label>
                                        <asp:Label ID="lblSCLFReq" runat="server" ForeColor="Red" Visible="true">*</asp:Label>
                                    </td>
                                    <td valign="top" rowspan="3">
                                        <customControls:ListBox_CT EnableViewState="true" ID="ctlScfl" runat="server" ControlMultipleValueSelection="true"
                                            LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                            ControlEmptyErrorMessage="Reference information substance classification can't be empty."
                                            OnInputValueChanged="ctlScflInputValueChanged" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Button ID="btnAddSclf" runat="server" Text="Add" Width="75px" OnClick="btnAddSclfOnClick" />
                                        <uc1:ucPopupFormSCLF ID="UcPopupFormSCLF" runat="server" ViewStateMode="Enabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <asp:Button ID="btnEditSclf" runat="server" Text="Edit" Width="75px" Enabled="false"
                                            OnClick="btnEditSclfOnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <asp:Button ID="btnRemoveSclf" runat="server" Text="Remove" Width="75px" Enabled="false"
                                            OnClick="btnRemoveSclfOnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="pnpSubstanceRelationshipPopUp" runat="server" GroupingText="Substance relationship">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="150" style='table-layout: fixed' align="right">
                                        <asp:Label ID="lblRELTitle" runat="server">Substance relationship: </asp:Label>
                                    </td>
                                    <td valign="top" rowspan="3">
                                        <customControls:ListBox_CT ID="ctlRel" runat="server" ControlMultipleValueSelection="true"
                                            LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                            OnInputValueChanged="ctlRelInputValueChanged" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Button ID="btnAddRel" runat="server" Text="Add" Width="75px" OnClick="btnAddRelOnClick" />
                                        <uc1:ucPopupFormREL ID="UcPopupFormREL" runat="server" ViewStateMode="Enabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <asp:Button ID="btnEditRel" runat="server" Text="Edit" Width="75px" Enabled="false"
                                            OnClick="btnEditRelOnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <asp:Button ID="btnRemoveRel" runat="server" Text="Remove" Width="75px" Enabled="false"
                                            OnClick="btnRemoveRelOnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlGeneElement" runat="server" GroupingText="Gene element">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="150" style='table-layout: fixed' align="right">
                                        <asp:Label ID="lblGETitle" runat="server">Genes elements: </asp:Label>
                                    </td>
                                    <td valign="top" rowspan="3">
                                        <customControls:ListBox_CT ID="ctlGe" runat="server" ControlMultipleValueSelection="true"
                                            LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                            OnInputValueChanged="ctlGeInputValueChanged" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Button ID="btnAddGe" runat="server" Text="Add" Width="75px" OnClick="btnAddGeOnClick" />
                                        <uc1:ucPopupFormGE ID="UcPopupFormGE" runat="server" ViewStateMode="Enabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <asp:Button ID="btnEditGe" runat="server" Text="Edit" Width="75px" Enabled="false"
                                            OnClick="btnEditGeOnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <asp:Button ID="btnRemoveGe" runat="server" Text="Remove" Width="75px" Enabled="false"
                                            OnClick="btnRemoveGeOnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="pnlGene" runat="server" GroupingText="Gene">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="150" style='table-layout: fixed' align="right">
                                        <asp:Label ID="lblGN" runat="server">Genes: </asp:Label>
                                    </td>
                                    <td valign="top" rowspan="3">
                                        <customControls:ListBox_CT ID="ctlGene" runat="server" ControlMultipleValueSelection="true"
                                            LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                            OnInputValueChanged="ctlGnInputValueChanged" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Button ID="btnAddGn" runat="server" Text="Add" Width="75px" OnClick="btnAddGnOnClick" />
                                        <uc1:ucPopupFormGN ID="UcPopupFormGN" runat="server" ViewStateMode="Enabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <asp:Button ID="btnEditGn" runat="server" Text="Edit" Width="75px" Enabled="false"
                                            OnClick="btnEditGnOnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <asp:Button ID="btnRemoveGn" runat="server" Text="Remove" Width="75px" Enabled="false"
                                            OnClick="btnRemoveGnOnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>

                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTarget" runat="server" GroupingText="Target" Visible="true">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="152" style='table-layout: fixed' align="right">
                                        <asp:Label ID="lblTargetTitle" runat="server">Targets:</asp:Label>
                                    </td>
                                    <td valign="top" rowspan="3">
                                        <div style="margin-left: -3px">
                                            <customControls:ListBox_CT ID="ctlTrg" runat="server" ControlMultipleValueSelection="true"
                                                LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                                OnInputValueChanged="ctlTrgInputValueChanged" />
                                        </div>
                                    </td>
                                    <td valign="middle">
                                        <div style="margin-left: 0px">
                                            <asp:Button ID="btnAddTrg" runat="server" Text="Add" Width="75px" OnClick="btnAddTrgOnClick" />
                                            <uc1:ucPopupFormTRG ID="UcPopupFormTRG" runat="server" ViewStateMode="Enabled" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <div style="margin-left: 0px">
                                            <asp:Button ID="btnEditTrg" runat="server" Text="Edit" Width="75px" Enabled="false"
                                                OnClick="btnEditTrgOnClick" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td valign="middle">
                                        <div style="margin-left: 0px">
                                            <asp:Button ID="btnRemoveTrg" runat="server" Text="Remove" Width="75px" Enabled="false"
                                                OnClick="btnRemoveTrgOnClick" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%--   </asp:Panel>--%>
    </fieldset>


    <%--<asp:Panel ID="pnlExtended" runat="server" GroupingText="Extended" Visible ="false">--%>
    <fieldset id="pnlExtended" runat="server" class="pnlBorderBold" visible="false">
        <legend>Extended</legend>
        <asp:Panel ID="pnlSingleSubstance" runat="server" GroupingText="Single substance"
            Visible="false">
            <table cellpadding="2" cellspacing="0">
                <tr>
                    <td align="left">
                        <customControls:DropDownList_CT ID="ctlSubstanceType" runat="server" AutoPostback="True"
                            LabelColumnWidth="137px" ControlInputWidth="300px" ControlErrorMessage="" ControlLabel="Substance Type:"
                            ControlEmptyErrorMessage="Substance type can't be empty." OnInputValueChanged="ctlSubstanceTypeInputValueChanged" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlStructPopUp" runat="server" GroupingText="Structure" Visible="true">
                <br />
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="150" style='table-layout: fixed' align="right">
                            <asp:Label ID="lblStructures" runat="server">Structures:</asp:Label>
                        </td>
                        <td valign="top" rowspan="3">
                            <customControls:ListBox_CT ID="ctlStructList" runat="server" ControlMultipleValueSelection="true"
                                LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                OnInputValueChanged="ctlStructListInputValueChanged" />
                        </td>
                        <td valign="middle">
                            <asp:Button ID="btnStructAdd" runat="server" Text="Add" Width="75px" OnClick="btnAddStructCodeOnClick" />
                            <uc1:ucPopupFormSTRUCT ID="UcPopupFormSTRUCT" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td valign="middle">
                            <asp:Button ID="btnStructEdit" runat="server" Text="Edit" Width="75px" OnClick="btnEditStructCodeOnClick" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td valign="middle">
                            <asp:Button ID="btnStructRemove" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveStructOnClick" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%--<customControls:Label_CT ID="ctlID" runat="server" ControlLabelAlign="left" TotalControlWidth="100%" />
        <%--<customControls:TextBox_CT ID="ctlresolutionmode" runat="server" LabelColumnWidth="150px" MaxLength="300" ControlInputWidth="300px" ControlLabel="Resolution mode" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />--%>
            <%--<customControls:TextBox_CT ID="ctlname" runat="server" LabelColumnWidth="150px" MaxLength="50" ControlInputWidth="300px" ControlLabel="Name" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Name can't be empty." />
	    <customControls:TextBox_CT ID="ctlPID" runat="server" LabelColumnWidth="150px" MaxLength="300" ControlInputWidth="300px" ControlLabel="ID" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
	    <customControls:DropDownList_CT ID="ctlresponsible_user" runat="server" LabelColumnWidth="150px" ControlInputWidth="300px" ControlLabel="Responsible user" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
	    <customControls:TextBox_CT ID="ctldescription" runat="server" LabelColumnWidth="150px" MaxLength="2000" ControlInputWidth="300px" ControlLabel="Description" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
	    <customControls:TextBox_CT ID="ctlcomments" runat="server" LabelColumnWidth="150px" MaxLength="250" ControlInputWidth="300px" ControlLabel="Comments" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />--%>
            <br />
            <asp:Panel ID="pnlChemical" runat="server" GroupingText="Chemical" Visible="true">
                <br />
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td />
                                    <td />
                                    <td>
                                        <asp:Label runat="server" ID="PDYes" Text="Yes" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="PDNo" Text="No" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150px" align="right">
                                        <asp:Label runat="server" ID="ctlStoichiometric" Text="Stoichiometric: " />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Width="10px" ID="Stoichiometic_asterix" Text="*" ForeColor="Red" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RadioButtonYes" runat="server" AutoPostBack="true" EnableViewState="true"
                                            Visible="true" LabelColumnWidth="150px" ControlLabel="" ControlErrorMessage=""
                                            ControlEmptyErrorMessage="" GroupName="rbtNS" OnCheckedChanged="NSYesChecked" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RadioButtonNo" runat="server" AutoPostBack="true" EnableViewState="true"
                                            Visible="true" LabelColumnWidth="150px" ControlLabel="" ControlErrorMessage=""
                                            ControlEmptyErrorMessage="" GroupName="rbtNS" OnCheckedChanged="NSNoChecked" />
                                    </td>
                                </tr>
                            </table>
                            <%--<customControls:CheckBoxList_CT ID="ctlStoichiometric" runat="server" LabelColumnWidth="150px"
                            ControlInputWidth="300px" ControlLabel="Stoichiometric:" ControlErrorMessage=""
                            ControlEmptyErrorMessage="" AutoPostback="true" OnInputValueChanged="ctlStoichiometricInputValueChanged" />--%>
                        </td>
                    </tr>
                    <tr />
                    <tr>
                        <td>
                            <customControls:TextArea_CT ID="ctlcomment" runat="server" LabelColumnWidth="150px"
                                MaxLength="300" ControlInputWidth="300px" ControlLabel="Comment:" IsMandatory="false"
                                ControlErrorMessage="" ControlEmptyErrorMessage="" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlNS" runat="server" GroupingText="NonStoichiometric" Visible="true">
                    <br />
                    <table>
                        <tr>
                            <td width="150px" align="center">
                                <asp:Label ID="NoOfMoieties" Text="Number of moieties: " Width="115px" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="NumMoiety" ReadOnly="true" runat="server" Width="20px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="170" style='table-layout: fixed' align="right">
                                            <asp:Label ID="Label1" runat="server">Moieties:</asp:Label>
                                            <asp:Label ID="Moiety_asterix" runat="server" Text="*" ForeColor="Red" />
                                        </td>
                                        <td valign="top" rowspan="3">
                                            <customControls:ListBox_CT ID="ctlMoiety" runat="server" ControlMultipleValueSelection="true"
                                                LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                                OnInputValueChanged="ctlMoietyListInputValueChanged" />
                                        </td>
                                        <td valign="middle">
                                            <asp:Button ID="btnAddMoiety" runat="server" Text="Add" Width="75px" OnClick="btnAddMoietyOnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td valign="middle">
                                            <asp:Button ID="btnEditMoiety" runat="server" Text="Edit" Width="75px" OnClick="btnEditMoietyOnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td valign="middle">
                                            <asp:Button ID="btnRemoveMoiety" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveMoietyOnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td width="170" style='table-layout: fixed' align="right">
                                            <asp:Label ID="Label2" runat="server">Properties:</asp:Label>
                                        </td>
                                        <td valign="top" rowspan="3">
                                            <customControls:ListBox_CT ID="ctlProperty" runat="server" ControlMultipleValueSelection="true"
                                                LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                                OnInputValueChanged="ctlPropertyListInputValueChanged" />
                                        </td>
                                        <td valign="middle">
                                            <asp:Button ID="btnAddProperty" runat="server" Text="Add" Width="75px" OnClick="btnAddPropertyOnClick" />
                                            <uc1:ucPopupFormPROP ID="PropertyPopupForm" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td valign="middle">
                                            <asp:Button ID="btnEditProperty" runat="server" Text="Edit" Width="75px" OnClick="btnEditPropertyOnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td valign="middle">
                                            <asp:Button ID="btnRemoveProperty" runat="server" Text="Remove" Width="75px" OnClick="btnRemovePropertyOnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp
                                        </td>
                                    </tr>
                                </table>
                            </td>
                    </table>
                </asp:Panel>
                <br />
                <%--<asp:Panel ID="pnlRefSource" runat="server" GroupingText="Reference Source" Visible="true">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="180" style='table-layout: fixed' align="right">
                            <asp:Label ID="lblRefSourceTitle" runat="server">Reference sources: </asp:Label>
                            <asp:Label ID="lblRefSourceReq" runat="server" ForeColor="Red">*</asp:Label>
                        </td>
                        <td valign="top" rowspan="3">
                            <customControls:ListBox_CT ID="ctlRefSources" runat="server" ControlMultipleValueSelection="true"
                                LabelColumnWidth="0px" ControlInputWidth="300px" ControlLabel="" AutoPostback="true"
                                OnInputValueChanged="ctlRefSourcesListInputValueChanged" />
                        </td>
                        <td valign="middle">
                            <asp:Button ID="btnAddRefSource" runat="server" Text="Add" Width="75px" OnClick="btnAddRefSourceOnClick" />
                            <uc1:ucPopupFormRS ID="RefSourcesPopupForm" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td valign="middle">
                            <asp:Button ID="btnEditRefSource" runat="server" Text="Edit" Width="75px" OnClick="btnEditRefSourceOnClick" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td valign="middle">
                            <asp:Button ID="btnRemoveRefSources" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveRefSourcesOnClick" />
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>--%>
                <uc1:ucPopupFormMOIETY ID="MoietyPopupForm" runat="server" />
                <br />
            </asp:Panel>
            <br />
        </asp:Panel>
    </fieldset>
    <%--</asp:Panel>--%>
</asp:Panel>
