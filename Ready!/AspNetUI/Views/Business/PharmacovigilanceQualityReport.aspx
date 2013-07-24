<%@ Page Language="C#" MasterPageFile="../Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="PharmacovigilanceQualityReport.aspx.cs" Inherits="AspNetUI.Views.Business.PharmacovigilanceQualityReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeRangeBox.ascx" TagName="DateTimeRangeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">

    <div id="pvq-report-search-controls">
        <uc:TextBoxSr ID="txtSrActiveIngredient" runat="server" Label="Active ingredient:" SearchType="Substance" />
        <uc:TextBoxSr ID="txtSrProduct" runat="server" Label="Product:" SearchType="Product" />
        <uc:TextBoxSr ID="txtSrActivity" runat="server" Label="Activity:" SearchType="Activity" />
        <uc:TextBoxSr ID="txtSrTask" runat="server" Label="Task:" SearchType="Task" />
        <uc:TextBox ID="txtTaskId" runat="server" Label="Task ID:" MaxLength="450" LabelWidth="150px" TextWidth="330px" />
        <uc:DropDownList ID="ddlTaskResponsibleUser" runat="server" Label="Responsible user:" />
        <uc:DropDownList ID="ddlTaskPerformanceIndicator" runat="server" Label="Performance indicator:" />

        <div id="dt-rng-container">
            <uc:DateTimeRangeBox ID="dtRngTaskStartDate" runat="server" Label="Start date:" />
            <uc:DateTimeRangeBox ID="dtRngTaskExpectedFinishedDate" runat="server" Label="Expected finished date:" />
            <uc:DateTimeRangeBox ID="dtRngTaskActualFinishedDate" runat="server" Label="Actual finished date:" />
        </div>
        <div id="action-container">
            <asp:Button ID="btnCreateReport" runat="server" Text="Create report" />
            <asp:Button ID="btnCreateReportTest" runat="server" Text="Create report TEST" Visible="False"/>
        </div>
    </div>

    <div style="word-break: break-all;">
        <rsweb:ReportViewer ID="rvMain" runat="server" Font-Names="Verdana" Font-Size="9pt"
            ProcessingMode="Remote" SizeToReportContent="True" Style="width: 100%;">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
