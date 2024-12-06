$(document).ready(function () {
    $('#cbo_Type').on('change', function () {
        const selectedValue = $(this).val();
        const $listItem = $('#packageDetail');

        if (selectedValue === "27") {
            $listItem.show();
        } else {
            $listItem.hide();
            tinymce.get("txt_Itinerary").setContent("");
            $('#cbo_Childs').val([]).prop('selected', false);
            $('#articleTab .nav-link').removeClass('active');
            $('#articleTab .nav-link').first().addClass('active');

            $('.tab-pane').removeClass('show active');
            $('.tab-pane').first().addClass('show active');
        }
    });
});

FilePond.registerPlugin(
    FilePondPluginFileRename,
    FilePondPluginFileValidateSize,
    FilePondPluginFileValidateType,
    FilePondPluginImagePreview,
)

//FilePond.setOptions({
//    fileRenameFunction: (file) =>
//        new Promise((resolve) => {
//            resolve(window.prompt('Enter new filename', file.name));
//        }),
//});

document.querySelectorAll(".tiny-editor").forEach(function (element) {
    tinymce.init({
        selector: '#' + element.id,
        toolbar_mode: 'wrap',
        plugins: 'preview importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media codesample table charmap pagebreak nonbreaking anchor insertdatetime advlist lists wordcount help charmap quickbars emoticons accordion',
        editimage_cors_hosts: ['picsum.photos'],
        menubar: 'file edit view insert format tools table help',
        toolbar: "undo redo | accordion accordionremove | blocks fontfamily fontsize | bold italic underline strikethrough | align numlist bullist | link image | table media | lineheight outdent indent| forecolor backcolor removeformat | charmap emoticons | code fullscreen preview | save print | pagebreak anchor codesample | ltr rtl",
        height: "680",
        // Add the image_dimensions option
        image_dimensions: true,
        // Set max_width to 100% for automatically adjusting the width
        image_max_width: "100%",
        //content_style: 'body { font-size: 14px; }',
        //font_size_formats: "8px 10px 12px 14px 18px 24px 36px"

        relative_urls: false, // Set relative_urls to false
        remove_script_host: false,
        convert_urls: true,
    });
});

const $filePond = FilePond.create(document.getElementById("txt_UrlImage"), {
    credits: null,
    allowImagePreview: true,
    allowMultiple: false,
    allowFileEncode: false,
    required: false,
    storeAsFile: true,
    maxFileSize: '5MB',
    maxTotalFileSize: '50MB',
    allowFileSizeValidation: true,
    acceptedFileTypes: ['image/*'],
});

FilePond.create(document.getElementById("txt_Icon"), {
    credits: null,
    allowImagePreview: true,
    allowMultiple: false,
    allowFileEncode: false,
    required: false,
    storeAsFile: true,
    maxFileSize: '5MB',
    maxTotalFileSize: '50MB',
    allowFileSizeValidation: true,
    acceptedFileTypes: ['image/*'],
});

document.querySelectorAll(".multiple-files-filepond").forEach(function (element) {
    FilePond.create(element, {
        credits: null,
        allowImagePreview: true,
        allowMultiple: true,
        allowFileEncode: false,
        required: false,
        storeAsFile: true,
        allowFileRename: false,
        maxFileSize: '15MB',
        maxTotalFileSize: '50MB',
        allowFileSizeValidation: true,
        labelMaxFileSizeExceeded: 'Maximum file size should not exceed 15MB',
        labelMaxTotalFileSizeExceeded: 'All files must not exceed 60MB',
    })
});

$(".select2").select2({
    width: "100%",
    placeholder: "--Choose--"
});

