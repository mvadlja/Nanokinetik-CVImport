var cancel = false;
var countCancel = 0;


//$(".Save, .downloadAttachment, .boldText, .list-box-buttons a, .modal_title_bar a, .gvLinkTextBold, .pltp, .pltp a, .gridHeaderCell a, .shadow a").live('click', function () {
$(".Save, .downloadAttachment, .boldText, .list-box-buttons a, .modal_title_bar a, .gvLinkTextBold, .pltp, .pltp a, .gridHeaderCell a, .disableLeaveThisPage, .ppLeaveThisPage, .roles-action-icon, .Export, .GridColumns, .SaveLay, .LoadLay").live('click', function () {
    if (!$(this).hasClass("Cancel")) {
        $(window).unbind('beforeunload');
    }
});

$(".Cancel, .layoutHeader, .controlButtons, .tabs").live('click', function () {
    cancel = false;
    handleBeforeUnload();
});

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

$(".Cancel").live("click", function () {

    if ($.browser.msie) {
        var href = $(this).attr("href");

        if (href != "#") {
            $(this).attr("href", "#");
            $(this).attr("rel", href);
        } else {
            href = $(this).attr("rel");
        }
        try { eval(href); } catch (ex) { }
    }
});

function handleBeforeUnload() {

    var currentUrl = window.location.href;

    if (currentUrl.search("Form.aspx") > 0) {

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

if ($.browser.msie) {
    $(document).ready(function () {
        $(".alertParentLink").filter(function () {
            return (/^javascript\:/i).test($(this).attr('href'));
        }).each(function () {
            var hrefscript = $(this).attr('href');
            hrefscript = hrefscript.substr(11);
            $(this).data('hrefscript', hrefscript);
        }).click(function () {
            var hrefscript = $(this).data('hrefscript');
            eval(hrefscript);
            return false;
        }).attr('href', '#');
    });
}