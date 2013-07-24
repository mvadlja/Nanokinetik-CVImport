<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucSearcherSUBST.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucSearcherSUBST" %>
<%@ Register Src="~/Views/PartialShared/Operational/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="operational" %>

<div id="PopupControls_SearchEntity_Container" runat="server" class="modal">
    <div id="PopupControls_SearchEntity_ContainerContent" class="modal_container">
        <div class="modal_title_bar">
            <div id="divHeader">
                Search</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_Click" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server" GroupingText="Search"
                Width="100%">
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlSubName" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtSubName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtSubEVCODE" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="gvData" runat="server" DataKeyNames="ID" AllowSorting="True" Width="100%"
                                OnRowDataBound="gvData_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtEdit" runat="server" CssClass="gvLinkTextBold" CommandName="Select"
                                                Text='<%# Eval("Name") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <operational:GridViewPager ID="gvPager" runat="server" Width="100%" SortOrderBy="Name" />
                        </td>
                    </tr>
                </table>
                <div class="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" SkinID="blue" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnOk" runat="server" Text="Cancel" OnClick="btnClose_Click" UseSubmitBehavior="false" />
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
