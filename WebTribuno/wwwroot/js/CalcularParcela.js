$(document).ready(function () {
    $("input[name='Data']").inputmask("mask", { "mask": "99/99/9999" });
    $("input[name='Preco']").inputmask("mask", { "mask": "999.999,99" }, { reverse: true });   
    $("input[name='Cpf']").inputmask("mask", { "mask": "999.999.999-99" }, { reverse: true });
    $("input[name='Telefone']").inputmask("mask", { "mask": "(99) 9999-99999" });
    $("#input[name='Cep']").inputmask("mask", { "mask": "99999-999" });
    $("#input[name='Valor']").inputmask("mask", { "mask": "#.##9,99" }, { reverse: true });
    $("#input[name='Ip']").inputmask("mask", { "mask": "999.999.999.999" });
    $("input[name='MoedaCifrao']").maskMoney({ prefix: 'R$', allowNegative: true, thousands: '.', decimal: ',', affixesStay: false });
    $("input[name='Moeda']").maskMoney({ allowNegative: true, thousands: '.', decimal: ',', affixesStay: false });
    $("#ValorParcela").maskMoney({ allowNegative: true, thousands: '.', decimal: ',', affixesStay: false });

    $("#btGravar").click(function () {
        $.ajax({
            url: "/CadastrarOperacao/Create",
            type: "POST",
            data: GerarObjetoOpercaoModel(),
            datatype: "json",
            success: function (data) {
                $(".body").html(data);
                alert("Operação cadastrada !");
            }
        });
    });
});

$("#btnCalcular").click(function () {
    $.ajax({
        url: "/CadastrarOperacao/CalcularParcela",
        type: "POST",
        data: GerarObjetoOpercaoModel(),
        datatype: "json",
        success: function (data) {        
            $("#formCadastroOperacao").replaceWith(data);
        }
    });
});

function GerarObjetoOpercaoModel() {

    var Parcelas = [];

    $("#tbParcelas > tbody  > tr").each(function (index, tr) {  
        const ParcelaModel = {
            NumeroParcela: $(tr).find('#TbNumeroParcela').text(),
            ValorParcela: $(tr).find('#TbValorParcela').val(),
            DataVencimento: $(tr).find('#TbData').val(),
        }    
        if (ParcelaModel.NumeroParcela !== "")
            Parcelas.push(ParcelaModel);
    });

    const OperacaoModel = {
        IdOperacao: $("#IdOperacao").val(),
        NomeOperacao: $("#NomeOperacao").val(),
        Descricao: $("#Descricao").val(),
        SimulacaoParcela: {
            QuantidadeParcela: $("#QuantidadeParcela").val(),
            ValorParcela: $("#ValorParcela").val(),
            DataPrimeiroVencimento: $("#SimulacaoParcela_DataPrimeiroVencimento").val(),
            TipoOperacao: $("#TipoOperacao").val(),
            TipoCalculo: $("#TipoCalculo").val(),
            Parcelas: Parcelas
        }
    };

    return OperacaoModel;
}

