﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Preview.aspx.cs" Inherits="AspNetUI.Views.TimeUnitView.Preview" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvTimeUnit" runat="server" Label="Time unit:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel ID="pnlProperties" runat="server" class="preview" CssClass="properties padding-0">
        <div id="linkedEntities" class="linkedEntities">
            <uc:LabelPreview ID="lblPrvProducts" runat="server" Label="Products:" TextWidth="400px" TextBold="True" />
            <uc:LabelPreview ID="lblPrvActivity" runat="server" Label="Activity:" TextWidth="400px" TextBold="True" />
        </div>
        <div id="divLeftPane" class="leftPane">
            <uc:LabelPreview ID="lblPrvName" runat="server" Label="Name:" Required="true" />
            <uc:LabelPreview ID="lblPrvResponsibleUser" runat="server" Label="Responsible user:" />
            <uc:LabelPreview ID="lblPrvInsertedBy" runat="server" Label="Inserted by:" />
            <uc:LabelPreview ID="lblPrvActualDate" runat="server" Label="Actual date:" />
            <uc:LabelPreview ID="lblPrvTime" runat="server" Label="Time:" />
            <uc:LabelPreview ID="lblPrvDescription" runat="server" Label="Description:" />
            <uc:LabelPreview ID="lblPrvComment" runat="server" Label="Comment:" />
            <uc:LabelPreview ID="lblPrvLastChange" runat="server" Label="Last change:" />
        </div>
        <div id="divRightPane" class="rightPane">
            
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlFooter" runat="server" class="previewBottom clear">
        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="button Delete"></asp:LinkButton>
    </asp:Panel>
</asp:Content>
