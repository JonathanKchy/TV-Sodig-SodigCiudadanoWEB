
const init = () => {
    let blobVideo; 
    const tieneSoporteUserMedia = () =>
        !!(navigator.mediaDevices.getUserMedia)

    if (typeof MediaRecorder === "undefined" || !tieneSoporteUserMedia()) {
        mensajes(true, "Tu navegador web no cumple los requisitos; por favor, actualiza a un navegador como Firefox o Google Chrome")
        return;
    }
       

    const $dispositivosDeAudio = document.querySelector("#dispositivosDeAudio"),
        $dispositivosDeVideo = document.querySelector("#dispositivosDeVideo"),
        $duracion = document.querySelector("#duracion"),
        $video = document.querySelector("#video"),
        $btnComenzarGrabacion = document.querySelector("#btnComenzarGrabacion"),
        $btnDetenerGrabacion = document.querySelector("#btnDetenerGrabacion"),  
       $btnContinuar = document.querySelector("#btnContinuar");    
        var tagVideo = document.querySelector("#videoGrabado");
        var tagsource = document.querySelector('#source');
        var panelVideoGrabado = document.querySelector('#panelVideoGrabado');
        var canvas = document.querySelector('.js-canvas')
        var videoCorrecto = true;    
        var existeRostro = false;

    const limpiarSelect = elemento => {
        for (let x = elemento.options.length - 1; x >= 0; x--) {
            elemento.options.remove(x);
        }
    }

    const segundosATiempo = numeroDeSegundos => {
        let horas = Math.floor(numeroDeSegundos / 60 / 60);
        numeroDeSegundos -= horas * 60 * 60;
        let minutos = Math.floor(numeroDeSegundos / 60);
        numeroDeSegundos -= minutos * 60;
        numeroDeSegundos = parseInt(numeroDeSegundos);
        if (horas < 10) horas = "0" + horas;
        if (minutos < 10) minutos = "0" + minutos;
        if (numeroDeSegundos < 10) numeroDeSegundos = "0" + numeroDeSegundos;

        return `${horas}:${minutos}:${numeroDeSegundos}`;
    };
    // Variables "globales"
    let tiempoInicio, mediaRecorder, idIntervalo;
    const refrescar = async () => { 
        $duracion.textContent = segundosATiempo((Date.now() - tiempoInicio) / 1000);
        if ($duracion.textContent === '00:00:50') {
            detenerGrabacion();
        }
        if ($duracion.textContent <= '00:00:05') {
            videoCorrecto = false;
        } else {
            videoCorrecto = true;
        }
        
        if ($duracion.textContent >= '00:00:06') {
            for (var i = 0; i < 1000; i++) {
                if (i == 400) {
                    if (!existeRostro) {
                        var data = canvas.toDataURL('image/png');
                        await capturarCuadroVideo(data);
                    }
                    break;
                }    
            }                                
        }
                   
    }

    // Consulta la lista de dispositivos de entrada de audio y llena el select
    const llenarLista = () => {
        navigator
            .mediaDevices
            .enumerateDevices()
            .then(dispositivos => {
                limpiarSelect($dispositivosDeAudio);
                limpiarSelect($dispositivosDeVideo);
                dispositivos.forEach((dispositivo, indice) => {
                    if (dispositivo.kind === "audioinput") {
                        const $opcion = document.createElement("option");
                        $opcion.text = dispositivo.label || `Micrófono ${indice + 1}`;
                        $opcion.value = dispositivo.deviceId;
                        $dispositivosDeAudio.appendChild($opcion);
                    } else if (dispositivo.kind === "videoinput") {
                        const $opcion = document.createElement("option");
                        $opcion.text = dispositivo.label || `Cámara ${indice + 1}`;
                        $opcion.value = dispositivo.deviceId;
                        $dispositivosDeVideo.appendChild($opcion);
                    }
                })
            })
    };
    // Ayudante para la duración; no ayuda en nada pero muestra algo informativo
    const comenzarAContar = () => {        
        tiempoInicio = Date.now();
        idIntervalo = setInterval(refrescar, 500);
    };

    async function capturarCuadroVideo(canvas) {        
        let photo = document.querySelector('#photo')
        photo.setAttribute('src', canvas);
        var imagen = photo.src        
        await $.post('/SodigCiudadanoZona6/PruebaVida/CapturarCuadroVideo', {
            img: imagen
        }, function (data, status) {
                if (status) {
                    console.log(data)
                if (data.estado) {
                    if (data.existeRostro) {                        
                        existeRostro = true;
                    }
                } else
                    mensajes(false, data.mensaje);
            } else {
                mensajes(false, 'No ha sido posible conectarse al servidor');
            }
        });
    }

    // Comienza a grabar el audio con el dispositivo seleccionado
    const comenzarAGrabar = () => {
        $('#play').attr('hidden', true);
        $('#btnComenzarGrabacion').attr('hidden', true);
        $('#btnDetenerGrabacion').attr('hidden', false);
      /*  var title = document.getElementById("title");
        title.style.display = "none";*/

        videoCorrecto = true;
        existeRostro = false;
        panelVideoGrabado.hidden = true;
        $video.hidden = false;
        if (!$dispositivosDeAudio.options.length) return alert("No hay micrófono");
        if (!$dispositivosDeVideo.options.length) return alert("No hay cámara");
        // No permitir que se grabe doblemente
        if (mediaRecorder) return alert("Ya se está grabando");

        navigator.mediaDevices.getUserMedia({
            audio: {
                deviceId: $dispositivosDeAudio.value, // Indicar dispositivo de audio
            },
            video: {
                deviceId: $dispositivosDeAudio.value, // Indicar dispositivo de vídeo
            }
        })
            .then(stream => {                
                // Poner stream en vídeo                
                const context = canvas.getContext('2d')
                $video.srcObject = stream;
                $video.play();               
                var marco = 0;
                const reDraw = async () => {
                    context.drawImage($video, 0, 0, 320, 250)
                    if (marco === 0) {
                        context.drawImage($video, 5, 5, 310, 240)
                    }
                    requestAnimationFrame(reDraw)
                }
                requestAnimationFrame(reDraw)
                // Comenzar a grabar con el stream
                mediaRecorder = new MediaRecorder(stream, {
                    mimeType: 'video/webm;codecs=opus,vp8',
                });
                mediaRecorder.start();
                comenzarAContar();
                // En el arreglo pondremos los datos que traiga el evento dataavailable
                const fragmentosDeAudio = [];
                // Escuchar cuando haya datos disponibles
                mediaRecorder.addEventListener("dataavailable", evento => {
                    // Y agregarlos a los fragmentos                    
                    fragmentosDeAudio.push(evento.data);
                });

                // Cuando se detenga (haciendo click en el botón) se ejecuta esto
                mediaRecorder.addEventListener("stop", () => {
                    // Pausar vídeo
                    $video.pause();
                    // Detener el stream
                    stream.getTracks().forEach(track => track.stop());
                    // Detener la cuenta regresiva
                    detenerConteo();
                    // Convertir los fragmentos a un objeto binario
                    blobVideo = new Blob(fragmentosDeAudio, { type: 'video/webm' });
                    
                    tagsource.setAttribute('type', 'video/webm');
                    tagVideo.src = URL.createObjectURL(blobVideo);
                    tagVideo.appendChild(tagsource);
                    $video.hidden = true;
                    panelVideoGrabado.hidden = false;
                    tagVideo.setAttribute('controls', 'controls');  
                                        
                    // subirVideo(blobVideo);
                });

            })
            .catch(error => {
                // Aquí maneja el error, tal vez no dieron permiso
                mensajes(false, error)
            });
    };

    const detenerConteo = () => {
        clearInterval(idIntervalo);
        tiempoInicio = null;
        $duracion.textContent = "";
    }

    const detenerGrabacion = () => {

        $('#btnComenzarGrabacion').attr('hidden', false);
        $('#btnDetenerGrabacion').attr('hidden', true);
        if (!mediaRecorder) return alert("No se está grabando");
        mediaRecorder.stop();
        mediaRecorder = null;
    };

    const subirVideo = () => {
        if (!videoCorrecto) {
            mensajes(false, "El video debe ser al menos de 5 segundos.");
            return false;
        }
        if (!existeRostro) {
            mensajes(false, "No se ha detectado un rostro en el video de prueba de vida, esto se puede deber a que tiene la cámara apagada,la persona está muy alejada a la cámara, la posición del rostro no está centrado a la cámara, el video es de baja calidad o la iluminación no es la adecuado. Por favor, grabe nuevamente su prueba de vida.");
            return false;
        } 
	$('#btnContinuar').html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>').attr('disabled', true);
        let photo = document.querySelector('#photo')
        var imagen = photo.src
        const formData = new FormData();
        formData.append('video', blobVideo);
        formData.append('frame', imagen);
        const rutaServidor = '/SodigCiudadanoZona6/PruebaVida/SubirVideo'
        event.preventDefault();
        fetch(rutaServidor, {
            method: 'POST',
            body: formData
        }).then(res => res.json())
            .then(data => {
                if (data.estado) {
                    //window.location.href = "/PruebaVida/SubirImagen";
                   var  divPruebaVida = document.getElementById("div-PruebaVida");
                    divPruebaVida.style.display = 'none';

                   var  divCaptPhoto = document.getElementById("flotanteCaptPhoto");
                    divCaptPhoto.style.display = 'block';

                } else {
                    mensajes(false, data.mensaje);
                }
            }).catch(err => {
                mensajes(false, err);
            });
    };

    const mensajes = (estado, mensaje) => {
        $('#btnContinuar').html('<span role="status" aria-hidden="true"></span>Continuar').attr('disabled', false);
        let mensajeAlerta = document.querySelector('#mensajeAlerta');
        mensajeAlerta.hidden = estado;
        mensajeAlerta.innerHTML = mensaje;
        setTimeout(function () {
            mensajeAlerta.hidden = true;
        }, 5000);
    }

    $btnComenzarGrabacion.addEventListener("click", comenzarAGrabar);

    $btnDetenerGrabacion.addEventListener("click", detenerGrabacion);

    $btnContinuar.addEventListener("click", subirVideo);

    // Cuando ya hemos configurado lo necesario allá arriba llenamos la lista

    llenarLista();
}



// Esperar a que el documento esté listo...
document.addEventListener("DOMContentLoaded", init);