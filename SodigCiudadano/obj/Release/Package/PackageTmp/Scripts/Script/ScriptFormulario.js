let paisDomicilio = document.querySelector('#paisDomicilio');
let provinciaDomicilio = document.querySelector('#provinciaDomicilio');
let ciudadDomicilio = document.querySelector('#ciudadDomicilio');
let paisOficina = document.querySelector('#paisOficina');
let provinciaOficina = document.querySelector('#provinciaOficina');
let ciudadOficina = document.querySelector('#ciudadOficina');
let cboIdUsoCertificado = document.querySelector('#IdUsoCertificado');
let codigoOtroUsoCertificado = document.querySelector('#codigoOtroUsoCertificado');
let codigoTelefono = document.querySelector('#codigoTelefono');

var codigoComparacion = "0";
paisDomicilio.addEventListener('change', actualizarProvincia);
provinciaDomicilio.addEventListener('change', actualizarCiudad);
paisOficina.addEventListener('change', actualizarProvinciaOficina);
provinciaOficina.addEventListener('change', actualizarCiudadOficina);
cboIdUsoCertificado.addEventListener('change', actualizarUsoCertificado)

$(document).ready(function () {
    $('.alert').fadeIn();
    setTimeout(function () {
        $(".alert").fadeOut();
    }, 10000);

    var codigoTelefonoDomicilio = document.getElementById('telefonoDomicilio');
    var telefonoOficina = document.getElementById('telefonoOficina');
    codigoTelefonoDomicilio.value = codigoTelefono.value;
    telefonoOficina.value = codigoTelefono.value;
    codigoComparacion = codigoTelefono.value;
});

async function actualizarProvincia() {
    await $.get("/DatosUsuario/ObtenerListaProvincias?idPais=" + paisDomicilio.value)
        .done(function (response) {
            if (response.estado) {
                provinciaDomicilio.innerHTML = "";
                let opt = document.createElement("option");
                opt.appendChild(document.createTextNode("Seleccione una opción"));
                /*       provinciaDomicilio.setAttribute('required', 'required')*/
                opt.value = ''
                provinciaDomicilio.appendChild(opt);
                response.lista.forEach(function (element) {
                    opt = document.createElement("option");
                    opt.appendChild(document.createTextNode(element.provincia));
                    opt.value = element.idProvincia;
                    provinciaDomicilio.appendChild(opt);
                });
                ciudadDomicilio.innerHTML = "";
                let opt1 = document.createElement("option");
                opt1.value = ''
                opt1.appendChild(document.createTextNode("Seleccione una opción"));
                ciudadDomicilio.appendChild(opt1);
            } else
                alert(response.mensaje);

        });
}

async function actualizarCiudad() {
    await $.get("/DatosUsuario/ObtenerListaCiudades?idProvincia=" + provinciaDomicilio.value)
        .done(function (response) {
            if (response.estado) {
                ciudadDomicilio.innerHTML = "";
                let opt = document.createElement("option");
                opt.appendChild(document.createTextNode("Seleccione una opción"));
                opt.value = ''
                ciudadDomicilio.appendChild(opt);
                var codigoTelefonoDomicilio = document.getElementById('telefonoDomicilio');
                response.lista.forEach(function (element) {
                    opt = document.createElement("option");
                    opt.appendChild(document.createTextNode(element.ciudad));
                    opt.value = element.idCiudad;
                    ciudadDomicilio.appendChild(opt);
                    codigoTelefonoDomicilio.value = element.codigoProvincia;
                    codigoComparacion = element.codigoProvincia;
                });
            } else
                alert(response.mensaje);
        });
}

async function actualizarProvinciaOficina() {
    await $.get("/DatosUsuario/ObtenerListaProvincias?idPais=" + paisOficina.value)
        .done(function (response) {
            if (response.estado) {
                provinciaOficina.innerHTML = "";
                let opt = document.createElement("option");
                opt.appendChild(document.createTextNode("Seleccione una opción"));
                opt.value = '';
                provinciaOficina.appendChild(opt);
                response.lista.forEach(function (element) {
                    opt = document.createElement("option");
                    opt.appendChild(document.createTextNode(element.provincia));
                    opt.value = element.idProvincia;
                    provinciaOficina.appendChild(opt);
                });
                ciudadOficina.innerHTML = "";
                let opt1 = document.createElement("option");
                opt1.appendChild(document.createTextNode("Seleccione una opción"));
                opt1.value = ''
                ciudadOficina.appendChild(opt1);
            } else
                alert(response.mensaje);

        });
}

async function actualizarCiudadOficina() {
    await $.get("/DatosUsuario/ObtenerListaCiudades?idProvincia=" + provinciaOficina.value)
        .done(function (response) {
            if (response.estado) {
                ciudadOficina.innerHTML = "";
                let opt = document.createElement("option");
                opt.value = '';
                opt.appendChild(document.createTextNode("Seleccione una opción"));
                ciudadOficina.appendChild(opt);
                var codigoTelefonoDomicilio = document.getElementById('telefonoOficina');
                response.lista.forEach(function (element) {
                    opt = document.createElement("option");
                    opt.appendChild(document.createTextNode(element.ciudad));
                    opt.value = element.idCiudad;
                    ciudadOficina.appendChild(opt);
                    codigoTelefonoDomicilio.value = element.codigoProvincia;
                });
            } else
                alert(response.mensaje);
        });
}

