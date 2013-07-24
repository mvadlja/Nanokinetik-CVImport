<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.DocumentViewAll.List" %>

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
    <div ID="contextMenu_ContextMenuLayout" class="controlButtons" style="margin-top: 0px; margin-bottom: 0px;" runat="server">
    </div>
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
                <uc:TextBoxSr ID="txtSrAuthorisedProduct" runat="server" Label="Authorised product:" SearchType="AuthorisedProduct" />
                <uc:TextBoxSr ID="txtSrPharmaceuticalProduct" runat="server" Label="Pharmaceutical product:" SearchType="PharmaceuticalProduct" />
                <uc:TextBoxSr ID="txtSrProject" runat="server" Label="Project:" SearchType="Project" />
                <uc:TextBoxSr ID="txtSrActivity" runat="server" Label="Activity:" SearchType="Activity" />
                <uc:TextBoxSr ID="txtSrTask" runat="server" Label="Task:" SearchType="Task" />
                <uc:TextBox ID="txtEvCode" runat="server" Label="EVCODE:" MaxLength="500" />
                <uc:TextBox ID="txtDocumentName" runat="server" Label="Document name:" MaxLength="500" />
                <uc:TextBox ID="txtTextSearch" runat="server" Label="Text search:" MaxLength="500" />
                <uc:DropDownList ID="ddlDocumentType" runat="server" Label="Document type:" />
                <uc:TextBox ID="txtVersionNumber" runat="server" Label="Version number:" MaxLength="500" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:DropDownList ID="ddlVersionLabel" runat="server" Label="Version label:" />
                <uc:TextBox ID="txtDocumentNumber" runat="server" Label="Document number:" MaxLength="500" />
                <uc:DropDownList ID="ddlRegulatoryStatus" runat="server" Label="Regulatory status:" />
                <uc:TextBox ID="txtLanguageCode" runat="server" Label="Language code:" MaxLength="500" />
                <uc:TextBox ID="txtComments" runat="server" Label="Comments:" MaxLength="500" />
                <uc:DateTimeRangeBox ID="dtRngChangeDate" runat="server" Label="Change date:" />
                <uc:DateTimeRangeBox ID="dtRngEffectiveStartDate" runat="server" Label="Effective start date:" />
                <uc:DateTimeRangeBox ID="dtRngEffectiveEndDate" runat="server" Label="Effective end date:" />
                <uc:DateTimeRangeBox ID="dtRngVersionDate" runat="server" Label="Version date:" />

                <asp:Panel ID="pnlSearchButtons" runat="server" CssClass="pnl-search-buttons">
                    <br />
                    <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchClick"  CssClass="button"/>&nbsp;&nbsp;
                    <asp:LinkButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClearClick"  CssClass="button"/>
                    <asp:LinkButton ID="btnSaveSearch" runat="server" Text="Save Quick link" OnClick="btnSaveSearchClick" Visible="true" CssClass="CreateQuickLink button" />
                    <asp:LinkButton ID="btnDeleteSearch" runat="server" Text="Delete Quick link" OnClick="btnDeleteSearchClick" Visible="false" CssClass="DeleteQuickLink button"/>
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
        <possGrid:PossGrid ID="DocumentGridAll" GridId="DocumentGridAll" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false" CellPadding="4"
            DataKeyNames="document_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="DocumentName" DefaultSortingOrder="ASC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="document_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="15%" Caption="Document name" FieldName="DocumentName" FilterType="Text">
                    <ItemTemplate>
                        <div class="status">
                            <span runat="server" id="pnlStatusColor"></span>
                            <asp:HyperLink ID="hlId" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/DocumentView/Preview.aspx?EntityContext=Document&idDoc="+ Eval("document_PK") %>' Text='<%# HandleMissing(Eval("DocumentName")) %>'
                                Font-Bold="true"></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="10%" Caption="Attachments" FieldName="Attachments" FilterType="Text">
                    <ItemTemplate>
                        <asp:Panel ID="pnlAttachments" runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="10%" Caption="Document type" FieldName="DocumentType" FilterType="Text" />
                <possGrid:PossTemplateField Width="22%" Caption="Related To" FieldName="RelatedEntities" FilterType="Text" Visible="true">
                    <ItemTemplate>
                        <asp:Panel ID="pnlRelatedEntities" runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="8%" Caption="Version number" FieldName="VersionNumber" FilterType="Text" />
                <possGrid:PossBoundField Width="6%" Caption="Version label" FieldName="VersionLabel" FilterType="Text" />
                <possGrid:PossBoundField Width="8%" Caption="Regulatory status" FieldName="RegulatoryStatus" FilterType="Text" />
                <possGrid:PossBoundField Width="7%" Caption="Document number" FieldName="DocumentNumber" FilterType="Text" />
                <possGrid:PossBoundField Width="7%" Caption="Language code" FieldName="LanguageCode" FilterType="Text" />
                <possGrid:PossBoundField Width="7%" Caption="Change date" FieldName="ChangeDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                <possGrid:PossBoundField Width="7%" Caption="Effective start date" FieldName="EffectiveStartDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                <possGrid:PossBoundField Width="7%" Caption="Effective end date" FieldName="EffectiveEndDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                <possGrid:PossBoundField Width="8%" Caption="Responsible user" FieldName="ResponsibleUser" FilterType="Text" />
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />

        </possGrid:PossGrid>
    </asp:Panel>
</asp:Content>
