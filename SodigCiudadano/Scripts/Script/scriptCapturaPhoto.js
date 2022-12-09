// JavaScript source code
$(document).ready(function () {
    $('.alert').fadeIn();
    setTimeout(function () {
        $(".alert").fadeOut();
    }, 15000);
});

var esEcuatoriano = false;

async function iniciarVideoCapt(marco = 0) {


    try {
        if (marco !== 0) {
            const panelRecomendaciones = document.getElementById('panelRecomendaciones');
            panelRecomendaciones.hidden = false;
        }

        const btnIniciarCamara = document.getElementById('btnIniciarCamara');
        btnIniciarCamara.hidden = true;
        const panelVideo = document.getElementById('panelVideo');
        panelVideo.hidden = false;
        const panelCaptura = document.getElementById('panelCaptura');
        panelCaptura.hidden = true;
        const videoContainer = document.querySelector('.js-video')
        const canvas = document.querySelector('.js-canvas')
        let photo = document.querySelector('#photo')
        const context = canvas.getContext('2d')
        const video = await navigator.mediaDevices.getUserMedia({ video: true })
        let imagenCanvas = document.getElementById('imagenCanvas');


        videoContainer.srcObject = video

        const reDraw = async () => {
            context.drawImage(videoContainer, 0, 0, 320, 250)
            if (marco === 0) {
                context.drawImage(imagenCanvas, 5, 5, 310, 240)
            }
            requestAnimationFrame(reDraw)
        }
        requestAnimationFrame(reDraw)
    } catch (e) {
        alert(e);
    }

}


async function apagarVideo() {
    const videoContainer = document.querySelector('.js-video')

    let stream = videoContainer.srcObject;
    let tracks = stream.getTracks();

    await tracks.forEach(function (track) {
        track.stop();
    });

    const camara = document.getElementById("alertCam");
    camara.style.display = "";
    const btnIniciarCamara = document.getElementById('btnIniciarCamara');
    btnIniciarCamara.hidden = false;
    const panelVideo = document.getElementById('panelVideo');
    panelVideo.hidden = true;
    const panelCaptura = document.getElementById('panelCaptura');
    panelCaptura.hidden = true;
}


async function capturarImagen(bandera = 0) {
    if (bandera === 0) {
        const panelRecomendaciones = document.getElementById('panelRecomendaciones');
        panelRecomendaciones.hidden = true;
    }

    await apagarVideo();
    const camara = document.getElementById("alertCam");
    camara.style.display = "none";
    const camFront = document.getElementById("camFront");
    camFront.style.display = "";
    const btnIniciarCamara = document.getElementById('btnIniciarCamara');
    btnIniciarCamara.hidden = true;
    const panelVideo = document.getElementById('panelVideo');
    panelVideo.hidden = true;
    const panelCaptura = document.getElementById('panelCaptura');
    panelCaptura.hidden = false;
    const canvas = document.querySelector('.js-canvas')
    let photo = document.querySelector('#photo')
    var data = canvas.toDataURL('image/png');
    photo.setAttribute('src', data);
}


async function analizarImagen() {
    event.preventDefault();
    let photo = document.querySelector('#photo')
    var imagen = photo.src
    $('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
    await $.post('/CapturaDocumento/CapturaCedulaFrontalCamara', {
        img: imagen
    }, function (data, status) {
        if (status) {
            if (data.estado) {
                divFront = document.getElementById("CaptureFrontal")
                divFront.style.display = 'none';

                div = document.getElementById("flotanteCamPost");
                div.style.display = 'block';

            } else {
                mensajes(false, data.mensaje);
            }

        } else {
            ;
            mensajes(false, 'No ha sido posible conectarse al servidor');
        }
    });
}

function mensajes(estado, mensaje) {
    $('#btn-one').html('<span role="status" aria-hidden="true"></span>Continuar').attr('disabled', false);
    let mensajeAlerta = document.querySelector('#mensajeAlerta');
    mensajeAlerta.hidden = estado;
    mensajeAlerta.innerHTML = mensaje;
    setTimeout(function () {
        mensajeAlerta.hidden = true;
    }, 15000);
}




