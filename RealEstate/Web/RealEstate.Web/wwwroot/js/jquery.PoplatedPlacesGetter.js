$(document).ready(function () {

    console.log(true);
    $("#PopulatedPlaceId").prop("disabled", true);
});

$('#region').change(function () {
    var id = $(this).val();

    console.log(12);
    $.ajax({
        type: 'POST',
        dataType: "JSON",
        url: "/Property/GetPopulatedPlaces",
        data: { id: id },
        success:
            function (response) {
                var markup;

                if (!id) {
                    markup += "<option>Select Populated place</option>";
                    $(document).ready(function () {
                        $("#PopulatedPlaceId").prop("disabled", true);
                    });
                    console.log(true);
                }
                else {
                    for (var i = 0; i < response.data.length; i++) {

                        markup += " <option value=" + response.data[i].id + ">" + response.data[i].name + "</option>";
                        console.log(i);
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
