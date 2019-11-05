function HesapHareketDuzenle(
    selectedParaBirimId,
    selectedBorcluHesapId,
    selectedBorcluAltHesapId,
    selectedAlacakliHesapId,
    selectedAlacakliAltHesapId,
    personelListele) {

    $(function () {

        Simdi($("#Model_HareketTarihi"));

        $("#Model_ParaBirimId").val(selectedParaBirimId).trigger("change");

        ParaBirimHesap(
            $("#AlacakliHesapId"),
            selectedAlacakliHesapId,
            3,
            $("#Model_ParaBirimId"));

        ParaBirimBagliHesap(
            $("#AlacakliAltHesapId"),
            selectedAlacakliAltHesapId,
            3,
            $("#Model_ParaBirimId"),
            $("#AlacakliHesapId"));

        ParaBirimHesap(
            $("#BorcluHesapId"),
            selectedBorcluHesapId,
            3,
            $("#Model_ParaBirimId"));

        if (personelListele) {
            SubePersonel(
                $("#BorcluAltHesapId"),
                null,
                $("#AlacakliHesapId"),
                false);
        }
        else {
            ParaBirimBagliHesap(
                $("#BorcluAltHesapId"),
                selectedAlacakliAltHesapId,
                2,
                $("#Model_ParaBirimId"),
                $("#AlacakliHesapId"));
        }
    });
}