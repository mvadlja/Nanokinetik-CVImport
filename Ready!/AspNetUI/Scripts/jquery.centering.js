
function CenterElementInClient(elementToCenterID, elementCanScroll, marginOffsetX, marginOffsetY) {
    (function($) {

//        $("*[id$=" + elementToCenterID + "]").centerInClient(elementToCenterID, marginOffsetX, marginOffsetY);

//        $(window).bind("resize", function(arg) {
//            $("*[id$=" + elementToCenterID + "]").centerInClient(elementToCenterID, marginOffsetX, marginOffsetY);
//        });

        if (elementCanScroll == true) {
            $(window).bind("scroll", function(arg) {
                $("*[id$=" + elementToCenterID + "]").centerInClient(elementToCenterID, marginOffsetX, marginOffsetY);
            });
        }

    })(jQuery);
}


/* Center positioning function */
(function($) {
    $.fn.centerInClient = function(elementToCenterID, marginOffsetX, marginOffsetY) {

        var el = $("*[id$=" + elementToCenterID + "]");
        var jWin = $(window);

        // height is off a bit so fudge it
        var heightFudge = 2.0;

        var elementWidth = el.width();
        var elementHeight = el.height();

        if (elementWidth == undefined || elementWidth == 0) elementWidth = parseInt(el.css("width").replace("px", ""));
        if (elementHeight == undefined || elementHeight == 0) elementHeight = parseInt(el.css("height").replace("px", ""));

        var x = (jWin.width() / 2) - (elementWidth / 2);
        var y = (jWin.height() / heightFudge) - (elementHeight / 2);

        // Including margins
        if (marginOffsetX == undefined) marginOffsetX = 0;
        if (marginOffsetY == undefined) marginOffsetY = 0;

        x = x + jWin.scrollLeft() + marginOffsetX;
        y = y + jWin.scrollTop() + marginOffsetY;

        // Check if panel is heigher than window
        if (elementHeight + 40 >= jWin.height()) y = 40 + jWin.scrollTop();

        el.css("display", "inline");

        el.css("text-align", "center");
        el.css("vertical-align", "middle");
        el.css("top", y);
        el.css("left", x);
        el.css("position", "absolute");
        el.css("z-index", "111");

        return this;
    }
})(jQuery);