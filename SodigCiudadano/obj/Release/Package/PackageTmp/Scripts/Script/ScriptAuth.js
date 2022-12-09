$(document).ready(function () {
	$('.alert').fadeIn();
	setTimeout(function () {
		$(".alert").fadeOut();
	}, 60000);
});



let modal = document.getElementById('miModal');

async function abrirModal() {
	modal.hidden = false;
	modal.style.display = 'block';
}

function cerrarModal() {
	modal.hidden = true;
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

$('#checkTerminos').change(function () {
	if ($(this).prop("checked")) {
		abrirModal()
		var elemento = document.getElementById("mensajeTérmino");
		elemento.hidden = true;
		return false;
	}
});


function getMobileOperatingSystem() {
	var userAgent = navigator.userAgent || navigator.vendor || window.opera;

	// Windows Phone debe ir primero porque su UA tambien contiene "Android"
	if (/windows phone/i.test(userAgent)) {
		alert("Windows Phone")
		return "Windows Phone";
	}

	if (/android/i.test(userAgent)) {
		alert("Android")
		return "Android";
	}

	if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
		alert("iOS")
		return "iOS";
	}

	return "desconocido";
}


function validar() {
	var identificacion = $('#identificacion').val();
	var email = $('#email').val();
	var email2 = $('#email2').val();
	//var recaptcha = $('#g-recaptcha-response').val();

	if (identificacion.trim() == "") {
		var elemento = document.getElementById("mensajeIdentificacion");
		elemento.hidden = false;
		return false;
	}
	else if (email.trim() != email2.trim()) {
		var elemento = document.getElementById("mensajeEmail");
		elemento.hidden = false;
		return false;
	}
	/*else if (recaptcha.trim() == "") {
		var elemento = document.getElementById("mensajeCaptcha");
		elemento.hidden = false;
		return false;
	} */
	else if (!checkTerminos.checked) {
		var elemento = document.getElementById("mensajeTérmino");
		elemento.hidden = false;
		return false;
	}
	$('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
}

$(function () {
	$("#identificacion").keydown(function (event) {
		if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && event.keyCode !== 190 && event.keyCode !== 110 && event.keyCode !== 8 && event.keyCode !== 9) {
			return false;
		}
	});
});

$(function () {
	var regExp = /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
	//var regExp = /^([\w\.\+]{1,})([^\W])(@)([\w]{1,})(\.[\w]{1,})+$/;

	$('[type="email"]').on('keyup', function () {
		$('.message').hide();
		regExp.test($(this).val()) ? $('.message.success').show() : $('.message.error').show();
	});

});

function habilitar() {
	var correo = document.getElementById('correo');
	var correo2 = document.getElementById('email2');
	var boton = document.getElementById('btn-one');

	if (correo.value != correo2.value) {	
		boton.disabled = true;
	} else {
		boton.disabled = false;
	}
}