<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="True" CodeBehind="RolesView.aspx.cs" Inherits="AspNetUI.Views.RolesView" %>
<%@ Register src="Forms/ListForms/RoleForm_list.ascx" tagname="RoleForm_list" tagprefix="forms" %>
<%@ Register src="Forms/DetailsForms/RoleForm_details.ascx" tagname="RoleForm_details" tagprefix="forms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <!-- View's forms as partial views -->
    <forms:RoleForm_list ID="RoleForm_list1" runat="server" />
    <forms:RoleForm_details ID="RoleForm_details1" runat="server" />

</asp:Content>