
$("#btnCalcular").click(function () {
    $.ajax({
        url: "/CadastrarOperacao/CalcularParcela",
        type: "POST",
        data: GerarObjetoOpercaoModel(),
        datatype: "json",
        success: function (data) {
            $("#formCadastroOperacao").html(data);
        }
    });
});

function GerarObjetoOpercaoModel() {      
   
    const OperacaoModel = {
        NomeOperacao: $("#NomeOperacao").val(),
        Descricao: $("#Descricao").val(),
        SimulacaoParcela : {
            QuantidadeParcela: $("#QuantidadeParcela").val(),
            ValorParcela: $("#ValorParcela").val(),
            DataPrimeiroVencimento: $("#DataPrimeiroVencimento").val(),
            TipoOperacao: $("#TipoOperacao").val(),
            TipoCalculo: $("#TipoCalculo").val(),
        }
    };   

    return OperacaoModel;
}

