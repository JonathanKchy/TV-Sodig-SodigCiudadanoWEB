let modal = document.getElementById('miModal');

$(document).ready(function () {
    $('.alert').fadeIn();
    setTimeout(function () {
        $(".alert").fadeOut();
    }, 15000);
});


function validar() {
    

    alertify.confirm("Confirmación", "¿Está seguro que los datos del nombre son correctos?.",
        function () {
            $('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>Procesando...').attr('disabled', true);
            document.getElementById("formularioDatosNombres").submit();
        },
        function () {
            
        }
    ).set('labels', { ok: 'Confirmar', cancel: 'Cancelar' });

  
    return false;
    
}


function abrirModal() {       
    modal.style.display = 'block';
    var tituloModal = document.getElementById('tituloModal')
    tituloModal.textContent = "Confirmación";    
    bloquearBarraDesplazamiento();
}

function cerrarModal() {
    modal.style.display = 'none';
    activarBarraDesplazamiento();
}

function bloquearBarraDesplazamiento() {
    $('body').css({ 'overflow': 'hidden' });
    $(document).bind('scroll', function () {
        window.scrollTo(0, 0);
    });
}

function activarBarraDesplazamiento() {
    $(document).unbind('scroll');
    $('body').css({ 'overflow': 'visible' });
}
