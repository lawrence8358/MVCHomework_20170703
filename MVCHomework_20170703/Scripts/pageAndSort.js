function initPageAndSort(routerUrl) {
    $('.pagination>li>a[href]').each(function (i, item) {
        var page = $(item).attr('href').replace(routerUrl + '?pageNo=', '');
        $(item).attr('href', '#').click(function () { postPage(page); });
    });

    $('.tableSort').each(function (i, item) {
        var sortName = $(item).attr('href');
        $(item).attr('href', '#').click(function () { postSort(sortName); });

        if (sortName === $('#Table_CurrentSortName').val()) {
            if ($('#Table_Sort').val() === "")
                $(item).children(".sortOrder").html("🔼"); //小到大
            else
                $(item).children(".sortOrder").html("🔽"); //大到小
        }
    });
}

function postSort(sortName) {
    $('#Table_CurrentSortName').val(sortName);
    if ($('#Table_Sort').val() === sortName) $('#Table_Sort').val("");
    else $('#Table_Sort').val(sortName);
    tableAction();
};

function postPage(page) {
    $('#Table_PageNo').val(page);
    tableAction();
};

function tableAction() {
    var targetFormId = '#form1';
    if ($(targetFormId).size() > 0) {
        $('<input>')
            .attr({ type: 'hidden', id: 'pageNo', name: 'pageNo', value: $('#Table_PageNo').val() })
            .appendTo($(targetFormId));
        $('<input>')
            .attr({ type: 'hidden', id: 'sortName', name: 'sortName', value: $('#Table_Sort').val() })
            .appendTo($(targetFormId));
        $('<input>')
            .attr({ type: 'hidden', id: 'currentSortName', name: 'currentSortName', value: $('#Table_CurrentSortName').val() })
            .appendTo($(targetFormId));
        $(targetFormId).submit();
    }
}