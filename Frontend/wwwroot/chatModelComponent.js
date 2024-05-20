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


window.openNav = function() {
    document.getElementById("mySidebar").style.width = "250px";
    document.getElementById("main").style.marginLeft = "250px";
};

window.closeNav = function() {
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginLeft= "0";
};