<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="RoleForm_list.ascx.cs" Inherits="AspNetUI.Views.RoleForm_list" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="toolkit" %>
<!-- Operational controls -->
<%@ Register Src="~/Views/PartialShared/Operational/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="operational" %>
<%@ Register src="~/Views/PartialShared/Operational/SearcherDisplay.ascx" tagname="SearcherDisplay" tagprefix="operational" %>
<%@ Register Src="~/Views/PartialShared/Operational/GridViewItemsPerPage.ascx" TagName="ItemsPerPage" TagPrefix="operational" %>
<!-- Custom input controls -->
<%@ Register Src="~/Views/PartialShared/ControlTemplates/Label_CT.ascx" TagName="Label_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/TextBox_CT.ascx" TagName="TextBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/TextArea_CT.ascx" tagname="TextArea_CT" tagprefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/DateBox_CT.ascx" TagName="DateBox_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/MoneyBox_CT.ascx" tagname="MoneyBox_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/DropDownList_CT.ascx" tagname="DropDownList_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ListBox_CT.ascx" tagname="ListBox_CT" tagprefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/RadioButtonList_CT.ascx" TagName="RadioButtonList_CT" TagPrefix="customControls" %>
<%@ Register Src="~/Views/PartialShared/ControlTemplates/CheckBoxList_CT.ascx" TagName="CheckBoxList_CT" TagPrefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/ComboBox_CT.ascx" tagname="ComboBox_CT" tagprefix="customControls" %>
<%@ Register src="~/Views/PartialShared/ControlTemplates/AutoCompleteBox_CT.ascx" tagname="AutoCompleteBox_CT" tagprefix="customControls" %>


<!-- Optional searcher goes here -->
<asp:Panel ID="pnlDataSearcher" runat="server" GroupingText="Searcher" Visible="False">
    <br />

</asp:Panel>

<!-- Result list goes here -->
<asp:Panel ID="pnlDataList" runat="server">
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" DataKeyNames="IDRole" AllowSorting="False" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="ID" SortExpression="IDRole" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtEdit" runat="server" CssClass="gvLinkTextBold" CommandName="Select" Text='<%# Eval("IDRole") %>' Font-Bold="true"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="50px" />
            </asp:TemplateField>
            
			<asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" />
			<asp:BoundField DataField="display_name" HeaderText="Display name" SortExpression="display_name" HeaderStyle-HorizontalAlign="Left" />
			<asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" HeaderStyle-HorizontalAlign="Left" />
			<asp:CheckBoxField DataField="Active" HeaderText="Active" SortExpression="Active" ItemStyle-HorizontalAlign="Center" />

<%--            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="ibtDeleteItem" runat="server" CommandName="Delete" CommandArgument='<%# Eval("IDRole") %>'
                        ToolTip="Delete" OnClientClick='<%# "DisplayConfirmModalPopup(\"Warning!\", \"Are you sure that you want to delete this record?\", this, " + Container.DataItemIndex + "); return false;" %>'>
                        <asp:Image ID="imgDel" runat="server" ImageUrl="~/Images/delete.gif" /></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:TemplateField>--%>
        </Columns>
    </asp:GridView>
    <operational:GridViewPager ID="gvPager" runat="server" Width="100%" SortOrderBy="IDRole" />
</asp:Panel>