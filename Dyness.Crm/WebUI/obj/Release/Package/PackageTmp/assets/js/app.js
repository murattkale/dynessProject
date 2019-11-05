/* ------------------------------------------------------------------------------
 *
 *  # Template JS core
 *
 *  Includes minimum required JS code for proper template functioning
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var App = function () {


    //
    // Setup module components
    //

    // Transitions
    // -------------------------

    // Disable all transitions
    var _transitionsDisabled = function () {
        $('body').addClass('no-transitions');
    };

    // Enable all transitions
    var _transitionsEnabled = function () {
        $('body').removeClass('no-transitions');
    };


    // Sidebars
    // -------------------------

    //
    // On desktop
    //

    // Resize main sidebar
    var _sidebarMainResize = function () {

        // Flip 2nd level if menu overflows
        // bottom edge of browser window
        var revertBottomMenus = function () {
            $('.sidebar-main').find('.nav-sidebar').children('.nav-item-submenu').hover(function () {
                var totalHeight = 0,
                    $this = $(this),
                    navSubmenuClass = 'nav-group-sub',
                    navSubmenuReversedClass = 'nav-item-submenu-reversed';

                totalHeight += $this.find('.' + navSubmenuClass).filter(':visible').outerHeight();
                if ($this.children('.' + navSubmenuClass).length) {
                    if (($this.children('.' + navSubmenuClass).offset().top + $this.find('.' + navSubmenuClass).filter(':visible').outerHeight()) > document.body.clientHeight) {
                        $this.addClass(navSubmenuReversedClass)
                    }
                    else {
                        $this.removeClass(navSubmenuReversedClass)
                    }
                }
            });
        }

        // If sidebar is resized by default
        if ($('body').hasClass('sidebar-xs')) {
            revertBottomMenus();
        }

        // Toggle min sidebar class
        $('.sidebar-main-toggle').on('click', function (e) {
            e.preventDefault();

            $('body').toggleClass('sidebar-xs').removeClass('sidebar-mobile-main');
            revertBottomMenus();
        });
    };

    // Toggle main sidebar
    var _sidebarMainToggle = function () {
        $(document).on('click', '.sidebar-main-hide', function (e) {
            e.preventDefault();
            $('body').toggleClass('sidebar-main-hidden');
        });
    };

    // Toggle secondary sidebar
    var _sidebarSecondaryToggle = function () {
        $(document).on('click', '.sidebar-secondary-toggle', function (e) {
            e.preventDefault();
            $('body').toggleClass('sidebar-secondary-hidden');
        });
    };


    // Toggle content sidebar
    var _sidebarComponentToggle = function () {
        $(document).on('click', '.sidebar-component-toggle', function (e) {
            e.preventDefault();
            $('body').toggleClass('sidebar-component-hidden');
        });
    };


    //
    // On mobile
    //

    // Expand sidebar to full screen on mobile
    var _sidebarMobileFullscreen = function () {
        $('.sidebar-mobile-expand').on('click', function (e) {
            e.preventDefault();
            var $sidebar = $(this).parents('.sidebar'),
                sidebarFullscreenClass = 'sidebar-fullscreen'

            if (!$sidebar.hasClass(sidebarFullscreenClass)) {
                $sidebar.addClass(sidebarFullscreenClass);
            }
            else {
                $sidebar.removeClass(sidebarFullscreenClass);
            }
        });
    };

    // Toggle main sidebar on mobile
    var _sidebarMobileMainToggle = function () {
        $('.sidebar-mobile-main-toggle').on('click', function (e) {
            e.preventDefault();
            $('body').toggleClass('sidebar-mobile-main').removeClass('sidebar-mobile-secondary sidebar-mobile-right');

            if ($('.sidebar-main').hasClass('sidebar-fullscreen')) {
                $('.sidebar-main').removeClass('sidebar-fullscreen');
            }
        });
    };

    // Toggle secondary sidebar on mobile
    var _sidebarMobileSecondaryToggle = function () {
        $('.sidebar-mobile-secondary-toggle').on('click', function (e) {
            e.preventDefault();
            $('body').toggleClass('sidebar-mobile-secondary').removeClass('sidebar-mobile-main sidebar-mobile-right');

            // Fullscreen mode
            if ($('.sidebar-secondary').hasClass('sidebar-fullscreen')) {
                $('.sidebar-secondary').removeClass('sidebar-fullscreen');
            }
        });
    };

    // Toggle right sidebar on mobile
    var _sidebarMobileRightToggle = function () {
        $('.sidebar-mobile-right-toggle').on('click', function (e) {
            e.preventDefault();
            $('body').toggleClass('sidebar-mobile-right').removeClass('sidebar-mobile-main sidebar-mobile-secondary');

            // Hide sidebar if in fullscreen mode on mobile
            if ($('.sidebar-right').hasClass('sidebar-fullscreen')) {
                $('.sidebar-right').removeClass('sidebar-fullscreen');
            }
        });
    };

    // Toggle component sidebar on mobile
    var _sidebarMobileComponentToggle = function () {
        $('.sidebar-mobile-component-toggle').on('click', function (e) {
            e.preventDefault();
            $('body').toggleClass('sidebar-mobile-component');
        });
    };


    // Navigations
    // -------------------------

    // Sidebar navigation
    var _navigationSidebar = function () {

        // Define default class names and options
        var navClass = 'nav-sidebar',
            navItemClass = 'nav-item',
            navItemOpenClass = 'nav-item-open',
            navLinkClass = 'nav-link',
            navSubmenuClass = 'nav-group-sub',
            navSlidingSpeed = 250;

        // Configure collapsible functionality
        $('.' + navClass).each(function () {
            $(this).find('.' + navItemClass).has('.' + navSubmenuClass).children('.' + navItemClass + ' > ' + '.' + navLinkClass).not('.disabled').on('click', function (e) {
                e.preventDefault();

                // Simplify stuff
                var $target = $(this),
                    $navSidebarMini = $('.sidebar-xs').not('.sidebar-mobile-main').find('.sidebar-main .' + navClass).children('.' + navItemClass);

                // Collapsible
                if ($target.parent('.' + navItemClass).hasClass(navItemOpenClass)) {
                    $target.parent('.' + navItemClass).not($navSidebarMini).removeClass(navItemOpenClass).children('.' + navSubmenuClass).slideUp(navSlidingSpeed);
                }
                else {
                    $target.parent('.' + navItemClass).not($navSidebarMini).addClass(navItemOpenClass).children('.' + navSubmenuClass).slideDown(navSlidingSpeed);
                }

                // Accordion
                if ($target.parents('.' + navClass).data('nav-type') == 'accordion') {
                    $target.parent('.' + navItemClass).not($navSidebarMini).siblings(':has(.' + navSubmenuClass + ')').removeClass(navItemOpenClass).children('.' + navSubmenuClass).slideUp(navSlidingSpeed);
                }
            });
        });

        // Disable click in disabled navigation items
        $(document).on('click', '.' + navClass + ' .disabled', function (e) {
            e.preventDefault();
        });

        // Scrollspy navigation
        $('.nav-scrollspy').find('.' + navItemClass).has('.' + navSubmenuClass).children('.' + navItemClass + ' > ' + '.' + navLinkClass).off('click');
    };

    // Navbar navigation
    var _navigationNavbar = function () {

        // Prevent dropdown from closing on click
        $(document).on('click', '.dropdown-content', function (e) {
            e.stopPropagation();
        });

        // Disabled links
        $('.navbar-nav .disabled a, .nav-item-levels .disabled').on('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
        });

        // Show tabs inside dropdowns
        $('.dropdown-content a[data-toggle="tab"]').on('click', function (e) {
            $(this).tab('show');
        });
    };


    // Components
    // -------------------------

    // Tooltip
    var _componentTooltip = function () {

        // Initialize
        $('[data-popup="tooltip"]').tooltip();

        // Demo tooltips, remove in production
        var demoTooltipSelector = '[data-popup="tooltip-demo"]';
        if ($(demoTooltipSelector).is(':visible')) {
            $(demoTooltipSelector).tooltip('show');
            setTimeout(function () {
                $(demoTooltipSelector).tooltip('hide');
            }, 2000);
        }
    };

    // Popover
    var _componentPopover = function () {
        $('[data-popup="popover"]').popover();
    };

    // Uniform
    var _componentUniform = function () {
        if (!$().uniform) {
            console.warn('Warning - uniform.min.js is not loaded.');
            return;
        }

        // Default initialization
        $('.form-check-input-styled').uniform();


        //
        // Contextual colors
        //

        // Primary
        $('.form-check-input-styled-primary').uniform({
            wrapperClass: 'border-primary-600 text-primary-800'
        });

        // Danger
        $('.form-check-input-styled-danger').uniform({
            wrapperClass: 'border-danger-600 text-danger-800'
        });

        // Success
        $('.form-check-input-styled-success').uniform({
            wrapperClass: 'border-success-600 text-success-800'
        });

        // Warning
        $('.form-check-input-styled-warning').uniform({
            wrapperClass: 'border-warning-600 text-warning-800'
        });

        // Info
        $('.form-check-input-styled-info').uniform({
            wrapperClass: 'border-info-600 text-info-800'
        });

        // Custom color
        $('.form-check-input-styled-custom').uniform({
            wrapperClass: 'border-indigo-600 text-indigo-800'
        });
    };

    var _componenetICheck = function () {

        $(".icheck").each(function () {
            var checkboxClass = $(this).attr("data-checkbox") ? $(this).attr("data-checkbox") : "icheckbox_square-blue";
            var radioClass = $(this).attr("data-radio") ? $(this).attr("data-radio") : "iradio_square-blue";

            if (checkboxClass.indexOf("_line") > -1 || radioClass.indexOf("_line") > -1) {
                $(this).iCheck({
                    checkboxClass: checkboxClass,
                    radioClass: radioClass,
                    insert: '<div class="icheck_line-icon"></div>' + $(this).attr("data-label")
                });
            } else {
                $(this).iCheck({
                    checkboxClass: checkboxClass,
                    radioClass: radioClass
                });
            }
        });
    }

    // Bootstrap switch
    var _componentBootstrapSwitch = function () {
        if (!$().bootstrapSwitch) {
            console.warn('Warning - switch.min.js is not loaded.');
            return;
        }

        // Initialize
        $('.form-check-input-switch').bootstrapSwitch();
    };

    // Select2 select
    var _componentSelect21 = function () {
        if (!$().select2) {
            console.warn('Warning - select2.min.js is not loaded.');
            return;
        }

        // Initialize
        var $select = $('.form-control-select2').select2({
            minimumResultsForSearch: Infinity
        });

        // Trigger value change when selection is made
        $select.on('change', function () {
            $(this).trigger('blur');
        });
    };

    // Mask 
    var _componentMask = function () {
        $('.phone-number').attr("data-mask", "9999 999 99 99");
        $('.pickadate').attr("data-mask", "99.99.9999");
        $('.pickadatetime').attr("data-mask", "99.99.9999 99:99");
        $('.tcNo').attr("data-mask", "99999999999");

        function clearDate(item) {
            item.val("__/__/____");
            item.focus();
        }

        $(".pickadate").keyup(function () {
            var value = $(this).val();

            var splitted = value.split('.');

            if (splitted.length > 0) {
                var day = splitted[0];
                var month = splitted[1];
                var year = splitted[2];

                if (day[0] > 3 ||
                    day > 31 ||
                    month[0] > 1 ||
                    month > 12 ||
                    (year[0] < 1 || year[0] > 2) ||
                    (year < 1900 || year > 2100)) {
                    clearDate($(this));
                }
            }
        });
    };

    // MaxLength
    var _componenetMaxLength = function () {
        $('input[type=file]').removeAttr("data-val");
        $('input[type=file]').removeAttr("data-val-maxlength");
        $('input[type=file]').removeAttr("data-val-maxlength-max");

        $('input[data-val-maxlength-max]').maxlength({
            alwaysShow: true
        });

        $('textarea[data-val-maxlength-max]').maxlength({
            alwaysShow: true
        });
    };

    var _componentFileInput = function () {
        $('.form-control-uniform').uniform();
    };

    // Pickadate picker
    var _componentPickadate = function () {
        if (!$().datepicker) {
            console.warn('Warning - datepicker.js and/or datepicker.tr.js is not loaded.');
            return;
        }

        $('.pickadate').datepicker({
            format: "dd.mm.yyyy",
            todayBtn: "linked",
            clearBtn: true,
            language: "tr",
            autoclose: true,
            todayHighlight: true
        });
    };

    var _componentMultiSelect = function () {

        $('.multiselect-filtering').multiselect({
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            includeSelectAllOption: true,
            allSelectedText: 'Tümü Seçildi',
            filterPlaceholder: 'Filtrele',
            nSelectedText: 'Seçildi',
            nonSelectedText: 'Seçili Değil',
            selectAllText: 'Tümünü Seç',
            onDropdownShown: function (event) {

                if (this.$filter != null && this.$filter.find('.multiselect-search').length > 0)
                    this.$filter.find('.multiselect-search').focus();
            }
        });
    };

    var _componentSelect2 = function () {
        if (!$().select2) {
            console.warn('Warning - select2.min.js is not loaded.');
            return;
        }

        var $select = $('.select2').select2();

        // Tagging support
        var $select1 = $('.select2-multiple').select2({
            tags: true,
            allowClear: true
        });

        $('.select2-no-search').select2({
            tags: false,
            allowClear: false,
            minimumResultsForSearch: -1
        });

        // Trigger value change when selection is made
        $select.on('change', function () {
            $(this).trigger('blur');
        });

        $select1.on('change', function () {
            $(this).trigger('blur');
        });

        // Initialize
        //$('.select-remote-data').select2({
        //    ajax: {
        //        url: 'https://api.github.com/search/repositories',
        //        dataType: 'json',
        //        delay: 250,
        //        data: function (params) {
        //            return {
        //                q: params.term, // search term
        //                page: params.page
        //            };
        //        },
        //        processResults: function (data, params) {

        //             parse the results into the format expected by Select2
        //             since we are using custom formatting functions we do not need to
        //             alter the remote JSON data, except to indicate that infinite
        //             scrolling can be used
        //            params.page = params.page || 1;

        //            return {
        //                results: data.items,
        //                pagination: {
        //                    more: (params.page * 30) < data.total_count
        //                }
        //            };
        //        },
        //        cache: true
        //    },
        //    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //    minimumInputLength: 1,
        //    templateResult: formatRepo, // omitted for brevity, see the source of this page
        //    templateSelection: formatRepoSelection // omitted for brevity, see the source of this page
        //});


    };

    // Card actions
    // -------------------------

    // Reload card (uses BlockUI extension)
    var _cardActionReload = function () {
        $('.card [data-action=reload]:not(.disabled)').on('click', function (e) {
            e.preventDefault();
            var $target = $(this),
                block = $target.closest('.card');

            // Block card
            $(block).block({
                message: '<i class="icon-spinner2 spinner"></i>',
                overlayCSS: {
                    backgroundColor: '#fff',
                    opacity: 0.8,
                    cursor: 'wait',
                    'box-shadow': '0 0 0 1px #ddd'
                },
                css: {
                    border: 0,
                    padding: 0,
                    backgroundColor: 'none'
                }
            });

            // For demo purposes
            window.setTimeout(function () {
                $(block).unblock();
            }, 2000);
        });
    };

    // Collapse card
    var _cardActionCollapse = function () {
        var $cardCollapsedClass = $('.card-collapsed');

        // Hide if collapsed by default
        $cardCollapsedClass.children('.card-header').nextAll().hide();

        // Rotate icon if collapsed by default
        $cardCollapsedClass.find('[data-action=collapse]').addClass('rotate-180');

        // Collapse on click
        $('.card [data-action=collapse]:not(.disabled)').on('click', function (e) {
            var $target = $(this),
                slidingSpeed = 150;

            e.preventDefault();
            $target.parents('.card').toggleClass('card-collapsed');
            $target.toggleClass('rotate-180');
            $target.closest('.card').children('.card-header').nextAll().slideToggle(slidingSpeed);
        });
    };

    // Remove card
    var _cardActionRemove = function () {
        $('.card [data-action=remove]').on('click', function (e) {
            e.preventDefault();
            var $target = $(this),
                slidingSpeed = 150;

            // If not disabled
            if (!$target.hasClass('disabled')) {
                $target.closest('.card').slideUp({
                    duration: slidingSpeed,
                    start: function () {
                        $target.addClass('d-block');
                    },
                    complete: function () {
                        $target.remove();
                    }
                });
            }
        });
    };

    // Card fullscreen mode
    var _cardActionFullscreen = function () {
        $('.card [data-action=fullscreen]').on('click', function (e) {
            e.preventDefault();

            // Define vars
            var $target = $(this),
                cardFullscreen = $target.closest('.card'),
                overflowHiddenClass = 'overflow-hidden',
                collapsedClass = 'collapsed-in-fullscreen',
                fullscreenAttr = 'data-fullscreen';

            // Toggle classes on card
            cardFullscreen.toggleClass('fixed-top h-100 rounded-0');

            // Configure
            if (!cardFullscreen.hasClass('fixed-top')) {
                $target.removeAttr(fullscreenAttr);
                cardFullscreen.children('.' + collapsedClass).removeClass('show');
                $('body').removeClass(overflowHiddenClass);
                $target.siblings('[data-action=move], [data-action=remove], [data-action=collapse]').removeClass('d-none');
            }
            else {
                $target.attr(fullscreenAttr, 'active');
                cardFullscreen.removeAttr('style').children('.collapse:not(.show)').addClass('show ' + collapsedClass);
                $('body').addClass(overflowHiddenClass);
                $target.siblings('[data-action=move], [data-action=remove], [data-action=collapse]').addClass('d-none');
            }
        });
    };


    // Misc
    // -------------------------

    // Dropdown submenus. Trigger on click
    var _dropdownSubmenu = function () {

        // All parent levels require .dropdown-toggle class
        $('.dropdown-menu').find('.dropdown-submenu').not('.disabled').find('.dropdown-toggle').on('click', function (e) {
            e.stopPropagation();
            e.preventDefault();

            // Remove "show" class in all siblings
            $(this).parent().siblings().removeClass('show').find('.show').removeClass('show');

            // Toggle submenu
            $(this).parent().toggleClass('show').children('.dropdown-menu').toggleClass('show');

            // Hide all levels when parent dropdown is closed
            $(this).parents('.show').on('hidden.bs.dropdown', function (e) {
                $('.dropdown-submenu .show, .dropdown-submenu.show').removeClass('show');
            });
        });
    };

    // Header elements toggler
    var _headerElements = function () {

        // Toggle visible state of header elements
        $('.header-elements-toggle').on('click', function (e) {
            e.preventDefault();
            $(this).parents('[class*=header-elements-]').find('.header-elements').toggleClass('d-none');
        });

        // Toggle visible state of footer elements
        $('.footer-elements-toggle').on('click', function (e) {
            e.preventDefault();
            $(this).parents('.card-footer').find('.footer-elements').toggleClass('d-none');
        });
    };

    //
    // Return objects assigned to module
    //
    return {

        // Disable transitions before page is fully loaded
        initBeforeLoad: function () {
            _transitionsDisabled();
        },

        // Enable transitions when page is fully loaded
        initAfterLoad: function () {
            _transitionsEnabled();
        },

        // Initialize all sidebars
        initSidebars: function () {

            // On desktop
            _sidebarMainResize();
            _sidebarMainToggle();
            _sidebarSecondaryToggle();
            _sidebarComponentToggle();

            // On mobile
            _sidebarMobileFullscreen();
            _sidebarMobileMainToggle();
            _sidebarMobileSecondaryToggle();
            _sidebarMobileRightToggle();
            _sidebarMobileComponentToggle();
        },

        // Initialize all navigations
        initNavigations: function () {
            _navigationSidebar();
            _navigationNavbar();
        },

        // Initialize all components
        initComponents: function () {
            _componentTooltip();
            _componentPopover();
            //_componentUniform();
            _componenetICheck();
            //_componentBootstrapSwitch();
            _componentSelect2();
            _componentMask();
            _componenetMaxLength();
            _componentFileInput();
            _componentPickadate();
            _componentMultiSelect();
        },

        // Initialize all card actions
        initCardActions: function () {
            _cardActionReload();
            _cardActionCollapse();
            _cardActionRemove();
            _cardActionFullscreen();
        },

        // Dropdown submenu
        initDropdownSubmenu: function () {
            _dropdownSubmenu();
        },

        initHeaderElementsToggle: function () {
            _headerElements();
        },

        // Initialize core
        initCore: function () {
            App.initSidebars();
            App.initNavigations();
            App.initComponents();
            App.initCardActions();
            App.initDropdownSubmenu();
            App.initHeaderElementsToggle();
        }
    }
}();


// Initialize module
// ------------------------------

// When content is loaded
document.addEventListener('DOMContentLoaded', function () {
    App.initBeforeLoad();
    App.initCore();
});

// When page is fully loaded
window.addEventListener('load', function () {
    App.initAfterLoad();
});

// Date clientside validationu geçmesini sağlıyor
jQuery.validator.methods["date"] = function (value, element) { return true; }

function OnChangeToUpdate(url) {
    window.location.href = url;
}

function FillMultiSelect(parameter, url, select, selectedVal) {

    $.getJSON(url,
        parameter,
        function (response) {
            select.empty();

            $.each(response, function (index, item) {

                var p = new Option(item.Text, item.Value);
                select.append(p);
            });
        }).then(function () {

            if (selectedVal != null)
                select.val(selectedVal).trigger("change");
        });
};

function SetSelectedValue(selector, selectedValue) {
    $(selector).val(selectedValue).trigger("change");
    //$(selector).multiselect("refresh");
};

function SetUniform(selector, checked) {

    if (checked) {
        $(selector).iCheck('check');
    }
    else {
        $(selector).iCheck('uncheck');
    }
}

function DatePickerOnchange(selector, funcion) {
    selector.datepicker({
        format: "dd.mm.yyyy",
        todayBtn: "linked",
        clearBtn: true,
        language: "tr",
        autoclose: true,
        todayHighlight: true,
        onSelect: function () {
            $(this).change();
        }
    }).on("change", function () {
        funcion();
    });
}

function GetStringParameterFromArray(idler) {
    var idlerStr = "";

    if (idler.length > 0) {
        for (var i = 0; i < idler.length; i++) {
            idlerStr += idler[i] + ",";
        }
    }

    return idlerStr;
}

var decodeHTML = function (html) {
    var txt = document.createElement('textarea');
    txt.innerHTML = html;
    return txt.value;
};

function UlkeSehirIlce(selectUlke, selectSehir, selectedSehirId, selectIlce, selectedIlceId) {

    function ilceListele(id, selected) {

        id = id == null || id === ""
            ? 0
            : id;

        var select = selectIlce;

        var parameter = { SehirId: id };

        FillMultiSelect(parameter, "/IlceSehirUlke/IlceListele", select, selected);
    }

    selectUlke.on('change', function () {

        var select = selectSehir;

        var ulkeId = selectUlke.val();

        ulkeId = ulkeId == null || ulkeId === ""
            ? 0
            : ulkeId;

        var parameter = { UlkeId: ulkeId };

        FillMultiSelect(parameter, "/IlceSehirUlke/SehirListele", select);

        ilceListele(selectedSehirId, selectedIlceId);
    });

    selectSehir.on('change', function () {

        var sehirId = selectSehir.val();

        ilceListele(sehirId, selectedIlceId);
    });

    var select = selectSehir;

    var ulkeId = selectUlke.val();

    ulkeId = ulkeId == null || ulkeId === ""
        ? 0
        : ulkeId;

    var parameter = { UlkeId: ulkeId };

    FillMultiSelect(parameter, "/IlceSehirUlke/SehirListele", select, selectedSehirId);

    //ilceListele(selectedSehirId, selectedIlceId);
}

function Siniflar(selectSinif, selectSube, selectSezon, selectBrans, selectedSinifId) {

    function listele() {

        var subeId = selectSube.length > 0 && selectSube.val() != null ? selectSube.val() : 0;
        var sezonId = selectSezon.length > 0 && selectSezon.val() != null ? selectSezon.val() : 0;
        var bransId = selectBrans.length > 0 && selectBrans.val() != null ? selectBrans.val() : 0;

        var parameters = { SubeId: subeId, SezonId: sezonId, BransId: bransId };

        FillMultiSelect(parameters, "/Sinif/SinifListele", selectSinif, selectedSinifId);
    }

    listele();

    selectSube.on('change', function () {
        listele();
    });

    selectSezon.on('change', function () {
        listele();
    });

    selectBrans.on('change', function () {
        listele();
    });
}

function Sezonlar(selectSezon, selectSube, selectSinif) {

    function listele() {

        selectSinif.empty();

        var subeIdler = selectSube.val();

        var subeIdlerStr = GetStringParameterFromArray(subeIdler);

        var parameters = { subeIdler: subeIdlerStr };

        FillMultiSelect(parameters, "/Sezon/SezonlarListele", selectSezon, null);
    }

    selectSube.on('change', function () {
        listele();
    });
}

function SmsSinavlar(selectSinav, selectSezon, selectSube) {

    function listele() {

        var subeIdler = selectSube.val();
        var subeIdlerStr = GetStringParameterFromArray(subeIdler);

        var sezonIdler = selectSezon.val();
        var sezonIdlerStr = GetStringParameterFromArray(sezonIdler);

        var parameters = { subeIdler: subeIdlerStr, sezonIdler: sezonIdlerStr };

        FillMultiSelect(parameters, "/Sinav/SinavlarListele", selectSinav, null);
    }

    selectSezon.on('change', function () {
        listele();
    });
}

function SmsSiniflar(selectSinif, selectSezon, selectSube) {

    function listele() {

        var subeIdler = selectSube.val();
        var subeIdlerStr = GetStringParameterFromArray(subeIdler);

        var sezonIdler = selectSezon.val();
        var sezonIdlerStr = GetStringParameterFromArray(sezonIdler);

        var parameters = { subeIdler: subeIdlerStr, sezonIdler: sezonIdlerStr };

        FillMultiSelect(parameters, "/Sinif/SiniflarListele", selectSinif, null);
    }

    selectSezon.on('change', function () {
        listele();
    });
}

function HesapSubeParaBirim(selectHesap, selectedHesapId, selectSube, selectParaBirim, subeDetaylar) {

    function hesapListle() {

        var subeId = selectSube.val();

        subeId = subeId == null || subeId === ""
            ? 0
            : subeId;

        var paraBirimId = selectParaBirim.val();

        paraBirimId = paraBirimId == null || paraBirimId === ""
            ? 0
            : paraBirimId;

        if (paraBirimId > 0) {
            var parameter = { SubeId: subeId, ParaBirimId: paraBirimId };

            FillMultiSelect(parameter, "/Hesap/HesapSubeIdParaBirimIdListele", selectHesap, selectedHesapId);
        }
    }

    hesapListle();

    selectSube.on('change', function () {

        hesapListle();

        var subeId = selectSube.val();

        subeId = subeId == null
            ? 0
            : subeId;

        var sube = null;

        for (var i = 0; i < subeDetaylar.length; i++) {
            if (subeDetaylar[i].SubeId == subeId) {
                sube = subeDetaylar[i];
                break;
            }
        }

        if (sube !== null) {
            selectParaBirim.val(sube.ParaBirimId).trigger('change');
        }
    });

    selectParaBirim.on('change', function () { hesapListle(); });
}

function SubePersonel(selectPersonel, selectedPersonelId, selectSube, nullable) {

    function personelListle() {

        var subeId = selectSube.val();

        subeId = subeId == null || subeId === ""
            ? 0
            : subeId;

        var parameter = { SubeId: subeId, Nullable: nullable };

        FillMultiSelect(parameter, "/Personel/SubePersonelListele", selectPersonel, selectedPersonelId);
    }

    personelListle();

    selectSube.on('change', function () {

        personelListle();
    });
}

function SubeServis(selectServis, selectedServisId, selectSube) {

    function servisListle() {

        var subeId = selectSube.val();

        subeId = subeId == null || subeId === ""
            ? 0
            : subeId;

        var parameter = { SubeId: subeId };

        FillMultiSelect(parameter, "/Servis/SubeServisListele", selectServis, selectedServisId);
    }

    servisListle();

    selectSube.on('change', function () {

        servisListle();
    });
}

function BransDers(selectDers, selectedDersId, selectBrans) {

    function dersListle() {

        var bransId = selectBrans.val();

        bransId = bransId == null || bransId === ""
            ? 0
            : bransId;

        var parameter = { BransId: bransId };

        FillMultiSelect(parameter, "/Ders/BransDersListele", selectDers, selectedDersId);
    }

    dersListle();

    selectBrans.on('change', function () {
        dersListle();
    });
}

function ParaBirimHesap(selectHesap, selectedHesapId, hesapTurGrupId, selectParaBirim) {

    function hesapListele() {

        var paraBirimId = selectParaBirim.val();

        paraBirimId = paraBirimId == null || paraBirimId === ""
            ? 0
            : paraBirimId;

        var parameter = { ParaBirimId: paraBirimId, HesapTurGrupId: hesapTurGrupId };

        FillMultiSelect(parameter, "/Hesap/HesapParaBirimIdListele", selectHesap, selectedHesapId);
    }

    hesapListele();

    selectParaBirim.on('change', function () {

        hesapListele();
    });
}

function ParaBirimBagliHesap(selectHesap, selectedHesapId, hesapTurGrupId, selectParaBirim, selectUstHesap) {

    function hesapListele() {

        var paraBirimId = selectParaBirim.val();

        paraBirimId = paraBirimId == null || paraBirimId === ""
            ? 0
            : paraBirimId;

        var ustHesapId = selectUstHesap.val();

        ustHesapId = ustHesapId == null || ustHesapId === ""
            ? 0
            : ustHesapId;

        var parameter = { ParaBirimId: paraBirimId, UstHesapId: ustHesapId, HesapTurGrupId: hesapTurGrupId };

        FillMultiSelect(parameter, "/Hesap/HesapParaBirimIdAltHesapListele", selectHesap, selectedHesapId);
    }

    hesapListele();

    selectParaBirim.on('change', function () {

        hesapListele();
    });

    selectUstHesap.on('change', function () {

        hesapListele();
    });
}

function HesapTur(selectHesapTur, selectedHesapTurId, hesapTurGrupId) {

    function hesapTurListele() {

        var parameter = { HesapTurGrupId: hesapTurGrupId };

        FillMultiSelect(parameter, "/HesapTur/HesapTurListele", selectHesapTur, selectedHesapTurId);
    }

    hesapTurListele();
}

function DersKonular(selectKonu, selectedKonuId, dersId) {

    function konuListele() {

        var parameter = { DersId: dersId };

        FillMultiSelect(parameter, "/Konu/DersKonuListele", selectKonu, selectedKonuId);
    }

    konuListele();
}

function DersVideoKategoriler(selectVideoKategori, selectedVideoKategoriId, dersId) {

    function videoKategoriListele() {

        var parameter = { DersId: dersId };

        FillMultiSelect(parameter, "/VideoKategori/DersVideoKategoriListele", selectVideoKategori, selectedVideoKategoriId);
    }

    videoKategoriListele();
}

function KurumSubeler(selectSube, selectedSubeId, kurumIdler) {

    function subeListele() {

        var kurumIdlerStr = GetStringParameterFromArray(kurumIdler);

        var parameter = { KurumIdler: kurumIdlerStr };

        FillMultiSelect(parameter, "/Sube/KurumlarSubeListele", selectSube, selectedSubeId);
    }

    subeListele();
}

function KurumSezonlar(selectSezon, selectedSezonId, kurumIdler) {

    function sezonListele() {

        var kurumIdlerStr = GetStringParameterFromArray(kurumIdler);

        var parameter = { KurumIdler: kurumIdlerStr };

        FillMultiSelect(parameter, "/Sezon/KurumlarSezonListele", selectSezon, selectedSezonId);
    }

    sezonListele();
}

function KurumBranslar(selectBrans, selectedBransId, kurumIdler) {

    function bransListele() {

        var kurumIdlerStr = GetStringParameterFromArray(kurumIdler);

        var parameter = { KurumIdler: kurumIdlerStr };

        FillMultiSelect(parameter, "/Brans/KurumlarBransListele", selectBrans, selectedBransId);
    }

    bransListele();
}

function SubeSezonBransSiniflar(selectSinif, selectedSinifId, subeIdler, sezonIdler, bransIdler) {

    function sinifListele() {

        var subeIdlerStr = GetStringParameterFromArray(subeIdler);
        var sezonIdlerStr = GetStringParameterFromArray(sezonIdler);
        var bransIdlerStr = GetStringParameterFromArray(bransIdler);

        var parameter = { SubeIdler: subeIdlerStr, SezonIdler: sezonIdlerStr, BransIdler: bransIdlerStr };

        FillMultiSelect(parameter, "/Sinif/SubeSezonBransSinifListele", selectSinif, selectedSinifId);
    }

    sinifListele();
}

function VideoKategoriler(selectVideoKategori, dersId) {

    function videoKategoriListele() {

        var parameter = { DersId: dersId };

        FillMultiSelect(parameter, "/OgrenciBilgi/DersVideoKategoriListele", selectVideoKategori, null);
    }

    videoKategoriListele();
}

function VideoKonular(selectVideoKonu, dersId) {

    function videoKonuListele() {

        var parameter = { DersId: dersId };

        FillMultiSelect(parameter, "/OgrenciBilgi/DersKonuListele", selectVideoKonu, null);
    }

    videoKonuListele();
}

function TcKimlikDogrula() {

    $(".tcNo").keyup(function () {

        if ($(this).val().replace("_", "").length > 10) {

            var tcKimlikNo = $(this).val();
            var tcKimlikNoId = $(this).attr("id");
            var ad = $("#" + tcKimlikNoId.replace("TcKimlikNo", "Ad")).val();
            var soyad = $("#" + tcKimlikNoId.replace("TcKimlikNo", "Soyad")).val();
            var dogumTarihi = $("#" + tcKimlikNoId.replace("TcKimlikNo", "DogumTarihi")).val();

            var parameter = { TcKimlikNo: tcKimlikNo, Ad: ad, Soyad: soyad, DogumTarihi: dogumTarihi };

            $.getJSON(
                "/Kullanici/TcKimlikDogrula",
                parameter,
                function (response) {

                    $("#div" + tcKimlikNoId).attr("class", response.StatusClass);
                    $("#div" + tcKimlikNoId).children("span").text(response.Message);

                    console.log(response);
                });
        }
    });
}

function Simdi(elem) {
    var currentdate = new Date();

    var now = (currentdate.getDate() < 10 ? "0" + currentdate.getDate() : currentdate.getDate()) +
        "." +
        (currentdate.getMonth() + 1 < 10 ? "0" + (currentdate.getMonth() + 1) : currentdate.getMonth() + 1) +
        "." +
        currentdate.getFullYear() +
        " " +
        (currentdate.getHours() < 10 ? "0" + currentdate.getHours() : currentdate.getHours()) +
        ":" +
        (currentdate.getMinutes() < 10 ? "0" + currentdate.getMinutes() : currentdate.getMinutes());

    elem.val(now);
}

function ExcelAktar(ButtonId, DataUrl) {

    $(document).ready(function () {

        $('#' + ButtonId).click(function () {

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

            window.location.href = url;
        });
    });
}

(function ($) {
    $.fn.extend({

        upperFirstAll: function () {
            $(this).keyup(function (event) {
                var txt = $(this).val();


                for (var i = 0; i < txt.length; i++) {
                    if (i == 0) {
                        $(this).val(txt.charAt(0).toLocaleUpperCase('tr-TR') + txt.slice(1));
                    }
                    else {
                        if (txt[i] == " " && txt.length > i + 1) {
                            $(this).val(txt.substr(0, txt.length - (txt.charAt(i + 1).toLocaleUpperCase('tr-TR') + txt.slice(i + 2)).length) + txt.charAt(i + 1).toLocaleUpperCase('tr-TR') + txt.slice(i + 2));
                        }
                    }
                }

                //$(this).val(txt.toLowerCase().replace(/^(.)|(\s|\-)(.)/g,
                //    function (c) {
                //        return c.toLocaleUpperCase('tr');
                //    }));
            });
        },

        upperFirst: function () {
            $(this).keyup(function (event) {
                var txt = $(this).val();

                $(this).val(txt.toLowerCase().replace(/^(.)/g,
                    function (c) {
                        return c.toLocaleUpperCase('tr');
                    }));
            });
        },

        lowerCase: function () {
            $(this).keyup(function (event) {
                var txt = $(this).val();

                $(this).val(txt.toLocaleLowerCase('tr'));
            });
        },

        upperCase: function () {
            $(this).keyup(function (event) {
                var txt = $(this).val();

                $(this).val(txt.toLocaleUpperCase('tr'));
            });
            return this;
        }

    });
}(jQuery));

$(document).ready(function () {
    $('.firstCapitalUpper').upperFirstAll();

    TcKimlikDogrula();
});

$(document).on("keydown", "input", function (e) {
    if (e.which == 13) e.preventDefault();
});

$(document).ajaxComplete(function () {

    $(document).on("keydown", "input", function (e) {
        if (e.which == 13) e.preventDefault();
    });

    $('.firstCapitalUpper').upperFirstAll();

    TcKimlikDogrula();
});