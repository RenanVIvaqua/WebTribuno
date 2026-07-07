let parcelas = [];

$(function () {

    aplicarMascaras();

    $(document).on("click", "#btnCalcular", calcularParcelas);

    $(document).on("click", "#btGravar", salvarOperacao);

});

function aplicarMascaras() {

    $("#ValorParcela").maskMoney({
        allowNegative: true,
        thousands: '.',
        decimal: ',',
        affixesStay: false
    });

}

function calcularParcelas() {

    let botao = $("#btnCalcular");

    if (botao.prop("disabled")) {
        return;
    }

    let textoOriginal = botao.html();

    $("#formCadastroOperacao")
        .find("input, select, textarea, button")
        .prop("disabled", true);

    botao.html(`
        <i class="fa-solid fa-spinner fa-spin"></i>
        Calculando...
    `);

    $.ajax({

        url: "/CadastrarOperacao/CalcularParcela",

        type: "POST",

        data: gerarOperacaoModel(),

        success: function (response) {

            parcelas = response.parcelas ?? response.Parcelas;

            let valorTotal = response.valorTotal ?? response.ValorTotal;

            atualizarResumo(valorTotal);

            renderizarTabela();

        },

        error: function () {

            if (typeof mostrarToast === "function") {
                mostrarToast("Erro ao calcular parcelas.", "error");
            } else {
                alert("Erro ao calcular parcelas.");
            }

        },

        complete: function () {

            $("#formCadastroOperacao")
                .find("input, select, textarea, button")
                .prop("disabled", false);

            botao.html(textoOriginal);

        }

    });

}

function salvarOperacao() {

    let botao = $("#btGravar");

    let textoOriginal = botao.html();

    botao
        .prop("disabled", true)
        .html(`
            <i class="fa-solid fa-spinner fa-spin"></i>
            Salvando...
        `);

    $.ajax({

        url: "/CadastrarOperacao/Create",

        type: "POST",

        data: gerarOperacaoModel(),

        success: function (response) {

            mostrarToast(
                response.mensagem,
                "success"
            );

            setTimeout(function () {

                window.location.href = "/Home";

            }, 1800);

        },

        error: function (xhr) {

            let mensagem = "Erro ao salvar operação.";

            if (xhr.responseJSON &&
                xhr.responseJSON.mensagem) {

                mensagem = xhr.responseJSON.mensagem;

            }

            mostrarToast(
                mensagem,
                "error"
            );

            botao
                .prop("disabled", false)
                .html(textoOriginal);

        }

    });

}

function gerarOperacaoModel() {

    return {

        IdOperacao: $("#IdOperacao").val(),

        NomeOperacao: $("#NomeOperacao").val(),

        Descricao: $("#Descricao").val(),

        SimulacaoParcela: {

            QuantidadeParcela: $("#QuantidadeParcela").val(),

            ValorParcela: $("#ValorParcela").val(),

            DataPrimeiroVencimento: $("#SimulacaoParcela_DataPrimeiroVencimento").val(),

            TipoOperacao: $("#TipoOperacao").val(),

            TipoCalculo: $("#TipoCalculo").val(),

            Parcelas: parcelas

        }

    };

}

function renderizarTabela() {

    let tbody = $("#tbParcelas tbody");

    tbody.empty();

    if (parcelas.length === 0) {

        tbody.append(`
            <tr>

                <td colspan="3" class="text-center">

                    Nenhuma parcela calculada.

                </td>

            </tr>
        `);

        return;

    }

    parcelas.forEach(function (item) {

        let data = new Date(item.dataVencimento);

        let dataFormatada =
            data.toLocaleDateString("pt-BR");

        let valor = Number(item.valorParcela)
            .toLocaleString("pt-BR",
                {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });

        tbody.append(`

            <tr>

                <td>${item.numeroParcela}</td>

                <td>R$ ${valor}</td>

                <td>${dataFormatada}</td>

            </tr>

        `);

    });

}

function atualizarResumo(valorTotal) {

    $("#valorTotalOperacao").text(

        Number(valorTotal).toLocaleString(

            "pt-BR",

            {

                style: "currency",

                currency: "BRL"

            })

    );

}