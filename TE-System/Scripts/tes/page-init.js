$(document).delegate('a, :button', 'click', function (e) {
    var lastClicked = $.data(this, 'lastClicked'),
        now = new Date().getTime();

    if (lastClicked && (now - lastClicked < 1000)) {
        e.preventDefault();
    } else {
        $.data(this, 'lastClicked', now);
    }
});

$(document).ready(function () {          
    $('#txtSearchWeek').datepicker({
        title: 'Pick A Date To Search',
        todayBtn: true,
        calendarWeeks: true,
        orientation: 'top auto',
        autoclose: true,
        clearBtn: true,
    });
});  