<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEDMS.aspx.cs" Inherits="AspNetUI.Views.TestEDMS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <label for="txtDocumentId">Document ID:</label>
        <asp:TextBox runat="server" ID="txtEDMSDocumentId"></asp:TextBox>
        <asp:Button runat="server" ID="btnFetchEDMS" OnClick="btnFetchEDMS_Click" Text="Fetch EDMS" />
        <br />
        <br />
        <br />
        <div runat="server" id="divLocalDataHeader" Visible="False">Data fetched locally:</div>
        <div id="divLocalGrid" runat="server">
        </div>
        <br />
        <br />
        <div runat="server" id="divServiceDataHeader" Visible="False">Data fetched from service:</div>
        <div id="divServiceGrid" runat="server">
        </div>
    </form>
</body>
</html>
