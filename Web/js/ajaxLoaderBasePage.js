var aLoader = null;
$(document).ajaxStart(function () {
    if ($('#hd_leftTreeRefresh').val() != '1') {
        aLoader = new ajaxLoader($('#mainBody')[0]);
    }

});
$(document).ajaxStop(function () {
    if (aLoader) {
        aLoader.remove();
    }
});
