
function test($param) {
    var valor = $param;
    var url =' /Cancelacion/llamadoPoliza?poliza=param_id';
    url = url.replace("param_id", valor);
    $.get(url, function (url) {
        $("#myModal").html(url);
        $("#myModal").modal('show');
    });
};

function Queja($param) {
    var valor = $param;
    var url = ' /Quejas/DetallesQuejas?num_Documento=param_id';
    url = url.replace("param_id", valor);
    $.get(url, function (url) {
        $("#Quejas").html(url);
        $("#Quejas").modal('show');
    });
};

function Devengar($param) {
    var valor = $param;
    var url = ' /Cancelacion/MontoDevengar?certificado=param_id';
    url = url.replace("param_id", valor);
    $.get(url, function (url) {
        $("#myModalprod").html(url);
        $("#myModalprod").modal('show');
    });
};
