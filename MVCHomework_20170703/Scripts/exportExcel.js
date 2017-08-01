function CheckExportExcel(routerUrl, queryString) { 
    $.get(routerUrl, queryString, function (data) { 
        if (data.ExcelCount == 0) {
            if (confirm("查無資料，確定要匯出Excel?")) ExportExcel(data.ExcelUrl);
        }
        else {
            ExportExcel(data.ExcelUrl);
        }
    });
}

function ExportExcel(url) {
    window.location = url;
}