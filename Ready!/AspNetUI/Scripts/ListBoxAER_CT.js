var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_endRequest(function (s, e) {
    listBoxAER_initializeOnPostback(s, e);
});

$(function () {
    listBoxAER_initializeOnPostback(null, null);
}
    );

function listBoxAER_ctlInputChanged(control) {
    var selectedCnt = 0;
    $(control).children("option:selected").each(function () {
        selectedCnt++;
    });

    var removeButton = $(control).closest("table[clientIdAttr=AER_CT_table]").first().find("a[clientIdAttr=AER_CT_btnRemove]").first();
    if (selectedCnt < 1) {
        removeButton.addClass("aspNetDisabled");
        removeButton.css("cursor", "text");
    } else {
        removeButton.removeClass("aspNetDisabled");
        removeButton.css("cursor", "pointer");
    }

    var editButton = $(control).closest("table[clientIdAttr=AER_CT_table]").first().find("a[clientIdAttr=AER_CT_btnEdit]").first();
    if (selectedCnt != 1) {
        editButton.addClass("aspNetDisabled");
        editButton.css("cursor", "text");
    } else {
        editButton.removeClass("aspNetDisabled");
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