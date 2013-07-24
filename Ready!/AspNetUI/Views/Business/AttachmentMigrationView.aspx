<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="True" CodeBehind="AttachmentMigrationView.aspx.cs" Inherits="AspNetUI.Views.AttachmentMigrationView" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div runat="server" id="btnDoItBiv">
        <asp:Button ID="doThaJob" runat="server" Text="Do it!" OnClick="DoIt_click"/>
    </div>
    <div id="info" runat="server"></div>


</asp:Content>