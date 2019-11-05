$(function () {
    var div2 = $('#divFilters').detach();
    $('.datatable-header').append(div2);
});

function SetDataTable(IsServerSide, TableName, DataUrl, SortIndex, SortType, Columns, EditUrl, Id, OtherItems, NotAddUpdateItem, Paging, PageLength) {

    var _componentDataTableAddTitle = function () {
        $.each(Columns, function (index, value) {
            $('#' + TableName + '>thead>tr').append('<th>' + value.Name + '</th>');
        });

        if (OtherItems != null || EditUrl != null && EditUrl != "") {
            $('#' + TableName + '>thead>tr').append('<th>İşlemler</th>');
        }
    }

    var _componentDatatableAdvanced = function () {

        // Setting datatable defaults
        $.extend($.fn.dataTable.defaults, {
            responsive: true,
            autoWidth: false,
            dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
            language: {
                sDecimal: ',',
                sEmptyTable: 'Kayıt bulunamadı',
                sInfo: '_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor',
                sInfoEmpty: 'Kayıt yok',
                sInfoFiltered: '(_MAX_ kayıt içerisinden bulunan)',
                sInfoPostFix: '',
                sInfoThousands: '.',
                lengthMenu: '<span>Kayıt Sayısı:</span> _MENU_',
                sLoadingRecords: 'Yükleniyor...',
                sProcessing: 'İşleniyor...',
                search: '_INPUT_',
                searchPlaceholder: IsServerSide ? 'Filtrele (Min 3)' : 'Filtrele',
                sZeroRecords: 'Kayıt bulunamadı',
                oPaginate: {
                    sFirst: 'İlk',
                    sLast: 'Son',
                    sNext: '&rarr;',
                    sPrevious: '&larr;'
                },
                oAria: {
                    sSortAscending: ': artan sütun sıralamasını aktifleştir',
                    sSortDescending: ': azalan sütun sıralamasını aktifleştir'
                },
                select: {
                    rows: {
                        '_': '%d kayıt seçildi',
                        '0': '',
                        '1': '1 kayıt seçildi'
                    }
                }
            }
        });

        // DOM positioning
        $('.datatable-dom-position').DataTable({
            dom: '<"datatable-header length-left"lp><"datatable-scroll"t><"datatable-footer info-right"fi>',
        });

        var columnParamters = [];
        var columnDefinitions = [];

        var columnDefsLeft = [{ className: "text-left", targets: [] }, { className: "text-left", targets: [] }];
        var columnDefsCenter = [{ className: "text-center", targets: [] }, { className: "text-center", targets: [] }];
        var columnDefsRight = [{ className: "text-right", targets: [] }, { className: "text-right", targets: [] }];
        var columnDefsJustify = [{ className: "text-justify", targets: [] }, { className: "text-justify", targets: [] }];

        $.each(Columns, function (index, value) {
            if (Columns[index].DataColumnBoolean != null) {
                columnParamters[columnParamters.length] = {
                    render: function (data, type, row) {

                        var innerHtml = '<span class="' + (row[value.DataColumnBoolean.BoolStatus] ? value.DataColumnBoolean.HtmlTrueClass : value.DataColumnBoolean.HtmlFalseClass) + '">' + row[value.DataColumnBoolean.Description] + '</span>';

                        return innerHtml;
                    }
                };
            }
            else if (Columns[index].DataColumnImage != null) {
                columnParamters[columnParamters.length] = {
                    render: function (data, type, row) {

                        var innerHtml = '<img src="' + row[value.DataColumnImage.ImgSource] + '" width="' + value.DataColumnImage.Width + '" height="' + value.DataColumnImage.Height + '" />';

                        return innerHtml;
                    }
                };
            }
            else if (Columns[index].DataColumnHtmlFuncs != null) {
                columnParamters[columnParamters.length] = {
                    render: function (data, type, row) {

                        var innerHtml = "";

                        if (value.DataColumnHtmlFuncs.Bold) {
                            innerHtml = '<strong>' + row[value.DataColumnHtmlFuncs.Value] + '</span>';
                        }
                        else {
                            innerHtml = '<span>' + row[value.DataColumnHtmlFuncs.Value] + '</span>';
                        }

                        return innerHtml;
                    }
                };
            }
            else {
                if (value.Width > 0)
                    columnParamters[index] = { data: value.DataName, autoWidth: value.AutoWidth, width: value.Width + '%' };
                else
                    columnParamters[index] = { data: value.DataName, autoWidth: true };
            }

            if (Columns[index].HeaderTextAlign != null) {

                switch (Columns[index].HeaderTextAlign) {
                    case "Left":
                        {
                            columnDefsLeft[1].targets[columnDefsLeft[1].targets.length] = index;
                            break;
                        }
                    case "Center":
                        {
                            columnDefsCenter[1].targets[columnDefsCenter[1].targets.length] = index;
                            break;
                        }
                    case "Right":
                        {
                            columnDefsRight[1].targets[columnDefsRight[1].targets.length] = index;
                            break;
                        }
                    case "Justify":
                        {
                            columnDefsJustify[1].targets[columnDefsJustify[1].targets.length] = index;
                            break;
                        }
                }

            }

            if (Columns[index].BodyTextAlign != null) {


                switch (Columns[index].BodyTextAlign) {
                    case "Left":
                        {
                            columnDefsLeft[0].targets[columnDefsLeft[0].targets.length] = index;
                            break;
                        }
                    case "Center":
                        {
                            columnDefsCenter[0].targets[columnDefsCenter[0].targets.length] = index;
                            break;
                        }
                    case "Right":
                        {
                            columnDefsRight[0].targets[columnDefsRight[0].targets.length] = index;
                            break;
                        }
                    case "Justify":
                        {
                            columnDefsJustify[0].targets[columnDefsJustify[0].targets.length] = index;
                            break;
                        }
                }

            }
        });

        if (columnDefsLeft[0].targets.length > 0) {
            columnDefinitions[columnDefinitions.length] = columnDefsLeft[0];
        }

        if (columnDefsLeft[1].targets.length > 0) {
            columnDefinitions[columnDefinitions.length] = columnDefsLeft[1];
        }

        if (columnDefsCenter[0].targets.length > 0) {
            columnDefinitions[columnDefinitions.length] = columnDefsCenter[0];
        }

        if (columnDefsCenter[1].targets.length > 0) {
            columnDefinitions[columnDefinitions.length] = columnDefsCenter[1];
        }

        if (columnDefsRight[0].targets.length > 0) {
            columnDefinitions[columnDefinitions.length] = columnDefsRight[0];
        }

        if (columnDefsRight[1].targets.length > 0) {
            columnDefinitions[columnDefinitions.length] = columnDefsRight[1];
        }

        if (columnDefsJustify[0].targets.length > 0) {
            columnDefinitions[columnDefinitions.length] = columnDefsJustify[0];
        }

        if (columnDefsJustify[1].targets.length > 0) {
            columnDefinitions[columnDefinitions.length] = columnDefsJustify[1];
        }

        if (!NotAddUpdateItem || (EditUrl != null && EditUrl != "")) {

            columnParamters[columnParamters.length] = {
                data: 'İşlemler',
                autoWidth: false,
                width: '5%',
                orderable: false,
                render: function (data, type, row) {

                    var otherItemsHtml = "";

                    if (OtherItems != null && OtherItems.length > 0) {

                        $.each(OtherItems, function (index, value) {

                            value.IdValue = row[value.Id];

                            OtherItemHtml(value);

                            otherItemsHtml += value.Html;
                        });
                    }

                    var innerHtml = '<div class="text-center">' +
                        '<div class="list-icons">' +
                        '<div class="dropdown">' +
                        '<a href="#" class="list-icons-item" data-toggle="dropdown">' +
                        '<i class="icon-menu9"></i>' +
                        '</a>' +
                        '<div class="dropdown-menu dropdown-menu-right">';

                    if (NotAddUpdateItem == null || !NotAddUpdateItem && EditUrl != '') {
                        innerHtml = innerHtml + '<a href="' + EditUrl + row[Id] + '" target="_blank" class="dropdown-item"><i class="icon-cog3"></i>Düzenle</a>';
                    }

                    innerHtml = innerHtml + otherItemsHtml;

                    innerHtml = innerHtml + '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>';

                    return innerHtml;
                }
            };
        }

        var pageLen = PageLength == null ? 10 : PageLength;

        var lastIdx = null;
        var table = $('#' + TableName).DataTable({
            paging: Paging == null || Paging == true,
            searching: !IsServerSide,
            pageLength: pageLen,
            ajax: {
                url: DataUrl,
                dataSrc: "data",
                type: "POST",
                datatype: "json"
            },
            columns: columnParamters,
            columnDefs: columnDefinitions,
            serverSide: IsServerSide,
            order: [SortIndex, SortType],
            processing: true,
            //stateSave : true,
            language: {
                processing: 'Yükleniyor... Lütfen Bekleyin'
            }
        });

        if (IsServerSide) {
            $('#' + TableName + '_filter input').unbind();
            $('#' + TableName + '_filter input').bind('keyup', function (e) {
                if (($(this).val().length >= 3 || $(this).val() == "") && e.keyCode == 13) {
                    table.search(this.value).draw();
                }
            });
        }

        $('.datatable-highlight tbody').on('mouseover', 'td', function () {
            var colIdx = table.cell(this).index().column;

            if (colIdx !== lastIdx) {
                $(table.cells().nodes()).removeClass('active');
                $(table.column(colIdx).nodes()).addClass('active');
            }
        }).on('mouseleave', function () {
            $(table.cells().nodes()).removeClass('active');
        });

        $("#btnFiltrele").on("click", function () {

            var url = DataUrl + "?";

            var selects = $("select.search-filter");

            selects.each(function (index, value) {

                var element = $(this);

                url += element.attr('id') + "=" + element.val() + "&";
            });

            var inputTexts = $("input.search-filter[type=text]");

            inputTexts.each(function (index, value) {

                var element = $(this);

                url += element.attr("id") + "=" + element.val() + "&";
            });

            var inputCheckBoxes = $("input.search-filter[type=checkBox]");

            inputCheckBoxes.each(function (index, value) {

                var element = $(this);

                if (element.is(':checked')) {
                    url += element.attr("id") + "=true&";
                }
            });

            url = url.substring(0, url.length - 1);

            table.ajax.url(url);
            table.draw();
        });

        $("#btnTemizle").on("click", function () {

            var selects = $("select.search-filter");

            selects.each(function (index, value) {

                SetSelectedValue($(this), "0");

            });

            var inputs = $("input.search-filter");

            inputs.each(function (index, value) {

                $(this).val("");
            });

            $('.icheck').iCheck('uncheck');

            //table.ajax.url(DataUrl);
            //table.draw();

            $("#btnFiltrele").click();
        });
    };

    document.addEventListener('DOMContentLoaded', function () {
        _componentDataTableAddTitle();
        _componentDatatableAdvanced();
    });
};

