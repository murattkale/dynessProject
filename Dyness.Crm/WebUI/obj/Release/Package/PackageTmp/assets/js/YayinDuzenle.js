function YayinDuzenle(selectedDersId) {

    $(function () {

        BransDers(
            $("#Model_DersId"),
            selectedDersId,
            $("#Model_BransId"));
    });
}