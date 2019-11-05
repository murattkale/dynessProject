function TakistOdemeAc() {
    $("#modalOdemeListele").modal("show");

    Simdi($("#ModelHesapHareket_HareketTarihi"));
}

function FaturaBilgiGuncellePopupAc(faturaBilgiId, sozlesmeId, ogrenciId) {

    $.ajax({
        url: "/Ogrenci/FaturaBilgiGetir/" + faturaBilgiId,
        type: 'POST',
        dataType: 'html',
        success: function (data) {
            $("#divFaturaBilgiAlan").html(data);
            $("#FaturaBilgiOgrenciSozlesmeId").val(sozlesmeId);
            $("#FaturaBilgiOgrenciId").val(ogrenciId);
        },
        error: function () {

        }
    }).done(function () {
        $('#modalFaturaBilgiGuncellePopup').modal('show');
    });
}

function SilPopupAc(id) {
    $("#OgrenciSozlesmeSilModel_OgrenciSozlesmeId").val(id);
    $("#modalOgrenciSozlesmeDelete").modal("show");
}

function TakistOdemeHesapHareketAc(hesapHareketId) {
    $("#modalOdemeListele").modal('show');
    Simdi($("#ModelHesapHareket_HareketTarihi"));

    $("#ModelHesapHareket_HesapHareketId").val(hesapHareketId).trigger("change");;
}

$(document).ready(function () {
    $("#ModelHesapHareket_HesapHareketId").on('change', function () {

        var id = $("#ModelHesapHareket_HesapHareketId").val();

        if (id !== "-100") {

            $("#TaksitToplamTutar").val("");

            $.ajax({
                url: "/HesapHareket/HesapHareketGetir/" + id,
                type: "POST",
                dataType: "json",
                success: function (data) {

                    var parameters;
                    if (data != null && data.HesapHareketId > 0) {

                        var dt = new Date(data.VadeTarihi);

                        var vadeTarihi = (dt.getDate() < 10 ? "0" : "") + dt.getDate() + "." + ((dt.getMonth() + 1) < 10 ? "0" : "") + (dt.getMonth() + 1) + "." + dt.getFullYear();
                        $("#ModelHesapHareket_VadeTarihi").val(vadeTarihi);

                        $("#ModelHesapHareket_Aciklama").val(data.Aciklama);

                        var subeId = data.SubeId;
                        var paraBirimId = data.ParaBirimId;

                        parameters = { UstHesapId: subeId, ParaBirimId: paraBirimId, HesapTurId: 4 };

                        FillMultiSelect(parameters, "/Hesap/HesapParaBirimIdHesapTurIdAltHesapListele", $("#ModelHesapHareket_BorcluHesapId"), 0);

                    }
                    else {
                        $("#ModelHesapHareket_Aciklama").val("");
                        $("#ModelHesapHareket_VadeTarihi").val("");

                        parameters = { UstHesapId: 0, ParaBirimId: 0, HesapTurId: 4 };

                        FillMultiSelect(parameters, "/Hesap/HesapParaBirimIdHesapTurIdAltHesapListele", $("#ModelHesapHareket_BorcluHesapId"), 0);
                    }
                },
                error: function () {
                    $("#ModelHesapHareket_Aciklama").val("");
                    $("#ModelHesapHareket_VadeTarihi").val("");
                }
            }).done(function () {

            });
        }
    });

    $("#TaksitToplamTutar").on('keyup', function () {

        var value = parseInt($("#TaksitToplamTutar").val());

        if (value > 0) {
            $("#ModelHesapHareket_HesapHareketId").val("-100").trigger("change");

            $("#ModelHesapHareket_HesapHareketId").attr("disabled", "disabled");

            var hesapHareketId = $('select[id=ModelHesapHareket_HesapHareketId] option:eq(1)').val();

            $.ajax({
                url: "/HesapHareket/HesapHareketGetir/" + hesapHareketId,
                type: 'POST',
                dataType: 'json',
                success: function (data) {

                    var parameters;

                    if (data != null && data.HesapHareketId > 0) {

                        var subeId = data.SubeId;
                        var paraBirimId = data.ParaBirimId;

                        parameters = { UstHesapId: subeId, ParaBirimId: paraBirimId, HesapTurId: 4 };

                        FillMultiSelect(parameters, "/Hesap/HesapParaBirimIdHesapTurIdAltHesapListele", $("#ModelHesapHareket_BorcluHesapId"), 0);

                    }
                    else {
                        $("#ModelHesapHareket_Aciklama").val("");
                        $("#ModelHesapHareket_VadeTarihi").val("");

                        parameters = { UstHesapId: 0, ParaBirimId: 0, HesapTurId: 4 };

                        FillMultiSelect(parameters, "/Hesap/HesapParaBirimIdHesapTurIdAltHesapListele", $("#ModelHesapHareket_BorcluHesapId"), 0);
                    }
                },
                error: function () {
                    $("#ModelHesapHareket_Aciklama").val("");
                    $("#ModelHesapHareket_VadeTarihi").val("");
                }
            }).done(function () {

            });
        }
        else {
            $("#ModelHesapHareket_HesapHareketId").val("").trigger("change");
            $("#ModelHesapHareket_HesapHareketId").removeAttr("disabled");
        }
    });
});

