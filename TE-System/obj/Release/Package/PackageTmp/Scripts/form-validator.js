// keypress numeric validator 
function Numeric(e) {       
    var regex = new RegExp("^[\.\d{0-9}\b\040]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
}
// keypress alphabet validator 
function Alphabet(e) {
    var regex = new RegExp("^[a-zA-Z.\b\040]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function NotifySuccess()
{
    $.notify('<strong>Record Saved!</strong> You have sucessfuly saved a record.', { type: 'success' });
}