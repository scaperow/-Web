var aLoader = null;
$(document).ajaxStart(function () {
    aLoader = new ajaxLoader($('body')[0], { left: 0, top: 0, bgColor: '#fff' });

});
$(document).ajaxStop(function () {
    if (aLoader) {
        aLoader.remove();
    }
});