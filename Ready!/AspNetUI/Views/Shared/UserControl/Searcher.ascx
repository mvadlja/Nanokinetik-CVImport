<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Searcher.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Searcher" %>
<%@ Register Src="~/Views/PartialShared/Operational/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="operational" %>

<div id="PopupControls_SearchEntity_Container" runat="server" class="modal">
    <div id="PopupControls_SearchEntity_ContainerContent" class="modal_container">
        <div class="modal_title_bar">
            <div id="divHeader">
                Search
            </div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content">
            <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server" GroupingText="Search" Width="100%">
                <div style="text-align: left;">
                    <asp:TextBox ID="txtName" runat="server" Width="330px"></asp:TextBox>
                    <asp:TextBox ID="txtDescription" runat="server" Width="330px"></asp:TextBox>
                </div>
                <asp:GridView ID="gvData" runat="server" DataKeyNames="ID" AllowSorting="True" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtEdit" runat="server" CssClass="gvLinkTextBold" CommandName="Select" Text='<%# HandleMissing(Eval("Name")) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <operational:GridViewPager ID="gvPager" runat="server" Width="100%" SortOrderBy="Name" RecordsPerPage="20" SortReverseOrder="False" Visible="False" CurrentPage="1" TotalRecordsCount="0" />

                <div class="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" SkinID="blue" OnClick="btnSearch_OnClick" />
                    <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_OnClick" Visible="false" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" UseSubmitBehavior="false" />
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