function ToplamHesapla() {

    var egitimTutar = parseFloat($("#EgitimTutar").val() == null || $("#EgitimTutar").val() === "" ? 0 : $("#EgitimTutar").val());
    var yayinTutar = parseFloat($("#YayinTutar").val() == null || $("#YayinTutar").val() === "" ? 0 : $("#YayinTutar").val());
    var servisTutar = parseFloat($("#ServisTutar").val() == null || $("#ServisTutar").val() === "" ? 0 : $("#ServisTutar").val());
    var kiyafetTutar = parseFloat($("#KiyafetTutar").val() == null || $("#KiyafetTutar").val() === "" ? 0 : $("#KiyafetTutar").val());

    var toplamTutar = egitimTutar + yayinTutar + servisTutar + kiyafetTutar;

    $("#ToplamTutar").val(toplamTutar);

    var odenenTutar = parseFloat($("#OdenenTutar").val() == null || $("#OdenenTutar").val() === "" ? 0 : $("#OdenenTutar").val());
    var kalanTutar = toplamTutar - odenenTutar;

    $("#KalanTutar").val(kalanTutar);
}

function OdenenTutarHesapla() {

    var odenenTutar;

    var toplamTutar = parseFloat($("#ToplamTutar").val() == null || $("#ToplamTutar").val() === "" ? 0 : $("#ToplamTutar").val());

    if (odemelerSilinsinMi) {
        odenenTutar = parseFloat($("#PesinatTutar").val() == null || $("#PesinatTutar").val() === "" ? 0 : $("#PesinatTutar").val());
    } else {
        odenenTutar = parseFloat($("#OdenenTutar").val() == null || $("#OdenenTutar").val() === "" ? 0 : $("#OdenenTutar").val());
    }

    var kalanTutar = toplamTutar - odenenTutar;

    $("#KalanTutar").val(kalanTutar);

    taksitHesapla(true);

    $("#lblToplamTutar").text(kalanTutar);
}

