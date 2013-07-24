<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.ActivityView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeRangeBox.ascx" TagName="DateTimeRangeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/QuickLinksPopup.ascx" TagName="QuickLinksPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/ColumnsPopup.ascx" TagName="ColumnsPopup" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <style type="text/css">
          #resizable { width: 150px; height: 150px; padding: 0.5em; }
          #resizable h3 { text-align: center; margin: 0; }
    </style>
    <div class="buttonsHolder">
        <div class="rightSection">
            <asp:Button ID="btnColumns" runat="server" Text="Columns" CssClass="button" Visible="True" UseSubmitBehavior="False" />
            <asp:Button ID="btnSaveLayout" runat="server" Text="Save" CssClass="SaveLay button" Visible="True" />
            <asp:Button ID="btnClearLayout" runat="server" Text="Clear" CssClass="LoadLay button" Visible="True" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="ResetLay button" Visible="True" />
            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Export button" Style="margin-left: 25px;" />
        </div>
    </div>

    <asp:Panel runat="server" ID="pnlSearch" Visible="True">
        <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <uc:QuickLinksPopup ID="QuickLinksPopup" runat="server" />
                <uc:TextBoxSr ID="txtSrProduct" runat="server" Label="Product:" SearchType="Product" />
                <uc:TextBoxSr ID="txtSrProject" runat="server" Label="Project:" SearchType="Project" />
                <uc:TextBox ID="txtActivityName" runat="server" Label="Name:" MaxLength="500" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:TextBox ID="txtProcedureNumber" runat="server" Label="Procedure number:" MaxLength="500" />
                <uc:DropDownList ID="ddlProcedureType" runat="server" Label="Procedure type:" />
                <uc:DropDownList ID="ddlType" runat="server" Label="Type:" />
                <uc:DropDownList ID="ddlRegulatoryStatus" runat="server" Label="Regulatory status:" />
                <uc:DropDownList ID="ddlInternalStatus" runat="server" Label="Internal status:" />
                <uc:DropDownList ID="ddlActivityMode" runat="server" Label="Activity mode:" />
                <uc:DropDownList ID="ddlApplicant" runat="server" Label="Applicant:" />
                <uc:DropDownList ID="ddlCountry" runat="server" Label="Country:" />
                <uc:TextBox ID="txtLegalBasis" runat="server" Label="Legal basis of Application:" MaxLength="200" />
                <uc:TextBox ID="txtActivityId" runat="server" Label="Activity ID:" MaxLength="200" />
                <uc:RadioButtonYn ID="rbYnBillable" runat="server" Label="Billable:" />
                <uc:DateTimeRangeBox ID="dtRngStartDate" runat="server" Label="Start date:" />
                <uc:DateTimeRangeBox ID="dtRngExpectedFinishedDate" runat="server" Label="Expected finished date:" />
                <uc:DateTimeRangeBox ID="dtRngActualFinishedDate" runat="server" Label="Actual finished date:" />
                <uc:DateTimeRangeBox ID="dtRngSubmissionDate" runat="server" Label="Submission date:" />
                <uc:DateTimeRangeBox ID="dtRngApprovalDate" runat="server" Label="Approval date:" />

                <asp:Panel ID="pnlSearchButtons" runat="server" CssClass="pnl-search-buttons">
                    <br />
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchClick" CssClass="button" />&nbsp;&nbsp;
                    <asp:LinkButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClearClick" CssClass="button" />
                    <asp:LinkButton ID="btnSaveSearch" runat="server" Text="Save Quick link" OnClick="btnSaveSearchClick" Visible="true" CssClass="CreateQuickLink button" />
                    <asp:LinkButton ID="btnDeleteSearch" runat="server" Text="Delete Quick link" OnClick="btnDeleteSearchClick" Visible="false" CssClass="DeleteQuickLink button" />
                    <div style="position: absolute; right: 10px">
                        <asp:LinkButton ID="btnExportLower" runat="server" Text="Export" Style="margin-top: -20px" Visible="False" CssClass="button" />
                    </div>
                </asp:Panel>
                <br />

            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Panel ID="pnlGrid" runat="server" Visible="True">
        <uc:ColumnsPopup ID="ColumnsPopup" runat="server" />

        <div class="entity-name">
            <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Task:" Visible="False" />
        </div>
        <div class="subtabs" id="subtabs" runat="server">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <possGrid:PossGrid ID="ActivityGrid" GridId="ActivityGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false" CellPadding="4"
            DataKeyNames="activity_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="StartDate" DefaultSortingOrder="DESC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="activity_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="17%" Caption="Activity name" FieldName="ActivityName" FilterType="Text">
                    <ItemTemplate>
                        <div class="status">
                            <span runat="server" id="pnlStatusColor"></span>
                            <asp:HyperLink ID="hlId" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ActivityView/Preview.aspx?idAct=" + Eval("activity_PK") %>' Text='<%# HandleMissing(Eval("ActivityName")) %>' Font-Bold="true"></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="10%" Caption="Countries" FieldName="Countries" FilterType="Text" />
                <possGrid:PossTemplateField Width="22%" Caption="Products" FieldName="Products" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID="pnlProducts" runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="7%" Caption="Regulatory status" FieldName="RegulatoryStatus" FilterType="Combo" />
                <possGrid:PossBoundField Width="6%" Caption="Submission date" FieldName="SubmissionDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                <possGrid:PossBoundField Width="6%" Caption="Start date" FieldName="StartDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                <possGrid:PossBoundField Width="6%" Caption="Approval date" FieldName="ApprovalDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                <possGrid:PossBoundField Width="6%" Caption="ExpectedFinishedDate" FieldName="ExpectedFinishedDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" Visible="False" />
                <possGrid:PossBoundField Width="6%" Caption="Internal status" FieldName="InternalStatus" FilterType="Combo" />
                <possGrid:PossTemplateField Width="5%" ItemStyle-HorizontalAlign="Right" Caption="Tasks" FieldName="TasksCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlTaskCount" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/TaskView/List.aspx?idAct=" + Eval("activity_PK") %>' Text='<%# Eval("TasksCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewTask" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/TaskView/Form.aspx?Action=New&idAct=" + Eval("activity_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="4%" ItemStyle-HorizontalAlign="Right" Caption="Time" FieldName="TimeCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlTimeCount" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/TimeUnitView/List.aspx?idAct=" + Eval("activity_PK") %>' Text='<%# Eval("TimeCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewTime" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/TimeUnitView/Form.aspx?Action=New&idAct=" + Eval("activity_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossboundField Width="6%" Caption="Activity ID" Fieldname="ActivityID" Filtertype="Text" />   
                <possGrid:PossTemplateField Width="5%" ItemStyle-HorizontalAlign="Right" Caption="Docs" FieldName="DocumentsCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlDocumentsCount" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/List.aspx?idAct=" + Eval("activity_PK") %>' Text='<%# Eval("DocumentsCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewDocument" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/Form.aspx?Action=New&idAct=" + Eval("activity_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />

        </possGrid:PossGrid>
    </asp:Panel>
</asp:Content>
