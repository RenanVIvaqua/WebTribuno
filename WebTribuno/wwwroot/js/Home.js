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

//==========================================================
// TRIBUNO DASHBOARD
// home.js
//==========================================================

document.addEventListener("DOMContentLoaded", function () {

    inicializarSidebar();
    inicializarTooltips();
    animarCards();
    animarValores();
    efeitoRipple();
    destacarMenu();

});

//==========================================================
// Sidebar
//==========================================================

function inicializarSidebar() {

    const btnMenu = document.querySelector(".btn-menu");
    const sidebar = document.querySelector(".sidebar");

    if (!btnMenu || !sidebar)
        return;

    btnMenu.addEventListener("click", function () {

        sidebar.classList.toggle("collapsed");

    });

}

//==========================================================
// Tooltips Bootstrap
//==========================================================

function inicializarTooltips() {

    const tooltipTriggerList =
        [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));

    tooltipTriggerList.map(function (tooltipTriggerEl) {

        return new bootstrap.Tooltip(tooltipTriggerEl);

    });

}

//==========================================================
// Animação dos Cards
//==========================================================

function animarCards() {

    const cards = document.querySelectorAll(".card-dashboard");

    cards.forEach(function (card, index) {

        card.style.opacity = "0";
        card.style.transform = "translateY(25px)";

        setTimeout(function () {

            card.style.transition = ".45s";

            card.style.opacity = "1";
            card.style.transform = "translateY(0px)";

        }, index * 120);

    });

}

//==========================================================
// Contadores
//==========================================================

function animarValores() {

    const elementos = document.querySelectorAll(".card-dashboard h2");

    elementos.forEach(function (elemento) {

        const texto = elemento.innerText;

        if (!texto.includes("R$"))
            return;

        let numero = texto
            .replace("R$", "")
            .replace(/\./g, "")
            .replace(",", ".");

        numero = parseFloat(numero);

        if (isNaN(numero))
            return;

        let atual = 0;

        const incremento = numero / 60;

        elemento.innerText = "R$ 0,00";

        const timer = setInterval(function () {

            atual += incremento;

            if (atual >= numero) {

                atual = numero;

                clearInterval(timer);

            }

            elemento.innerText =
                atual.toLocaleString("pt-BR",
                    {
                        style: "currency",
                        currency: "BRL"
                    });

        }, 15);

    });

}

//==========================================================
// Ripple Effect
//==========================================================

function efeitoRipple() {

    const botoes = document.querySelectorAll(".btn");

    botoes.forEach(function (botao) {

        botao.addEventListener("click", function (e) {

            const circle = document.createElement("span");

            circle.classList.add("ripple");

            const rect = this.getBoundingClientRect();

            circle.style.left = (e.clientX - rect.left) + "px";
            circle.style.top = (e.clientY - rect.top) + "px";

            this.appendChild(circle);

            setTimeout(function () {

                circle.remove();

            }, 600);

        });

    });

}

//==========================================================
// Menu ativo
//==========================================================

function destacarMenu() {

    const menu = document.querySelectorAll(".menu a");

    menu.forEach(function (item) {

        item.addEventListener("click", function () {

            menu.forEach(function (m) {

                m.classList.remove("active");

            });

            this.classList.add("active");

        });

    });

}

//==========================================================
// Scroll suave
//==========================================================

window.scrollTo({

    top: 0,
    behavior: "smooth"

});

