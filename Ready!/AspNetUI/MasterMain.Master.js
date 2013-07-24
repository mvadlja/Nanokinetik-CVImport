var cancel = false;
var countCancel = 0;

$(".Save, .downloadAttachment, .boldText, .list-box-buttons a, .modal_title_bar a, .gvLinkTextBold, .pltp, .pltp a, .gridHeaderCell a, .shadow a").live('click', function () {
    if (!$(this).hasClass("Cancel")) {
        $(window).unbind('beforeunload');
    }
});
$(".Cancel, .layoutHeader, .controlButtons, .tabs").live('hover', function () { cancel = false; countCancel = 0; handleBeforeUnload(); });
$("body a, body input").live('click', function () {
    if ($(this).hasClass("Cancel")) {
        cancel = true;
    } else {
        cancel = false;
        countCancel = 0;
    }
});
$("body").live('keydown', function () { handleBeforeUnload(); });

$(document).ready(function () { handleBeforeUnload(); });

$(".Save").live('click', function () {
    countCancel = 10; // force Save to unbind beforeunload event
});

function handleBeforeUnload() {

    var currentUrl = window.location.href;

    if (currentUrl.search("Form.aspx") > 0 ||
        currentUrl.search("f=dn") > 0 ||
        currentUrl.search("f=sa") > 0 ||
        currentUrl.search("f=d") > 0 ||
        (currentUrl.search("f=p") > 0 && $(".Save").length > 0) ||
        (currentUrl.search("f=l") > 0 && $(".Save").length > 0)) {

        if (cancel == true) {
            countCancel++;
        }

        if (countCancel <= 0) {
            $(window).on('beforeunload', function () {

                $(".progressPanel").hide();
                $(".modalOverlayPogress").hide();

                return "This page may contain unsaved data.";

            });
        } else {
            $(window).unbind('beforeunload');
        }


    }

}


$(function () {
    //initializePreventableClose();
    refreshEmptyPagerCount();
});

var firstSubmit = true;
var lastClickedIsCancel = true;
function refreshFirstSubmit() {
    firstSubmit = true;
}

function initializePreventableClose() {

    setTimeout("$('#inFormPostBack').val('false')", 100);
    setTimeout("$('#skipPrevent').val('false')", 100);
    var visibleForms = $(".preventableClose:visible");
    if (visibleForms.length > 0) {
        $("#hasChanges").val("true");

        $(".preventableClose").click(function (e) {
            if (
                e.target.toString().indexOf("javascript:") > -1
                || (e.target.href != null && (e.target.target != "_blank" || e.target.href.indexOf("javascript") > -1))
                || (e.target.nodeName.toLowerCase().indexOf("input") > -1 && (e.target.type.toLowerCase().indexOf("button") > -1 || e.target.id.toLowerCase().indexOf("save") > -1))
                ) {
                $("#inFormPostBack").val("true");
            }

        });

        $(document).click(function (event) {
            lastClickedIsCancel = $(event.target).hasClass("cancel") || $(event.target).hasClass("Cancel");
             firstSubmit = true;
        });
    } else {
        $("#hasChanges").val("false");
    }
    $(".Save").click(function () { $("#skipPrevent").val("true"); });
    $(window).bind("beforeunload", function () {
        var changed = $("#hasChanges").val();
        var skipPrevent = $("#skipPrevent").val();
        var inFormPostBack = $("#inFormPostBack").val();

        if (changed == "true") {
            if (inFormPostBack == "false") {
                if (skipPrevent == "false") {

                    if (firstSubmit) {
                     
                        firstSubmit = false;
                        //  setTimeout("refreshFirstSubmit()", 5000);
                        return "This page may contain unsaved data.";
                    }
                } else {
                    $("#skipPrevent").val("true");
                }
            } else {

            }
        }
    });
}

function refreshEmptyPagerCount() {
    $(".dxpSummary").each(function () {
        var pagerValue = $(this).html();
        if (pagerValue == "Page 1 of 0 (0 items)") {
            $(this).html("Page 0 of 0 (0 items)");
        }
    });
}
