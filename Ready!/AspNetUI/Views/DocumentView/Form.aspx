<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Template/Default.Master" AutoEventWireup="false" CodeBehind="Form.aspx.cs" Inherits="AspNetUI.Views.DocumentView.Form" %>

<%@ Register Src="../Shared/UserControl/TextBox.ascx" TagName="TextBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DateTimeBox.ascx" TagName="DateTimeBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/LabelPreview.ascx" TagName="LabelPreview" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/DropDownList.ascx" TagName="DropDownList" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/TabMenu.ascx" TagName="TabMenu" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Reminder.ascx" TagName="Reminder" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBoxSr.ascx" TagName="ListBoxSr" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ListBox.ascx" TagName="ListBox" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/UploadPanel.ascx" TagName="UploadPanel" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/ModalPopup.ascx" TagName="ModalPopup" TagPrefix="uc" %>
<%@ Register Src="../Shared/UserControl/Popup/BrowseEDMSPopup.ascx" TagName="BrowseEDMSPopup" TagPrefix="uc" %>

<asp:Content ID="contentHeadDefault" ContentPlaceHolderID="cphHeadDefault" runat="server">
</asp:Content>
<asp:Content ID="contentBodyMain" ContentPlaceHolderID="cphBodyMain" runat="server">
    <uc:Reminder ID="Reminder" runat="server" />
    <uc:ModalPopup ID="mpDelete" runat="server" />
    <uc:BrowseEDMSPopup ID="BrowseEDMSPopup" runat="server" />
    <div class="entity-name">
        <uc:LabelPreview ID="lblPrvParentEntity" runat="server" Label="Document:" />
    </div>
    <div class="subtabs">
        <uc:TabMenu ID="tabMenu" runat="server" />
    </div>
    <asp:Panel ID="pnlForm" runat="server" class="form preventableClose">
        <div id="divLeftPane" class="leftPane">
            <fieldset>
                <uc:ListBoxSr ID="lbSrRelatedEntities" runat="server" LabelWidth="150px" SelectionMode="Single" Text="Related entity:" VisibleRows="5" TextWidth="336px" Actions="Add, Remove" />
                <uc:TextBox ID="txtDocumentName" runat="server" Label="Document name:" />
                <uc:TextBox ID="txtDescription" runat="server" Label="Description:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:DropDownList ID="ddlDocumentType" runat="server" Label="Document type:" />
                <uc:DropDownList ID="ddlVersionNumber" runat="server" Label="Version number:" />
                <uc:DropDownList ID="ddlResponsibleUser" runat="server" Label="Responsible user:" />
                <uc:DropDownList ID="ddlVersionLabel" runat="server" Label="Version label:" />
                <uc:TextBox ID="txtDocumentNumber" runat="server" Label="Document number:" />
                <uc:DropDownList ID="ddlRegulatoryStatus" runat="server" Label="Regulatory status:" />
                <uc:TextBox ID="txtComment" runat="server" Label="Comment:" MaxLength="2000" TextMode="MultiLine" Columns="55" Rows="5" />
                <uc:DropDownList ID="ddlAttachmentType" runat="server" Label="Attachment type:" />
                <uc:ListBox ID="lbLanguageCodes" runat="server" Label="Language code:" TextWidth="200" Height="150" SelectionMode="Multiple" />
                <uc:TextBox ID="txtEDMSBindingRule" runat="server" Label="Binding rule:" />
                <uc:DateTimeBox ID="dtChangeDate" runat="server" Label="Change date:" MaxLength="100" />
                <uc:DateTimeBox ID="dtEffectiveStartDate" runat="server" Label="Effective start date:" MaxLength="100" ShowReminder="True" />
                <uc:DateTimeBox ID="dtEffectiveEndDate" runat="server" Label="Effective end date:" MaxLength="100" ShowReminder="True" />
                <uc:DateTimeBox ID="dtEDMSModifyDate" runat="server" Label="Modify date:" MaxLength="100" />
                <uc:DateTimeBox ID="dtVersionDate" runat="server" Label="Version date:" MaxLength="100" />
                <uc:TextBox ID="txtEvcode" runat="server" Label="EVCODE:" Enabled="False" />
                <uc:UploadPanel ID="upAttachments" runat="server" Label="Attachments:" />
                <asp:Button ID="btnBrowseEDMSUC" runat="server" Text="Link from EDMS" CssClass="button BrowseEDMS" UseSubmitBehavior="False" />
            </fieldset>
        </div>
        <div id="divRightPane" class="rightPane" style="margin-top: 0px; margin-left: -74px;">
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
                __doPostBack($(this).attr("id"), lbInputDblClick.attr('name'));
            });
        });

        function handleExecutedCheckAction(action, url, file, id) {
            if (action == "CheckOut" || action == "CheckOutFileNameWarning") {
                if ($.browser.msie) {
                    if (action == "CheckOutFileNameWarning") {
                        showPopupMessage("Warning", "Attachment name contains one or more invalid characters ('/', '\\', ':', '*', '?', '\"', '<', '>', '|' ) therefore they were replaced with '_' in checked out file's name.");
                    }
                    setTimeout(function () {
                        edit(file, url, true);
                    }, 200);
                    
                } else {
                    showPopupMessage("Warning", "Automatic check out is supported only in Internet Explorer.<br/><br/>Please use Internet Explorer or download and open file manually.");
                }
            }
            else if (action == "CheckIn") {
                if ($.browser.msie) {
                    if (checkIfLocalFileExists(file)) {
                        fileUpload(file, url);
                        window.__doPostBack($("#" + id).attr("href").match(".*__doPostBack\\('(.*)',''\\)")[1], '');
                        deleteLocalFile(file);
                    } else {
                        showPopupMessage("Error", "Check in failed. File can't be found.");
                    }
                } else {
                    showPopupMessage("Warning", "Automatic check in is supported only in Internet Explorer.<br/><br/>Please use Internet Explorer or upload file manually.");
                }
            }
            else if (action == "CancelCheckout") {
                if ($.browser.msie) {
                    if (checkIfLocalFileExists(file)) {
                        deleteLocalFile(file);
                    }
                }
            }
            else if (action == "CheckInInvalidFileType") {
                showPopupMessage("Error", "Invalid attachment file type. Please upload attachment with same file type.");
            }
            else if (action == "Refresh") {
                showPopupMessage("Warning", " Changes were made since page was loaded therefore requested action can't be performed.<br/><br/> Page is now refreshed. Please try again.");
            }
        }
        
        function showPopupMessage(header, message) {
            var popup = $("#modalPopup_mpContainer");
            popup.find("#modalPopup_pnlHeader").html(header);
            popup.find("#modalPopup_pnlContent").html("<br/><div style=\"text-align:center;\">" + message + "</div><br/>");
            popup.find("#mpYContainerMain").css("margin-top", "200px");
            popup.find("#modalPopup_btnYes").hide();
            popup.find("#modalPopup_btnNo").hide();
            popup.find("#modalPopup_btnCancel").hide();
            popup.css("display", "inline");
        }

        var initFileSystemObjectErrorMsg = "Internet options setup error. Please contact system administrator for further instructions.";
        var activeXForEditAction = "WScript.Shell";
        var askUserToDeleteLocalFileOnEditAction = true;
        var confirmLocalFileDeleteMsg = "File already exists on local machine. Delete existing file?";
        var localFileMissingErrorMsg = "File can't be found on local machine.";
        var openLocalFileErrorMsg = "Error occurred while opening local file.";
        var detailsStr = "Details";
        var writeContentToServerErrMsg = "Error occurred while uploading file to server.";
        var deleteFileErrorMsg = "Error occurred while deleting local file. File is in use.";
        var writeLocalFileErrorMsg = "Error occurred while saving file to local folder.";
        
    </script>
</asp:Content>
