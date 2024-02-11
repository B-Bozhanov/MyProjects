// Write your JavaScript code.

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

function RemoveProperty(value) {
    let message = "Are you sure?";

    if (confirm(message)) {
        var id = value;
        var token = $('input:hidden[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: 'POST',
            dataType: "JSON",
            url: "/MyProperties/RemoveUserProperty",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XhrToken", token);
            },
            cors: true,
            data: { propertyId: id },
            success:
                function (response) {
                    if (response.data) {
                        window.location.reload();
                    }
                }
        });
    }
}

$(function SearchFormHider() {
    var form = $("#searchForm");
    var button = $('#Search');
    let isToHide = button.val();
    form.hide();
    let isClicked = true;

    if (isToHide == "false") {
        form.show();
        isClicked = false;
    }
    button.on('click', function () {
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
        } else {
            $('#submitButton').prop('disabled', true);
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
   /* $("#PopulatedPlacesHide").hide();*/
    var form = $('#searchForm');
    var token = $('input:hidden[name="__RequestVerificationToken"]').val();

    $('#location').on('change', function () {
        var id = $(this).val();

        $.ajax({
            type: 'POST',
            dataType: "JSON",
            url: "/Property/GetPopulatedPlaces",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XhrToken", token);
            },
            cors: true,
            data: {locationId: id},
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
                        //$("#PopulatedPlaceId").prop("disabled", false);
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