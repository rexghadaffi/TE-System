$(document).ready(function () {
    $.ajax("/Entry/IndexData", {      
        success: function (data) {
            $('#panelBody').empty().append(data);
            $('.table-record').bootstrapTable({
                pagination: true,
                classes: 'table table-hover table-condensed',
                search: true,
                showColumn: true,
                uniqueId: 'ID',
                toolbarAlign: 'left',
                toolbar: '#weekNavbar',
                pageList: [10, 20, 50, 100],
            });
            
        },
        error: function (data) {
            $('#loadingbarInitial').hide();
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });   
});

function ShowEditModal(id) {
    $('#editModalContainer').empty();
    $('#editModal').modal("show");  
    $('#loadingbar').show();   
    $.ajax("/Entry/Edit" ,{     
        data: { tid: id },
        success: function (data) {
            $('#editModalContainer').append(data);           
            $('form').removeData("validator");

            //Refresh drop down list
            $.validator.unobtrusive.parse(document);
          
            $('.selectize').selectize();
            $('#loadingbar').hide();            
        },
        error: function (data) {           
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
}

function ShowAddModal() {
    $('#addModal').modal('show');   
    $('#loadingbarAdd').show();
    $('#addModalContainer').empty();
    $.ajax("/Entry/RenderModal",
    {         
        success: function (data) {
            $('#addModalContainer').append(data);
            $('form').removeData("validator");

            //Refresh the validators
            $.validator.unobtrusive.parse(document);
            //Refresh drop down list
            $('.selectize').selectize();
            $('#loadingbarAdd').hide();
        },
        error: function (data) {
            $.notify('<strong>System Error!</strong> server is not responding.. <br/> "' + data + '" .', { type: 'danger' });
        }
    });
}

function reinitpopover() {
    $('[data-toggle="popover"]').popover({
        html: true,
        placement: 'left',
        trigger: 'hover',
    });
    $('[data-toggle="tooltip"]').tooltip();
}

function PreviousWeek() {   
    $.ajax({
        url: "/Entry/PreviousWeek",
        type: 'Get',       
        cache: false,       
        success: function (data) {
            $('#tblEntriesBody').empty().append(data);
        },
        error: function () {
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });   
}

$('#btnSearchWeekEntry').on('click', function () {
    var inputVal = $('#txtSearchWeek').val();

    if (inputVal != "") {
        window.location.href = "/Entry/SearchWeek?date=" + inputVal;
    }
    else {
        $.notify('<strong>Invalid Input!</strong> Select a valid week number.', { type: 'danger' });
    }
});

function ShowRemarks(recordID) {
    $.ajax({       
        url: "/Entry/GetRemarksForEntry",
        type: 'Get',
        cache: false,
        data: { id: recordID },
        success: function (data) {
            //Render Sidebar
            $('#sidebar').empty().append(data);
            reinitpopover();
            //Show Sidebar
            $('#sidebargo').click();
        },
        error: function () {
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
}
