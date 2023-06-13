var form = $("#searchForm");
var isClicked = true;

$(document).ready(function () {
    form.hide();
});

$('#Search').click(function () {
    if (isClicked) {
        form.show();
        isClicked = false;
    }
    else {
        form.hide();
        isClicked = true;
    }
});