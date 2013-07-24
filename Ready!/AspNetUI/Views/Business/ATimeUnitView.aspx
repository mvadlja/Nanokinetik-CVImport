<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="True" CodeBehind="ATimeUnitView.aspx.cs" Inherits="AspNetUI.Views.ATimeUnitView" %>
<%@ Register src="Forms/ListForms/TimeUnitForm_list.ascx" tagname="TimeUnitForm_list" tagprefix="forms" %>
<%@ Register src="Forms/DetailsForms/ATimeUnitForm_details.ascx" tagname="ATimeUnitForm_details" tagprefix="forms" %>
<%@ Register Src="~/Views/PartialShared/Operational/TabMenu.ascx" TagName="TabMenu" TagPrefix="operational" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="longWordbreakProperty">
    <customControls:Label_CT ID="lblName" Visible="false" runat="server" ControlLabelAlign="Center" FontValueBold="true" ControlLabel="Activity:"/>
    </div>
    <!-- View's forms as partial views -->
     <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="border-bottom: 1px solid #AAAAAA;">
                <!-- Navigation for level2 forms -->
                <operational:TabMenu ID="tabMenu" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <!-- View's forms as partial views -->
                <forms:TimeUnitForm_list ID="ATimeUnitForm_list1" runat="server"/>
                <forms:ATimeUnitForm_details ID="ATimeUnitForm_details1" runat="server"/>
            </td>
        </tr>
    </table>
</asp:Content>