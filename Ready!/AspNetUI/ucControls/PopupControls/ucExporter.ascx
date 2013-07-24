<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucExporter.ascx.cs" Inherits="AspNetUI.ucControls.PopupControls.ucExporter" %>

<div id="PopupControls_Entity_Container" runat="server" class="modal">

    <div id="PopupControls_Entity_ContainerContent" class="modal_container_small">
        
        <div class="modal_title_bar">
            <div id="divHeader" runat="server" />
            <asp:LinkButton ID="lbtClose" runat="server" OnClick="btnClosePopupForm_Click" />
        </div>

        <div id="modalPopupContainerBody" runat="server" class="modal_content">

            <asp:Label ID="lblResult" Text="" runat="server" />
            <div class="center">
                <asp:ImageButton ID="btnXlsExport" runat="server" ImageUrl="~/Images/xls.png" />
                <asp:ImageButton ID="btnXlsxExport" runat="server" ImageUrl="~/Images/xlsx.png" />
            </div>
        </div>

    </div>

</div>

