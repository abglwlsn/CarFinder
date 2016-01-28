function deadImage() {
    $("img").error(function () {
        $(this).hide();
    });
};