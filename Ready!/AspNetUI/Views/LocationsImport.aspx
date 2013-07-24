<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocationsImport.aspx.cs" Inherits="AspNetUI.Views.LocationsImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnImportLocations" Text="Import locations and update path" runat="server" OnClick="btnImportLocations_Click" />
            <asp:Button ID="btnUpdateFullPath" Text="Update full path" runat="server" OnClick="btnUpdateFullPath_Click" />
        </div>
    </form>
</body>
</html>
