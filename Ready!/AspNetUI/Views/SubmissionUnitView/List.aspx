<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.SubmissionUnitView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
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
                <uc:TextBoxSr ID="txtSrProduct" runat="server" Label="Product:" SearchType="Product" />
                <uc:TextBoxSr ID="txtSrActivity" runat="server" Label="Activity:" SearchType="Activity" />
                <uc:TextBoxSr ID="txtSrTask" runat="server" Label="Task:" SearchType="Task" />
                <uc:DropDownList ID="ddlSubmissionUnitDescription" runat="server" Label="Submission description:" />
                <uc:DropDownList ID="ddlAgency" runat="server" Label="Agency:" />
                <uc:DropDownList ID="ddlRms" runat="server" Label="RMS:" />
                <uc:TextBox ID="txtSubmissionId" runat="server" Label="Submission ID:" MaxLength="500" />
                <uc:DropDownList ID="ddlSubmissionFormat" runat="server" Label="Submission format:" />
                <uc:TextBox ID="txtSequence" runat="server" Label="Sequence:" MaxLength="500" />
                <uc:DropDownList ID="ddlDtdSchemaVersion" runat="server" Label="DTD/Schema version:" />
                <uc:DateTimeRangeBox ID="dtRngDispatchDate" runat="server" Label="Dispatch date:" />
                <uc:DateTimeRangeBox ID="dtRngReceiptDate" runat="server" Label="Receipt date:" />

                <asp:Panel ID="pnlSearchButtons" runat="server" CssClass="pnl-search-buttons">
                    <br />
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchClick"  CssClass="button"/>&nbsp;&nbsp;
                    <asp:LinkButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClearClick"  CssClass="button"/>
                    <asp:LinkButton ID="btnSaveSearch" runat="server" Text="Save Quick link" OnClick="btnSaveSearchClick" Visible="true" CssClass="CreateQuickLink button" />
                    <asp:LinkButton ID="btnDeleteSearch" runat="server" Text="Delete Quick link" OnClick="btnDeleteSearchClick" Visible="false" CssClass="DeleteQuickLink button" />
                    <div style="position: absolute; right: 10px">
                        <asp:LinkButton ID="btnExportLower" runat="server" Text="Export" Style="margin-top: -20px" Visible="False"  CssClass="button"/>
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
        <possGrid:PossGrid ID="SubmissionUnitGrid" GridId="SubmissionUnitGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="submission_unit_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="SubmissionUnitDescription" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="subbmission_unit_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="15%" Caption="Submission description" FieldName="SubmissionUnitDescription" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlID" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/SubmissionUnitView/Preview.aspx?EntityContext=SubmissionUnit&idSubUnit=" + Eval("subbmission_unit_PK") %>'
                            Text='<%# HandleMissing(Eval("SubmissionUnitDescription")) %>' Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="11%" Caption="Attachment" FieldName="Attachments" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID="pnlAttachments" runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="16%" Caption="Products" FieldName="Products" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID="pnlProducts" runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="16%" Caption="Activity" FieldName="ActivityName" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlViewActivity" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ActivityView/Preview.aspx?EntityContext=SubmissionUnit&idAct=" + Eval("activityPk") + "&idSubUnit=" + Eval("subbmission_unit_PK") %>' Text='<%# Eval("ActivityName") %>'></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="10%" Caption="Task" FieldName="TaskName" FilterType="Text">
                    <ItemTemplate>
                         <asp:HyperLink ID="hlViewTask" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/TaskView/Preview.aspx?EntityContext=SubmissionUnit&idTask=" + Eval("taskPk")+"&idSubUnit="+Eval("subbmission_unit_PK") %>' Text='<%# Eval("TaskName") %>'></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="10%" Caption="Submission ID" FieldName="SubmissionId" FilterType="Text" />
                <possGrid:PossBoundField Width="10%" Caption="Format" FieldName="SubmissionFormat" FilterType="Combo" />
                <possGrid:PossTemplateField Width="6%" Caption="Sequence" FieldName="Sequence" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID="pnlSequence" runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="7%" Caption="Dispatch date" FieldName="DispatchDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                <possGrid:PossBoundField Width="9%" Caption="Agency" FieldName="Agency" FilterType="Text" />
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />

        </possGrid:PossGrid>
    </asp:Panel>
</asp:Content>