function FilterItem() {
    item, paramName
    this.Item = item;
    this.ParamName = paramName;
}

function DataColumnHtmlFuncs(value, bold) {
    this.Value = value;
    this.Bold = bold;
}

function DataColumnBoolean(boolStatus, htmlFalseClass, htmlTrueClass, description) {
    this.BoolStatus = boolStatus;
    this.HtmlFalseClass = htmlFalseClass;
    this.HtmlTrueClass = htmlTrueClass;
    this.Description = description;
}

function DataColumnImage(imgSource, height, width) {
    this.ImgSource = imgSource;
    this.Height = height;
    this.Width = width;
}

function DataColumn(name, dataName, autoWidth, width, headerTextAlign, bodyTextAlign, dataColumnHtmlFuncs, dataColumnImage, dataColumnBoolean) {
    this.Name = name;
    this.DataName = dataName;
    this.AutoWidth = autoWidth;
    this.Width = width;
    this.HeaderTextAlign = headerTextAlign;
    this.BodyTextAlign = bodyTextAlign;
    this.DataColumnHtmlFuncs = dataColumnHtmlFuncs;
    this.DataColumnImage = dataColumnImage;
    this.DataColumnBoolean = dataColumnBoolean;
}

function OtherItem(url, functionParam, id, idValue, target, icon, text, html) {
    this.Url = url;
    this.Function = functionParam;
    this.Id = id;
    this.IdValue = idValue;
    this.Target = target;
    this.Icon = icon;
    this.Text = text;
    this.Html = html;
}

