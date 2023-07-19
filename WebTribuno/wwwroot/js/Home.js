$(document).ready(function () {

    $("p[name='Data']").inputmask("mask", { "mask": "99/99/9999" });

    var coll = document.getElementsByClassName("collapsible");
    var i;

    for (i = 0; i < coll.length; i++) {
        coll[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var content = this.nextElementSibling;
            if (content.style.display === "block") {
                content.style.display = "none";
            } else {
                content.style.display = "block";
            }
        });
    }    
});

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