async function actualizarUsoCertificado() {
    let panelOtro = document.getElementById('panelOtro')
    $("#IdUsoCertificado option").each(function () {
        if ($(this).attr('value').trim() === codigoOtroUsoCertificado.value.trim()) {
            panelOtro.hidden = false;

        } else {
            panelOtro.hidden = true;
        }
    });

    //if (cboIdUsoCertificado.value.trim() === codigoOtroUsoCertificado.value.trim()) {
    //    panelOtro.hidden = false;
    //} else {
    //    panelOtro.hidden = true;
    //}
}

$('#codigoPostal').on('input', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
    if (this.value.length >= 2) {
        codigo = this.value.substr(0, 2)
        var advCodigoPostal = document.getElementById('advCodigoPostal');
        var codigoProvinciaPostal = provinciaDomicilio.value;
        if (codigoProvinciaPostal.length === 1)
            codigoProvinciaPostal = '0' + codigoProvinciaPostal
        if (codigo !== codigoProvinciaPostal) {
            advCodigoPostal.hidden = false;
            var codigoPostal = document.getElementById('codigoPostal');
            advCodigoPostal.innerText = 'Ingrese un código postal válido';
            codigoPostal.value = '';
        }
        else {
            advCodigoPostal.hidden = true;
        }

    }
});

$('#telefonoDomicilio').on('input', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
});

$('#celularDomicilio').on('input', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
});

$('#telefonoOficina').on('input', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
});

