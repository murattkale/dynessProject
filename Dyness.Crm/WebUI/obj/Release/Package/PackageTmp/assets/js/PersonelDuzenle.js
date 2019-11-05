function PersonelDuzenle() {
    $(function () {
        $('#SelectedUcretAldigiSubeler').on('change', function (e) {

            $(".PersonelSubeUcretlendirme").hide("fast");

            var seciliDersler = [];
            $.each($("#SelectedUcretAldigiSubeler option:selected"), function () {
                seciliDersler.push($(this).val());
            });

            for (var i = 0; i < seciliDersler.length; i++) {
                $("#divPersonelSubeUcretlendirme" + seciliDersler[i]).show("slow");
            }
        });
    });
}