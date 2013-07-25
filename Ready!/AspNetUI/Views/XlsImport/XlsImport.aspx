<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="true" CodeBehind="XlsImport.aspx.cs" Inherits="AspNetUI.Views.XlsImport.XlsImport" %>

<%@ Register Src="../Shared/UserControl/FileUploadCtrl.ascx" TagName="FileUploadCtrl" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">

    <asp:Panel ID="pnlForm" runat="server" class="form">
        <div>
            <uc:FileUploadCtrl runat="server" ID="fileUpload" Label="Browse for file to import" />
        </div>     
    </asp:Panel>

    <asp:Panel ID="pnlFooter" runat="server" class="bottom clear bottomControlsHolder" valign="center">    
        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="button Cancel" OnClick="btnCancel_OnClick" />    
        <asp:LinkButton ID="btnImport" runat="server" Text="Import" CssClass="button Save" OnClick="btnImport_OnClick" />
    </asp:Panel>   

</asp:Content>
