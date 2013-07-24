<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="ColumnsPopup.ascx.cs" Inherits="AspNetUI.Views.Shared.UserControl.Popup.ColumnsPopup" %>

<%@ Register Src="../ListBoxAu.ascx" TagName="ListBoxAu" TagPrefix="uc" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">
    <div id="PopupControls_Entity_ContainerContent" class="modal_container_RS" style="width: 796px">
        <div class="modal_title_bar">
            <div id="divHeader" runat="server">Columns</div>
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClose_OnClick" />
        </div>
        <div id="modalPopupContainerBody" runat="server" class="modal_content popup-columns" style="width: 796px">
            <span id="available_columns">Available columns</span>
            <span id="selected_columns">Selected columns</span>
            <uc:ListBoxAu ID="lbAuColumns" runat="server" SelectionMode="Multiple" VisibleRowsFrom="5" VisibleRowsTo="5" AllowDoubleClick="True" />
            <div class="center" style="margin-top: 12px; width: 738px;">
                <asp:Button ID="btnOk" runat="server" Text="OK" Width="75px" SkinID="blue" OnClick="btnOk_OnClick" UseSubmitBehavior="true" />
                <asp:Button ID="btnCancel" SkinID="blue" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_OnClick" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $("[lbAuAllowDblClick='true']").live("dblclick", function () {
            var lbInputDblClick = $(this).siblings().first();
            lbInputDblClick.val("doubleclicked");
            __doPostBack($(this).attr("id"), lbInputDblClick.attr('name'));
        });
    });
    </script>