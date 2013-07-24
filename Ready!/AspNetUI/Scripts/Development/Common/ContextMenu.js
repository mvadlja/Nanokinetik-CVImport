function respositionContextMenu() {
    var container = $("#additionalContextMenuItems");
    var btnExport = $('input[id*="btnExport"]');
    var btnSave = $('input[id*="btnSaveLayout"]');
    var btnLoad = $('input[id*="btnLoadLayout"]');
    var btnRefresh = $('input[id*="btnRefresh"]');

    if (btnSave) container.append(btnSave);
    if (btnLoad) {
        btnLoad.css("margin-right", "34px");
        container.append(btnLoad);
    }

    if (btnRefresh) container.append(btnRefresh);
    if (btnExport) container.append(btnExport);
}