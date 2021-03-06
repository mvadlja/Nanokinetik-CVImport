﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Preview.aspx.cs" Inherits="AspNetUI.Views.ActivityView.Preview" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Reminder.ascx" TagName="Reminder" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:Reminder ID="Reminder" runat="server" />
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Activity:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel ID="pnlProperties" runat="server" class="preview" CssClass="properties padding-0">
        <div id="linkedEntities" class="linkedEntities">
            <uc:LabelPreview ID="lblPrvProducts" runat="server" Label="Products:" Required="True" TextWidth="400px"  TextBold="True" />
            <uc:LabelPreview ID="lblPrvProjects" runat="server" Label="Projects:" TextWidth="400px"  TextBold="True" />
        </div>
        <div id="divLeftPane" class="leftPane">
            <uc:LabelPreview ID="lblPrvName" runat="server" Label="Name:" Required="True"/>
            <uc:LabelPreview ID="lblPrvActivityId" runat="server" Label="Activity ID:" />
            <uc:LabelPreview ID="lblPrvDescription" runat="server" Label="Description:" />
            <uc:LabelPreview ID="lblPrvResponsibleUser" runat="server" Label="Responsible user:" />
            <uc:LabelPreview ID="lblPrvProcedureNumber" runat="server" Label="Procedure number:" />
            <uc:LabelPreview ID="lblPrvProcedureType" runat="server" Label="Procedure type:" Required="True"/>
            <uc:LabelPreview ID="lblPrvTypes" runat="server" Label="Types:" Required="True"/>
            <uc:LabelPreview ID="lblPrvRegulatoryStatus" runat="server" Label="Regulatory status:" />
            <uc:LabelPreview ID="lblPrvInternalStatus" runat="server" Label="Internal status:" Required="True"/>
            <uc:LabelPreview ID="lblPrvActivityMode" runat="server" Label="Activity mode:" />
            <uc:LabelPreview ID="lblPrvApplicant" runat="server" Label="Applicant:" />
            <uc:LabelPreview ID="lblPrvCountries" runat="server" Label="Countries:" Required="True"/>
            <uc:LabelPreview ID="lblPrvLegalBasisOfApplication" runat="server" Label="Legal basis of application:" />
            <uc:LabelPreview ID="lblPrvComment" runat="server" Label="Comment:" />
        </div>
        <div id="divRightPane" class="rightPane">
            <uc:LabelPreview ID="lblPrvStartDate" runat="server" Label="Start date:" Required="True" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvExpectedFinishedDate" runat="server" Label="Expected finished date:" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvActualFinishedDate" runat="server" Label="Actual finished date:" />
            <uc:LabelPreview ID="lblPrvSubmissionDate" runat="server" Label="Submission date:" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvApprovalDate" runat="server" Label="Approval date:" ShowReminder="true" />
            <uc:LabelPreview ID="lblPrvBillable" runat="server" Label="Billable:" />
            <uc:LabelPreview ID="lblPrvAutomaticAlerts" runat="server" Label="Automatic alerts:" />
            <uc:LabelPreview ID="lblPrvLastChange" runat="server" Label="Last change:" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlFooter" runat="server" class="previewBottom clear">
        <asp:LinkButton ID="btnAddDocument" runat="server" Text="Add document" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnAddTask" runat="server" Text="Add task" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnAddTimeUnit" runat="server" Text="Add time unit" CssClass="button"></asp:LinkButton>
        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="button Delete marginLeft"></asp:LinkButton>
    </asp:Panel>
</asp:Content>
