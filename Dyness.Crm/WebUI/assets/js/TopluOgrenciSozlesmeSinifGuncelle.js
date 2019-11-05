function ValueItem(value, selected) {
    this.Value = value;
    this.Selected = selected;
}

var values = new Array();

var selectedValue = 0;

$(document).ready(function () {
    $(".select2-multiple").on("change",
        function (e) {

            var thisId = $(this).attr("id");

            $(".select2-multiple").each(function () {

                var id = $(this).attr("id");

                if (id !== thisId) {

                    for (var i = 0; i < values.length; i++) {
                        if (values[i].Selected) {
                            $("#" + id + " option[value=\"" + values[i].Value + "\"]").prop("disabled", true);
                        } else {
                            $("#" + id + " option[value=\"" + values[i].Value + "\"]").prop("disabled", false);
                        }
                    }

                    $("#" + id).select2();
                }
            });

        });

    $(".select2-multiple").on("select2:selecting",
        function (e) {

            var varMi = false;

            selectedValue = e.params.args.data.id;

            for (var i = 0; i < values.length; i++) {
                if (values[i].Value === selectedValue) {
                    varMi = true;
                    values[i].Selected = true;
                    break;
                }
            }

            if (!varMi) {
                values.push(new ValueItem(selectedValue, true));
            }

            var thisId = $(this).attr("id");
            var index = thisId.replace("OgrenciSozlesmeSiniflar_", "").replace("__OgrenciSozlesmeIdler", "");
            var sinifId = $("#OgrenciSozlesmeSiniflar_" + index + "__SinifId").val();

            var selectedElemCount = $('input[id^="OgrenciSozlesmeSiniflar_' + index + '__OgrenciSozlesmeIdler_"]').length;

            var hiddenValue = '<input data-val="true" data-val-number="Int32 alanı sayısal değer olmalıdır." data-val-required="Int32 alanı gereklidir." id="OgrenciSozlesmeSiniflar_' + index + '__OgrenciSozlesmeIdler_' + selectedElemCount + '_" name="OgrenciSozlesmeSiniflar[' + index + '].OgrenciSozlesmeIdler[' + selectedElemCount + ']" type="hidden" value="' + selectedValue + '" />';

            $('#divSinif' + sinifId).append(hiddenValue);
        });

    $(".select2-multiple").on("select2:unselecting",
        function (e) {

            selectedValue = e.params.args.data.id;

            for (var i = 0; i < values.length; i++) {
                if (values[i].Value === selectedValue) {
                    values[i].Selected = false;
                }
            }

            var thisId = $(this).attr("id");
            var index = thisId.replace("OgrenciSozlesmeSiniflar_", "").replace("__OgrenciSozlesmeIdler", "");
            var sinifId = $("#OgrenciSozlesmeSiniflar_" + index + "__SinifId").val();

            $('#divSinif' + sinifId + ' :input[value="' + selectedValue + '"]').remove();
        });

    $(".select2-multiple").each(function (i) {

        var options = $(this).children("option");

        for (var i = 0; i < options.length; i++) {
            if (options[i].selected) {
                values.push(new ValueItem(options[i].value, true));
            }
        }
    });

    $(".select2-multiple").each(function () {

        var id = $(this).attr("id");

        for (var i = 0; i < values.length; i++) {
            if (values[i].Selected) {
                $("#" + id + " option[value=\"" + values[i].Value + "\"]").prop("disabled", true);
            } else {
                $("#" + id + " option[value=\"" + values[i].Value + "\"]").prop("disabled", false);
            }
        }

        $("#" + id).select2();
    });
});