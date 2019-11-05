function yayin() {

    var sinifSeviyeId = $("#Model_SinifSeviyeId").val();
    var bransId = $("#Model_BransId").val();

    $("[class^='OgrenciSozlesmeYayin']").hide();

    $(".OgrenciSozlesmeYayin" + bransId + sinifSeviyeId).show("fast");
    $(".OgrenciSozlesmeYayin" + sinifSeviyeId).show("fast");

    if (sinifSeviyeId > 0) {
        $("#divOgrenciSozlesmeYayinBilgi").hide();
        $("#divOgrenciSozlesmeYayinlar").show();
    }
    else {
        $("#divOgrenciSozlesmeYayinBilgi").show();
        $("#divOgrenciSozlesmeYayinlar").hide();
    }
}

function setUnSelected() {
    SetSelectedValue("#Model_SinifSeviyeId", "0");
    SetSelectedValue("#Model_BransId", "0");
    SetSelectedValue("#Model_SinifId", "0");

    SetSelectedValue("#Model_OkulTurId", "0");
    SetSelectedValue("#Model_ServisId", "0");

    SetSelectedValue("#Model_EtkinlikId", "0");
    SetSelectedValue("#Model_DanismanPersonelId", "0");

    SetSelectedValue("#Model_EhliyetTurId", "0");

    SetSelectedValue("#SelectedDersler", "0");

    SetUniform("#Model_YemekDahilMi", false);

    $(".ogrenciSozlesmeDetay").hide();

    $("#divSinifSeviye").hide();
    $("#divBrans").hide();
    $("#divSinif").hide();

    $("#divOgrenciSozlesmeTur2").hide();
    $("#divOgrenciSozlesmeTur3").hide();
    $("#divOgrenciSozlesmeTur4").hide();
    $("#divOgrenciSozlesmeTur5").hide();

    $("#divOgrenciSozlesmeYayinBilgi").show("fast");
    $("#divOgrenciSozlesmeYayinlar").hide();

    $("#divKiyafetler").hide();

    $("#divOdemeBilgi").show("fast");
    $("#divOdeme").hide();
    $("#divOdemeOkulDetayBilgi").hide();

    $("#divOgrenciSozlesmeYayinGenel").hide();
    $("#divYayinTutar").hide();

    $("#divOdemePlaniBilgi").show();
    $("#divOdemePlani").hide();

    yayin();
}

var counter = 0;

function ogrenciSozlesmeTur(selectedSinifId) {
    var ogrenciSozlesmeTurId = $("#Model_OgrenciSozlesmeTurId").val();

    if (counter > 0) {
        setUnSelected();
        counter++;
    }

    $('div[id^="divOgrenciSozlesmeTur"]').hide();

    $("#divOgrenciSozlesmeTur" + ogrenciSozlesmeTurId).show("fast");

    // Kurs
    if (ogrenciSozlesmeTurId === "0") {
        setUnSelected();
    }
    else if (ogrenciSozlesmeTurId === "1") {
        $("#divSinifSeviye").show("fast");
        $("#divBrans").show("fast");
        $("#divSinif").show("fast");

        $("#divOgrenciSozlesmeYayinGenel").show("fast");
        $("#divYayinTutar").show("fast");
    }
    // Okul
    else if (ogrenciSozlesmeTurId === "2") {
        $("#divSinifSeviye").show("fast");
        $("#divBrans").show("fast");
        $("#divSinif").show("fast");

        $("#divOgrenciSozlesmeYayinGenel").show("fast");
        $("#divYayinTutar").show("fast");

        $("#divOdemeOkulDetayBilgi").show("fast");
        $("#divKiyafetler").show("fast");
    }
    // Özel Ders
    else if (ogrenciSozlesmeTurId === "3") {
        $("#divSinifSeviye").show("fast");
        $("#divYayinTutar").hide();
    }
    // Etkinlik
    else if (ogrenciSozlesmeTurId === "4") {
        $("#divSinifSeviye").show("fast");
        $("#divBrans").show("fast");
        $("#divSinif").show("fast");

        $("#divOgrenciSozlesmeYayinGenel").show("fast");
        $("#divYayinTutar").show("fast");
    }
    // Sürücü Kursu
    else if (ogrenciSozlesmeTurId === "5") {
        $("#divSinif").show("fast");

        $("#divOgrenciSozlesmeYayinGenel").show("fast");
        $("#divYayinTutar").show("fast");
    }

    if (ogrenciSozlesmeTurId !== "2") {
        $("#divOdemeOkulDetayBilgi").hide();
        $("#divKiyafetler").hide();
    }

    /* Ödeme Göster Gizle*/
    if (ogrenciSozlesmeTurId !== "0") {
        $("#divOdemeBilgi").hide();
        $("#divOdeme").show("fast");
    }
    else {
        $("#divOdemeBilgi").show();
        $("#divOdeme").hide();
    }

    Siniflar(
        $("#Model_SinifId"),
        $("#Model_SubeId"),
        $("#Model_SezonId"),
        $("#Model_BransId"),
        selectedSinifId);

    yayin();

    taksitHesapla();
}

