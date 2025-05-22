var App = function () {

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
        $cardCollapsedClass.find('[data-action=collapse]');

        // Collapse on click
        $('.card [data-action=collapse]:not(.d-none)').on('click', function (e) {
            var $target = $(this),
                slidingSpeed = 150;

            e.preventDefault();
            if ($target.parents('.card').hasClass('fixed-top')) {
                if ($target.closest('.card').hasClass('card-collapsed')) {
                    $target.parents('.card').toggleClass('card-collapsed');
                    $target.closest('.card').children('.card-body').removeAttr('style');
                }
            }
            else {
                $target.parents('.card').toggleClass('card-collapsed');
                $target.closest('.card').children('.card-header').nextAll().slideToggle(slidingSpeed);
            }
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

    // Select2
    var _component_select2 = function (p_select) {
        if (!$().select2) {
            console.warn('Warning - Select2 Js is not loaded.');
            return;
        }

        var select = $('.select2');
        if (p_select) select = p_select;

        if (select.length > 0) {
            $(select).select2({
                language: "vi",
                minimumResultsForSearch: 5,
                selectOnClose: true
            });
            //thonv edit
            $(select).on('select2:selecting', function (e) {
                var value = e.params.args.data.id;
                if (value != '') {
                    var thisAlert = $(e.currentTarget).parent();
                    $(thisAlert).attr("data-validate", "");
                    $(thisAlert).removeClass('alert-validate');
                    $(thisAlert).removeClass('active');
                    $(thisAlert).addClass("has-success").removeClass("has-error");
                    $(e.currentTarget)[0].setCustomValidity('');
                }
            });
        }
    }

    // Datepicker
    var _component_datepicker = function (p_datepicker) {

        if (!$().datepicker) {
            return false;
        }

        var datepicker = $('.datepicker');
        if (p_datepicker) datepicker = p_datepicker;

        if ($().datepicker) {

            $(datepicker).datepicker({
                firstDay: 1,
                showButtonPanel: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                onSelect: function (date, obj) {
                    //thonv Edit
                    $(this).change();
                },
                beforeShow: function (input, obj) {
                    if ($(input).attr('readonly')) {
                        return false;
                    }

                    var picker = $(obj.dpDiv);
                    var v_type = $(input).attr('type');
                    var btn_today = picker.find('.ui-datepicker-current');

                    if (v_type == 'date') {
                        $(input).datepicker('option', 'dateFormat', 'yy-mm-dd');
                    }

                },

            })

        }
    }

    // Datatable 
    var _component_datatable = function (p_table = '') {

        var v_table = $('.dataTable');

        if (p_table) {
            v_table = p_table;
        }

        if (!$().DataTable) {
            console.warn('Warning - datatables.min.js is not loaded.');
            return;
        }

        // Setting datatable defaults
        $.extend($.fn.dataTable.defaults, {
            autoWidth: false,
            processing: true,
            searching: false, paging: false, info: false,
            responsive: {
                details: {
                    type: 'column'
                },
                breakpoints: [
                    { name: 'desktop', width: Infinity },
                    { name: 'tablet-l', width: 1200 },
                    { name: 'tablet-p', width: 992 },
                    { name: 'mobile-l', width: 576 },
                    { name: 'mobile-p', width: 320 }
                ]
            },
            dom: '<"datatable-header"fr><"datatable-body"t><"datatable-footer"<"datatable-li"li>p>',
            language: {
                decimal: "",
                emptyTable: "Không có dữ liệu trong bảng",
                info: " Tổng số _TOTAL_ bản ghi",
                infoEmpty: "Không có bản ghi nào",
                infoFiltered: "(danh sách từ _MAX_ bản ghi)",
                infoPostFix: "",
                thousands: ",",
                lengthMenu: " _MENU_ ",
                loadingRecords: "Đang tải...",
                processing: "Đang xử lý...",
                search: "",
                searchPlaceholder: 'Nhập tìm nhanh...',
                zeroRecords: "Không tìm thấy hồ sơ phù hợp",
                paginate: {
                    first: "Đầu",
                    last: "Cuối",
                    next: "Sau <i class='fa fa-chevron-right fa-xs'></i>",
                    previous: "<i class='fa fa-chevron-left fa-xs'></i> Trước"
                },
                aria: {
                    sortAscending: ": kích hoạt để sắp xếp cột tăng dần",
                    sortDescending: ": kích hoạt để sắp xếp cột giảm dần"
                },
                buttons: {
                    copyTitle: 'Đã thêm vào clipboard',
                    copyKeys: 'Nhấn ctrl hoặc <i>\u2318</i> + C để sao chép dữ liệu từ bảng vào khay nhớ tạm của bạn. Để hủy, bấm vào tin nhắn này hoặc nhấn Esc.',
                    copySuccess: {
                        _: 'Sao chép %d dòng ',
                        1: 'Sao chép 1 dòng '
                    }
                }
            },
            lengthMenu: [
                [10, 20, 50, 100, 200, 300, 400, 500, 1000],
                [10, 20, 50, 100, 200, 300, 400, 500, 1000]
            ],
        });



        if (v_table) {
            var v_datatable = $(v_table).DataTable({
                columnDefs: [
                    {
                        className: 'w1p control not-desktop text-center',
                        orderable: false,
                        targets: 0
                    },
                ]
            });
            _datatable_responsive_display(v_datatable);

            if ($('.dataTables_search').length > 0) {
                $('div.dataTables_filter').appendTo('.dataTables_search');
            }
        }


        // Reponsive recall 
        function _datatable_responsive_display(p_datatable) {

            p_datatable.on('responsive-display', function (e, datatable, row, showHide, update) {
                var li_dtr = $(this).find('tbody > tr.child > td.child > ul.dtr-details > li');
                li_dtr.each(function (index, li) {
                    var v_dtr_title = $(li).find('.dtr-title');
                    var v_dtr_data = $(li).find('.dtr-data');

                    if (v_dtr_title.is(':empty')) {
                        $(li).addClass('dtr-title-empty');
                    }

                    if (v_dtr_data.is(':empty')) {
                        $(li).addClass('dtr-data-empty');
                    }

                });

                // recall
                var select2 = $(this).find('.select');
                var datepicker = $(this).find('.datepicker');

                if (datepicker.hasClass('hasDatepicker')) {
                    datepicker.removeClass('hasDatepicker')
                        .removeData('datepicker')
                        .removeAttr('id')
                        .unbind();
                }

                _component_select2(select2);
                _component_datepicker(datepicker);
            });
        }

        $("[datatable-collapse]").on("shown.bs.collapse", function () {
            $.each($.fn.dataTable.tables(true), function () {
                $(this).DataTable().columns.adjust().draw();
            });
        });

        $('[datatable-modal]').on('shown.bs.modal', function (e) {
            $($.fn.dataTable.tables(true)).DataTable()
                .columns.adjust()
                .responsive.recalc();
        });

        $("[datatable-tab], .steps > .nav-tabs").on("shown.bs.tab", function (e) {
            $($.fn.dataTable.tables(true)).DataTable()
                .columns.adjust()
                .responsive.recalc();
        });


    };

    var _component_responsive_cell_datatables = function (p_table = '') {
        if (typeof p_table == 'undefined' || p_table == '') {
            p_table = '.dataTable';
        }
        // init control responsive
        $(p_table).each(function (i, e) {
            var v_th = $(e).children('thead').find('tr:first-child');
            var v_td = $(e).children('tbody').children('tr');

            if (!v_th.children().hasClass('cell')) {
                var rowspan = v_th.children().attr('rowspan');
                if (typeof rowspan !== 'undefined') {
                    v_th.prepend('<th class="cell" rowspan="' + rowspan + '"><i class="fa fa-ellipsis-v"></i></th>');
                } else {

                    v_th.prepend('<th class="cell"><i class="fa fa-ellipsis-v"></i></th>');
                }
            }
            if (!v_td.children().hasClass('cell')) {
                v_td.prepend('<td class="cell"></td>');
            }
        });
    };
    //
    // Tra ve cac object duoc gan cho module
    //

    return {

        // Init truoc khi load trang
        initBeforeLoad: function () {
            _component_responsive_cell_datatables();
        },

        // Init sau khi load trang
        initAfterLoad: function () {
        },

        // Initialize all components
        initComponents: function () {
            _component_select2();
            _component_datepicker();

        },

        // Initialize all card actions
        initCardActions: function () {
            _cardActionReload();
            _cardActionCollapse();
            _cardActionRemove();
            _cardActionFullscreen();
        },

        // Initialize core
        initCore: function () {
            App.initComponents();
            App.initCardActions();
            _component_datatable();
        },

        // Initialize Select2
        initSelect2: function () {
            _component_select2();
        },

        // Initialize Datepicker
        initDatepicker: function (p_datepicker) {
            _component_datepicker(p_datepicker);
        },
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
