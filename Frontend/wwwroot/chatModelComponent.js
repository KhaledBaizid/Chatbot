window.showModalAndFocus = function() {
    $('#myModal').on('shown.bs.modal', function () {
        $('#myInput').trigger('focus');
    });
    $('#myModal').modal('show');
}


window.scrollToBottom = function(element) {
    element.scrollTop = element.scrollHeight;
}