function taksitHesapla() {

    var toplamUcret = parseFloat($("#Model_ToplamUcret").val());
    var pesinatTutar = parseFloat($("#Model_OgrenciSozlesmeOdemeBilgi_PesinatTutar").val());

    var taksitAdet = $("#Model_OgrenciSozlesmeOdemeBilgi_TaksitAdet").val() === ""
        ? 0
        : parseInt($("#Model_OgrenciSozlesmeOdemeBilgi_TaksitAdet").val());

    var maksimumTaksitAdet = ayarlar.MaksimumTaksitAdeti;

    if (taksitAdet > maksimumTaksitAdet) {
        $("#Model_OgrenciSozlesmeOdemeBilgi_TaksitAdet").val(maksimumTaksitAdet);
        taksitAdet = maksimumTaksitAdet;
    }

    var taksitTutar = $("#Model_OgrenciSozlesmeOdemeBilgi_TaksitTutar").val() === ""
        ? 0
        : parseFloat($("#Model_OgrenciSozlesmeOdemeBilgi_TaksitTutar").val());

    //$("#lblToplamTaksitTutari").text(taksitTutar > 0 ? taksitTutar : "");

    var taksitBaslangicTarihi = $("#Model_OgrenciSozlesmeOdemeBilgi_TaksitBaslangicTarihi").val();

    var hesapla = toplamUcret > 0 && pesinatTutar >= 0 && taksitAdet >= 0 && taksitTutar >= 0;

    if (taksitAdet === 0 && taksitTutar === 0) {
        $("#divOdemePlani").hide();
    }

    if (hesapla) {

        var dateNow = new Date();

        var d = dateNow.getDate();
        var m = dateNow.getMonth() + 1;
        var y = dateNow.getFullYear();

        var formattedDate = d + "." + m + "." + y;

        var tarih = taksitBaslangicTarihi == null || taksitBaslangicTarihi === ""
            ? formattedDate
            : taksitBaslangicTarihi;

        var day = parseInt(tarih.split(".")[0]);

        var month = parseInt(tarih.split(".")[1]);
        month = $("#Model_OgrenciSozlesmeOdemeBilgi_TaksitBaslangicTarihi").val() !== ""
            ? month - 1
            : month;

        var year = parseInt(tarih.split(".")[2]);

        if (day < d && month < m && y < year) {
            $("#Model_OgrenciSozlesmeOdemeBilgi_TaksitBaslangicTarihi").val(formattedDate);
            tarih = formattedDate;
        }

        var bolunecekTutar = toplamUcret - pesinatTutar;

        var bolumdenKalanTutar = 0;

        if (taksitTutar > 0) {

            taksitAdet = parseInt(bolunecekTutar / taksitTutar);
        }
        else {
            taksitTutar = parseInt(bolunecekTutar / taksitAdet);
        }

        if (bolunecekTutar - (taksitAdet * taksitTutar) > taksitTutar) {
            taksitAdet++;
        }

        if (taksitAdet > maksimumTaksitAdet) {
            taksitAdet = maksimumTaksitAdet;
        }

        bolumdenKalanTutar = bolunecekTutar - (taksitAdet * taksitTutar);

        var newTaksitTutar = (bolunecekTutar - bolumdenKalanTutar) / taksitAdet;

        $("#lblTaksitAdet").text(taksitAdet);

        for (var i = 1; i < maksimumTaksitAdet + 2; i++) {

            if (i < taksitAdet + 1) {

                $("#divOdemeTaksitDetaylar").show();

                $('#tblTaksitler tr').eq(i).show();

                // ay 12'den büyükse, başa dönsün
                var newMonth = (month + i) % 12;

                newMonth = newMonth === 0 ? 12 : newMonth;

                var newYear = parseInt(((month + i - 1) / 12)) + year;

                var mothLastDay = new Date(newYear, newMonth, 0).getDate();

                var newDay = day > mothLastDay
                    ? mothLastDay
                    : day;

                var newFormattedDate = ((newDay < 10 ? "0" : "") + newDay) + "." + ((newMonth < 10 ? "0" : "") + newMonth) + "." + newYear;

                $("#Model_OgrenciSozlesmeOdemeBilgi_AylikTaksitBilgiler_" + (i - 1) + "__VadeTarihi").val(newFormattedDate);
                $("#Model_OgrenciSozlesmeOdemeBilgi_AylikTaksitBilgiler_" + (i - 1) + "__TaksitTutari").val(newTaksitTutar);

                // Bu sonuncusu, kalan tutar buraya eklenecek.
                if (i === taksitAdet) {
                    $("#Model_OgrenciSozlesmeOdemeBilgi_AylikTaksitBilgiler_" + (i - 1) + "__TaksitTutari").val(newTaksitTutar + bolumdenKalanTutar);
                    $("#lblSonTaksitTarihi").text(newFormattedDate);
                    $("#lblSonTaksitTutar").text(newTaksitTutar + bolumdenKalanTutar);
                }

                if (i === 1) {
                    $("#lblIlkTaksitTarihi").text(newFormattedDate);
                }
            }
            else {
                $("#tblTaksitler tr").eq(i).hide();

                $("#Model_OgrenciSozlesmeOdemeBilgi_AylikTaksitBilgiler_" + (i - 1) + "__VadeTarihi").val("");
                $("#Model_OgrenciSozlesmeOdemeBilgi_AylikTaksitBilgiler_" + (i - 1) + "__TaksitTutari").val("");

                if (i === 1) {
                    $("#divOdemeTaksitDetaylar").hide();
                }

            }
        }

        $("#divOdemePlani").show();
    }
    else {
        $("#divOdemePlani").hide();
        $(".taksitTutar").val("");
        $(".taksitTarih").val("");
    }
}

