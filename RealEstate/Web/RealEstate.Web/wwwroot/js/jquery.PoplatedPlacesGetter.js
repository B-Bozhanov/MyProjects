$(document).ready(function () {
    $("#PopulatedPlacesHide").hide();
});

$('#location').change(function () {
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
