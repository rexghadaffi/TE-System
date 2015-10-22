$(document).ready(function () {
    $('#tblStaffEntries').bootstrapTable({
        url: '/ValidateEntry/Data',
        cache: false,
        pagination: true,
        classes: 'table table-hover table-condensed',
        search: true,
        showColumn: true,
        sidePagination: 'server',
        queryParamsType: 'Else',
        uniqueId: 'ID',
        toolbarAlign: 'left',
        toolbar: '#staffEntriesToolbar',
        pageList: [10, 20, 50, 100],
        onClickRow: function (row, $element) {
            var key = row["ID"];
            ShowSideBar(key);
        }
    });
});

$('#btnSearchWeekValidateEntry').on('click', function () {
    var inputVal = $('#txtSearchWeek').val();

    if (inputVal != "") {
        window.location.href = "/ValidateEntry/SearchWeek?date=" + inputVal;
    }
    else {
        $.notify('<strong>Invalid Input!</strong> Select a valid week number.', { type: 'danger' });
    }
});
function ShowSideBar(recordID) {   
    $('#sidebargo').click();
    $.ajax({
        // Get Student PartialView
        url: "/ValidateEntry/GetEntryDetail",
        type: 'Get',
        cache: false,
        data: { id: recordID },
        success: function (data) {
            //Render Sidebar
            $('#sidebar').empty().append(data);          
            //Initialize Tooltip
            InitTooltip();
            $('#loadingSidebar').hide();
        },
        error: function () {
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
}

function InitTooltip() {
    $('[data-toggle="tooltip"]').tooltip();
}
function CloseSideBar() {
    $('#sidebarclose').click();
}
function EntryValid() {
    var id = $('#ID').val();
    $.ajax({
        // Dim Entry as Valid
        url: "/ValidateEntry/AcceptEntry",
        type: 'Get',
        data: { recordID: id },
        success: function (data) {
            $('#remarksModal').modal('hide');
            alert(data);            
            // Close Sidebar
            $('#sidebargo').click();
        },
        error: function () {
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
}
function FlagBillability() {
    $('#flag').val('/ValidateEntry/FlagBillability');
}
function FlagInfo() {
    $('#flag').val('/ValidateEntry/FlagInfo');
}
function FlagEntryAjax(form) {
    var recordID = $('#ID').val();
    var urlRequest = $('#flag').val();

    $("#RemarksForm").attr('action', '/ValidateEntry/AddRemarks?recordID=' + recordID);

    $.ajax({
        // Dim Entry as Valid
        url: urlRequest,
        type: 'Get',
        data: { entry: recordID },
        success: function (data) {
            form.submit();
        },
        error: function () {
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
}
function ShowRemarksForm() {
    // shows the modal for remarks
    $('#remarksModal').modal('show');
    var id = $('#ID').val();
    $.ajax({
        // Dim Entry as Valid
        url: "/ValidateEntry/RemarksForm",
        type: 'Get',
        data: { recordID: id },
        success: function (data) {
            $('#loadingRemarks').hide();
            $('#RemarksFormContainer').empty().append(data);
            $('form').removeData("validator");
            //Refresh drop down list
            $.validator.unobtrusive.parse(document);
            // bind custom submit handler
            $('form').data("validator").settings.submitHandler = function (form) {
                FlagEntryAjax(form);
            };
        },
        error: function () {
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
}
