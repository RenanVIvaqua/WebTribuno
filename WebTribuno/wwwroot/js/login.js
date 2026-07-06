// ==========================================
// TRIBUNO LOGIN
// login.js
// ==========================================

document.addEventListener("DOMContentLoaded", () => {

    const txtSenha = document.getElementById("Senha");
    const btnMostrarSenha = document.getElementById("btnMostrarSenha");

    const btnEntrar = document.getElementById("btnEntrar");
    const textoBotao = document.getElementById("textoBotao");
    const loadingBotao = document.getElementById("loadingBotao");

    const form = document.getElementById("loginForm");

    const txtUsuario = document.querySelector("input[name='LoginUsuario']");

    //==========================================
    // Foco inicial
    //==========================================

    if (txtUsuario) {
        txtUsuario.focus();
    }

    //==========================================
    // Mostrar/Ocultar senha
    //==========================================

    btnMostrarSenha.addEventListener("click", () => {

        const icon = btnMostrarSenha.querySelector("i");

        if (txtSenha.type === "password") {

            txtSenha.type = "text";

            icon.classList.remove("fa-eye");
            icon.classList.add("fa-eye-slash");

        } else {

            txtSenha.type = "password";

            icon.classList.remove("fa-eye-slash");
            icon.classList.add("fa-eye");

        }

    });

    //==========================================
    // Loading do botão
    //==========================================

    form.addEventListener("submit", () => {

        textoBotao.style.display = "none";
        loadingBotao.style.display = "flex";

        btnEntrar.disabled = true;

    });

});