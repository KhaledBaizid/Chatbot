window.showModalAndFocus = function() {
    $('#myModal').on('shown.bs.modal', function () {
        $('#myInput').trigger('focus');
    });
    $('#myModal').modal('show');
}


window.scrollToBottom = function(element) {
    element.scrollTop = element.scrollHeight;
}


window.scrollToBottom2 = function(className) {
    var element = document.getElementsByClassName(className)[0];
    if (element) {
        element.scrollTop = element.scrollHeight;
    } else {
        console.error('Element with class ' + className + ' not found');
    }
}