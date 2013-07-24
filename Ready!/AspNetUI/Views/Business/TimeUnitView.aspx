<%@ Page Title="" Language="C#" MasterPageFile="../Shared/Template/Default.Master" AutoEventWireup="True" CodeBehind="TimeUnitView.aspx.cs" Inherits="AspNetUI.Views.TimeUnitViewOLD" %>
<%@ Register src="Forms/ListForms/TimeUnitForm_list.ascx" tagname="TimeUnitForm_list" tagprefix="forms" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <div class="longWordbreakProperty">
    <customControls:Label_CT ID="lblName" Visible="false" runat="server" ControlLabelAlign="Center" FontValueBold="true" ControlLabel="Activity:"/>
    </div>

    <forms:TimeUnitForm_list ID="TimeUnitForm_list1" runat="server"/>
</asp:Content>