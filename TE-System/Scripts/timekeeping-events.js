$(document).ready(function () {
    // Init Table
    $('#tblInvalidEntries').bootstrapTable({
        url: '/ValidateTime/Data',
        pagination: true,
        classes: 'table table-hover table-condensed',
        showColumn: true,
        sidePagination: 'server',
        queryParamsType: 'Else',
        uniqueId: 'ID',
        toolbarAlign: 'left',
        toolbar: '#staffEntriesToolbar',
        pageList: [10, 20, 50, 100],
        onDblClickRow: function (row, $element) {
            var key = row["ID"];
            ShowEditTime(key);
        }
    });
    //  Hide Loadingbar
    $('#loadingbarTable').hide();
    // Fetch Users
    $.ajax({
        // Get Student PartialView
        url: "/ValidateTime/FlaggedUsers",
        type: 'Get',
        cache: false,
        success: function (data) {
            $('#flaggedStaffContainer').empty().append(data);          
            $('#loadingbarSide').hide();
        },
        error: function () {
            $('#editModal').modal("hide");
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
});

function ViewEntries(id) {
    $('#dummyRow').empty();
    $('#loadingbarTable').show();   
    $.ajax({
        // Get Student PartialView
        url: "/ValidateTime/ViewFlaggedUserEntry",
        type: 'Get',
        cache: false,
        data: { userID: id },
        success: function (data) {
            $('#staffEntriesContainer').empty().append(data);         
        },
        error: function () {
            $('#editModal').modal("hide");
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
}

$('#btnSearchWeekValidateBillability').on('click', function () {
    var inputVal = $('#txtSearchWeek').val();

    if (inputVal != "") {
        window.location.href = "/ValidateTime/SearchWeek?date=" + inputVal;
    }
    else {
        $.notify('<strong>Invalid Input!</strong> Select a valid week number.', { type: 'danger' });
    }
});

function ShowEditTime(id, isValid) {  
    if (isValid == null || isValid == '') {
        $('#editModal').modal("show");
        $.ajax({
            // Get Student PartialView
            url: "/ValidateTime/Edit",
            type: 'Get',
            data: { tid: id },
            cache: false,
            success: function (data) {
                $('#editModalContainer').empty().append(data);
                $('form').removeData("validator");

                //Refresh drop down list
                $.validator.unobtrusive.parse(document);
                $('.selectize').selectize();
                $('#loadingbar').hide();
            },
            error: function () {
                $('#editModal').modal("hide");
                $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
            }
        });
    }
    else { $.notify('<strong>Unable to Edit!</strong> You cannot edit an entry that has not been flagged.', { type: 'danger' }); }   
}