function OtherItemHtml(otherItem) {
    if (otherItem.Function == "") {
        otherItem.Html = '<a href="' + otherItem.Url + otherItem.IdValue + '" ' + (otherItem.Target != '' ? 'target="_blank"' : '') + ' class="dropdown-item"><i class="' + otherItem.Icon + '"></i> ' + otherItem.Text + '</a>';
    }
    else {
        otherItem.Html = '<a href="#" onclick="' + otherItem.Function + '(' + otherItem.IdValue + ');" class="dropdown-item"><i class="' + otherItem.Icon + '"></i> ' + otherItem.Text + '</a>';
    }
}

var htmlFalseClass = "badge badge-danger";
var htmlTrueClass = "badge badge-success";

var AlignEnum = {
    Left: "Left",
    Center: "Center",
    Right: "Right",
    Justify: "Justify"
};

//var otherItems = new Array();
//otherItems.push(new OtherItem('Deneme/Url/', '', 'KurumId', '', 'Blank', 'icon-quotes-left', 'Deneme İçerik', ''));
//otherItems.push(new OtherItem('Deneme/Url/Denesen/', '', 'KurumId', '', '', 'icon-enlarge7', 'Deneme İçerik Deneme Yine', ''));
//otherItems.push(new OtherItem('', 'Alertimiz', 'KurumId', '', '', 'icon-spinner2', 'Alert bu alert', ''));

//function Alertimiz(id) { alert(id); return; }


// { className: "dt-center", targets: [0, 1, 2] }