// JavaScript source code

function mostrar() {
    div = document.getElementById("flotante");
    div.style.display = '';

    div = document.getElementById("img-preca");
    div.style.display = 'none';
}

function cerrar() {
    div = document.getElementById('flotante');
    div.style.display = 'none';

    div = document.getElementById("img-preca");
    div.style.display = '';
}


function mostrarCamara() {
    var camara = document.getElementById("flotanteCamara");
    camara.style.display = '';

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
}