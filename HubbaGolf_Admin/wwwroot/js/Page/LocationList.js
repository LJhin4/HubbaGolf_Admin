const _UrlGetCategory = Utils.baseUrl("/Website/GetCategoryById");
const _UrlDeleteCategory = Utils.baseUrl("/Website/DeleteCategoryById");

$(function () {

    $("#cbo_Parent").select2({
        width: "100%",
        placeholder: "--Choose--"
    });

    $('a[edit-id]').on('click', function (event) {
        var id = $(this).attr("edit-id");
        if (id != 0) {
            $("#txt_ID").val(id);
            GetCategory(id);
        }
        else {
            NewCategory();
        }
    });

    //thông báo xoá
    $('a[delete-id]').on('click', function (event) {
        var id = $(this).attr("delete-id");
        $.confirm(Utils.createConfirmDialog('red', 'Warning!', 'Are you sure to delete this information?',
            function () {
                DeleteCategory(id);
            }));
    });

});
function NewCategory() {
    Utils.clearInput("popupLocation");
    $("#popupLocation").modal("show");
    $("#cbo_Parent").val(0);
}
function GetCategory(id) {
    $.ajax({
        type: "GET",
        url: _UrlGetCategory,
        data: {
            id: id
        },
        beforeSend: function (xhr) {

        },
        success: function (r) {
            $("#txt_ID").val(r.id);

            $("#txt_Sort").val(r.sort);
            $("#txt_Code").val(r.code);
            $("#txt_NameVN").val(r.name);
            $("#cbo_Parent").val(r.parent).trigger("change")

            $("#popupLocation").modal("show");
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', status, error);
        },
        complete: function (e) {

        }
    });
}
function DeleteCategory(id) {
    $.ajax({
        type: "POST",
        url: _UrlDeleteCategory,
        data: {
            id: id
        },
        beforeSend: function (xhr) {

        },
        success: function (r) {
            Utils.showToast(r.message, Utils.removeRow(id));
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', status, error);
        },
        complete: function (e) {

        }
    });
}