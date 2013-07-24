<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="True" CodeBehind="SSIFView.aspx.cs" Inherits="AspNetUI.Views.SSIFView" %>
<%@ Register src="~/Views/Business/Forms/DetailsForms/SSIFForm_details.ascx" tagname="SSIFForm_details" tagprefix="forms" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <br />
    <forms:SSIFForm_details ID="SSIFForm_details1" runat="server" />

</asp:Content>