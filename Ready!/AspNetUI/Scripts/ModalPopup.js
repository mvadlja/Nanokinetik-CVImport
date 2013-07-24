Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

// Postback handler
function EndRequestHandler(sender, args) {
    if (args.get_error() == undefined) {
        HandleJQueryPostback();
    }
}

// Centering all .modalPopupPanel panels
function HandleJQueryPostback() {
    $(function() {

        $(".modal_container_wide").each(
            function(intIndex) {
                CenterElementInClient($(this).attr("id"), false, 0, 0);
            }
        );

            $(".modal_container").each(
            function (intIndex) {
                CenterElementInClient($(this).attr("id"), false, 0, 0);
            }
        );

        $(".modal_container_small").each(
            function(intIndex) {
                CenterElementInClient($(this).attr("id"), false, 0, 0);
            }
        );
    });
}

// Hide modal popup
function HideModalPopup(modalPopupID) {

    $("*[id$=" + modalPopupID + "]").css("display", "none");
}

// Message modal popup
function DisplayModalPopupContainer(header, message) {
    $(function() {

        $("*[id$=modalPopupContainer]").css("display", "inline");
        var divHeader = $("*[id$=modalPopupContainer] *[id$=divHeader]");
        var divMessage = $("*[id$=modalPopupContainer] *[id$=divMessage]");

        $(divHeader).html(header);
        $(divMessage).html(message);

        CenterElementInClient($("*[id$=modalPopupContainerContent]").attr("id"), false, 0, 0);
    });
}

// Message modal popup
function DisplayMessageModalPopup(header, message) {
    $(function() {

        $("*[id$=messageModalPopupContainer]").css("display", "inline");
        var divHeader = $("*[id$=messageModalPopupContainer] *[id$=divHeader]");
        var divMessage = $("*[id$=messageModalPopupContainer] *[id$=divMessage]");

        $(divHeader).html(header);
        $(divMessage).html(message);

        CenterElementInClient($("*[id$=messageModalPopupContainerContent]").attr("id"), false, 0, 0);
    });
}

// Confirm modal popup
function DisplayConfirmModalPopup(header, message, sender, argument) {
    $(function () {

        $("*[id$=confirmModalPopupContainer]").css("display", "inline");
        var divHeader = $("*[id$=confirmModalPopupContainer] *[id$=divHeader]");
        var divMessage = $("*[id$=confirmModalPopupContainer] *[id$=divMessage]");

        $(divHeader).html(header);
        $(divMessage).html(message);

        CenterElementInClient($("*[id$=confirmModalPopupContainerContent]").attr("id"), false, 0, 0);

        var controlID = $(sender).attr("id");
        $("*[id$=hiddenConfirmModalPostbackControlID]").val(controlID);
        $("*[id$=hiddenConfirmModalPostbackArgument]").val(argument);
    });
}

// Confirm modal popup callback
function AnswerConfirmModalPopup(answer) {
    $(function () {

        if (answer == "yes") {
            var postbackControlID = $("*[id$=hiddenConfirmModalPostbackControlID]").val();
            var argument = $("*[id$=hiddenConfirmModalPostbackArgument]").val();
            //window.location = "";
            //alert(postbackControlID);
            __doPostBack(postbackControlID, argument);

            //$("#ContentPlaceHolder_btnTest").click();
        }

        HideModalPopup($("*[id$=confirmModalPopupContainer]").attr("id"));
    });
}