<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="True" CodeBehind="SSIView.aspx.cs" Inherits="AspNetUI.Views.SSIView" %>
<%@ Register src="Forms/DetailsForms/SSIForm_details.ascx" tagname="SSIForm_details" tagprefix="forms" %>
<%@ Register Src="~/Views/PartialShared/Operational/TabMenu.ascx" TagName="TabMenu" TagPrefix="operational" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <!-- View's forms as partial views -->
    <br />
    <forms:SSIForm_details ID="SSIForm_details1" runat="server" />

</asp:Content>