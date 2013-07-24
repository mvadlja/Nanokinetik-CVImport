<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearcherATC.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucSearcherATC" %>
<%@ Register Src="~/Views/PartialShared/Operational/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="operational" %>

<div id="PopupControls_SearchEntity_Container" runat="server" class="modal">
    <div id="PopupControls_SearchEntity_ContainerContent" class="modal_container">
        <div class="modal_title_bar">
            <div id="divHeader">
                Search</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_Click"/>
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server" GroupingText="Search" Width="100%">
                <%--<br />--%>
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            
                            <asp:Panel ID="pnlATC" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>                              
                                        <td style="text-align: center;" width="100%">
                                            <asp:TextBox ID="txtATC" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td> 
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtATCName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
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
                            <asp:GridView ID="gvData" runat="server" DataKeyNames="ID" AllowSorting="True" Width="100%">
                                <Columns>
                                    <asp:TemplateField  HeaderText="ATC Code" SortExpression="ATCcode" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            
                                            <asp:LinkButton ID="lbtEdit" runat="server" CssClass="gvLinkTextBold" CommandName="Select"
                                                Text='<%# Eval("ATCcode") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        
                                        <ItemStyle Width="50%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <operational:GridViewPager ID="gvPager" runat="server" Width="100%" SortOrderBy="ATCcode" />
                            <!--<operational:GridViewPager ID="gvPagerATC" runat="server" Width="100%" SortOrderBy="ATCcode" />-->
                        </td>
                    </tr>
                </table>

            <div class="center">
            <asp:Button ID="btnSearch" runat="server" Text="Search" SkinID="blue" OnClick="btnSearch_Click" />
            <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnClose_Click" UseSubmitBehavior="false" />
            </div>
            </asp:Panel>
        </div>
    </div>
</div>