var test = $('#optionTypes');

var optionId;

test.on('change', function () {
    optionId = $(this).val();
    window.location.href = "/Property?optionId=" + optionId;
    $('#submitOption').trigger('click');
});

test = $(this).val().name();
