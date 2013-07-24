<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListBoxAER_CT.ascx.cs" Inherits="AspNetUI.Support.ListBoxAER_CT" %>
<table id="tableCanvas" clientIdAttr="AER_CT_table" runat="server" cellpadding="2" cellspacing="0">
    <tr>
        <td id="tdLabel" runat="server" valign="top">
            <div style="text-align: right; margin-top:2px;">
                <asp:Label ID="ctlLabel" runat="server" Text="Label" />
            </div>
        </td>
        <td style="width: 8px" valign="top"><div style="margin-top:2px;">
            <asp:Label ID="ctlMark" runat="server" Text="*" Style="color: Red;" Visible="false" /></div>
        </td>
        <td id="tdControl" runat="server">
            <asp:ListBox ID="ctlInput" clientIdAttr="AER_CT_ctlInput" runat="server" AutoPostBack="false" onchange="listBoxAER_ctlInputChanged(this);" OnSelectedIndexChanged="ctlInput_SelectedIndexChanged" style="background-color:#F4F4F4;"/>
        </td>
        <td>
            &nbsp
        </td>
        <td id="tdAERControls" runat="server" style="width: 75px;">
            <%--<div id="AERButtons" runat="server">
                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="75px" OnClick="btnAddOnClick" />
                <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="75px" OnClick="btnEditOnClick" />
                <asp:Button ID="btnRemove" runat="server" Text="Remove" Width="75px" OnClick="btnRemoveOnClick" />
            </div>--%>
            <div id="AERLinkButtons" runat="server" style="text-align: center; margin-left: -18px;">
                <asp:LinkButton ID="lbtnAdd" runat="server" Text="Add" Width="75px" CssClass="boldText" OnClick="btnAddOnClick" />
                <asp:LinkButton ID="lbtnEdit" clientIdAttr="AER_CT_btnEdit" runat="server" Text="Edit" Width="75px" CssClass="boldText" OnClick="btnEditOnClick" />
                <asp:LinkButton ID="lbtnRemove" clientIdAttr="AER_CT_btnRemove" runat="server" Text="Remove" Width="75px" CssClass="boldText" OnClick="btnRemoveOnClick" />
            </div>
        </td>
        <%--<td id="tdInputUnitsLabel" runat="server" visible="false">
            <asp:Label ID="ctlInputUnitsLabel" runat="server" />
        </td>--%>
        <td style="width: 24px">
            <asp:Image ID="ctlDescription" runat="server" Width="16" ImageUrl="~/Images/info.png" Style="padding: 2px;" Visible="false" />
        </td>
        <%--<td style="width: 24px">
            <asp:Image ID="ctlAlert" runat="server" Width="20" ImageUrl="~/Images/alert.png" Style="padding: 2px;" Visible="false" />
        </td>--%>
    </tr>
</table>
<div ID="divScript" runat="server">
    <script type="text/javascript">
//        var prm = Sys.WebForms.PageRequestManager.getInstance();
//        prm.add_endRequest(function (s, e) {
//            listBoxAER_initializeOnPostback(s, e);
//        });

     

        function listBoxAER_ctlInputChanged(control) {
            var selectedCnt = 0;
            $(control).children("option:selected").each(function () {
                selectedCnt++;
            });

            var removeButton = $(control).closest("table[clientIdAttr=AER_CT_table]").first().find("a[clientIdAttr=AER_CT_btnRemove]").first();
            if (selectedCnt < 1) {
                removeButton.addClass("aspNetDisabled");
                removeButton.css("color", "#000");
                removeButton.css("cursor", "text");
            } else {
                removeButton.removeClass("aspNetDisabled");
                removeButton.css("color", "#333");
                removeButton.css("cursor", "pointer");
            }

            var editButton = $(control).closest("table[clientIdAttr=AER_CT_table]").first().find("a[clientIdAttr=AER_CT_btnEdit]").first();
            if (selectedCnt != 1) {
                editButton.addClass("aspNetDisabled");
                editButton.css("color", "#000");
                editButton.css("cursor", "text");
            } else {
                editButton.removeClass("aspNetDisabled");
                editButton.css("color", "#333");
                editButton.css("cursor", "pointer");
            }
        }



        function initializeAERControl(control) {

            $(control).find("a[clientIdAttr=AER_CT_btnRemove]").click(function (e) {
                if ($(e.target).hasClass("aspNetDisabled")) e.preventDefault();
            });

            $(control).find("a[clientIdAttr=AER_CT_btnEdit]").click(function (e) {
                if ($(e.target).hasClass("aspNetDisabled")) e.preventDefault();
            });

            listBoxAER_ctlInputChanged($(control).find("select[clientIdAttr=AER_CT_ctlInput]").get(0));
        }


        function listBoxAER_initializeOnPostback(sender, args) {

            $("table[clientIdAttr=AER_CT_table]").each(
            function (key, value) {
                initializeAERControl(value);
            }
        );


        }
    </script>
</div>