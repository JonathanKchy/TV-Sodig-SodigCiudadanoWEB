$(document).ready(function () {
	$('.alert').fadeIn();
	setTimeout(function () {
		$(".alert").fadeOut();
	}, 15000);
});

function validar() {
	var idTipoIdentificacion = $('#idTipoIdentificacion').val();
	var elemento = document.getElementById("mensajeTipoIdentificacion");
	if (idTipoIdentificacion.trim() == "") {		
		elemento.hidden = false;
		return false;
	} 
	elemento.hidden = true;
 
	$('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
}