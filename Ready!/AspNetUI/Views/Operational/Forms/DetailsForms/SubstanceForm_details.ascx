<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SubstanceForm_details.ascx.cs" Inherits="AspNetUI.Views.SubstanceForm_details" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="toolkit" %>
<!-- Operational controls -->
<%@ Register Src="~/Views/PartialShared/Operational/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="operational" %>
<%@ Register Src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" TagName="SearcherDisplay" TagPrefix="operational" %>
<!-- Custom input controls -->
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" TagName="TextArea_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/MoneyBox_CT.ascx" TagName="MoneyBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" TagName="DropDownList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" TagName="ListBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/RadioButtonList_CT.ascx" TagName="RadioButtonList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/CheckBoxList_CT.ascx" TagName="CheckBoxList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/ComboBox_CT.ascx" TagName="ComboBox_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/AutoCompleteBox_CT.ascx" TagName="AutoCompleteBox_CT" TagPrefix="customControls" %>
<asp:Panel ID="pnlDataDetails" runat="server" class="preventableClose" >
    <br />
    <customControls:TextBox_CT ID="ctlName" runat="server" LabelColumnWidth="150px" MaxLength="100" ControlInputWidth="300px" ControlLabel="Name:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Substance name can't be empty." />
    <customControls:TextBox_CT ID="ctlEVCode" runat="server" LabelColumnWidth="150px" MaxLength="100" ControlInputWidth="300px" ControlLabel="EVCODE:" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="EV Code can't be empty." />
    <table>
        <tr>
            <td style="width:150px;text-align:right;">
                <asp:Label ID="lblLastChangeTitle" runat="server">Last change:</asp:Label>
            </td>
            <td style="padding-left:-13px">
            <div style="margin-left:-6px">
                <customControls:TextBox_CT ID="lblLastChange" runat="server" FontBold="false" CurrentControlState="YouCantChangeMe" LabelColumnWidth="0px" MaxLength="100" ControlInputWidth="330px" ControlLabel=""></customControls:TextBox_CT>
            </div>
            </td>
        </tr>
    </table>
    <br />
    <div class="bottomControlsHolder" valign="center">
        <asp:LinkButton ID="lbtCancel" visible="true" runat="server" CssClass="button Cancel" CommandArgument="Cancel" CommandName="EventType" OnClick="btnCancelOnClick" Text=" Cancel" />
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button Save" OnClick="btnSaveOnClick"></asp:Button>
    </div>
    <%--<customControls:TextBox_CT ID="ctlSynonym1" runat="server" LabelColumnWidth="150px"
        MaxLength="100" ControlInputWidth="300px" ControlLabel="Synonym 1:" IsMandatory="false"
        ControlErrorMessage="" ControlEmptyErrorMessage="" />
    <customControls:TextBox_CT ID="ctlSynonym1Language" runat="server" LabelColumnWidth="150px"
        MaxLength="100" ControlInputWidth="300px" ControlLabel="Synonym 1 language:"
        IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
    <customControls:TextBox_CT ID="ctlSynonym2" runat="server" LabelColumnWidth="150px"
        MaxLength="100" ControlInputWidth="300px" ControlLabel="Synonym 2:" IsMandatory="false"
        ControlErrorMessage="" ControlEmptyErrorMessage="" />
    <customControls:TextBox_CT ID="ctlSynonym2Language" runat="server" LabelColumnWidth="150px"
        MaxLength="100" ControlInputWidth="300px" ControlLabel="Synonym 2 language:"
        IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />--%>
    <asp:Panel runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <table cellpadding="3" cellspacing="0" class="tablewidth1">
                        <tr>
                            <td style="width: 175px" valign="top">
                                <asp:Label ID="lblNameTitle" runat="server" Font-Bold="false">Name:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblName" runat="server" Font-Bold="True" valign="top"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="lblEVcodeTitle" runat="server" Font-Bold="false">EVCODE:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEVcode" runat="server" Font-Bold="True" valign="top"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="lblSynonym1Title" runat="server" Font-Bold="false">Synonym 1:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSynonym1" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="lblSynonym1LanguageTitle" runat="server" Font-Bold="false">Synonym 1 language:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSynonym1Language" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="lblSynonym2Title" runat="server" Font-Bold="false">Synonym 2:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSynonym2" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4" valign="top">
                                <asp:Label ID="lblSynonym2LanguageTitle" runat="server" Font-Bold="false">Synonym 2 language:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSynonym2Language" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>
