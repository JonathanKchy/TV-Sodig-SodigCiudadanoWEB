$(document).ready(function () {
    $('.alert').fadeIn();
    setTimeout(function () {
        $(".alert").fadeOut();
    }, 100000);
});

var esEcuatoriano = false;

async function iniciarVideo(marco = 0) {
    try {        
        if (marco !== 0) {
            const panelRecomendaciones = document.getElementById('panelRecomendaciones');
            panelRecomendaciones.hidden = false;
        }


        const informacion = document.getElementById('infoCapt');
        informacion.hidden = false;
        const imgceduFront = document.getElementById('imgceduFront');
        imgceduFront.hidden = false;
        const btnweb = document.getElementById("btn-web");
        btnweb.style.display = "none";
        const camara = document.getElementById("alertCam");
        camara.style.display = "none";
        const camaraPost = document.getElementById("flotanteCamPost");
        camaraPost.style.display = 'none';
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
            //if (marco === 0) {
            //    context.drawImage(imagenCanvas, 5, 5, 310, 240)
            //} 
            requestAnimationFrame(reDraw)
        }
        requestAnimationFrame(reDraw)
    } catch (e) {
        alert(e);
    }
    
}

async function iniciarVideoPost(marcoPost = 0) {
    try {
        if (marcoPost !== 0) {
            const panelRecomendacionesPost = document.getElementById('panelRecomendacionespost');
            panelRecomendacionesPost.hidden = false;
        }

        const informacion = document.getElementById('infoCapt');
        informacion.hidden = false;
        const imgceduPost = document.getElementById('imgceduPost');
        imgceduPost.hidden = false;
        const btnweb = document.getElementById("btn-web");
        btnweb.style.display = "none";
        const alert = document.getElementById("alertCamPost");
        alert.style.display = "none";
        const btnIniciarCamara = document.getElementById('btnIniciarCamaraPost');
        btnIniciarCamara.hidden = true;
        const panelVideoPost = document.getElementById('panelVideoPost');
        panelVideoPost.hidden = false;
        const panelCapturaPost = document.getElementById('panelCapturaPost');
        panelCapturaPost.hidden = true;
        const videoContainerPost = document.querySelector('.js-videoPost')
        const canvas = document.querySelector('.js-canvaspost')
        let photo = document.querySelector('#photo')
        const context = canvas.getContext('2d')
        const video = await navigator.mediaDevices.getUserMedia({ video: true })



        videoContainerPost.srcObject = video

        const reDraw = async () => {
            context.drawImage(videoContainerPost, 0, 0, 320, 250)
            //if (marcoPost === 0) {
            //    context.drawImage(imagenCanvasPost, 5, 5, 310, 240)
            //}
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

    const btnweb = document.getElementById("btn-web");
    btnweb.style.display = "";
    const informacion = document.getElementById('infoCapt');
    informacion.hidden = true;
    const imgceduFront = document.getElementById('imgceduFront');
    imgceduFront.hidden = true;
    const camara = document.getElementById("alertCam");
    camara.style.display = "";
    const btnIniciarCamara = document.getElementById('btnIniciarCamara');
    btnIniciarCamara.hidden = false;
    const panelVideo = document.getElementById('panelVideo');
    panelVideo.hidden = true;
    const panelCaptura = document.getElementById('panelCaptura');
    panelCaptura.hidden = true;    
}

async function apagarVideoPost() {
    const videoContainerPost = document.querySelector('.js-video')

    let stream = videoContainerPost.srcObject;
    let tracks = stream.getTracks();

    await tracks.forEach(function (track) {
        track.stop();
    });

    const alert = document.getElementById("alertCamPost");
    alert.style.display = "";
    const btnIniciarCamara = document.getElementById('btnIniciarCamaraPost');
    btnIniciarCamara.hidden = false;
    const panelVideoPost = document.getElementById('panelVideoPost');
    panelVideoPost.hidden = true;
    const panelCapturaPost = document.getElementById('panelCapturaPost');
    panelCapturaPost.hidden = true;
}



async function capturarImagen (bandera = 0) {
    if (bandera === 0) {
        const panelRecomendaciones = document.getElementById('panelRecomendaciones');
        panelRecomendaciones.hidden = true;
    }
    
    await apagarVideo();

    const btnweb = document.getElementById("btn-web");
    btnweb.style.display = "none";
    const camara = document.getElementById("alertCam");
    camara.style.display = "none";
    const informacion = document.getElementById('infoCapt');
    informacion.hidden = false;
    const imgceduFront = document.getElementById('imgceduFront');
    imgceduFront.hidden = false;
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

    //alert('Antes de cargar su identificación frontal, por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización del número de documento, este debe ser legible. Caso contrario, por favor, capture otra imagen.')
}

/*$(document).ready(function () {
    $('#alertbox').click(function () {
        $("#error").html("<img src=/Content/images/info.png / width=100%>");
        $('#myModal').modal("show");
    });
});*/

/*$(document).ready(function () {
    $('#alertbox2').click(function () {
        $("#error").html("<img src=/Content/images/info.png / width=100%>");
        $('#myModal').modal("show");
    });
});*/

$(document).ready(function () {
    $('#alertbox3').click(function () {
        $("#error").html("<img src=/Content/images/infopapeleta.png / width=100%>");
        $('#myModal').modal("show");
    });
});

async function capturarImagenPost (banderaP = 0) {
     
    await apagarVideoPost();
    const div = document.getElementById("CapturePost");
    div.style.display = '';
    const alert1 = document.getElementById("alertCamPost");
    alert1.style.display = "none";
    const camPost = document.getElementById("camPost");
    camPost.style.display = '';
    const btnIniciarCamara = document.getElementById('btnIniciarCamaraPost');
    btnIniciarCamara.hidden = true;
    const panelVideoPost = document.getElementById('panelVideoPost');
    panelVideoPost.hidden = true;
    const panelCapturaPost = document.getElementById('panelCapturaPost');
    panelCapturaPost.hidden = false;
    const canvas = document.querySelector('.js-canvaspost')
    let photo = document.querySelector('#photo2')
    var data = canvas.toDataURL('image/png');
    photo.setAttribute('src', data);
 //   alert('Antes de cargar su identificación posterior, por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización del documento. Caso contrario, por favor, capture otra imagen.')
}





async function analizarImagen() {
    event.preventDefault();
    let photo = document.querySelector('#photo')
    var imagen = photo.src
    $('#btn-one1').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
    await $.post('/CapturaDocumento/CapturaCedulaFrontalCamara', {
        img: imagen
    }, function (data, status) {
            if (status) {
                if (data.estado) {
                    if (data.redirigir) {
                        window.location.replace('/Home/Index?msg1=' + data.mensaje)
                    }
                    else {
                        divFront = document.getElementById("CaptureFrontal")
                        divFront.style.display = "none";
                        btnweb = document.getElementById("btn-web");
                        btnweb.style.display = "block";
                        informacion = document.getElementById('post');
                        informacion.hidden = false;
                        informacion = document.getElementById('infoCapt');
                        informacion.hidden = true;
                        imgceduFront = document.getElementById('imgceduFront');
                        imgceduFront.hidden = true;

                        div = document.getElementById("flotanteCamPost");
                        div.style.display = "block";
                        $('#btn-one').html('<span role="status" aria-hidden="true"></span>Continuar').attr('disabled', false);
                        let mensajeAlerta = document.querySelector('#mensajeAlertaSuccess');
                        mensajeAlerta.hidden = false;
                        mensajeAlerta.innerHTML = "Documento Capturado Correctamente";
                        setTimeout(function () {
                            mensajeAlerta.hidden = true;
                        }, 150000);
	
			
                    }                       
               
                } else {
                    if (data.ticket) {
                        window.location.replace('/Ticket/Index?msg=1');
                    }
                    else {
                        $('#btn-one1').html('<span role="status" aria-hidden="true"></span>Continuar').attr('disabled', false);
                        mensajes(false, data.mensaje);
                    }
                }
                    
            } else {
                $('#btn-one1').html('<span role="status" aria-hidden="true"></span>Continuar').attr('disabled', false);
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
    }, 150000);
}

async function analizarImagenPosterior() {
    event.preventDefault();
    let photo = document.querySelector('#photo2')
    var imagen = photo.src
    $('#btn-one2').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
    await $.post('/CapturaDocumento/CapturaCedulaPosteriorCamara', {
        img: imagen
    }, function (data, status) {
        if (status) {
            if (data.estado) {
                var idTipoElector = data.idTipoElector;
                if (idTipoElector.toString() == data.codigoTipoElectorVotante.toString()) {
                    var esEcuatoriano = data.esEcuatoriano;
                    if (esEcuatoriano)
                        window.location.replace('/VerificarExcepcion/verfificarCodigoDactilar')
                    else
                        window.location.replace('/IdentificacionSolicitante/Index')
                }
		        else
                    window.location.replace('/VerificarExcepcion/verfificarCodigoDactilar')
            } else {
                $('#btn-one2').html('<span role="status" aria-hidden="true"></span>Continuar').attr('disabled', false);
                mensajes(false, data.mensaje);
            }
              
        } else {
            $('#btn-one2').html('<span role="status" aria-hidden="true"></span>Continuar').attr('disabled', false);
            mensajes(false, 'No ha sido posible conectarse al servidor');
        }
    });
}

async function analizarPasaporte() {
    event.preventDefault();
    let photo = document.querySelector('#photo')
    var imagen = photo.src
    $('#btn-one').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
    await $.post('/CapturaDocumento/CapturaPasaporteCamara', {
        img: imagen
    }, function (data, status) {
        if (status) {
            if (data.estado) {
                var idTipoElector = data.idTipoElector;
                if (idTipoElector.toString() == data.codigoTipoElectorVotante.toString()) {
                    var esEcuatoriano = data.esEcuatoriano;
                    if (esEcuatoriano)
                        window.location.replace('/VerificarExcepcion/SeleccionTipoCargaPapeleta')
                    else
                        window.location.replace('/IdentificacionSolicitante/Index')
                }
                else
                    window.location.replace('/IdentificacionSolicitante/Index')
            } else
                mensajes(false, data.mensaje);
        } else {
            mensajes(false, 'No ha sido posible conectarse al servidor');
        }
    });
}


async function iniciarVideoPapeleta(marco = 0) {
    try {
        if (marco !== 0) {
            const panelRecomendaciones = document.getElementById('panelRecomendaciones');
            panelRecomendaciones.hidden = false;
        }
        const camara = document.getElementById("alertCam");
        camara.style.display = "none";
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
            //if (marco === 0) {
            //    context.drawImage(imagenCanvas, 5, 5, 310, 240)
            //}
            requestAnimationFrame(reDraw)
        }
        requestAnimationFrame(reDraw)
    } catch (e) {
        alert(e);
    }

}



async function capturarImagenPapeleta(bandera = 0) {
    //if (bandera === 0) {
    //    const panelRecomendaciones = document.getElementById('panelRecomendaciones');
    //    panelRecomendaciones.hidden = true;
    //}

    await apagarVideo();
    const camara = document.getElementById("alertCam");
    camara.style.display = "none";
    const imgpapeleta = document.getElementById("capture");
    imgpapeleta.style.display = "";
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
	//alert('Antes de cargar su Papelta de Votación, por favor valide que esta no tenga reflejos de luz o brillo que impidan la la visualización del número de cédula adicional este debe ser legible. . Caso contrario, por favor, capture otra imagen.')
}



async function analizarPapeleta() {
    event.preventDefault();
    let photo = document.querySelector('#photo')
    var imagen = photo.src
    $('#btn-one3').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
    await $.post('/VerificarExcepcion/CapturaPapeletaCamara', {
        img: imagen
    }, function (data, status) {
        if (status) {
            if (data.estado) {
                window.location.replace('/IdentificacionSolicitante/Index')
            } else
                mensajes(false, data.mensaje);
        } else {
            mensajes(false, 'No ha sido posible conectarse al servidor');
        }
    });
}

/*function onClickAceptar() {
    if (esEcuatoriano)
        window.location.replace('/VerificarExcepcion/Index')
    else
        window.location.replace('/DatosUsuario/Index')
}

function onClickRechazar() {
    window.location.replace('/TipoIdentificacion/Index');
}*/