$(document).ready(function () {
    $("#PopulatedPlaceId").prop("disabled", true);
});

$('#region').change(function () {
    var id = $(this).val();

    $.ajax({
        type: 'POST',
        dataType: "JSON",
        url: '/Property/GetPlacesByRegionId',
        data: { id: id },
        success:
            function (response) {
                var markup;

                if (!id) {
                    markup += "<option>Select Populated place</option>";
                    $(document).ready(function () {
                        $("#PopulatedPlaceId").prop("disabled", true);
                    });
                }
                else {
                    for (var i = 0; i < response.data.length; i++) {

                        markup += " <option value=" + response.data[i].id + ">" + response.data[i].name + "</option>";
                    }

                    $("#PopulatedPlaceId").prop("disabled", false);
                }

                markup += "<br />";
                $("#PopulatedPlaceId").html(markup);
            },
        error:
            function (response) {

            }
    });
});
