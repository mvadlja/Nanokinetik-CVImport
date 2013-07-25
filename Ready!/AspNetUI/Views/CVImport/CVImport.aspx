<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="true" CodeBehind="CVImport.aspx.cs" Inherits="AspNetUI.Views.CVImport.CVImport" %>


<%@ Register Src="../Shared/UserControl/FileUploadCtrl.ascx" TagName="FileUploadCtrl" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">

    <asp:Panel ID="pnlForm" runat="server" class="form">

        <uc:DropDownList runat="server" ID="ddlCVType" Label="CV type" AutoPostback="true" OnSelectedIndexChanged="ddlCVType_SelectedIndexChanged"/>


        <uc:FileUploadCtrl runat="server" ID="fileUpload" Label="Browse for file to import" Enabled="false" />
         <asp:Panel ID="pnlGrid" runat="server" Visible="True">
            <uc:ColumnsPopup ID="ColumnsPopup" runat="server" />
            <div class="entity-name">
                <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Product:" Visible="False" />
            </div>
            <div class="subtabs" id="subtabs" runat="server">
                <uc:TabMenu ID="tabMenu" runat="server" />
            </div>
            <possGrid:PossGrid ID="APGrid" GridId="APGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18"
                AutoGenerateColumns="false" CellPadding="4" DataKeyNames="ap_PK"
                AllowSorting="true"
                GridLines="None" GridHeight="250"
                Width="100%" MainSortingColumn="AuthorisedProductName" DefaultSortingOrder="ASC">
                <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
                <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
                <Columns>
                    <possGrid:PossTemplateField>
                        
                    </possGrid:PossTemplateField>
                    <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="ap_PK" Visible="false" FilterType="Text" />
                   
                    <possGrid:PossBoundField Width="5%" Caption="Client" FieldName="Client" FilterType="Text" />
                    <possGrid:PossBoundField Width="5%" Caption="EVCODE" FieldName="evcode" FilterType="Text" />
                
                </Columns>

                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
                <AlternatingRowStyle BackColor="White" />

            </possGrid:PossGrid>
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlFooter" runat="server" class="bottom clear bottomControlsHolder" valign="center">    
        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="button Cancel" OnClick="btnCancel_OnClick" />    
        <asp:LinkButton ID="btnImport" runat="server" Text="Import" CssClass="button Save" OnClick="btnImport_OnClick" />
    </asp:Panel>   

</asp:Content>