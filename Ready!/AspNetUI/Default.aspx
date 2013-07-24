<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AspNetUI.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <asp:Label ID="Label1" runat="server">
        <b>Welcome to READY!.</b>
        <hr />
        <asp:Label ID="Label2" runat="server">
            Select something from menu.
            <br /><br />
            <asp:Label ID="lblDowntimeMessage" runat="server" />
        </asp:Label>
        <br />

    </asp:Label>
</asp:Content>