function toplamTutar() {

    var egitimTutar = $("#Model_EgitimTutar").val().length > 0
        ? parseFloat($("#Model_EgitimTutar").val())
        : 0;

    var yemekTutar = $("#Model_YemekTutar").val().length > 0
        ? parseFloat($("#Model_YemekTutar").val())
        : 0;

    var servisTutar = $("#Model_ServisTutar").val().length > 0
        ? parseFloat($("#Model_ServisTutar").val())
        : 0;

    var kiyafetTutar = $("#Model_KiyafetTutar").val().length > 0
        ? parseFloat($("#Model_KiyafetTutar").val())
        : 0;

    var yayinTutar = $("#Model_YayinTutar").val().length > 0
        ? parseFloat($("#Model_YayinTutar").val())
        : 0;

    var toplamTutarSon = egitimTutar + yemekTutar + servisTutar + kiyafetTutar + yayinTutar;

    $("#Model_ToplamUcret").val(toplamTutarSon);

    var pesinatTutar = $("#Model_OgrenciSozlesmeOdemeBilgi_PesinatTutar").val().length > 0
        ? parseFloat($("#Model_OgrenciSozlesmeOdemeBilgi_PesinatTutar").val())
        : 0;

    if (pesinatTutar > toplamTutarSon) {
        $("#Model_OgrenciSozlesmeOdemeBilgi_PesinatTutar").val(toplamTutarSon);
    }
}

