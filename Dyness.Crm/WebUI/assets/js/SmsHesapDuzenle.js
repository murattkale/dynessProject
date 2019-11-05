$(document).ready(function () {

    $("input[name='PostedFilesDosyalar']").change(function (e) {

        var input = $(this);

        var list = $("#ulDosyalar");

        list.empty()

        if (e.target.files.length > 0) {
            $("#divDosyalar").show();

            for (var x = 0; x < e.target.files.length; x++) {

                if (e.target.files[x].name == "" || e.target.files[x].name == null)
                    continue;

                $("#ulDosyalar").append('<li class="list-group-item"><b>' + (x + 1) + ':</b>  ' + e.target.files[x].name + '</li>');
            }
        }
        else {
            $("#divDosyalar").hide();
        }
    });

});