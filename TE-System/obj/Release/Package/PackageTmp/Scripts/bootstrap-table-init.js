$(document).ready(function () {
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
});