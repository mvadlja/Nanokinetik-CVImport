<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="BrowseEDMSPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.BrowseEDMSPopup" %>

<%@ Register Namespace="PossGrid" Assembly="PossGrid" TagPrefix="possGrid" %>
<%@ Register Src="ColumnsPopup.ascx" TagName="ColumnsPopup" TagPrefix="uc" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal EDMS-modal-popup">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_RS" style="width: 1000px">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" class="EDMS-header-title">Link from EDMS</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" CssClass="EDMS-header-close" />
        </div>
        <div class="EDMS-popup-container">
            <div id="EDMS-left-pane" class="EDMS-pane">
                <div>Left pane</div>
            </div>
            <div id="EDMS-right-pane" class="EDMS-pane">
                <div style="position: relative;">
                    <div id="EDMS-right-pane-header">
                        <div id="EDMS-rph-rb-container">
                            <asp:RadioButtonList runat="server" ID="rbDocumentVersion" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">Selected version</asp:ListItem>
                                <asp:ListItem>Current</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RadioButtonList ID="rbDocumentType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem>PDF</asp:ListItem>
                                <asp:ListItem Selected="True">Original</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div id="EDMS-rph-buttons-container">
                            <asp:Button runat="server" Text="Link" ID="btnLinkDocument" UseSubmitBehavior="False" />
                            <asp:Button ID="btnColumns" runat="server" Text="Columns" CssClass="GridColumns" UseSubmitBehavior="False" />
                            <asp:Button ID="btnSaveLayout" runat="server" Text="Save" CssClass="SaveLay" UseSubmitBehavior="False" />
                            <asp:Button ID="btnClearLayout" runat="server" Text="Clear" CssClass="LoadLay" UseSubmitBehavior="False" />
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="ResetLay" UseSubmitBehavior="False" />
                            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Export" UseSubmitBehavior="False" />
                        </div>
                    </div>

                    <asp:Button runat="server" ID="btnBindGrid" CssClass="btnBindGrid display-none" OnClick="btnBindGrid_OnClick" Text="Bind grid" />
                    <asp:HiddenField runat="server" ID="hfSelectedFolderId" />
                    <asp:HiddenField runat="server" ID="hfSelectedDocumentId" />

                    <asp:Panel ID="pnlGrid" runat="server" Visible="True">
                        <uc:ColumnsPopup ID="ColumnsPopup" runat="server" />
                        <possGrid:PossGrid ID="EDMSGrid" GridId="EDMSGrid" GridVersion="1" runat="server" AllowPaging="True" PageSize="18" AutoGenerateColumns="false"
                            CellPadding="4" DataKeyNames="DocumentId" AllowSorting="true" GridLines="None" GridHeight="250" Width="100%" SecondSortingColumn="DocumentName" SecondSortingOrder="DESC" MainSortingColumn="DocumentName" MainSortingOrder="DESC">
                            <RowStyle BackColor="#EBEBEB" />
                            <SettingsPager AlwaysShowPager="False" DefaultPageSize="10000" Position="None" ItemsPerPagePosition="Bottom" ItemsPerPageMode="Custom" />
                            <Columns>
                                <possGrid:PossBoundField Width="3%" Caption="ID" FieldName="DocumentId" Visible="False" FilterType="Text" />
                                <possGrid:PossTemplateField Width="20%" Caption="Document name" FieldName="DocumentName" FilterType="Text">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSelectDocument" runat="server" CssClass="gvLinkTextBold" CommandName="Select" CommandArgument='<%# Eval("DocumentId") + "|" + Eval("VersionNumber") %>' Text='<%# HandleMissing(Eval("DocumentName")) %>' Font-Bold="true" OnClick="DocumentRow_OnClick"></asp:LinkButton>
                                    </ItemTemplate>
                                </possGrid:PossTemplateField>
                                <possGrid:PossBoundField Width="20%" Caption="Version label" FieldName="VersionLabel" FilterType="Text" />
                                <possGrid:PossBoundField Width="15%" Caption="Version number" FieldName="VersionNumber" FilterType="Text" />
                                <possGrid:PossBoundField Width="15%" Caption="Document format" FieldName="DocumentFormat" FilterType="Text" />
                                <possGrid:PossBoundField Width="14%" Caption="Content size" FieldName="ContentSize" FilterType="Text" />
                                <possGrid:PossBoundField Width="12%" Caption="Modify date" FieldName="ModifyDate" DataFormatString="{0:dd.MM.yyyy}" FilterType="Text" ItemStyle-HorizontalAlign="Right" />
                                 <possGrid:PossTemplateField Width="3%" Caption="" FieldName="Download" FilterType="None">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibDownloadEDMSAttachment" runat="server" CommandName="Download" CommandArgument='<%# HandleDocumentArguments(string.Empty, Eval("DocumentId"), Eval("VersionNumber"), Eval("DocumentFormat")) %>' ImageUrl="~/Images/preview-icon.png" />
                                    </ItemTemplate>
                                </possGrid:PossTemplateField>
                            </Columns>

                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />                          
                            <HeaderStyle BackColor="#CDDEF0" Font-Bold="True" ForeColor="Black" />
                            <AlternatingRowStyle BackColor="White" />
                        </possGrid:PossGrid>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript" src="../../Scripts/Development/Common/jquery.dynatree.js"></script>
<script type="text/javascript" src="../../Scripts/Development/Common/BrowseEDMS.js"></script>