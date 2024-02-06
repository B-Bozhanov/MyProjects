// Write your JavaScript code.

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

function RemoveProperty(value) {
    let message = "Are you sure?";

    if (confirm(message)) {
        var propertyId = value;
        $.ajax({
            type: 'POST',
            dataType: "JSON",
            url: "/MyProperties/RemoveUserProperty",
            cors: true,
            data: { propertyId: propertyId },
            success:
                function (response) {
                    if (response.data) {
                        console.log("Redirected");
                        window.location.href = "/MyProperties/ActiveProperties";
                    }
                }
        });
    }
}

$(function SearchFormHider() {
    var form = $("#searchForm");
    let isClicked = true;
    form.hide();
    $('#Search').on('click', function () {
        if (isClicked) {
            form.show();
            isClicked = false;
        }
        else {
            form.hide();
            isClicked = true;
        }
    });
});

$(function RegisterButtonDisable() {
    $('#registerForm :checkbox').on('change', function () {
        if (this.checked) {
            $('#submitButton').prop('disabled', false);
            console.log(true);
        } else {
            $('#submitButton').prop('disabled', true);
            console.log(false);
        }
    });
});

$(function PropertySorter() {
    var option = $('#optionTypes');

    var optionId;

    option.on('change', function () {
        optionId = $(this).val();
        window.location.href = "/Property/Index?optionId=" + optionId;
        $('#submitOption').trigger('click');
        option = $(this).val().name();
    });
});

$(function PopulatedPlaceGetter() {
    $("#PopulatedPlacesHide").hide();

    $('#location').on('change', function () {
        var id = $(this).val();

        $.ajax({
            type: 'POST',
            dataType: "JSON",
            url: "/Property/GetPopulatedPlaces",
            cors: true,
            data: { id: id },
            success:
                function (response) {
                    var markup;

                    if (!id) {
                        markup += "<option>Изберете населено място</option>";
                        $(function () {
                            $("#PopulatedPlacesHide").hide();
                        });
                    }
                    else {
                        for (var i = 0; i < response.data.length; i++) {

                            markup += " <option value=" + response.data[i].id + ">" + response.data[i].name + "</option>";
                        }
                        $("#PopulatedPlacesHide").show();
                        // $("#PopulatedPlaceId").prop("disabled", false);
                    }

                    markup += "<br />";
                    $("#PopulatedPlaceId").html(markup);
                },
            error:
                function (response) {

                }
        });
    });
});