function taksitHesapla(ilk) {

    var toplamUcret = parseFloat($("#ToplamTutar").val());

    var odenenTutar = parseFloat($("#OdenenTutar").val() == null || $("#OdenenTutar").val() === "" ? 0 : $("#OdenenTutar").val());

    odenenTutar = odemelerSilinsinMi ? 0 : odenenTutar;

    toplamUcret = toplamUcret - odenenTutar;

    var pesinatTutar = parseFloat($("#PesinatTutar").val() == null || $("#PesinatTutar").val() === "" ? 0 : $("#PesinatTutar").val());

    var taksitAdet = $("#TaksitAdet").val() === ""
        ? 0
        : parseInt($("#TaksitAdet").val());

    var maksimumTaksitAdet = ayarlar.MaksimumTaksitAdeti;

    if (taksitAdet > maksimumTaksitAdet) {
        $("#TaksitAdet").val(maksimumTaksitAdet);
        taksitAdet = maksimumTaksitAdet;
    }

    var taksitTutar = $("#TaksitTutar").val() === ""
        ? 0
        : parseFloat($("#TaksitTutar").val());

    //$("#lblToplamTaksitTutari").text(taksitTutar > 0 ? taksitTutar : "");

    var taksitBaslangicTarihi = $("#TaksitBaslangicTarihi").val();

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
        month = $("#TaksitBaslangicTarihi").val() !== ""
            ? month - 1
            : month;

        var year = parseInt(tarih.split(".")[2]);

        if (day < d && month < m && y < year) {
            $("#TaksitBaslangicTarihi").val(formattedDate);
            tarih = formattedDate;
        }

        var bolunecekTutar = toplamUcret;

        if (odemelerSilinsinMi) {
            bolunecekTutar = toplamUcret - pesinatTutar;
        }

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

                if (!ilk) {
                    $("#AylikTaksitBilgiler_" + (i - 1) + "__VadeTarihi").val(newFormattedDate);
                    $("#AylikTaksitBilgiler_" + (i - 1) + "__TaksitTutari").val(newTaksitTutar);
                }

                // Bu sonuncusu, kalan tutar buraya eklenecek.
                if (i === taksitAdet && !ilk) {
                    $("#AylikTaksitBilgiler_" + (i - 1) + "__TaksitTutari").val(newTaksitTutar + bolumdenKalanTutar);
                    $("#lblSonTaksitTarihi").text(newFormattedDate);
                    $("#lblSonTaksitTutar").text(newTaksitTutar + bolumdenKalanTutar);
                }

                if (i === 1) {
                    $("#lblIlkTaksitTarihi").text(newFormattedDate);
                }
            }
            else {
                $("#tblTaksitler tr").eq(i).hide();

                $("#AylikTaksitBilgiler_" + (i - 1) + "__VadeTarihi").val("");
                $("#AylikTaksitBilgiler_" + (i - 1) + "__TaksitTutari").val("");

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

    var egitimTutar = $("#EgitimTutar").val().length > 0
        ? parseFloat($("#EgitimTutar").val())
        : 0;

    var yemekTutar = $("#YemekTutar").val().length > 0
        ? parseFloat($("#YemekTutar").val())
        : 0;

    var servisTutar = $("#ServisTutar").val().length > 0
        ? parseFloat($("#ServisTutar").val())
        : 0;

    var kiyafetTutar = $("#KiyafetTutar").val().length > 0
        ? parseFloat($("#KiyafetTutar").val())
        : 0;

    var yayinTutar = $("#YayinTutar").val().length > 0
        ? parseFloat($("#YayinTutar").val())
        : 0;

    var tutar = egitimTutar + yemekTutar + servisTutar + kiyafetTutar + yayinTutar;

    $("#ToplamTutar").val(tutar);

    var pesinatTutar = $("#PesinatTutar").val().length > 0
        ? parseFloat($("#PesinatTutar").val())
        : 0;

    if (pesinatTutar > tutar) {
        $("#PesinatTutar").val(tutar);
    }
}

var odemelerSilinsinMi = false;

