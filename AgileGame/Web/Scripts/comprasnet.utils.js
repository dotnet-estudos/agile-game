swal.setDefaults({ confirmButtonColor: '#337ab7', allowOutsideClick: false });

var comprasnet = {
    alertSucesso: function (mensagem) {
        swal({
            text: mensagem,
            title: "",
            type: "success"
        });

    }
    , alertSucessoComCallback: function (mensagem, callback) {
        swal({
            title: "",
            text: mensagem,
            type: "success"
        }).then(callback);
    }
	, alertErro: function (mensagem) {
	    swal("", mensagem, "error");
	}
	, alertErroComCallback: function (mensagem, callback) {
	    swal("", mensagem, "error").then(callback);
	}
    , alertAtencao: function (mensagem) {
        swal("", mensagem, "warning");
    }
    , alertAtencaoComCallback: function (mensagem, callback) {
        swal("", mensagem, "warning").then(callback);;
    }
	, confirm: function (mensagem, callbackConfirm, callbackCancel) {
	    swal({
	        title: "",
	        text: mensagem,
	        type: "warning",
	        showCancelButton: true,
	        confirmButtonText: "Confirmar",
	        cancelButtonText: "Cancelar"
	    }).then(function () {
	        callbackConfirm();
	    }, function (cancel) {
	        if (callbackCancel)
	            callbackCancel();
	    });
	}
	, alert: function (titulo, mensagem) {
	    if (!titulo)
	        titulo = "Atenção";
	    swal(titulo, mensagem);
	}, showModal: function (modalTitle, responsive, content, width, height) {
	    $('#comprasnetModal .modal-title').text(modalTitle);

	    if (responsive) {
	        $('#comprasnetModal .modal-dialog').css('width', '');
	        $('#ModalContent').css('height', '');
	        $('#ModalContent').css('overflow-x', 'hidden');
	        $('#ModalContent').css('overflow-y', 'auto');
	    } else {
	        $('#comprasnetModal .modal-dialog').css('width', '96%');
	        $('#ModalContent').css('height', '585px');
	        $('#ModalContent').css('overflow-x', 'hidden');
	        $('#ModalContent').css('overflow-y', 'auto');
	    }

	    if (width) $('#comprasnetModal .modal-dialog').css('width', width);
	    if (height) $('#ModalContent').css('height', height);

	    if (content) {
	        $('#ModalContent').html(content);
	    }

	    $('#comprasnetModal').modal('show');
	}, CloseModal: function () {
	    $('#ModalContent').html('');
	    $('#comprasnetModal').modal('toggle');
	}, Upsert: function (url, idForm, callBackFunction) {
	    $(document).ready(function () {
	        $("#" + idForm).submit(function (event) {
	            event.preventDefault();
	            var formData = new FormData($(this)[0]);

	            $.ajax({
	                url: url,
	                type: "POST",
	                cache: false,
	                contentType: false,
	                processData: false,
	                async: true,
	                data: formData,
	                beforeSend: function () {
	                    $("#divCarregando").show();
	                    $(".field-validation-valid").html("");
	                },
	                success: function (result) {
	                    $("#divCarregando").hide();

	                    if (jQuery.isArray(result)) {
	                        $.each(result, function () {
	                            $(".field-validation-valid[data-valmsg-for='" + this.key + "']").html(this.msg).addClass("show text-danger");
	                            $("#" + this.key).focus();
	                        });
	                        return false;
	                    } else if (!result.key) {
	                        $("#comprasnetModal #ModalContent").html(result);
	                    } else {
	                        if (!callBackFunction) callBackFunction = "CallBackFunction";
	                        AlertOnReturn(result, callBackFunction);
	                    }
	                }
	            });
	        });
	    });
	}
    , ConfigurarCombosUnidadeSecretaria: function (idComboSecretaria, idComboNumUnidade, defaultText) {
        var comboSecretaria = document.getElementById(idComboSecretaria);
        var comboUnidade = document.getElementById(idComboNumUnidade);

        if (comboSecretaria !== null) {
            comboSecretaria.onchange = function () {
                var codSecretaria = this.options[this.selectedIndex].value;
                if (codSecretaria)
                    PopulaComboUnidades(comboUnidade, codSecretaria, defaultText);
                else
                    LimpaCombo(comboUnidade, defaultText);
            };
        }
    }
}

function PopulaComboUnidades(comboUnidade, codSecretaria, defaultText) {
    $.ajax({
        url: "/Base/ObtemUnidadesPorSecretaria",
        type: "GET",
        dataType: "JSON",
        data: { codSecretaria: codSecretaria },
        success: function (unidades) {
            LimpaCombo(comboUnidade, defaultText);
            $.each(unidades, function (i, unidade) {
                var option = $("<option></option>").val(unidade.NumUnidade).html( unidade.DescUnidade);
                $(comboUnidade).append(option);
            });
        }
    });
}

function LimpaCombo(combo, defaultText) {
    if (!defaultText)
        defaultText = "Todos";

    $(combo).html("");
    var option = $("<option></option>").val("").html(defaultText);
    $(combo).append(option);
}

function GetFormData(pindexedArray, sendFormData) {
    var indexedArray = {};

    if (pindexedArray) indexedArray = pindexedArray;

    var unindexedArray = $("form").serializeArray();
    
    if (sendFormData) {
      $.map(unindexedArray, function (n, i) {
          indexedArray[n["name"]] = n["value"];
      });
    }

    return indexedArray;
}

function ConfigurarMascara(idCampo, mascara) {
    $(document).ready(function () {
        var $campo = $("#" + idCampo);
        if (!Modernizr.inputtypes.date) {
            $campo.inputmask(mascara);
        } else {
            if ($campo.attr('type') !== 'date' && $campo.attr('type') !== 'time') {
                $campo.inputmask(mascara);
            } else if ($campo.attr('type') === 'date') {
                var from = $campo.attr('value').split('/'), d = new Date(from[2], from[1] - 1, from[0]),
                    day = (('' + d.getDate()).length < 2 ? '0' : '') + d.getDate(),
                    month = (('' + (d.getMonth() + 1)).length < 2 ? '0' : '') + (d.getMonth() + 1),
                    year = d.getFullYear();
                $campo.attr('value', [year, month, day].join('-'));
            }
        }
    });
}

function AlertOnReturn(result, callBackFunction) {
    if (result.key === "__Sucesso") {
        comprasnet.alertSucessoComCallback(result.msg, eval(callBackFunction));
        $("#comprasnetModal").modal("hide");
    } else if (result.key === "__Erro") {
        comprasnet.alertErro(result.msg);
    } else if (result.key === "__Alerta") {
        comprasnet.alertAtencao(result.msg);
    } else {
        comprasnet.alertErro("Ocorreu algum erro ao executar a operação.");
        $("#comprasnetModal").modal("hide");
    }
}

function ConfirmaRemoveArquivo(id) {
    var $element = $('#' + id);

    if ($element.data("excluir")) {
        return true;
    }

    comprasnet.confirm("Tem certeza que deseja remover este registro?", function () {
        $element.data("excluir", true);
        $element.click();
    });
    return false;
}
