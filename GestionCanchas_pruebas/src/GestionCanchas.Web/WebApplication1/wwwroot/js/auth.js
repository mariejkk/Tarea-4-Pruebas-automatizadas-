
function protegerPagina() {
    if (!localStorage.getItem("token")) {
        window.location.href = "login.html";
    }
}

function mostrarUsuarioActual() {
    const el = document.getElementById("usuarioActual");
    if (!el) return;
    const usuarioRaw = localStorage.getItem("usuario");
    if (usuarioRaw) {
        const usuario = JSON.parse(usuarioRaw);
        el.textContent = "Hola, " + (usuario.nombreCompleto || usuario.nombreUsuario);
    }
}

function cerrarSesion() {
    localStorage.removeItem("token");
    localStorage.removeItem("usuario");
    window.location.href = "login.html";
}

document.addEventListener("DOMContentLoaded", function () {
    mostrarUsuarioActual();
    const btnLogout = document.getElementById("btnLogout");
    if (btnLogout) {
        btnLogout.addEventListener("click", cerrarSesion);
    }

    const formLogin = document.getElementById("formLogin");
    if (formLogin) {
        formLogin.addEventListener("submit", manejarLogin);
    }
});

async function manejarLogin(event) {
    event.preventDefault();

    const errorBox = document.getElementById("errorLogin");
    errorBox.textContent = "";
    errorBox.style.display = "none";

    const nombreUsuario = document.getElementById("NombreUsuario").value.trim();
    const password = document.getElementById("Password").value;

    const btn = document.getElementById("btnLogin");
    btn.disabled = true;
    btn.textContent = "Entrando...";

    try {
        const data = await api.post("/auth/login", { NombreUsuario: nombreUsuario, Password: password });
        localStorage.setItem("token", data.token);
        localStorage.setItem("usuario", JSON.stringify(data.usuario));
        window.location.href = "canchas.html";
    } catch (err) {
        errorBox.textContent = (err.errores && err.errores[0]) || "No se pudo iniciar sesión.";
        errorBox.style.display = "block";
    } finally {
        btn.disabled = false;
        btn.textContent = "Entrar";
    }
}
