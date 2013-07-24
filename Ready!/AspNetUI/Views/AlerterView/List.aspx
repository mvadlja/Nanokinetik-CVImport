<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.AlerterView.List" %>

<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TextBoxSr.ascx" TagName="TextBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/QuickLinksPopup.ascx" TagName="QuickLinksPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/ColumnsPopup.ascx" TagName="ColumnsPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/AlerterUserStatusPopup.ascx" TagName="AlerterUserStatusPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/DateRangeFilterPopup.ascx" TagName="DateRangeFilterPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <div id="contextMenu_ContextMenuLayout" class="controlButtons" style="margin-top: 0px; margin-bottom: 0px;" runat="server">
    </div>
    <div class="buttonsHolder">
        <div class="rightSection">
            <div class="rightSection">
                <asp:Button ID="btnToggleAlert" runat="server" Text="Show all" CssClass="button" Visible="False" />
                <asp:Button ID="btnColumns" runat="server" Text="Columns" CssClass="button" Visible="True" />
                <asp:Button ID="btnSaveLayout" runat="server" Text="Save" CssClass=" SaveLay button" Visible="True" />
                <asp:Button ID="btnClearLayout" runat="server" Text="Clear" CssClass="LoadLay button" Visible="True" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="ResetLay button" Visible="True" />
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Export button" Style="margin-left: 25px;" />
            </div>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlSearch" Visible="True">
        <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <uc:QuickLinksPopup ID="QuickLinksPopup" runat="server" />
                <uc:TextBoxSr ID="txtSrProduct" runat="server" Label="Product:" SearchType="Product" />
                <uc:TextBoxSr ID="txtSrAuthorisedProduct" runat="server" Label="Authorised product:" SearchType="AuthorisedProduct" />
                <uc:TextBoxSr ID="txtSrProject" runat="server" Label="Project:" SearchType="Project" />
                <uc:TextBoxSr ID="txtSrActivity" runat="server" Label="Activity:" SearchType="Activity" />
                <uc:TextBoxSr ID="txtSrTask" runat="server" Label="Task:" SearchType="Task" />
                <uc:TextBoxSr ID="txtSrDocument" runat="server" Label="Document:" SearchType="Document" />
                <uc:DropDownList ID="ddlReminderRepeatMode" runat="server" Label="Repeat:" />
                <uc:RadioButtonYn runat="server" ID="rbYnSendEmail" Label="Send email:" />
                
                <asp:Panel ID="pnlSearchButtons" runat="server" Style="margin-left: 166px;" CssClass="pnl-search-buttons">
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
        <uc:DateRangeFilterPopup ID="dateRangeFilterPopup" runat="server"/>
        <uc:AlerterUserStatusPopup ID="AlerterUserStatusPopup" runat="server" />
        <uc:ColumnsPopup ID="ColumnsPopup" runat="server" />
        <uc:ModalPopup ID="mpDelete" runat="server" />
        <div class="entity-name">
            <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Task:" Visible="False" />
        </div>
        <div class="subtabs" id="subtabs" runat="server">
            <uc:TabMenu ID="tabMenu" runat="server" />
        </div>
        <possGrid:PossGrid ID="AlerterGrid" GridId="AlerterGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="reminder_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="OverDue" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="5%" Caption="ID" FieldName="reminder_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="5%" Caption="Status" FieldName="ReminderUserStatus" FilterType="Text">
                    <ItemTemplate>
                        <div class="status">
                            <span runat="server" id="pnlStatusColor"></span>
                            <asp:LinkButton ID="btnStatus" runat="server" CommandArgument='<%# Eval("reminder_PK") %>' Text='<%# HandleMissing(Eval("ReminderUserStatus")) %>' OnClick="BtnStatusOnClick"></asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Caption="Over due" FieldName="OverDue" Width="5%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Alert date(s)" FieldName="reminder_dates" Width="7%" DataFormatString="{0:dd.MM.yyyy}" ItemStyle-HorizontalAlign="Right" FilterType="Text" />
                <possGrid:PossBoundField Caption="Days before" FieldName="time_before_activation" Width="6%" ItemStyle-HorizontalAlign="Right" FilterType="Text" />
                <possGrid:PossBoundField Caption="Related date" FieldName="related_attribute_value" Width="7%" DataFormatString="{0:dd.MM.yyyy}" ItemStyle-HorizontalAlign="Right" FilterType="DateRange" />
                <possGrid:PossBoundField Caption="Related date name" FieldName="related_attribute_name" Width="8%" FilterType="Text" />
                <possGrid:PossTemplateField Width="9%" Caption="Related Products" FieldName="RelatedProducts" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID="pnlProducts" runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Caption="Type" FieldName="reminder_type" Width="10%" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlType" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# Eval("navigate_url") %>' Text='<%# Eval("reminder_type") %>' Target="_blank" Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Caption="Name" FieldName="reminder_name" Width="10%" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlName" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# Eval("navigate_url") %>' Text='<%# Eval("reminder_name") %>' Target="_blank" Font-Bold="true"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Caption="Description" FieldName="description" Width="13%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Email addresses" FieldName="EmailAdresses" Width="12%" FilterType="Text" />
                <possGrid:PossBoundField Caption="Responsible user" FieldName="ResponsibleUser" Width="8%" FilterType="Text" />

                <possGrid:PossTemplateField Width="4%" Caption="Action" FieldName="Action" FilterType="None" CellStyleCssClass="not-sortable">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" CommandName="Edit" CommandArgument='<%# Eval("reminder_PK") %>' ImageUrl="~/Images/edit.png" OnClick="btnEditEntity_OnClick" />
                        <asp:ImageButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("reminder_PK") %>' ImageUrl="~/Images/delete.gif" OnClick="btnDeleteEntity_OnClick" />
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
