const _UrlChangeStatusArticle = Utils.baseUrl("/Website/ChangeStatusArticle");
const _UrlDeleteArticle = Utils.baseUrl("/Website/DeleteArticleById");
$(function () {

    $("#table").on("blur", "input[type='text']", function () {
        var input = $(this);
        var isDuplicate = false;
        // Loop through all input fields to check for duplicates
        $("#table input[type='text']").not(input).each(function () {
            if ($(this).val() === input.val() &&
                $(this).val().length > 0 &&
                input.val().length > 0) {
                isDuplicate = true;
                return false;  // Exit the loop if a duplicate is found
            }
        });

        if (isDuplicate) {
            alert("The order number is duplicated");
            input.val(''); // Clear the input or handle as needed
            input.focus();  // Set focus back to the input field
            return;
        } else {
            // Continue with other actions, such as an AJAX update
            console.log("Value is unique.");

            // Get the new rank value
            var newRank = input.val();

            // Get the row id from the parent tr
            var rowId = input.closest("tr").attr("id");

            input.prop("disabled", true);

            // Perform AJAX call
            $.ajax({
                url: Utils.baseUrl("/Website/UpdateRank"),  // Replace with your actual URL
                type: 'POST',
                data: {
                    id: rowId,
                    rank: newRank
                },
                success: function (response) {
                    Utils.showToast('Updated successfully');
                    // Optional: handle success response
                    console.log("Rank updated successfully.");
                },
                error: function (xhr, status, error) {
                    Utils.showToast('Error unable to update');
                    // Optional: handle error response
                    console.error("Error updating rank: ", error);
                },
                complete: function () {
                    input.prop("disabled", false);
                }
            });
        }
    });

    $('a[edit-id]').on('click', function (event) {
        var id = $(this).attr("edit-id");
        if (id != 0)
            GetArticle(id);
        else
            NewArticle();
    });

    $('a[delete-id]').on('click', function (event) {
        var id = $(this).attr("delete-id");
        $.confirm(Utils.createConfirmDialog('red', 'Warning!', 'Are you sure to delete this information?',
            function () {
                DeleteArticle(id);
            }));
    });

    $('input[edit-id]').on('click', function (event) {
        var id = $(this).attr("edit-id");
        if (id != 0)
            ChangeStatusArticle(id);
    });
});
function ChangeStatusArticle(id) {
    $.ajax({
        type: "POST",
        url: _UrlChangeStatusArticle,
        data: {
            id: id
        },
        beforeSend: function (xhr) {

        },
        success: function (r) {
            if (r.success) {
                Toastify({
                    text: r.message,
                    duration: 3000,
                    newWindow: true,
                    close: true,
                    gravity: "top",
                    position: "right",
                    stopOnFocus: true,
                    style: {
                        background: "#4CAF50",
                        color: "#fff",
                        boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
                        border: "1px solid #4CAF50",
                        borderRadius: "8px",
                        fontFamily: "Arial, sans-serif",
                    },
                    className: "custom-toast",
                    closeOnClick: true,
                }).showToast();
            }
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
            if (r.success) {
                Utils.showToast(r.message, Utils.removeRow(id));

            } else {
                Utils.showAlertError('Update error!', r.message);
            }
        },
        error: function (e) {

        },
        complete: function (e) {

        }
    });
}