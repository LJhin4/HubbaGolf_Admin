$(function () {
    $(document).on('click', 'a', function (e) {
        var href = $(this).attr('href');

        if (href && href.indexOf('#') === -1) {
            var target = $(this).attr('target');
            if (target == null || target == undefined) {
                $('.loading-overlay').show();
            }
        }
        else {
            e.preventDefault();
        }
    });

    $(document).on('submit', 'form', function (e) {
        e.preventDefault();
        $('.loading-overlay').show();
        $(e.target).get(0).submit();
    });

    $('#sidebar a').click(function (event) {
        event.preventDefault(); // Prevent the default link behavior

        //const pageTitle = $(this).text(); // Get the text of the clicked menu item
        const pageTitle = $(this).contents().filter(function () {
            return this.nodeType === 3;  // Filter only text nodes
        }).text().trim();
        localStorage.setItem('pageTitle', pageTitle); // Store the page title in localStorage

        // Redirect to the clicked link's href
        window.location.href = $(this).attr('href');
    });

    $('#btn_ChangePass').click(function () {
        openChangePass();
    });
});
function openChangePass() {
    $.confirm({
        type: 'yellow',
        columnClass: 'medium',
        title: 'Change Password!',
        content: '' +
            '<div class="form-group">' +
            '<input type="password" class="form-control passwordold" value="" placeholder="old password" required />' +
            '</div>' +
            '<div class="form-group">' +
            '<input type="password" class="form-control passwordnew1" value="" placeholder="new password" required />' +
            '</div>' +
            '<div class="form-group">' +
            '<input type="password" class="form-control passwordnew2" value="" placeholder="Re-enter new password" required />' +
            '</div>',
        buttons: {
            OK: function () {
                var passold = this.$content.find('.passwordold').val();
                var passnew1 = this.$content.find('.passwordnew1').val();
                var passnew2 = this.$content.find('.passwordnew2').val();

                if (passnew1 !== passnew2) {
                    $.alert('Check the entered password again');
                    return false;
                }

                $.ajax({
                    type: "POST",
                    url: Utils.baseUrl('/Auth/ChangePass'),
                    data: {
                        passold: passold,
                        passnew1: passnew1,
                        passnew2: passnew2
                    },
                    beforeSend: function (xhr) {
                        Utils.showWaiting();
                    },
                    success: function (r) {
                        Utils.showToast('Password changed');
                    },
                    error: function (xhr, status, error) {
                        var errorMessage = xhr.responseText;
                        Utils.showAlertError('Error', errorMessage);
                    },
                    complete: function (e) {
                        Utils.hideWaiting();
                    }
                });
            },
            cancel: function () {
                //close
            },
        },
        onContentReady: function () {
            $('.passwordnew2').on('input', function () {
                var password1 = $('.passwordnew1').val();
                var password2 = $(this).val();

                if (password1 !== password2) {
                    $(this).css('border', '2px solid red');
                } else {
                    $(this).css('border', '');
                }
            });
        }
    });
}