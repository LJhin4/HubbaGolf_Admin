// Function to initialize SweetAlert2
function initializeSweetAlert() {
    window.Swal2 = Swal.mixin({
        customClass: {
            input: 'form-control'
        }
    });

    window.Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer);
            toast.addEventListener('mouseleave', Swal.resumeTimer);
        }
    });

    // Example usage:
    // Toast.fire({ icon: 'success', title: 'SweetAlert2 is ready!' });
}

// Check if SweetAlert2 is defined and initialize if ready
function checkAndInitializeSweetAlert() {
    if (typeof Swal === 'undefined') {
        console.error('SweetAlert2 library not loaded.');
    } else {
        // SweetAlert2 is defined, initialize
        initializeSweetAlert();
    }
}

// Wait for document ready before checking and initializing SweetAlert2
$(document).ready(function () {
    checkAndInitializeSweetAlert();
});

// 
//function isLocalEnvironment() {
//    return window.location.hostname === "localhost" || window.location.hostname === "127.0.0.1";
//}

var Utils = {
    upperFirstChar: function (string) {
        if (string == undefined)
            return '';
        return string.replace(/\b\w/g, c => c.toUpperCase());
    },
    isExternalURL: function (url) {
        //if (url == undefined)
        //    return false;

        //if (url.includes('pacvn.vn')) {
        //    return false; // Bypass the check for this domain
        //}

        //const currentHostName = window.location.hostname;
        //if (currentHostName === 'localhost')
        //    return false;
        //const urlHostName = new URL(url).hostname;
        //return currentHostName !== urlHostName;
        return false;
    },

    getMoneyNumber: function (numberString) {
        var cleanedNumber = parseInt(numberString.replace(/[\.,]/g, ''), 10);
        return cleanedNumber;
    },
    showWaiting: function () { $('.loading-overlay').show(); },
    hideWaiting: function () { $('.loading-overlay').hide(); },

    isLocalEnvironment: function () { return window.location.hostname === "localhost" || window.location.hostname === "127.0.0.1"; },
    domain: "https://beta.pacvn.vn",
    baseUrl: function (url) {
        // Check if the current environment is not local
        if (window.location.hostname !== 'localhost') {
            var baseUrl = "/admin" + url;
            return baseUrl;
        }

        return url;
    },
    //Create a general announcement for all
    //type = red, dark, blue, purple, orange
    createConfirmDialog: function (type, title, content, customAction) {

        var defaultAction = {
            text: 'OK',
            btnClass: 'btn-red',
            action: customAction
        };

        return {
            title: title,
            content: content,
            type: type,
            typeAnimated: true,
            buttons: {
                OK: defaultAction,
                close: {
                    text: 'No',
                },
            }
        };
    },
    //delete all inputs of 1 element (enter only the element id name without the # sign)
    clearInput: function (elementId) {
        $("#" + elementId + " :input").val('');
        $("#" + elementId + " :radio").prop('checked', false);
        $("#" + elementId + " :checkbox").prop('checked', false);
        $("#" + elementId + " select").prop('selectedIndex', -1);
    },
    disableInput: function (elementId) {
        $("#" + elementId + " input[type='text']").prop('disabled', true);
        $("#" + elementId + " input[type='radio']").prop('disable', true);
        $("#" + elementId + " input[type='checkbox']").prop('disabled', true);
        $("#" + elementId + " select").prop('disabled', true);
        $("#" + elementId + " textarea").prop('disabled', true);
    },
    //effect deletes the row on the table (enter only the id element name without the # sign)
    removeRow: function (elementId) {
        var row = $("#" + elementId).closest('tr');

        row.css('background-color', '#FF5733');
        // Add a fadeOut effect
        row.fadeOut(500, function () {
            // Remove the row from the DOM after the fadeOut effect is complete
            row.remove();
            //location.reload(true);
        });
    },
    //show toast
    showToast: function (message, callback) {

        Toastify({
            text: message,
            duration: 1000,
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
            callback: function () {
                setTimeout(callback, 1000);
            }
        }).showToast();

    },
    //show alert
    showAlertError: function (title, text) {
        Swal2.fire({
            icon: 'error', //error - success - warning
            title: title,
            text: text,
        });
    },
    showAlertSuccess: function (title, text) {
        return Swal2.fire({
            icon: 'success',
            title: title,
            text: text,
            showConfirmButton: true
        });
    },
    showAlertWarning: function (title, text) {
        Swal2.fire({
            icon: 'warning', //error - success - warning
            title: title,
            text: text,
        });
    },
    checkRequired: function () {
        var Ok = true;
        $('.required').each(function () {
            var labelFor = $(this).prev('label').attr('for');
            var labelLastTwoChars = labelFor.substring(labelFor.length - 2); // Get the last two characters of the 'for' attribute
            var labelText = $(this).prev('label').text(); // Get the text of the label

            // Check if the value of the input is null or empty
            if ($(this).val().trim() === '') {
                var msg = labelText + ' ' + labelLastTwoChars;
                Utils.showToast('You must enter ' + msg);
                Ok = false;
                return false;
            }
        });

        return Ok;
    },
    ajaxRequest: function (type, url, data, successCallback, errorCallback) {
        $.ajax({
            type: type, //"POST", // or "GET", "PUT", etc. depending on your request type
            url: Utils.baseUrl(url),
            data: data,
            success: function (response) {
                // Call the success callback function with the response
                if (successCallback && typeof successCallback === 'function') {
                    successCallback(response);
                }
            },
            error: function (xhr, status, error) {
                // Call the error callback function with the error details
                if (errorCallback && typeof errorCallback === 'function') {
                    errorCallback(xhr, status, error);
                }
            }
        });
    },
    autoPassword: function (length) {

        var validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_+=[]{}|;:,.<>?"; // Include special characters
        var password = "";
        for (var i = 0; i < length; i++) {
            if (i > 0 && i % 6 == 0) {
                password += "-";
            }
            password += validChars.charAt(Math.floor(Math.random() * validChars.length));
        }
        return password;

    },
    goBackWithRefresh: function (event) {


        if ('referrer' in document) {
            location.replace(document.referrer);
        } else {
            window.history.back();
        }
    },
    initPopOver: function () {
        //$('a[data-toggle="popover"]').popover({
        //    placement: 'top',
        //    html: true
        //});
        //$('a[data-toggle="popover"]').hover(
        //    function () {
        //        $(this).popover('show');
        //        console.log('x');
        //    },
        //    function () {
        //        $(this).popover('hide');
        //        console.log('x');
        //    }
        //);
    },
    initAutoHeight: function (elm) {
        const $textarea = elm;

        const adjustHeight = function () {
            // Reset the height to auto so the scrollHeight can be calculated correctly
            $textarea.css('height', 'auto');
            // Set the height to the scrollHeight
            $textarea.css('height', $textarea[0].scrollHeight + 'px');
        };

        // Attach the input event to adjust the height
        $textarea.on('input', adjustHeight);

        // Trigger the adjustHeight function initially to set the height based on content
        adjustHeight();
    },
    moneyFormat: function (elm) {
        if (elm == null || elm == undefined) {
            $("input.number").each(function () {
                $(this).number(true, 0, ",", ".");
            });
        }
        else {
            $('#' + elm + '.number').each(function () {
                $(this).number(true, 0, ",", ".");
            });
        }
    },
    formatDateTimeToCSharp: function (dateTimeString) {
        //// Split the date-time string into date and time parts
        //var parts = dateTimeString.split(' ');
        //var dateParts = parts[0].split('/');
        //var timePart = parts[1];

        //// Create a new date-time string in MM/dd/yyyy HH:mm:ss format
        //var newDateTimeString = dateParts[1] + '/' + dateParts[0] + '/' + dateParts[2] + ' ' + timePart;
        //return newDateTimeString;

        if (!dateTimeString) return '';

        // Split the date-time string into date and time parts
        const parts = dateTimeString.split(' ');
        if (parts.length < 2) return '';

        const dateParts = parts[0].split('/');
        const timePart = parts[1] || '00:00:00'; // Default time if not present

        if (dateParts.length !== 3) return '';

        // Determine date format based on environment
        const isLocal = Utils.isLocalEnvironment();//isLocalEnvironment();

        console.log(isLocal);

        const newDateTimeString = isLocal
            ? `${dateParts[0]}/${dateParts[1]}/${dateParts[2]} ${timePart}`
            : `${dateParts[1]}/${dateParts[0]}/${dateParts[2]} ${timePart}`;

        return newDateTimeString;
    },
    formatDateToCSharp: function (dateString) {
        //// Split the date-time string into date and time parts
        //var parts = dateString.split(' ');
        //var dateParts = parts[0].split('/');

        //// Create a new date-time string in MM/dd/yyyy HH:mm:ss format
        //var newDateString = dateParts[1] + '/' + dateParts[0] + '/' + dateParts[2];
        //return newDateString;

        if (!dateString) return '';

        // Split the date string into date parts
        const dateParts = dateString.split('/');

        if (dateParts.length !== 3) return '';

        // Determine date format based on environment
        const isLocal = Utils.isLocalEnvironment();

        console.log(isLocal);

        const newDateString = isLocal
            ? `${dateParts[0]}/${dateParts[1]}/${dateParts[2]}`
            : `${dateParts[1]}/${dateParts[0]}/${dateParts[2]}`;

        return newDateString;
    }
}