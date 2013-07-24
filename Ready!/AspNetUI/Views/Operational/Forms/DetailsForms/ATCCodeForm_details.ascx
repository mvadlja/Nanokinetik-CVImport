<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ATCCodeForm_details.ascx.cs" Inherits="AspNetUI.Views.ATCCodeForm_details" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="toolkit" %>
<!-- Operational controls -->
<%@ Register Src="~/Views/PartialShared/Operational/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="operational" %>
<%@ Register src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" tagname="SearcherDisplay" tagprefix="operational" %>

<!-- Custom input controls -->
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" tagname="TextArea_CT" tagprefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/MoneyBox_CT.ascx" tagname="MoneyBox_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" tagname="ListBox_CT" tagprefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/RadioButtonList_CT.ascx" TagName="RadioButtonList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/CheckBoxList_CT.ascx" TagName="CheckBoxList_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ComboBox_CT.ascx" tagname="ComboBox_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/AutoCompleteBox_CT.ascx" tagname="AutoCompleteBox_CT" tagprefix="customControls" %>

<asp:Panel ID="pnlDataDetails" runat="server" class="preventableClose">
    <br />
	<customControls:TextBox_CT ID="ctlName" runat="server" LabelColumnWidth="150px" MaxLength="100" ControlInputWidth="330px" ControlLabel="Name:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Name can't be empty." />
	<customControls:TextBox_CT ID="ctlATCCode" runat="server" LabelColumnWidth="150px" MaxLength="100" ControlInputWidth="330px" ControlLabel="ATC code:" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="ATC code can't be empty." />
</asp:Panel>

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