<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="True" CodeBehind="ASView.aspx.cs" Inherits="AspNetUI.Views.ASView" %>
<%@ Register src="Forms/DetailsForms/ASForm_details.ascx" tagname="ASForm_details" tagprefix="forms" %>
<%@ Register Src="~/Views/PartialShared/Operational/TabMenu.ascx" TagName="TabMenu" TagPrefix="operational" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

     <table cellpadding="0" cellspacing="0" width="100%" runat="server" id="tabMenuContainer">
        <tr>
            <%--<td style="border-bottom: 1px solid #AAAAAA;">--%>
            <td>
                <!-- Navigation for level2 forms -->
                <operational:TabMenu ID="tabMenu" runat="server" />
            </td>
        </tr>
    </table>
    <forms:ASForm_details ID="ASForm_details1" runat="server" />

</asp:Content> 