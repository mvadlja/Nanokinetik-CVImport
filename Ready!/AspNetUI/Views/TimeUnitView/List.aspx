<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.TimeUnitView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>

<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
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
                <uc:TextBoxSr ID="txtSrActivity" runat="server" Label="Activity:" SearchType="Activity" />
                <uc:DropDownList ID="ddlName" runat="server" Label="Name:" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:DateTimeRangeBox ID="dtRngActualDate" runat="server" Label="Actual date:" />

                <asp:Panel ID="pnlSearchButtons" runat="server" CssClass="pnl-search-buttons">
                    <br />
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchClick"  CssClass="button" />&nbsp;&nbsp;
                    <asp:LinkButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClearClick" CssClass="button" />
                    <asp:LinkButton ID="btnSaveSearch" runat="server" Text="Save Quick link" OnClick="btnSaveSearchClick" Visible="true" CssClass="CreateQuickLink button" />
                    <asp:LinkButton ID="btnDeleteSearch" runat="server" Text="Delete Quick link" OnClick="btnDeleteSearchClick" Visible="false" CssClass="DeleteQuickLink button" />
                    <div style="position: absolute; right: 10px">
                        <asp:LinkButton ID="btnExportLower" runat="server" CssClass="button" Text="Export" Style="margin-top: -20px" Visible="False" />
                    </div>
                </asp:Panel>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Panel ID="pnlGrid" runat="server" Visible="True">
        <uc:ColumnsPopup ID="ColumnsPopup" runat="server" />
        <div class="entity-name">
            <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Product:" Visible="False" />
        </div>
        <div class="subtabs" id="subtabs" runat="server">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <possGrid:PossGrid ID="TimeUnitGrid" GridId="TimeUnitGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="time_unit_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="TimeUnitName" DefaultSortingOrder="ASC"
            AllowGrouping="True" GroupingColumn="actual_date">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="time_unit_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="12%" Caption="Name" FieldName="TimeUnitName" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlId" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/TimeUnitView/Preview.aspx?idTimeUnit=" + Eval("time_unit_PK") %>'
                            Text='<%# HandleMissing(Eval("TimeUnitName")) %>' Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="9%" Caption="Responsible user" FieldName="ResponsibleUser" FilterType="Text" />
                <possGrid:PossBoundField Width="6%" Caption="Actual date" FieldName="actual_date" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" />
                <possGrid:PossBoundField Width="4%" Caption="Time" FieldName="Time" FilterType="Text" />
                <possGrid:PossBoundField Width="15%" Caption="Description" FieldName="description" FilterType="Text" />
                <possGrid:PossTemplateField Width="12%" ItemStyle-HorizontalAlign="Left" Caption="Activity" FieldName="Activity" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlActivity" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ActivityView/Preview.aspx?idAct=" + Eval("activity_FK") + "&idTimeUnit=" +  Eval("time_unit_PK") %>' Text='<%# Eval("Activity") %>'></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="17%" Caption="Activity description" FieldName="ActivityDescription" FilterType="Text" />
                <possGrid:PossTemplateField Width="15%" Caption="Products" FieldName="Products" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID='pnlProducts' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="10%" Caption="Inserted by" FieldName="InsertedBy" FilterType="Text" />
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />

        </possGrid:PossGrid>
    </asp:Panel>
    <script type="text/javascript">
        $(document).ready(function () {

            //initGrid($(".possGrid"));
            //initGrouppingTable($(".possGrid"));

        });
    </script>

</asp:Content>
