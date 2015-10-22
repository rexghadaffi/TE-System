function ShowEditModal(id) {
    //alert(id);
    $('#editModalContainer').empty();
    $('#editModal').modal("show");  
    $('#loadingbar').show(); 
  
    $.ajax({
        // Get Student PartialView
        url: "/Entry/Edit",
        type: 'Get',
        cache: false,
        data: { tid: id },
        success: function (data) {
            $('#editModalContainer').append(data);
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

function ShowAddModal() {
    $('#addModal').modal('show');   
    $('#loadingbarAdd').show();
    $('#addModalContainer').empty();
    $.ajax({
        // Get Student PartialView
        url: "/Entry/RenderModal",
        type: 'Get',
        success: function (data) {
            $('#addModalContainer').append(data);
            $('form').removeData("validator");

            //Refresh the validators
            $.validator.unobtrusive.parse(document);
            //Refresh drop down list
            $('.selectize').selectize();
            $('#loadingbarAdd').hide();
        },
        error: function () {
            $('#editModal').modal("hide");
            $.notify('<strong>System Error!</strong> Something happened and the server is not responding.', { type: 'danger' });
        }
    });
}