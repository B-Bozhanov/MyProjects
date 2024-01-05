let form = $("#agencyName");
let isClicked = true;

$(document).ready(function () {
    console.log(1);
    form.hide();
});

$('#agencyName').click(function () {
    if (isClicked) {
        form.show();
        isClicked = false;
    }
    else {
        form.hide();
        isClicked = true;
    }
});