let modal = document.getElementById('miModal');
let textModal = document.getElementById('textModal');
let advertencia = document.getElementById('advertencia');
let idTipoElector = document.getElementById('idTipoElector');

function abrirModal() {    
    var radiovalue = document.getElementsByName('idTipoElector');
    var seleccionado = false;
    var idSeleccionado = 0;
    var textSeleccionado = "";

    for (var opcion of radiovalue) {
        if (opcion.checked) {
            advertencia.hidden = true;
            seleccionado = true;
            idSeleccionado = opcion.value
            let textLabel = document.getElementById('label_' + idSeleccionado)
            textSeleccionado = textLabel.textContent.trim();
        }
    }

    if (seleccionado) {
        idTipoElector.value = idSeleccionado
        if (idSeleccionado !== '0') {
            modal.style.display = 'block';
            var tituloModal = document.getElementById('tituloModal')
            tituloModal.textContent = "Confirmación";
            textModal.textContent = `Ha seleccionado "${textSeleccionado}" ¿Desea Continuar?`;
            bloquearBarraDesplazamiento();
            return false;
        }
    }
    else {
        advertencia.hidden = false;
        return false;
    }
    
}

async function tipoElector(idTipoElector) {
    await $.post('/VerificarExcepcion/Index', {
        idTipoElector: idTipoElector
    }, async function (response) {
        console.log(response);
        if (response.status) {
            if (response.response.Success) {
                $.post('/Auth/GenerarOtp', {
                }, async function (response) {
                    if (response.status) {
                        window.location.replace('/VerificacionOtp');
                    }
                })
            }
        }
    })
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