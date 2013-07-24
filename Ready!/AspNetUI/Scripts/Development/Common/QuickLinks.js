$(document).ready(function () {

    $("#divQuickLinksContainer span, #divQuickLinksContainer a.ql-groupname").click(function() {

        if ($(this).parent().hasClass("closed")) {
            $(this).parent().removeClass("closed");
            $(this).parent().addClass("opened");
            $(this).nextAll("ul").slideDown(600, "easeOutExpo");
        } else {
            $(this).parent().removeClass("opened");
            $(this).parent().addClass("closed");
            $(this).nextAll("ul").slideUp(600, "easeOutExpo");
        }
    });


    $("#divQuickLinksButtonOpen, #divQuickLinksContainer, #divQuickLinksContainer a, #divQuickLinksContainer li").click(function () {

        if (!$(this).is("a") && !$(this).is("li")) {

            if ($("#divQuickLinksButtonOpen").hasClass("open")) {

                $("#divQuickLinksButtonOpen").removeClass("open");

                $("#pnlQuickLinks").animate({
                    left: '-=252',
                }, 500);

            } else if ($(this).attr("id") == "divQuickLinksButtonOpen") {
                $("#divQuickLinksButtonOpen").addClass("open");
                $("#pnlQuickLinks").animate({
                    left: '+=252',
                }, 500);

                return false;
            }
            
        } else if ($(this).is("a") && !$(this).hasClass("ql-groupname")) {
            
            window.location = $(this).attr("href");

            return false;
        } else {
            return false;
        }
        return false;
    });

});