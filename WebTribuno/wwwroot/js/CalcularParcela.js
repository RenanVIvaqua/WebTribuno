
function CalcularParcela() {

    if (!validacaoCalculoParcela())
        return

    $.ajax({
        url: "/CadastrarOperacao/CalcularParcela",
        type: "POST",
        data: { valorParcela: $("#ValorParcela").val(), quantidadeParcela: $("#QuantidadeParcela").val(), dataVencimento: $("#DataPrimeiroVencimento").val() },
        datatype: "json",
        success: function (data) {

            $('#tbParcelas > tbody > tr').each(function (index) {
                this.remove()
            })

            $.each(data, function (index, value) {
                $('#tbParcelas > tbody:first').append('<tr id = numParc' + value.numeroParcela + '><th>' + value.numeroParcela + '</th><th>' + value.valorParcela + '</th> <th>' + value.dataVencimento + '</th> </tr>');
            })
        }
    });
}

$("#btGravar").click(function () {
    var form = $("#formCadastroOperacao").serialize();

    $.ajax({
        url: "/CadastrarOperacao/Create",
        type: "POST",
        data: form,
        async: false,
        success: function (retorno) {
            alert(retorno.message);
        },
        error: function (retorno) {
            //Sera implementado um modal 
            alert(retorno.message);
        }
    });
});


function validacaoCalculoParcela() {
    if ($("#ValorParcela").val() == "") {
        $("#labelValorParcela").addClass("invalid")
        alert("Informe o valor da parcela para realizar o calculo")
        return false
    }

    if ($("#QuantidadeParcela").val() == "") {
        alert("Informe a quantidade de parcela para realizar o calculo")
        return false
    }

    if ($("#DataPrimeiroVencimento").val() == "") {
        alert("Informe a data de vencimento para realizar o calculo")
        return false
    }

    return true
}