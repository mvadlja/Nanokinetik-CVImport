<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.ProjectView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeRangeBox.ascx" TagName="DateTimeRangeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/QuickLinksPopup.ascx" TagName="QuickLinksPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/ColumnsPopup.ascx" TagName="ColumnsPopup" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <div class="buttonsHolder">
        <div class="rightSection">
            <asp:Button ID="btnColumns" runat="server" Text="Columns" CssClass="button" Visible="True" />
            <asp:Button ID="btnSaveLayout" runat="server" Text="Save" CssClass=" SaveLay button" Visible="True" />
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
                <uc:TextBox ID="txtProjectName" runat="server" Label="Name:" MaxLength="200" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:DropDownList ID="ddlInternalStatus" runat="server" Label="Internal status:" />
                <uc:DropDownList ID="ddlCountry" runat="server" Label="Country:" />
                <uc:DateTimeRangeBox ID="dtRngStartDate" runat="server" Label="Start date:" />
                <uc:DateTimeRangeBox ID="dtRngExpectedFinishedDate" runat="server" Label="Expected finished date:" />
                <uc:DateTimeRangeBox ID="dtRngActualFinishedDate" runat="server" Label="Actual finished date:" />

                <asp:Panel ID="pnlSearchButtons" runat="server" CssClass="pnl-search-buttons">
                    <br />
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchClick"  CssClass="button"/>&nbsp;&nbsp;
                    <asp:LinkButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClearClick"  CssClass="button"/>
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
            <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Project:" Visible="False" />
        </div>
        <div class="subtabs" id="subtabs" runat="server">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <possGrid:PossGrid ID="ProjectGrid" GridId="ProjectGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="project_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn ="start_date" MainSortingOrder="DESC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Red" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="project_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="28%" Caption="Project" FieldName="ProjectName" FilterType="Text">
                    <ItemTemplate>
                        <div class="status">
                            <span runat="server" id="pnlStatusColor"></span>
                            <asp:HyperLink ID="hlId" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ProjectView/Preview.aspx?EntityContext=Project&idProj=" + Eval("project_PK")  %>'
                                Text='<%# HandleMissing(Eval("ProjectName")) %>' Font-Bold="true"></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="13%" Caption="Countries" FieldName="Countries" FilterType="Text" />
                <possGrid:PossTemplateField Width="24%" Caption="Related Activities" FieldName="Activities" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID='pnlActivities' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="6%" Caption="Start date" FieldName="start_date" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="6%" Caption="Expected date" FieldName="expected_finished_date" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="6%" Caption="Finished date" FieldName="actual_finished_date" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right"/>
                <possGrid:PossBoundField Width="7%" Caption="Internal status" FieldName="InternalStatus" FilterType="Text" />
                <possGrid:PossTemplateField Width="6%" ItemStyle-HorizontalAlign="Right" Caption="Activities" FieldName="ActCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlPreviewAct" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ActivityView/List.aspx?EntityContext=Project&idProj=" +  Eval("project_PK") %>' Text='<%# Eval("ActCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewActivity" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ActivityView/Form.aspx?EntityContext=Project&Action=New&idProj=" + Eval("project_PK") %>' Text="Add"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="4%" ItemStyle-HorizontalAlign="Right" Caption="Docs" FieldName="DocCount" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlPreviewDoc" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/List.aspx?EntityContext=Project&idProj=" +  Eval("project_PK") %>' Text='<%# Eval("DocCount") %>'></asp:HyperLink>
                        <asp:HyperLink ID="hlNewDocument" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/Form.aspx?EntityContext=Project&Action=New&idProj=" + Eval("project_PK") %>' Text="Add"></asp:HyperLink>
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
