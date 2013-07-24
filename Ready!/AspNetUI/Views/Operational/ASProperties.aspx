<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="True" CodeBehind="ASProperties.aspx.cs" Inherits="AspNetUI.Views.ASProperties" %>
<%@ Register src="Forms/DetailsForms/ASForm_details.ascx" tagname="ASForm_details" tagprefix="forms" %>
<%@ Register src="Forms/ListForms/ASPropertiesForm_list.ascx" tagname="ASPropertiesForm_list" tagprefix="forms" %>
<%@ Register Src="~/Views/PartialShared/Operational/TabMenu.ascx" TagName="TabMenu"
    TagPrefix="operational" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT"
    TagPrefix="customControls" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
 <customControls:Label_CT ID="lblName" Visible="false" runat="server" ControlLabelAlign="Center" FontValueBold="true" ControlLabel="Approved substance:" />
        <!-- View's forms as partial views -->
     <table cellpadding="0" cellspacing="0" width="100%" runat="server" id="tabMenuContainer">
        <tr>
            <td style="border-bottom: 1px solid #AAAAAA;">
                <!-- Navigation for level2 forms -->
                <operational:TabMenu ID="tabMenu" runat="server" />
            </td>
        </tr>
    </table>

    <br />
    <forms:ASPropertiesForm_list ID="ASForm_list1" runat="server" />
    <forms:ASForm_details ID="ASForm_details1" runat="server" />

</asp:Content> 