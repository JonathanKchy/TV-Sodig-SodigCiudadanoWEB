function cambiar() {
    var pdrs = document.getElementById('postedFile').files[0].name;
    document.getElementById('info').innerHTML = pdrs;
}


function cambiarPost() {
    var pdrs = document.getElementById('postedFilePosterior').files[0].name;
    document.getElementById('infoPost').innerHTML = pdrs;
}

$(document).ready(function () {
    $('.alert').fadeIn();
    setTimeout(function () {
        $(".alert").fadeOut();
    }, 10000);
});

$("#btnGrabar").click("click", function () {
    var md = $("#processing-modal");
    md.modal('show');
});

function validar() {
    var postedFile = document.getElementById("postedFile");
    var postedFilePost = document.getElementById("postedFilePosterior");
	if (postedFile.files.length === 0) {
        var elemento = document.getElementById("mensajeTérmino");
		elemento.hidden = false;
		return false;
    } else if (postedFilePost.files.length === 0) {
        var elemento2 = document.getElementById("mensajeTérmino2");
        elemento2.hidden = false;
        return false;
    }

    $('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>Procesando...').attr('disabled', true);
    $('#btn-one1').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>Procesando...').attr('disabled', true);
}

function validarArchivo() {
    var postedFile = document.getElementById("postedFile");

    if (postedFile.files.length === 0) {
        var elemento = document.getElementById("mensajeTérmino");
        elemento.hidden = true;
        return false;
    } 
}

function validarArchivoPost() {
    var postedFilePosterior= document.getElementById("postedFilePosterior");
    if (postedFilePosterior.files.length === 0) {
        var elemento = document.getElementById("mensajeTérmino2");
        elemento.hidden = true;
        return false;
    }
}

function mostrar() {
    alert('Antes de cargar su Cédula , por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su fotografía, de igual manera el número de cédula debe ser legible.')
    div = document.getElementById("flotante");
    div.style.display = '';

    div = document.getElementById("flotanteCamara");
    div.style.display = 'none';

    div = document.getElementById("img-preca");
    div.style.display = 'none';

    div = document.getElementById("CapturePost");
    div.style.display = 'none';

   

}

function cerrar() {

    div = document.getElementById("img-preca");
    div.style.display = '';

    div = document.getElementById('flotante');
    div.style.display = 'none';

    div = document.getElementById('flotantePapeleta');
    div.style.display = 'none';



}


function mostrarCamara() {
  var  camara = document.getElementById("flotanteCamara");
    camara.style.display = '';

    var camara = document.getElementById("flotante");
    camara.style.display = 'none';

   var div = document.getElementById("img-preca");
    div.style.display = 'none';
}

function mostrarPapeleta() {
    var camara = document.getElementById("flotante");
    camara.style.display = '';
    var div = document.getElementById("img-preca");
    div.style.display = 'none';


    var div = document.getElementById("flotantePapeleta");
    div.style.display = 'none';


}



function mostrarPapeletaCam() {
    var camara = document.getElementById("flotantePapeleta");
    camara.style.display = '';

    var camara = document.getElementById("flotante");
    camara.style.display = 'none';
    var div = document.getElementById("img-preca");
    div.style.display = 'none';

}


function cerrarCamara() {


    var camara = document.getElementById("flotanteCamara");
        camara.style.display = 'none';

    var camaraPost = document.getElementById("flotanteCamPost");
    camaraPost.style.display = 'none';

    var div = document.getElementById("img-preca");
    div.style.display = '';

   var  div = document.getElementById("CaptureFrontal");
    div.style.display = 'none';

   var  div = document.getElementById("CapturePost");
    div.style.display = 'none';

    var papeleta = document.getElementById("flotantePapeleta");
    papeleta.style.display = 'none';

}



function validarCedulaDigital() {
    var postedFile = document.getElementById("postedFile");
  
    if (postedFile.files.length === 0) {
        var elemento = document.getElementById("mensajeTérmino");
        elemento.hidden = false;
        return false;
    }
    
    $('#btn-oneCertificado').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>Procesando...').attr('disabled', true);
}