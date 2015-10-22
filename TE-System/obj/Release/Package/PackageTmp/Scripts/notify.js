function InvalidWeek() {
    $.notify('<strong>Unable to Edit!</strong> You cannot edit past records.', { type: 'danger' });
}

function InvalidSearch() {
    $.notify('<strong>Invalid Input!</strong> Select a valid week number.', { type: 'danger' });
}

function InvalidEntry() {
    $.notify('<strong>Warning!</strong> One of your entries is invalid, please modify them immediately.', { type: 'danger' });
}