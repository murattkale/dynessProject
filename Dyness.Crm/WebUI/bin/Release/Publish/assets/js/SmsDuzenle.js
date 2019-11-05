function GrupDivlerGosterGizle(gonnderilenGrupId, ilk, listeDoluMu) {
    if (gonnderilenGrupId == "1") {
        $("#divOgrenci").show();
        $("#divPersonel").hide();
    }
    else if (gonnderilenGrupId == "2") {
        $("#divOgrenci").hide();
        $("#divPersonel").show();
    }
    else {
        $("#divOgrenci").hide();
        $("#divPersonel").hide();
    }

    if (!ilk) {
        $("#SelectedSubeler").val("").trigger('change');
        $("#SelectedPersonelGruplar").val("").trigger('change');
    }

    if (!listeDoluMu) {
        $("#divTelefonBilgilerPartialView").hide();
    }
    else {
        var length = $("#tblTefonlar tbody tr").length;
        $("#spnToplamSms").text(length);
    }
}

function SmsDuzenle(GrupSecimiZorunlu, SubeSecimiZorunlu, SezonSecimiZorunlu, SmsGonderilecekSecin, ListeDoluMu) {
    $(document).ready(function () {
        $("#SelectedGonderilenGrup").on("change", function (e) {

            $("#divTelefonBilgiler").hide();
            $("#spnToplamSms").text("0");

            GrupDivlerGosterGizle($(this).val(), false, false);

        });

        GrupDivlerGosterGizle($("#SelectedGonderilenGrup").val(), true, ListeDoluMu);

        Sezonlar($("#SelectedSezonlar"), $("#SelectedSubeler"), $("#SelectedSiniflar"));

        SmsSiniflar($("#SelectedSiniflar"), $("#SelectedSezonlar"), $("#SelectedSubeler"));

        SmsSinavlar($("#SelectedSinavId"), $("#SelectedSezonlar"), $("#SelectedSubeler"));

        $("#btnFiltrele").click(function () {

            TelefonlariListele();
        });

        $(document).ajaxComplete(function () {

            $('#chcHepsiniSec').change(function () {
                if (this.checked) {
                    SetUniform("input[type=checkbox]", true);
                }
                else {
                    SetUniform("input[type=checkbox]", false);
                }
            });
        });

        $("#SelectedSmsSablonId").on("change", function (e) {

            var smsSablonId = $(this).val();

            $.ajax({
                url: "/SmsMetinSablon/Detay/" + smsSablonId,
                type: "POST",
                dataType: "json",
                success: function (data) {

                    if (data == null) {
                        $("#Model_Mesaj").text("");
                    }
                    else {
                        $("#Model_Mesaj").text(data.Sablon);
                    }
                },
                error: function () {
                }
            });
        });
    });

    function TelefonlariListele() {
        var grupId = $("#SelectedGonderilenGrup").val();

        if (grupId == "") {
            alert(decodeHTML(GrupSecimiZorunlu));
            return;
        }

        var ogrenciGrupIdler = $("#SelectedOgrenciGonderilenGrup").val();
        var ogrenciGrupIdStr = GetStringParameterFromArray(ogrenciGrupIdler);

        var subeIdler = $("#SelectedSubeler").val();
        var subeIdlerStr = GetStringParameterFromArray(subeIdler);

        if (subeIdlerStr == "") {
            alert(decodeHTML(SubeSecimiZorunlu));
            return;
        }

        var sezonIdler = $("#SelectedSezonlar").val();
        var sezonIdlerStr = GetStringParameterFromArray(sezonIdler);

        if (sezonIdlerStr == "" && grupId == "1") {
            alert(decodeHTML(SezonSecimiZorunlu));
            return;
        }

        var sinavId = $("#SelectedSinavId").val();

        var sinifIdler = $("#SelectedSiniflar").val();
        var sinifIdlerStr = GetStringParameterFromArray(sinifIdler);

        var personelGrupIdler = $("#SelectedPersonelGruplar").val();
        var personelGrupIdlerStr = GetStringParameterFromArray(personelGrupIdler);

        $.ajax({
            url: "/Sms/Filtrele?GrupId=" + grupId + "&SinavId=" + sinavId +  "&SubeIdler=" + subeIdlerStr + "&OgrenciGrupIdler=" + ogrenciGrupIdStr + "&SezonIdler=" + sezonIdlerStr + "&SinifIdler=" + sinifIdlerStr + "&PersonelGrupIdler=" + personelGrupIdlerStr,
            type: "POST",
            dataType: "html",
            success: function (data) {
                $("#divTelefonBilgiler").html(data);
                $("#divTelefonBilgiler").show();

                var length = $("#tblTefonlar tbody tr").length;
                $("#spnToplamSms").text(length);
            },
            error: function () {
                $("#divTelefonBilgiler").hide();
                $("#spnToplamSms").text("0");
            }
        });
    }
}

