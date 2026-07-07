function mostrarToast(mensagem, tipo) {

    let cor = tipo === "success"
        ? "#22c55e"
        : "#ef4444";

    let icone = tipo === "success"
        ? "fa-circle-check"
        : "fa-circle-xmark";

    const toast = $(`
        <div class="toast-modern">

            <i class="fa-solid ${icone}"></i>

            <span>${mensagem}</span>

        </div>
    `);

    toast.css("border-left", `4px solid ${cor}`);

    $("#toastContainer").append(toast);

    setTimeout(function () {

        toast.addClass("show");

    }, 50);

    setTimeout(function () {

        toast.removeClass("show");

        setTimeout(function () {

            toast.remove();

        }, 300);

    }, 2500);

}