function OgrenciSozlesmeDuzenle(
    subeDetaylar,
    ayarlar,
    selectedSehirId,
    selectedIlceId,
    selectedSinifId,
    selectedPesinatHesapId,
    selectedGorusenPersonelId,
    selectedKurumaGetirenId,
    danismanPersonelId,
    selectedServisId) {

    $(function () {

        ogrenciSozlesmeTur(selectedSinifId);

        UlkeSehirIlce(
            $("#Model_Ogrenci_UlkeId"),
            $("#Model_Ogrenci_SehirId"),
            selectedSehirId,
            $("#Model_Ogrenci_IlceId"),
            selectedIlceId);

        HesapSubeParaBirim(
            $("#Model_OgrenciSozlesmeOdemeBilgi_PesinatHesapId"),
            selectedPesinatHesapId,
            $("#Model_SubeId"),
            $("#Model_Sube_ParaBirimId"),
            subeDetaylar);

        SubePersonel(
            $("#Model_GorusenPersonelId"),
            selectedGorusenPersonelId,
            $("#Model_SubeId"),
            false);

        SubePersonel(
            $("#Model_KurumaGetirenPersonelId"),
            selectedKurumaGetirenId,
            $("#Model_SubeId"),
            false);

        SubePersonel(
            $("#Model_DanismanPersonelId"),
            danismanPersonelId,
            $("#Model_SubeId"),
            true);

        SubeServis(
            $("#Model_ServisId"),
            selectedServisId,
            $("#Model_SubeId"));

        /* Sözleşme türe göre OgrenciSozlesmeDetay gösteriliyor */
        $("#Model_OgrenciSozlesmeTurId").on('change', function () {
            ogrenciSozlesmeTur();
        });
        /* Sözleşme türe göre OgrenciSozlesmeDetay gösteriliyor */

        /* Şubeye göre taksit adet, peşinat oran */
        var minimumPesinatTutari = $("#spnMinimumPesinatTutari");

        var maksimumTaksitAdeti = $("#spnMaksimumTaksitAdeti");

        var minimumPesinatTutariDefault = ayarlar.MinimumPesinatOrani;
        var maksimumTaksitAdetiDefault = ayarlar.MaksimumTaksitAdeti;

        if (minimumPesinatTutariDefault == null || isNaN(minimumPesinatTutariDefault) || minimumPesinatTutariDefault === "" || parseInt(minimumPesinatTutariDefault) === 0) {
            $("#spnMinimumPesinatTutari").closest(".row").hide();
        }
        else {
            $("#spnMinimumPesinatTutari").closest(".row").show();
        }

        if (maksimumTaksitAdetiDefault == null || isNaN(maksimumTaksitAdetiDefault) || maksimumTaksitAdetiDefault === "" || parseInt(maksimumTaksitAdetiDefault) === 0) {
            $("#spnMaksimumTaksitAdeti").closest(".row").hide();
        }
        else {
            $("#spnMaksimumTaksitAdeti").closest(".row").show();
        }

        minimumPesinatTutari.text(minimumPesinatTutariDefault);
        maksimumTaksitAdeti.text(maksimumTaksitAdetiDefault);

        $("#Model_SubeId").on("change", function () {
            var subeId = $(this).val();

            if (subeId === 0) {
                $("#divOdeme").hide();
            }
            else {
                $("#divOdeme").show();
            }

            var sube = null;

            for (var i = 0; i < subeDetaylar.length; i++) {
                if (subeDetaylar[i].SubeId === subeId) {
                    sube = subeDetaylar[i];
                    break;
                }
            }

            if (sube == null) {

                if (minimumPesinatTutariDefault == null || isNaN(minimumPesinatTutariDefault) || minimumPesinatTutariDefault === "" || parseInt(minimumPesinatTutariDefault) === 0) {
                    $("#spnMinimumPesinatTutari").closest(".row").hide();
                }
                else {
                    $("#spnMinimumPesinatTutari").closest(".row").show();
                }

                if (maksimumTaksitAdetiDefault == null || isNaN(maksimumTaksitAdetiDefault) || maksimumTaksitAdetiDefault === "" || parseInt(maksimumTaksitAdetiDefault) === 0) {
                    $("#spnMaksimumTaksitAdeti").closest(".row").hide();
                }
                else {
                    $("#spnMaksimumTaksitAdeti").closest(".row").show();
                }

                minimumPesinatTutari.text(minimumPesinatTutariDefault);
                maksimumTaksitAdeti.text(maksimumTaksitAdetiDefault);
            }
            else {

                if (sube.MinimumPesinatOrani == null || isNaN(sube.MinimumPesinatOrani) || sube.MinimumPesinatOrani === "" || sube.MinimumPesinatOrani === 0) {
                    $("#spnMinimumPesinatTutari").closest(".row").hide();
                }
                else {
                    $("#spnMinimumPesinatTutari").closest(".row").show();
                }

                if (sube.MaksimumTaksitAdeti == null || isNaN(sube.MaksimumTaksitAdeti) || sube.MaksimumTaksitAdeti === "" || sube.MaksimumTaksitAdeti === 0) {
                    $("#spnMaksimumTaksitAdeti").closest(".row").hide();
                }
                else {
                    $("#spnMaksimumTaksitAdeti").closest(".row").show();
                }

                minimumPesinatTutari.text("%" + sube.MinimumPesinatOrani);
                maksimumTaksitAdeti.text(sube.MaksimumTaksitAdeti);
            }
        });
        /* Şubeye göre taksit adet, peşinat oran */

        $("#Model_SinifSeviyeId").on("change", function () {
            yayin();
        });

        $("#Model_BransId").on("change", function () {
            yayin();
        });

        // Yayın teslim edildi seçilirse, yayın da seçilmiş oluyor.
        $("input[name$='TeslimEdildiMi']").on("ifChecked", function (event) {
            SetUniform("#" + event.target.id.replace("TeslimEdildiMi", "SecildiMi"), true);
        });

        taksitHesapla();

        jQuery("#Model_ToplamUcret").on("input", function () {
            taksitHesapla();
        });

        jQuery("#Model_OgrenciSozlesmeOdemeBilgi_PesinatTutar").on("input", function () {
            taksitHesapla();
        });

        jQuery("#Model_OgrenciSozlesmeOdemeBilgi_TaksitAdet").on("input", function () {
            taksitHesapla();
        });

        jQuery("#Model_OgrenciSozlesmeOdemeBilgi_TaksitTutar").on("input", function () {
            taksitHesapla();
        });

        DatePickerOnchange($("#Model_OgrenciSozlesmeOdemeBilgi_TaksitBaslangicTarihi"), taksitHesapla);
        /* Ödeme Planı taksit hesaplama */
    });
}

$(function () {
    $(".toplamHesap").keyup(function () {
        toplamTutar();
    });
});