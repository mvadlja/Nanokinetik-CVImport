<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.UserRoleView.Form" EnableViewState="true" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/RadioButtonYn.ascx" TagName="RadioButtonYn" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxExt.ascx" TagName="ListBoxExt" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:TextBox ID="txtDisplayName" runat="server" Label="Display name:" Required="False" />
                <uc:TextBox ID="txtRole" runat="server" Label="Role:" Required="True" />
                <uc:TextBox ID="txtDescription" runat="server" Label="Description:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />

                <asp:Panel ID="pnlGroupActions" runat="server" GroupingText="Group actions" CssClass="user-role-grouping-actions">
                    <asp:CheckBox ID="cbInsertAll" runat="server" Text="Insert All" OnCheckedChanged="cbInsertAll_OnCheckedChanged" AutoPostBack="True"/>
                    <asp:CheckBox ID="cbEditAll" runat="server" Text="Edit All" OnCheckedChanged="cbEditAll_OnCheckedChanged" AutoPostBack="True"/>
                    <asp:CheckBox ID="cbSaveAsAll" runat="server" Text="Save As All" OnCheckedChanged="cbSaveAsAll_OnCheckedChanged" AutoPostBack="True"/>
                    <asp:CheckBox ID="cbDeleteAll" runat="server" Text="Delete All" OnCheckedChanged="cbDeleteAll_OnCheckedChanged" AutoPostBack="True"/>
                    <asp:CheckBox ID="cbViewAll" runat="server" Text="View All" OnCheckedChanged="cbViewAll_OnCheckedChanged" AutoPostBack="True"/>
                </asp:Panel>
                <div id="divPermissions" class="user-role-permission-row">
                        <uc:DropDownList ID="ddlLocation" runat="server" Label="Location:" OnSelectedIndexChanged="ddlLocation_OnSelectedIndexChanged" AutoPostback="True" />
                        <uc:DropDownList ID="ddlAction" runat="server" Label="Action:" />
                        <asp:LinkButton ID="lnkAddPermission" runat="server" Text="" CssClass="action-icon roles-action-icon" OnClick="lnkAddPermission_OnClick">
                            <asp:Image ID="imgAddPermission" runat="server" ImageUrl="~/Images/plus.png" />
                        </asp:LinkButton>
                </div>

                <div id="divSelectedPermissions" class="user-role-selected-permission-row">
                    <uc:ListBoxExt ID="lbExtSelectedPermissions" runat="server" Label="Selected permissions:" VisibleRows="40" SelectionMode="Multiple"/>
                    <asp:LinkButton ID="lnkRemovePermission" runat="server" Text="" CssClass="action-icon roles-action-icon" Width="24px" OnClick="lnkRemovePermission_OnClick">
                        <asp:Image ID="imgRemovePermission" runat="server" ImageUrl="~/Images/minus.png" />
                    </asp:LinkButton>
                </div>
                <uc:ListBoxAu ID="lbAuPerson" runat="server" Label="Person assignments:" SelectionMode="Multiple" VisibleRowsFrom="15" VisibleRowsTo="15" AllowDoubleClick="True" />
                <uc:RadioButtonYn ID="rbYnActive" runat="server" Label="Active" />
                <uc:TextBox ID="txtLastChange" runat="server" Label="Last change:" />
            </fieldset>
        </div>
        <div id="divRightPane" class="rightPane" style="margin-top: 0px; margin-left: -74px;">
            <fieldset>
            </fieldset>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlFooter" runat="server" class="bottom clear bottomControlsHolder" valign="center">
        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="button Cancel" OnClick="btnCancel_OnClick" />
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="button Save" OnClick="btnSave_OnClick" />
    </asp:Panel>

    <script type="text/javascript">
        $(document).ready(function () {
            $("[lbAuAllowDblClick='true']").live("dblclick", function () {
                var lbInputDblClick = $(this).siblings().first();
                lbInputDblClick.val("doubleclicked");
                window.__doPostBack($(this).attr("id"), lbInputDblClick.attr('name'));
            });
        });
    </script>
</asp:Content>
