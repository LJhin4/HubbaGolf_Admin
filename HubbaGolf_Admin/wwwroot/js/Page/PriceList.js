const _UrlGetPrice = Utils.baseUrl("/Website/GetPriceById");
const _UrlDeletePrice = Utils.baseUrl("/Website/DeletePriceById");

console.log(_UrlGetPrice);

//var fileImage = document.querySelector('#txt_FileImage');
//FilePond.create(fileImage);

$(function () {
    $('a[edit-id]').on('click', function (event) {
        var id = $(this).attr("edit-id");
        if (id != 0) {
            $("#txt_Id").val(id);
            GetPrice(id);
        }
        else {
            NewPrice();
        }
    });
    //notify delete
    $('a[delete-id]').on('click', function (event) {
        var id = $(this).attr("delete-id");
        $.confirm(Utils.createConfirmDialog('red', 'Warning!', 'Are you sure to delete this information?',
            function () {
                DeletePrice(id);
            }));
    });
    // Handle form submission
    $("#frmPost").submit(function (event) {
        var currentUrl = window.location.href;
        var paramValue = getUrlParameter(currentUrl, "GetPrice");
        console.log(paramValue);
        $("#txt_MenuId").val(paramValue);
    });
});
function NewPrice() {
    Utils.clearInput("popupPrice");
    $("#popupPrice").modal("show");
}
function GetPrice(id) {
    $.ajax({
        type: "GET",
        url: _UrlGetPrice,
        data: {
            id: id
        },
        beforeSend: function (xhr) {

        },
        success: function (r) {
            console.log(r);

            $("#txt_ID").val(r.id);
            $("#txt_Price").val(r.price);
            $("#cbo_Title").val(r.articleId);
            
            $("#popupPrice").modal("show");
        },
        error: function (e) {

        },
        complete: function (e) {

        }
    });
}
function DeletePrice(id) {
    $.ajax({
        type: "POST",
        url: _UrlDeletePrice,
        data: {
            id: id
        },
        beforeSend: function (xhr) {

        },
        success: function (r) {
            Utils.showToast(r.message, Utils.removeRow(id));
        },
        error: function (e) {

        },
        complete: function (e) {

        }
    });
}
function getUrlParameter(url, paramName) {
    try {
        // Extract the path from the URL
        var pathArray = new URL(url).pathname.split('/');

        // Find the index of the parameter name in the path
        var paramIndex = pathArray.indexOf(paramName);

        // If the parameter name is found, return the corresponding value
        if (paramIndex !== -1 && paramIndex < pathArray.length - 1) {
            return pathArray[paramIndex + 1];
        } else {
            return null;
        }
    } catch (error) {
        // Handle invalid URLs or other errors
        console.error('Error extracting parameter:', error);
        return null;
    }
}