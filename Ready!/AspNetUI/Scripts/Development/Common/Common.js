$(".ResetLay").live("click", function () {

    gridid = $("[gridid]").attr("gridid");
    gridversion = $("[gridversion]").attr("gridversion");

    grid = "PossDragtable" + "-" + gridid + "_" + gridversion;
    gridSize = "PossColumnSize" + "-" + gridid + "_" + gridversion;

    if (localStorage != undefined) {
        localStorage.removeItem(grid);
        localStorage.removeItem(gridSize);
    }
});