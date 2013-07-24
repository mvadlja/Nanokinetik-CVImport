<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="True" CodeBehind="UsersView.aspx.cs" Inherits="AspNetUI.Views.UsersView" %>
<%@ Register src="Forms/DetailsForms/UserForm_details.ascx" tagname="UserForm_details" tagprefix="forms" %>
<%@ Register Src="~/Views/PartialShared/Operational/TabMenu.ascx" TagName="TabMenu" TagPrefix="operational" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
 <!-- View's forms as partial views -->
     <table cellpadding="0" cellspacing="0" width="100%" runat="server" id="tabMenuContainer">
        <tr>
            <%--<td style="border-bottom: 1px solid #AAAAAA;">--%>
            <td>
                <!-- Navigation for level2 forms -->
                <operational:TabMenu ID="tabMenu" runat="server" />
            </td>
        </tr>
    </table>
    <!-- View's forms as partial views -->
    <forms:UserForm_details ID="UserForm_details1" runat="server" />

</asp:Content>