function OdemeBilgiGuncellePopupAc(sozlesmeId) {

    $.ajax({
        url: "/Ogrenci/OgrenciSozlesmeOdemeBilgiGetir/" + sozlesmeId,
        type: "POST",
        dataType: "html",
        success: function (data) {
            $("#divOdemeBilgiAlan").html(data);
        },
        error: function () {

        }
    }).done(function () {
        $("#modalOdemeBilgiGuncellePopup").modal("show");

        $("#EgitimTutar").on("keyup",
            function () {
                ToplamHesapla();
                OdenenTutarHesapla();
                taksitHesapla(false);
            });

        $("#YayinTutar").on("keyup",
            function () {
                ToplamHesapla();
                OdenenTutarHesapla();
                taksitHesapla(false);
            });

        $("#ServisTutar").on("keyup",
            function () {
                ToplamHesapla();
                OdenenTutarHesapla();
                taksitHesapla(false);
            });

        $("#KiyafetTutar").on("keyup",
            function () {
                ToplamHesapla();
                OdenenTutarHesapla();
                taksitHesapla(false);
            });

        $("#PesinatTutar").on("keyup",
            function () {
                OdenenTutarHesapla();
                taksitHesapla(false);
            });

        $("#OdenenlerSilinsinMi").on("ifChanged", function (event) {

            odemelerSilinsinMi = $(this).is(":checked");

            $("#PesinatTutar").val("");

            if (!odemelerSilinsinMi) {
                $("#PesinatTutar").attr("disabled", "disabled");
            } else {
                $("#PesinatTutar").removeAttr("disabled");
            }

            OdenenTutarHesapla();

            taksitHesapla(false);
        });

        OdenenTutarHesapla();

        /* Şubeye göre taksit adet, peşinat oran */

        jQuery("#ToplamUcret").on("input", function () {
            taksitHesapla(false);
        });

        jQuery("#PesinatTutar").on("input", function () {
            taksitHesapla(false);
        });

        jQuery("#TaksitAdet").on("input", function () {
            taksitHesapla(false);
        });

        jQuery("#TaksitTutar").on("input", function () {
            taksitHesapla(false);
        });

        DatePickerOnchange($("#TaksitBaslangicTarihi"), taksitHesapla);
        /* Ödeme Planı taksit hesaplama */
    });
}

function OdemeDuzenle(subeDetaylar, ayarlar) {

    $(function () {

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

        var subeId = $("#SubeId").val();

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
}

$(document).ajaxComplete(function () {
    $(".toplamHesap").keyup(function () {
        toplamTutar();
    });
});

function SozlesmeGuncellePopupAc(sozlesmeId) {

    $.ajax({
        url: "/OgrenciSozlesme/OgrenciSozlesmeGetir/" + sozlesmeId,
        type: "POST",
        dataType: "html",
        success: function (data) {
            $("#divSozlesmeBilgiAlan").html(data);

            var sinifId = $('#SinifId', data).val();

            ogrenciSozlesmeTur(sinifId);
        },
        error: function () {

        }
    }).done(function () {
        $("#modalSozlesmeGuncelle").modal("show");
    });
}

function ogrenciSozlesmeTur(selectedSinifId) {

    var ogrenciSozlesmeTurId = $("#Model_OgrenciSozlesmeTurId").val();

    $('div[id^="divOgrenciSozlesmeTur"]').hide();

    $("#divOgrenciSozlesmeTur" + ogrenciSozlesmeTurId).show("fast");

    if (ogrenciSozlesmeTurId === "1") {
        $("#divSinifSeviye").show("fast");
        $("#divBrans").show("fast");
        $("#divSinif").show("fast");
    }
    // Okul
    else if (ogrenciSozlesmeTurId === "2") {
        $("#divSinifSeviye").show("fast");
        $("#divBrans").show("fast");
        $("#divSinif").show("fast");
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
    }
    // Sürücü Kursu
    else if (ogrenciSozlesmeTurId === "5") {
        $("#divSinif").show("fast");
    }

   Siniflar(
       $("#Model_SinifId"),
       $("#Model_SubeId"),
       $("#Model_SezonId"),
       $("#Model_BransId"),
       selectedSinifId);
}

$("#Model_SubeId").on('change', function () {
    Siniflar(
        $("#Model_SinifId"),
        $("#Model_SubeId"),
        $("#Model_SezonId"),
        $("#Model_BransId"),
        0);
});

$("#Model_SezonId").on('change', function () {
    Siniflar(
        $("#Model_SinifId"),
        $("#Model_SubeId"),
        $("#Model_SezonId"),
        $("#Model_BransId"),
        0);
});

$("#Model_BransId").on('change', function () {
    Siniflar(
        $("#Model_SinifId"),
        $("#Model_SubeId"),
        $("#Model_SezonId"),
        $("#Model_BransId"),
        0);
});