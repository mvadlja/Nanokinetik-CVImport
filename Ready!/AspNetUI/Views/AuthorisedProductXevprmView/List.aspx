<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="AspNetUI.Views.AuthorisedProductXevprmView.List" %>

<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/XevprmValidationErrorPopup.ascx" TagName="XevprmValidationErrorPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/ColumnsPopup.ascx" TagName="ColumnsPopup" TagPrefix="uc" %>
<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <uc:XevprmValidationErrorPopup ID="XevprmValidationErrorPopup" runat="server" />
    <div style="float: left; margin-top: -32px; margin-left: 100px;">
        <asp:LinkButton ID="btnXevprmInsert" runat="server" Text="Insert" CssClass="button" OnClick="btnXevprmInsert_OnClick" Visible="True" />
        <asp:LinkButton ID="btnXevprmUpdate" runat="server" Text="Update" CssClass="button" OnClick="btnXevprmUpdate_OnClick" Visible="True" />
        <asp:LinkButton ID="btnXevprmVariation" runat="server" Text="Variation" CssClass="button" OnClick="btnXevprmVariation_OnClick" Visible="True" />
        <asp:LinkButton ID="btnXevprmNullify" runat="server" Text="Nullify" CssClass="button" OnClick="btnXevprmNullify_OnClick" Visible="True" />
        <asp:LinkButton ID="btnXevprmWithdraw" runat="server" Text="Withdraw" CssClass="button" OnClick="btnXevprmWithdraw_OnClick" Visible="True" />
    </div>
    <div class="buttonsHolder">
        <div class="rightSection">
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button" Visible="True" />
            <asp:Button ID="btnColumns" runat="server" Text="Columns" CssClass="button" Visible="True" />
            <asp:Button ID="btnSaveLayout" runat="server" Text="Save" CssClass="SaveLay button" Visible="True" />
            <asp:Button ID="btnClearLayout" runat="server" Text="Clear" CssClass="LoadLay button" Visible="True" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="ResetLay button" Visible="True" />
            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Export button" Style="margin-left: 25px;" />
        </div>
    </div>

    <asp:Panel runat="server" ID="pnlSearch" Visible="True">
        <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlSearchButtons" Style="margin-left: 166px;" runat="server">
                </asp:Panel>
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
        <possGrid:PossGrid ID="AuthorisedProductXevprmGrid" GridId="AuthorisedProductXevprmGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
            CellPadding="4" DataKeyNames="xevprm_message_PK" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" MainSortingColumn="xevprm_message_PK" DefaultSortingOrder="DESC">
            <RowStyle BackColor="#EBEBEB" ForeColor="Black" />
            <SettingsPager AlwaysShowPager="true" DefaultPageSize="15" Position="TopAndBottom" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
            <Columns>
                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="xevprm_message_PK" Visible="False" FilterType="Text" />
                <possGrid:PossTemplateField Width="4%" Caption="No." FieldName="message_number" FilterType="Text">
                    <ItemTemplate>
                        <div class="status">
                            <span runat="server" id="pnlStatusColor"></span>
                            <div style="margin-left: -5px;"><%# Eval("message_number")%></div>
                        </div>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="11%" Caption="Authorised product" FieldName="AuthorisedProduct" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlAuthorisedProduct" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/AuthorisedProductView/Preview.aspx?EntityContext=AuthorisedProduct&From=XevprmListForm&idAuthProd=" +  Eval("ap_FK") %>' Text='<%# Eval("AuthorisedProduct") %>'></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="6%" Caption="Package description" FieldName="PackageDescription" FilterType="Text" />
                <possGrid:PossBoundField Width="4%" Caption="Country" FieldName="Country" FilterType="Text" />
                <possGrid:PossTemplateField Width="9%" Caption="Product" FieldName="Product" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlProduct" runat="server" CssClass="gvLinkTextBold" NavigateUrl='<%# "~/Views/ProductView/Preview.aspx?EntityContext=Product&From=XevprmListForm&idProd=" +  Eval("product_FK") %>' Text='<%# Eval("Product") %>'></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="6%" Caption="License holder" FieldName="LicenseHolder" FilterType="Text" />
                <possGrid:PossBoundField Width="6%" Caption="Authorisation status" FieldName="AuthorisationStatus" FilterType="Text" />
                <possGrid:PossBoundField Width="5%" Caption="EVCODE" FieldName="EVCode" FilterType="Text" />
                <possGrid:PossTemplateField Width="7%" Caption="xEVPRM Status" FieldName="XevprmStatus" FilterType="Combo">
                    <ItemTemplate>
                        <asp:Label ID='lblStatus' runat="server" Font-Bold="true" Text='<%# Eval("XevprmStatus") %>' />
                        <asp:LinkButton ID="btnStatus" runat="server" Font-Bold="true" ForeColor="Red" CommandArgument='<%# Eval("xevprm_message_PK") %>' Text='<%# Eval("XevprmStatus") %>' Visible="False" OnClick="btnStatus_OnClick"></asp:LinkButton>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="5%" Caption="Operation" FieldName="Operation" FilterType="Combo">
                    <ItemTemplate>
                        <asp:Label ID='lblOperation' Font-Bold="true" Text='<%# Eval("Operation") %>' runat="server" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="7%" Caption="Action" FieldName="Action" FilterType="Text">
                    <ItemTemplate>
                        <div style="text-align: center;">
                            <asp:LinkButton ID='btnAction' Visible="False" CommandArgument='<%# Eval("xevprm_message_PK") %>' CommandName='<%# Eval("message_status_FK") %>'
                                CssClass="button xevprmActionBtn" runat="server" OnClick="btnAction_OnClick"></asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossTemplateField Width="3%" Caption="MSG" FieldName="XevprmXml" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlXML" runat="server" Visible="false" Text="XML"></asp:HyperLink><br />
                        <asp:HyperLink ID="hlPDF" runat="server" Visible="false" Text="PDF"></asp:HyperLink><br />
                        <asp:HyperLink ID="hlRTF" runat="server" Visible="false" Text="RTF"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="3%" Caption="MSGExportPDF" FieldName="XevprmXmlPdf" FilterType="Text" Visible="False" />
                <possGrid:PossBoundField Width="3%" Caption="MSGExportRTF" FieldName="XevprmXmlRtf" FilterType="Text" Visible="False" />
                <possGrid:PossTemplateField Width="5%" Caption="Submission status" FieldName="GatewaySubmissionStatus" FilterType="Combo">
                    <ItemTemplate>
                        <asp:Label ID='lblGatewaySubmissionStatus' Font-Bold="true" Visible="True" runat="server" Text='<%# Eval("GatewaySubmissionStatus") %>' />
                        <asp:LinkButton ID="btnGatewaySubmissionError" runat="server" Font-Bold="true" CommandArgument='<%# Eval("xevprm_message_PK") %>'
                            Text='<%# Eval("GatewaySubmissionStatus") %>' Visible="False" OnClick="btnStatusError_OnClick"></asp:LinkButton>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="6%" Caption="Submission date" FieldName="gateway_submission_date" DataFormatString="{0:dd.MM.yy HH:mm}" FilterType="Text" />
                <possGrid:PossTemplateField Width="5%" Caption="ACK" FieldName="AckXml" FilterType="Text">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlACK" runat="server" Visible="false"></asp:HyperLink><br />
                        <asp:HyperLink ID="hlACK_PDF" runat="server" Visible="false"></asp:HyperLink><br />
                        <asp:HyperLink ID="hlACK_RTF" runat="server" Visible="false"></asp:HyperLink>
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="3%" Caption="ACKExportPDF" FieldName="AckXmlPdf" FilterType="Text" Visible="False" />
                <possGrid:PossBoundField Width="3%" Caption="ACKExportRTF" FieldName="AckXmlRtf" FilterType="Text" Visible="False" />
                <possGrid:PossBoundField Width="5%" Caption="ACK date" FieldName="gateway_ack_date" DataFormatString="{0:dd.MM.yy HH:mm}" FilterType="Text" />
                <possGrid:PossBoundField Width="4%" Caption="SenderID" FieldName="SenderID" FilterType="Text" />
                <possGrid:PossTemplateField Width="2%" Caption="" FieldName="Delete" FilterType="None">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" CommandName="Delete" CommandArgument='<%# Eval("xevprm_message_PK") %>' ImageUrl="~/Images/delete.gif" OnClick="btnDeleteEntity_OnClick" />
                    </ItemTemplate>
                </possGrid:PossTemplateField>
                <possGrid:PossBoundField Width="5%" Caption="Submitted by" FieldName="SubmittedBy" FilterType="None" Visible="False" />
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />

            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
            <AlternatingRowStyle BackColor="White" />

        </possGrid:PossGrid>
    </asp:Panel>
</asp:Content>