$(function () {
    initSelectLink();

    //If you want to check whether the Vietnamese article is news or not, you must remove it
    if ($("#txt_TypeArticle").val() === 'IsArticle') {
        $("#txt_Summary").removeClass('required');
    }

    var $titleVn = $("#txt_TitleVn");
    var $titleMeta = $("#txt_MetaTitle");

    //automatically add unsigned title
    $titleVn.on("keyup", function () {
        var inputValue = $titleVn.val();
        //delete mark
        var withoutDiacritics = removeDiacritics(inputValue);
        //delete special characters
        var withLine = withoutDiacritics
            .replace(/\s+/g, '-')
            .replace(/[^a-zA-Z0-9-]/g, '');
        $titleMeta.val(withLine);
    });

    //When adding a new id always = 0, it will automatically check for publishing
    var id = $("#txt_ID").val();
    if (parseInt(id) === 0) {
        $("#chk_Status").prop("checked", true);
        $("#txt_Status").val(1);
        $("#txt_IsParent").val(0);
    }
    else {
        // Add the value of the publish button (Status) to HiddenStatus
        $("#txt_Status").val($("#chk_Status").prop("checked") ? 1 : 0);
        $("#txt_IsParent").val($("#chk_IsParent").prop("checked") ? 1 : 0);
    }

    $("#chk_Status").on("change", function () {
        $("#txt_Status").val($(this).prop("checked") ? 1 : 0);
    });

    $("#chk_IsParent").on("change", function () {
        $("#txt_IsParent").val($(this).prop("checked") ? 1 : 0);
    });

    //Open select cbo_Category again before submitting because the disabled attribute will not capture the value when post submit
    $("#btn_Save").click(function () {

        var contentVn = tinymce.get("txt_ContentVn").getContent(); // Ensure you have the correct ID
        $('#txt_ContentVn').val(contentVn);

        var editor = tinymce.get("txt_Itinerary");

        if (editor) {
            var Itinerary = editor.getContent();
            $('#txt_Itinerary').val(Itinerary);
        }

        const selectElement = document.getElementById("cbo_Childs");
        const selectedValues = Array.from(selectElement.selectedOptions).map(option => option.value);
        const joinedValues = selectedValues.join('|');

        $('#txt_Childs').val(joinedValues);

        var Ok = Utils.checkRequired();
        if (Ok) {
            $("#cbo_Category").prop("disabled", false);
            var date = $('#txt_DateExp').val(); // Assuming your date input has id 'yourDateInput'
            var time = '23:59:59';
            var dateTimeString = date + ' ' + time;
            var newdate = Utils.formatDateTimeToCSharp(dateTimeString)
            $('#txt_DateExp').val(newdate);

            var formData = new FormData($("#frmPost")[0]);
            var url = Utils.baseUrl("/website/SaveArticle");

            console.log(formData);

            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                contentType: false, // Important for file uploads
                processData: false, // Important for file uploads,
                beforeSend: function (xhr) {
                    Utils.showWaiting();
                },
                success: function (res) {
                    isDirty = false;
                    Utils.showToast('Updated successfully', 'success');
                    setTimeout(function () {
                        window.location.href = res; // Redirect after 1 seconds
                    }, 1000);
                },
                error: function (xhr, status, error) {
                    Utils.hideWaiting();
                    alert("An error occurred while saving the article: " + error);
                }
            });
        }
    });
});

function initSelectLink() {

    $('#cbo_Link').select2({
        ajax: {
            url: Utils.baseUrl('/website/SearchArticleForSelect'), // Backend API
            dataType: 'json',
            delay: 250, // Delay for better UX
            data: function (params) {
                return {
                    searchTerm: params.term // Pass the user input as search term
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        var link = `${item.metaTitle}/${item.id}`
                        return {
                            id: link,
                            text: item.titleVn
                        };
                    })
                };
            },
            cache: true
        },
        placeholder: 'Search posts (enter at least 5 characters)', // Optional placeholder
        minimumInputLength: 5 // Minimum characters to start searching
    });

    //get title làm text đã chọn của link
    var title = $("#txt_TitleVn").val();
    var link = $("#txt_Link").val();
    if (title.length > 0 && link.length > 0) {
        const option = `<option value='${link}'>${title}</option>`;
        $('#cbo_Link').append(option).trigger('change');
    }
}

function openLog(data, createdOn, createdName) {

    // Define the specific properties for Vietnamese and English
    const vnFields = {
        'Title': 'TitleVn',
        'Summary': 'SummaryVn',
        'Content': 'ContentVn'
    };

    const enFields = {
        'Title': 'TitleEn',
        'Summary': 'SummaryEn',
        'Content': 'ContentEn'
    };

    // Create the base HTML for the new tab with VN and EN tabs
    let newTabHtml = `
        <html>
        <head>
            <title>Comparison Results</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 20px; }
                .tabs { display: flex; }
                .tab { padding: 10px 20px; cursor: pointer; border: 1px solid #ccc; }
                .tab.active { background-color: #f0f0f0; border-bottom: none; }
                .content { padding: 20px; border: 1px solid #ccc; border-top: none; display: none; }
                .content.active { display: block; }
                h3 { margin: 10px 0; }
            </style>
            <script>
                function showTab(tabId) {
                    document.querySelectorAll('.content').forEach(c => c.classList.remove('active'));
                    document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
                    document.getElementById(tabId).classList.add('active');
                    document.querySelector('[data-tab="' + tabId + '"]').classList.add('active');
                }
            </script>
        </head>
        <body>
            <h3>Last update ${createdOn} by ${createdName}</h3>
            <div class="tabs">
                <div class="tab active" data-tab="vnContent" onclick="showTab('vnContent')">Vietnamese</div>
                <div class="tab" data-tab="enContent" onclick="showTab('enContent')">English</div>
            </div>
            <div id="vnContent" class="content active">
                <h3>Vietnamese Content</h3>`;

    // Loop through Vietnamese fields and add them to the VN tab
    for (const [label, propName] of Object.entries(vnFields)) {
        let value = data[propName] || '';
        newTabHtml += `<p><strong>${label}</strong> <br> ${value}</p>`;
    }

    newTabHtml += `
            </div>
            <div id="enContent" class="content">
                <h3>English Content</h3>`;

    // Loop through English fields and add them to the EN tab
    for (const [label, propName] of Object.entries(enFields)) {
        let value = data[propName] || '';
        newTabHtml += `<p><strong>${label}</strong> <br> ${value}</p>`;
    }

    newTabHtml += `
            </div>
        </body>
        </html>
    `;

    // Open a new tab and write the generated HTML content
    const newTab = window.open();
    newTab.document.write(newTabHtml);
    newTab.document.close();
}

window.addEventListener('beforeunload', function (event) {
    event.stopImmediatePropagation();
});