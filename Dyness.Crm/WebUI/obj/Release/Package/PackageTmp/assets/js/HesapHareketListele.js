$(window).on("load", function () {
    $('#Zaman').on('change', function () {
        var data = $("#Zaman").select2("val");

        if (data == 1) {
            var d = new Date();

            var month = d.getMonth() + 1;
            var day = d.getDate();
            var yil = d.getFullYear();

            $("#IlkTarih").val((day < 10 ? '0' + day : day) + "." + (month < 10 ? '0' + month : month) + "." + yil);
            $("#SonTarih").val((day < 10 ? '0' + day : day) + "." + (month < 10 ? '0' + month : month) + "." + yil);
        }
        else if (data == 2) {
            var curr = new Date;
            var first = curr.getDate() - curr.getDay() + 1;
            var last = first + 6;

            var firstday = new Date(curr.setDate(first));
            var lastday = new Date(curr.setDate(last));

            var ilkmonth = firstday.getMonth() + 1;
            var ilkday = firstday.getDate();
            var ilkyil = firstday.getFullYear();

            var sonmonth = lastday.getMonth() + 1;
            var sonday = lastday.getDate();
            var sonyil = lastday.getFullYear();


            $("#IlkTarih").val((ilkday < 10 ? '0' + ilkday : ilkday) + "." + (ilkmonth < 10 ? '0' + ilkmonth : ilkmonth) + "." + ilkyil);
            $("#SonTarih").val((sonday < 10 ? '0' + sonday : sonday) + "." + (sonmonth < 10 ? '0' + sonmonth : sonmonth) + "." + sonyil);
        }
        else if (data == 3) {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

            var ilkmonth = firstDay.getMonth() + 1;
            var ilkday = firstDay.getDate();
            var ilkyil = firstDay.getFullYear();

            var sonmonth = lastDay.getMonth() + 1;
            var sonday = lastDay.getDate();
            var sonyil = lastDay.getFullYear();

            $("#IlkTarih").val((ilkday < 10 ? '0' + ilkday : ilkday) + "." + (ilkmonth < 10 ? '0' + ilkmonth : ilkmonth) + "." + ilkyil);
            $("#SonTarih").val((sonday < 10 ? '0' + sonday : sonday) + "." + (sonmonth < 10 ? '0' + sonmonth : sonmonth) + "." + sonyil);
        }
        else {
            $("#IlkTarih").val("");
            $("#SonTarih").val("");
        }

        $("#btnFiltrele").click();
    });
});