$(document).ready(function () {
    $('.alert').fadeIn();
    setTimeout(function () {
        $(".alert").fadeOut();
    }, 5000);
});

function validar() {
    var codigo = $('#codigoOCR').val();
    if (codigo.trim() == "") {
        var elemento = document.getElementById("mensajecodigo");
        elemento.hidden = false;
        return false;
    }
    $('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
}