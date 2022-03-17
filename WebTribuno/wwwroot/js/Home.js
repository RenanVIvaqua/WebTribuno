
function alterarOperacao(idOperacao) {
    alert("Deseja alterar operacao:" + idOperacao)
}

function excluirOperacao(id) {
    $.ajax({
        url: "/Home/DeletarOperacao",
        type: "POST",
        data: { idOperacao: id },
        async: false,
        success: function (retorno) {
            atualizarGridOperacao();
            alert(retorno.message);
        },
        error: function (retorno) {
            alert(retorno.message);
        }
    });
}

function atualizarGridOperacao() {
    $.ajax({
        url: "/Home/AtualizarGridOperacao",
        type: "POST",
        async: false,
        success: function (data) {
            $("body").html(data);
        }
    });
}

