<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearcher.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucSearcher" %>

<%@ Register Src="~/Views/PartialShared/Operational/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="operational" %>

<div id="PopupControls_SearchEntity_Container" runat="server" class="modal">
    <div id="PopupControls_SearchEntity_ContainerContent" class="modal_container">
        <div class="modal_title_bar">
            <div id="divHeader">
                Search</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_Click"/>
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server" GroupingText="Search" Width="100%" >
                <%--<br />--%>
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlProductsSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <%--<tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                                        </td>
                                        <td style="text-align: center;" width="50%">
                                            <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                        </td>
                                        
                                        </tr>--%>
                                        <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtCountries" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlMAHSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="mahTxtName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlSubName" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtSubName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align:center;" width="50%">
                                            <asp:TextBox ID="txtSubEVCODE" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlAdminRoute" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <%--<asp:Label ID="lblAdminRouteCode" runat="server" Text="Code"></asp:Label>--%>
                                            <asp:TextBox ID="txtAdminRouteCode" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlMedDevice" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <%--<asp:Label ID="lblMedDeviceCode" runat="server" Text="Code"></asp:Label>--%>
                                            <asp:TextBox ID="txtMedDeviceCode" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlManufacturer" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="txtManufacturer" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlApplicant" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="txtApplicant" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlClient" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="txtClient" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlIndication" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="txtIndication" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
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
                            <asp:Panel ID="pnlPharmaProduct" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="txtPharmaProduct" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="txtConcise" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                               
                            </asp:Panel>
                            <asp:Panel ID="pnlPPIAttachmentSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="ppiTxtName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="ppiTxtDescription" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlDistributorSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="distributorTxtName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="distributorTxtDescription" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlMasterFileSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="masterFileTxtName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlQPPVSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="qppvTxtName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                            <asp:TextBox ID="qppvTxtCode" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlProjectSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtProjectName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtProjectInernalStatus" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>

                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlActivitiesSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtActivityName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align:center;" width="50%">
                                            <asp:TextBox ID="txtActivityApplicant" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlTasksSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtTaskName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align:center;" width="50%">
                                            <asp:TextBox ID="txtTaskActivity" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                              <asp:Panel ID="pnlActIngSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:TextBox ID="txtActiveIngrName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlPersonSearch" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="text-align: center;" width="50%">
                                            <asp:TextBox ID="txtPersonName" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                        <td style="text-align:center;" width="50%">
                                            <asp:TextBox ID="txtEmail" runat="server" SkinID="T3" Width="325px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <%--<tr>
                        <td id="Td2" style="text-align: center" runat="server">
                            <table cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" SkinID="blue" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" SkinID="blue" OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                </table>
            <%--</asp:Panel>
            <br />--%>
            <%--<asp:Panel ID="pnlResults" runat="server" GroupingText="Search results" Width="100%">
                <br />--%>
                <table cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="gvData" runat="server" DataKeyNames="ID" AllowSorting="True" Width="100%">
                                <Columns>
                                    <asp:TemplateField  HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left">
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
                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnClose_Click" UseSubmitBehavior="false" />
            </div>
            </asp:Panel>
        </div>
    </div>
</div>