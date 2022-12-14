
$(document).ready(function () {
	$('.alert').fadeIn();
	setTimeout(function () {
		$(".alert").fadeOut();
	}, 15000);
});

var codigoPasaporteInterno = document.getElementById('codigoPasaporteInterno').value;
var cboidTipoIdentificacion = document.getElementById('idTipoIdentificacion');


cboidTipoIdentificacion.addEventListener('change', mostrarPanel);



function mostrarPanel() {

	var panelTipoElector = document.getElementById('panelTipoElector');
	if (cboidTipoIdentificacion.value !== codigoPasaporteInterno) {
		panelTipoElector.hidden = false;
	}
	else {
		panelTipoElector.hidden = false;
	}
}



function validar() {

	var radiovalue = document.getElementsByName('idTipoElector');
	var seleccionado = false;
	var idSeleccionado = 0;
	var textSeleccionado = "";
	var idTipoSolicitud = $('#idTipoSolicitud').val();
	var idTipoPersona = $('#idTipoPersona').val();
	var idTipoContenedor = $('#idTipoContenedor').val();
	var checkTerminos = document.getElementById("checkTerminos");
	var idTipoIdentificacion = $('#idTipoIdentificacion').val();
	var elemento = document.getElementById("mensajeTipoIdentificacion");

	for (var opcion of radiovalue) {
		if (opcion.checked) {
			advertencia.hidden = true;
			seleccionado = true;
			idSeleccionado = opcion.value
			let textLabel = document.getElementById('label_' + idSeleccionado)
			textSeleccionado = textLabel.textContent.trim();
		}
	}

	if (idTipoSolicitud.trim() == "") {
		var elemento = document.getElementById("mensajeTipoSolicitud");
	    elemento.hidden = false;
		return false;
	}
	else if (idTipoPersona.trim() == "") {
		var elemento = document.getElementById("mensajeTipoPersona");
		elemento.hidden = false;
		var elemento = document.getElementById("mensajeTipoSolicitud");
		elemento.hidden = true;
		return false;
	}
	else if (idTipoContenedor.trim() == "") {
		var elemento = document.getElementById("mensajeTipoContenedor");
		elemento.hidden = false;
		var elemento = document.getElementById("mensajeTipoPersona");
		elemento.hidden = true;
		var elemento1 = document.getElementById("mensajeTérmino");
		elemento1.hidden = true;
		return false;
	}
	else if (!checkTerminos.checked) {
		var elemento = document.getElementById("mensajeTérmino");
		elemento.hidden = false;
		var elemento1 = document.getElementById("mensajeTipoContenedor");
		elemento1.hidden = true;
		var elemento = document.getElementById("mensajeTipoPersona");
		elemento.hidden = true;
		return false;
	}

	else if (idTipoIdentificacion.trim() == "") {
		elemento.hidden = false;
		return false;
	} 
	else if (seleccionado) {
		elemento.hidden = true;
		idTipoElector.value = idSeleccionado
		if (idSeleccionado !== '4') {
			var idTipoIdentificacionSeleccionado = document.getElementById('idTipoIdentificacionSeleccionado');
			idTipoIdentificacionSeleccionado.value = idTipoIdentificacion;
			modal.style.display = 'block';
			var tituloModal = document.getElementById('tituloModal')
			tituloModal.textContent = "Confirmación";
			textModal.textContent = `Ha seleccionado "${textSeleccionado}" ¿Desea Continuar?`;
			bloquearBarraDesplazamiento();
			return false;
		}
	}
	else {
		elemento.hidden = true;
		advertencia.hidden = false;
		return false;
	}
	elemento.hidden = true;

	$('#btnHome').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
}


let modal = document.getElementById('miModal');
let textModal = document.getElementById('textModal');
let advertencia = document.getElementById('advertencia');
let idTipoElector = document.getElementById('idTipoElector');

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


$('#checkTerminos').change(function () {
	if ($(this).prop("checked")) {
		abrirModal();
		var elemento = document.getElementById("mensajeTérmino");
		elemento.hidden = true;
		return false;
	}
});


async function abrirModal() {
	modal.hidden = false;
	modal.style.display = 'block';
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