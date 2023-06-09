$('#registerForm :checkbox').change(function () {
    if (this.checked) {
        $('#submitButton').prop('disabled', false);
        console.log(true);
    } else {
        $('#submitButton').prop('disabled', true);
        console.log(false);
    }
});
