$(document).ready(function () {
	$('.alert').fadeIn();
	setTimeout(function () {
		$(".alert").fadeOut();
	}, 15000);
});

function validar() {
	var idTipoContenedor = $('#idTipoContenedor').val();
	var checkTerminos = document.getElementById("checkTerminos");
	if (idTipoContenedor.trim() == "") {
		var elemento = document.getElementById("mensajeTipoContenedor");
		elemento.hidden = false;
		var elemento1 = document.getElementById("mensajeTérmino");
		elemento1.hidden = true;		
		return false;
	}
	else if (!checkTerminos.checked) {
		var elemento = document.getElementById("mensajeTérmino");
		elemento.hidden = false;
		var elemento1 = document.getElementById("mensajeTipoContenedor");
		elemento1.hidden = true;	
		return false;
    }
	
	$('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
}

$('#checkTerminos').change(function () {
	if ($(this).prop("checked")) {
		var elemento = document.getElementById("mensajeTérmino");
		elemento.hidden = true;
		return false;
	}
});