function validarFormulario() {
    var codigoPaisEcuador = document.getElementById('codigoPaisEcuador');
    var idActividadEconomica = document.getElementById('idActividadEconomica');
    var paisDomicilio = document.getElementById('paisDomicilio');
    var provinciaDomicilio = document.getElementById('provinciaDomicilio');
    var ciudadDomicilio = document.getElementById('ciudadDomicilio');
    var direccionDomicilio = document.getElementById('direccionDomicilio');
    var sectorDomicilio = document.getElementById('sectorDomicilio');
    var telefonoDomicilio = document.getElementById('telefonoDomicilio');
    var celularDomicilio = document.getElementById('celularDomicilio');
    var correoPrincipal = document.getElementById('correoPrincipal');
    var correoAlternativo = document.getElementById('correoAlternativo');
    var IdUsoCertificado = document.getElementById('IdUsoCertificado');
    var codigoPostal = document.getElementById('codigoPostal');
    var paisOficina = document.getElementById('paisOficina');
    var provinciaOficina = document.getElementById('provinciaOficina');
    var ciudadOficina = document.getElementById('ciudadOficina');
    var direccionOficina = document.getElementById('direccionOficina');
    var telefonoOficina = document.getElementById('telefonoOficina');
    var otroUso = document.getElementById('otroUso');

    recorrerInputs();

    if (idActividadEconomica.value.trim() === '') {
        mostrarMensaje('advActividadEconomica', 'Seleccione una actividad Económica', idActividadEconomica);
        return false;
    } else if (paisDomicilio.value.trim() === '') {
        mostrarMensaje('advIdPais', 'Seleccione el país de domicilio', paisDomicilio);
        return false;
    } else if (paisDomicilio.value.trim() === codigoPaisEcuador.value.trim()) {
        if (provinciaDomicilio.value.trim() === '') {
            mostrarMensaje('advProvincia', 'Seleccione una provincia', provinciaDomicilio);
            return false;
        } else if (ciudadDomicilio.value.trim() === '') {
            mostrarMensaje('advCiudad', 'Seleccione una ciudad', ciudadDomicilio);
            return false;
        }
    }

    if (direccionDomicilio.value.trim() === '') {
        mostrarMensaje('advDireccion', 'Ingrese una dirección', direccionDomicilio);
        return false;
    } else if (sectorDomicilio.value.trim() === '') {
        mostrarMensaje('advSector', 'Ingrese un sector', sectorDomicilio);
        return false;
    } else if (codigoPostal.value.trim() === '') {
        mostrarMensaje('advCodigoPostal', 'Ingrese un código postal', codigoPostal);
        return false;
    } else if (codigoPostal.value.length < 6) {
        mostrarMensaje('advCodigoPostal', 'Ingrese 6 caracteres', codigoPostal);
        return false;
    } else if (telefonoDomicilio.value.trim() === '') {
        mostrarMensaje('advTelefonoDomicilio', 'Ingrese un teléfono', telefonoDomicilio);
        return false;
    } else if (telefonoDomicilio.value.length < 9) {
        mostrarMensaje('advTelefonoDomicilio', 'El número de teléfono debe tener mínimo 9 caracteres', telefonoDomicilio);
        return false;
    } else if (celularDomicilio.value.trim() === '') {
        mostrarMensaje('advCelularDomicilio', 'Ingrese un Celular', celularDomicilio);
        return false;
    } else if (correoPrincipal.value.trim() === '') {
        mostrarMensaje('advCorreoPrincipal', 'Ingrese un correo electrónico', correoPrincipal);
        return false;
    } else if (!validarEmail('correoPrincipal')) {
        mostrarMensaje('advCorreoPrincipal', 'Ingrese un correo electrónico válido', correoPrincipal);
        return false;
    } else if (correoAlternativo.value.trim() === '') {
        mostrarMensaje('advCorreoAlternativo', 'Ingrese un correo electrónico alternativo', correoAlternativo);
        return false;
    } else if (!validarEmail('correoAlternativo')) {
        mostrarMensaje('advCorreoAlternativo', 'Ingrese un correo electrónico válido', correoAlternativo);
        return false;
    } else if (paisOficina.value.trim() === '') {
        mostrarMensaje('advPaisOficina', 'Seleccione un país', paisOficina);
        return false;
    } else if (paisOficina.value.trim() === codigoPaisEcuador.value.trim()) {

        if (provinciaOficina.value.trim() === '') {
            mostrarMensaje('advProvinciaOficina', 'Seleccione una provincia', provinciaOficina);
            return false;
        } else if (ciudadOficina.value.trim() === '') {
            mostrarMensaje('advCiudadOficina', 'Seleccione una ciudad', ciudadOficina);
            return false;
        }
    }

    if (telefonoDomicilio.value.length > 2) {

        var codigo = telefonoDomicilio.value.substr(0, 2);
        if (codigo === '09') {
            if (telefonoDomicilio.value.length < 10) {
                mostrarMensaje('advTelefonoDomicilio', 'El número de teléfono debe tener mínimo 10 caracteres', telefonoDomicilio);
                return false;
            }
        }
        else if (codigo != codigoComparacion) {
            mostrarMensaje('advTelefonoDomicilio', 'El código es diferente a la provincia de domicilio', telefonoDomicilio);
            return false;
        }
    }


    if (direccionOficina.value.trim() === '') {
        mostrarMensaje('advDireccionOficina', 'Ingrese una dirección', direccionOficina);
        return false;
    } else if (telefonoOficina.value.trim() === '') {
        mostrarMensaje('advTelefonoOficina', 'Ingrese un teléfono', telefonoOficina);
        return false;
    } else if (telefonoOficina.value.length < 9) {
        mostrarMensaje('advTelefonoOficina', 'El número de teléfono debe tener 9 caracteres', telefonoOficina);
        return false;
    } else if (IdUsoCertificado.value.trim() === '') {
        mostrarMensaje('advUsoCertificado', 'Seleccione el uso del certificado', IdUsoCertificado);
        return false;
    } else if (IdUsoCertificado.value.trim() === '35') {
        if (otroUso.value.trim() === '') {
            mostrarMensaje('advOtroUso', 'Ingrese el uso del certificado', otroUso);
            return false;
        }
    }

    if (telefonoDomicilio.value.length == 9) {
        telefonoDomicilio.value = "0" + telefonoDomicilio.value;
    }    
    $("#IdUsoCertificado option").each(function () {        
         if ($(this).attr('value').trim() === '35') {
            if (otroUso.value.trim() === '') {
                mostrarMensaje('advOtroUso', 'Ingrese el uso del certificado', otroUso);
                return false;
            }
        }
    });
    $('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);   
}


function recorrerInputs() {
    let list = ['idActividadEconomica', 'paisDomicilio', 'provinciaDomicilio', 'ciudadDomicilio', 'direccionDomicilio', 'sectorDomicilio', 'telefonoDomicilio', 'celularDomicilio', 'correoPrincipal', 'correoAlternativo', 'codigoPostal', 'paisOficina', 'provinciaOficina', 'ciudadOficina', 'direccionOficina', 'telefonoOficina', 'otroUso'];

    for (var i of list) {
        var elemento = document.getElementById(i);
        elemento.setAttribute('class', 'form-control');
    }
}

function mostrarMensaje(item, mensaje, input) {
    let list = ['advActividadEconomica', 'advIdPais', 'advProvincia', 'advCiudad', 'advDireccion', 'advSector', 'advTelefonoDomicilio', 'advCelularDomicilio', 'advCorreoPrincipal', 'advCorreoAlternativo', 'advUsoCertificado', 'advCodigoPostal', 'advPaisOficina', 'advProvinciaOficina', 'advCiudadOficina', 'advDireccionOficina', 'advTelefonoOficina', 'advUsoCertificado', 'advOtroUso'];

    for (var i of list) {
        if (i.trim() === item) {
            var elemento = document.getElementById(i);
            console.log(elemento);
            input.setAttribute('class', 'form-control is-invalid');
            elemento.innerText = mensaje;
            elemento.hidden = false;
        } else {
            var elemento = document.getElementById(i);
            elemento.hidden = true;
        }
    }
}

function validarEmail(elemento) {
    var texto = document.getElementById(elemento).value;
    var regex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;

    if (!regex.test(texto)) {
        return false;
    } else {
        return true;
    }

}
