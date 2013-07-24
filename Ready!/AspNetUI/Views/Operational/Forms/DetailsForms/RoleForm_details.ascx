<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="RoleForm_details.ascx.cs" Inherits="AspNetUI.Views.RoleForm_details" %>
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


<asp:Panel ID="pnlDataDetails" runat="server"  class="preventableClose" >
    <customControls:Label_CT ID="ctlID" runat="server" ControlLabelAlign="left" TotalControlWidth="100%" />
	
	<customControls:TextBox_CT ID="ctlName" runat="server" LabelColumnWidth="150px" MaxLength="300" ControlInputWidth="300px" ControlLabel="Name" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Name can't be empty." />
	<customControls:TextBox_CT ID="ctlDisplayName" runat="server" LabelColumnWidth="150px" MaxLength="300" ControlInputWidth="300px" ControlLabel="Display name" IsMandatory="true" ControlErrorMessage="" ControlEmptyErrorMessage="Display name can't be empty." />
	<customControls:TextArea_CT ID="ctlDescription" runat="server" LabelColumnWidth="150px" MaxLength="300" ControlInputWidth="300px" ControlVisibleRows="5" ControlLabel="Description" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
	<customControls:CheckBoxList_CT ID="ctlActive" runat="server" LabelColumnWidth="150px" ControlLabel="Active" IsMandatory="false" ControlErrorMessage="" ControlEmptyErrorMessage="" />
</asp:Panel>