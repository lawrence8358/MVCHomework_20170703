
//載入日期控制項
function LoadDatePicker() {
    /*
    <!-- bootstrap datepicker -->
    <link rel="stylesheet" href="/Plugins/datepicker/datepicker3.css">

    <!-- bootstrap datepicker --> 
    <script src="/Plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="/Plugins/datepicker/locales/bootstrap-datepicker.zh-TW.js"></script>
    */

    $('.datepicker').datepicker({
        autoclose: true,
        weekStart: 0,
        todayBtn: "linked",
        clearBtn: true,
        keyboardNavigation: false,
        language: "zh-TW",
        format: "yyyy/mm/dd"
    });
}

$(function () {

    //Input Type Number新增MaxLength支援
    $("input[type=number]").on("input change", function (e) { 
        if ($(this).attr("maxlength") != null && $(this).attr("maxlength") != "") {
            $(this).data("old", $(this).data("new") || "");
            $(this).data("new", $(this).val());

            var oldValue = $(this).data("old");
            var newValue = $(this).data("new");

            if ($(this).val().length > parseInt($(this).attr("maxLength"))) {
                if (newValue.length > parseInt($(this).attr("maxLength"))) {
                    newValue = oldValue;
                    $(this).data("new", oldValue);
                }
                var newInt = oldValue.slice(0, parseInt($(this).attr("maxLength")));
                $(this).val(newInt);
            }
        }
    });

});