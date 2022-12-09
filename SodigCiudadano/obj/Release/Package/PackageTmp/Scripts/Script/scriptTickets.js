$(document).ready(function () {
    $('.alert').fadeIn();
    setTimeout(function () {
        $(".alert").fadeOut();
    }, 15000);
    
});


$(document).ready(function () {
    $("#mostrarmodal").modal("show");
});

$('#telefono').on('input', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
});

$('#nombre').on('input', function () {
    validarTextoEntrada(this, "[a-z ]")
});

function validarTextoEntrada(input, patron) {
    var texto = input.value
    var letras = texto.split("")
    for (var x in letras) {
        var letra = letras[x]
        if (!(new RegExp(patron, "i")).test(letra)) {
            letras[x] = ""
        }
    }
    input.value = letras.join("")
}

function validarCedula(numeroCedula) {
    var cad = numeroCedula;
    var total = 0;
    var longitud = cad.length;
    var longcheck = longitud - 1;

    if (cad !== "" && longitud === 10) {
        for (i = 0; i < longcheck; i++) {
            if (i % 2 === 0) {
                var aux = cad.charAt(i) * 2;
                if (aux > 9) aux -= 9;
                total += aux;
            } else {
                total += parseInt(cad.charAt(i)); // parseInt o concatenará en lugar de sumar
            }
        }

        total = total % 10 ? 10 - total % 10 : 0;

        if (cad.charAt(longitud - 1) == total) {
            return 1;
        } else {
            return 0;
        }
    }
}


function validarFormulario() {
    let identificacion = document.getElementById('identificacion');
    let telefono = document.getElementById('telefono');
    let nombre = document.getElementById('nombre');
    let mail = document.getElementById('mail');
    let detalle = document.getElementById('detalle');
    let idTipo = document.getElementById('idTipo');
    let archivo = document.getElementById('archivo');
    mail.value = mail.value.trim();
    recorrerInputs();
    var esnumero = false;
    try {
        const regex = /^[0-9]*$/;
        const onlyNumbers = regex.test(identificacion.value);
        if (onlyNumbers ) {
            esnumero = true;
        }
        else {
            esnumero = false;
        }
        
    } catch (e) {
        esnumero = false;
    }   

    if (identificacion.value.trim() === '') {
        mostrarMensaje('advIdentificacion', 'Por favor, ingrese la identificación.', identificacion);
        return false;
    } else if (esnumero &&  identificacion.value.length > 10) {
        mostrarMensaje('advIdentificacion', 'Por favor, ingrese máximo 10 caracteres', identificacion);
        return false;
    } else if (esnumero &&  identificacion.value.length < 10) {
        mostrarMensaje('advIdentificacion', 'Por favor, ingrese mínimo 10 caracteres', identificacion);
        return false;
    }  else if (telefono.value.trim() === '') {
        mostrarMensaje('advTelefono', 'Por favor, ingrese el teléfono ', telefono);
        return false;
    } else if (telefono.value.length > 10) {
        mostrarMensaje('advTelefono', 'Solo se permite hasta 10 caracteres', telefono);
        return false;
    } else if (telefono.value.length < 9) {
        mostrarMensaje('advTelefono', 'Ingrese un teléfono con el código de la provincia', telefono);
        return false;
    } else if (nombre.value.trim() === '') {
        mostrarMensaje('advNombre', 'Por favor, ingrese el nombre.', nombre);
        return false;
    } else if (mail.value.trim() === '') {
        mostrarMensaje('advCorreo', 'Por favor, ingrese el correo.', mail);
        return false;
    } else if (detalle.value.trim() === '') {
        mostrarMensaje('advDetalle', 'Por favor, ingrese el detalle.', detalle);
        return false;
    } else if (idTipo.value.trim() === '') {
        mostrarMensaje('advTipo', 'Por favor, seleccion el tipo.', idTipo);
        return false;
    } else if (!validarEmail('mail')) {
        mostrarMensaje('advCorreo', 'Por favor, ingrese un correo válido.', mail);
        return false;
    }

    if (telefono.value.length > 2) {
        var codigo = telefono.value.substr(0, 2);
        if (codigo === '09') {
            if (telefono.value.length < 10) {
                mostrarMensaje('advTelefono', 'El número de teléfono debe tener mínimo 10 caracteres', telefono);
                return false;
            }
        }
        else {
            if (telefono.value.length > 9) {
                mostrarMensaje('advTelefono', 'El número de teléfono debe tener máximo 9 caracteres incluido el código de la provincia', telefono);
                return false;
            }
        }
    }

     if (esnumero && identificacion.value.length == 10) {
        if (validarCedula(identificacion.value) == 0) {
            mostrarMensaje('advIdentificacion', 'Por favor, ingrese una cédula válida', identificacion);
            return false;
        }
    }

    $('#btn-submit').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
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

function recorrerInputs() {
    let list = ['identificacion', 'telefono', 'nombre', 'mail', 'detalle', 'idTipo', 'archivo'];

    for (var i of list) {
        var elemento = document.getElementById(i);
        elemento.setAttribute('class', 'form-control');
    }
}

function mostrarMensaje(item, mensaje, input) {
    let list = ['advIdentificacion', 'advTelefono', 'advNombre', 'advCorreo', 'advDetalle', 'advTipo'];

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

function validarFormularioBuscar() {
    let identificacion = document.getElementById('identificacion');
    let nTicket = document.getElementById('nTicket');

    recorrerInputsBuscar();

    if (nTicket.value.trim() === '' && identificacion.value.trim() === '') {
        mostrarMensajeBuscar('advTicket', 'Por favor, ingrese un campo de búsqueda', nTicket);
        mostrarMensajeBuscar('advIdentificacion', 'Por favor, ingrese un campo de búsqueda', identificacion);
        return false;
    }
    //} else if (identificacion.value.trim() === '') {
    //    mostrarMensajeBuscar('advIdentificacion', 'Por favor, ingrese la identificación', identificacion);
    //    return false;
    //} 

    $('#btn-submit').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
}

function recorrerInputsBuscar() {
    let list = ['nTicket', 'identificacion'];

    for (var i of list) {
        var elemento = document.getElementById(i);
        elemento.setAttribute('class', 'form-control');
    }
}

function mostrarMensajeBuscar(item, mensaje, input) {
    let list = ['advTicket', 'advIdentificacion'];

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