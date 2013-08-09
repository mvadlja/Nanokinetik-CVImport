<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="true" CodeBehind="CVImport.aspx.cs" Inherits="AspNetUI.Views.CVImport.CVImport" %>


<%@ Register Src="../Shared/UserControl/FileUploadCtrl.ascx" TagName="FileUploadCtrl" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">

    <asp:Panel ID="pnlForm" runat="server" class="form">

        <asp:Panel ID="pnlImport" runat="server">

            <uc:DropDownList runat="server" ID="ddlCVType" Label="CV type"/>
            <uc:FileUploadCtrl runat="server" ID="fileUpload" Label="Browse for file to import" />

            

        </asp:Panel>

        <asp:Panel ID="pnlGrid" runat="server" Visible="false">

            <h1 runat="server" id="h1CVTitle" />

            <possGrid:PossGrid ID="grid" GridId="grid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18"
                AutoGenerateColumns="false" CellPadding="4" DataKeyNames="PrivateKey"
                AllowSorting="true"
                GridLines="None" GridHeight="250"
                Width="100%" MainSortingColumn="PrivateKey" DefaultSortingOrder="ASC"
                OnRowDataBound="grid_RowDataBound"
                >
                <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
                <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
                <Columns>
                    <possGrid:PossTemplateField Width="50px" FieldName="ID" Caption="Select">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" id="chkAction"
                                          AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged"
                                          ItemID='<%# Eval("PrivateKey") %>' EVCode='<%# Eval("EVCode") %>'
                                          EmaValue='<%# Eval("EmaValue") %>' Action='<%# Eval("Action") %>'/>
                        </ItemTemplate>
                    </possGrid:PossTemplateField>               
                    <possGrid:PossBoundField Width="25%" Caption="Current Value" FieldName="CurrentValue" FilterType="None" />
                    <possGrid:PossBoundField Width="25%" Caption="EMA Value" FieldName="EmaValue" FilterType="None" />
                    <possGrid:PossBoundField Width="10%" Caption="EV Code" FieldName="EVCode" FilterType="None" />
                    <possGrid:PossBoundField Width="10%" Caption="Action" FieldName="Action" FilterType="Combo" />
                
                </Columns>

                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
                <AlternatingRowStyle BackColor="White" />

            </possGrid:PossGrid>
        </asp:Panel>
        <asp:Panel ID="pnlSummary" runat="server" Visible="false">            
            <uc:LabelPreview runat="server" ID="lblInsert" Label="No. of records to insert:" LabelWidth="150px"/>
            <uc:LabelPreview runat="server" ID="lblUpdate" Label="No. of records to update:" LabelWidth="150px"/>
            <uc:LabelPreview runat="server" ID="lblDelete" Label="No. of records to delete:" LabelWidth="150px"/>
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlFooter" runat="server" class="bottom clear bottomControlsHolder" valign="center">    
        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="button Cancel" OnClick="btnCancel_OnClick" />    
        <asp:LinkButton ID="btnImport" runat="server" Text="Import" CssClass="button" OnClick="btnImport_OnClick" />  
        <asp:LinkButton ID="btnNext" runat="server" Text="Next" CssClass="button" OnClick="btnNext_Click" Visible="false" />
        <asp:LinkButton ID="btnApply" runat="server" Text="Apply" CssClass="button Save" OnClick="btnApply_Click" Visible="false" />
    </asp:Panel>   

</asp:Content>