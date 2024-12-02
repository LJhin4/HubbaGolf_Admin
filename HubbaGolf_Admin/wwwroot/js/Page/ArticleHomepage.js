const _UrlGetArticle = Utils.baseUrl("/Website/GetArticleHomepageById");
const _UrlDeleteArticle = Utils.baseUrl("/Website/DeleteArticleHomepageById");

console.log(_UrlGetArticle);

//var fileImage = document.querySelector('#txt_FileImage');
//FilePond.create(fileImage);

$(function () {
    $('a[edit-id]').on('click', function (event) {
        var id = $(this).attr("edit-id");
        if (id != 0) {
            $("#txt_Id").val(id);
            GetArticle(id);
        }
        else {
            NewArticle();
        }
    });
    //thông báo xoá
    $('a[delete-id]').on('click', function (event) {
        var id = $(this).attr("delete-id");
        $.confirm(Utils.createConfirmDialog('red', 'Warning!', 'Are you sure to delete this information?',
            function () {
                DeleteArticle(id);
            }));
    });
    // Handle form submission
    $("#frmPost").submit(function (event) {
        var currentUrl = window.location.href;
        var paramValue = getUrlParameter(currentUrl, "GetArticleHomepage");
        console.log(paramValue);
        $("#txt_MenuId").val(paramValue);
    });
});
function NewArticle() {
    Utils.clearInput("popupArticle");
    $("#popupArticle").modal("show");
}
function GetArticle(id) {
    $.ajax({
        type: "GET",
        url: _UrlGetArticle,
        data: {
            id: id
        },
        beforeSend: function (xhr) {

        },
        success: function (r) {
            console.log(r);

            $("#txt_ID").val(r.id);
            $("#txt_Description").val(r.description);
            $("#txt_Title").val(r.title);
            
            $("#popupArticle").modal("show");
        },
        error: function (e) {

        },
        complete: function (e) {

        }
    });
}
function DeleteArticle(id) {
    $.ajax({
        type: "POST",
        url: _UrlDeleteArticle,
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