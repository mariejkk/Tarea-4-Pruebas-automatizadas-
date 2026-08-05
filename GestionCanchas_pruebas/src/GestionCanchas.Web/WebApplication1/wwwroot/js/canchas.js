protegerPagina();

let canchaAEliminarId = null;

document.addEventListener("DOMContentLoaded", function () {
    cargarCanchas();

    document.getElementById("btnNuevaCancha").addEventListener("click", () => abrirModalCancha(null));
    document.getElementById("btnCancelarModalCancha").addEventListener("click", cerrarModalCancha);
    document.getElementById("formCancha").addEventListener("submit", guardarCancha);

    document.getElementById("btnCancelarEliminarCancha").addEventListener("click", cerrarModalEliminar);
    document.getElementById("btnConfirmarEliminarCancha").addEventListener("click", confirmarEliminarCancha);
});

async function cargarCanchas() {
    const tbody = document.getElementById("tbodyCanchas");
    tbody.innerHTML = "";

    let canchas = [];
    try {
        canchas = await api.get("/canchas");
    } catch (err) {
        tbody.innerHTML = `<tr><td colspan="6">${(err.errores && err.errores[0]) || "No se pudieron cargar las canchas."}</td></tr>`;
        return;
    }

    canchas.forEach(c => tbody.appendChild(crearFilaCancha(c)));
    initDataTable(document.getElementById("contenedorTablaCanchas"));
}

function crearFilaCancha(c) {
    const tr = document.createElement("tr");
    tr.id = "fila-cancha-" + c.id;

    const badge = c.disponible
        ? '<span class="badge badge-success">Disponible</span>'
        : '<span class="badge badge-muted">No disponible</span>';

    tr.innerHTML = `
        <td>
            <div class="action-buttons">
                <button type="button" id="editar-${c.id}" class="icon-btn icon-btn-edit" title="Editar">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 20h9"/><path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4Z"/></svg>
                </button>
                <button type="button" id="disponibilidad-${c.id}" class="icon-btn icon-btn-view" title="Alternar disponibilidad">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"/></svg>
                </button>
                <button type="button" id="eliminar-${c.id}" class="icon-btn icon-btn-delete" title="Eliminar">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"/><path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6"/><path d="M10 11v6"/><path d="M14 11v6"/></svg>
                </button>
            </div>
        </td>
        <td>${escapeHtml(c.nombre)}</td>
        <td>${escapeHtml(c.tipoDeporte)}</td>
        <td>${escapeHtml(c.ubicacion)}</td>
        <td>$${Number(c.precioPorHora).toFixed(2)}</td>
        <td>${badge}</td>
    `;

    tr.querySelector("#editar-" + c.id).addEventListener("click", () => abrirModalCancha(c));
    tr.querySelector("#disponibilidad-" + c.id).addEventListener("click", () => alternarDisponibilidad(c.id));
    tr.querySelector("#eliminar-" + c.id).addEventListener("click", () => abrirModalEliminar(c));

    return tr;
}

function abrirModalCancha(cancha) {
    const esEdicion = !!cancha;
    document.getElementById("tituloModalCancha").textContent = esEdicion ? "Editar Cancha" : "Nueva Cancha";
    document.getElementById("erroresCancha").style.display = "none";
    document.getElementById("erroresCancha").innerHTML = "";

    document.getElementById("CanchaId").value = esEdicion ? cancha.id : 0;
    document.getElementById("Nombre").value = esEdicion ? cancha.nombre : "";
    document.getElementById("TipoDeporte").value = esEdicion ? cancha.tipoDeporte : "";
    document.getElementById("Ubicacion").value = esEdicion ? cancha.ubicacion : "";
    document.getElementById("PrecioPorHora").value = esEdicion ? cancha.precioPorHora : "";
    document.getElementById("Disponible").checked = esEdicion ? cancha.disponible : true;

    document.getElementById("modalCancha").classList.add("abierto");
}

function cerrarModalCancha() {
    document.getElementById("modalCancha").classList.remove("abierto");
}

async function guardarCancha(event) {
    event.preventDefault();

    const id = parseInt(document.getElementById("CanchaId").value, 10);
    const dto = {
        Id: id,
        Nombre: document.getElementById("Nombre").value.trim(),
        TipoDeporte: document.getElementById("TipoDeporte").value,
        Ubicacion: document.getElementById("Ubicacion").value.trim(),
        PrecioPorHora: parseFloat(document.getElementById("PrecioPorHora").value),
        Disponible: document.getElementById("Disponible").checked
    };

    const btn = document.getElementById("btnGuardarCancha");
    btn.disabled = true;

    try {
        if (id > 0) {
            await api.put(`/canchas/${id}`, dto);
        } else {
            await api.post("/canchas", dto);
        }
        cerrarModalCancha();
        await cargarCanchas();
    } catch (err) {
        mostrarErrores("erroresCancha", err.errores);
    } finally {
        btn.disabled = false;
    }
}

async function alternarDisponibilidad(id) {
    try {
        await api.patch(`/canchas/${id}/disponibilidad`);
        await cargarCanchas();
    } catch (err) {
        alert((err.errores && err.errores[0]) || "No se pudo actualizar la disponibilidad.");
    }
}

function abrirModalEliminar(cancha) {
    canchaAEliminarId = cancha.id;
    document.getElementById("nombreCanchaAEliminar").textContent = cancha.nombre;
    document.getElementById("modalConfirmarEliminar").classList.add("abierto");
}

function cerrarModalEliminar() {
    canchaAEliminarId = null;
    document.getElementById("modalConfirmarEliminar").classList.remove("abierto");
}

async function confirmarEliminarCancha() {
    if (!canchaAEliminarId) return;
    try {
        await api.delete(`/canchas/${canchaAEliminarId}`);
        cerrarModalEliminar();
        await cargarCanchas();
    } catch (err) {
        alert((err.errores && err.errores[0]) || "No se pudo eliminar la cancha.");
    }
}

function mostrarErrores(contenedorId, errores) {
    const box = document.getElementById(contenedorId);
    box.innerHTML = "<ul>" + (errores || ["Ocurrió un error."]).map(e => `<li>${escapeHtml(e)}</li>`).join("") + "</ul>";
    box.style.display = "block";
}

function escapeHtml(texto) {
    const div = document.createElement("div");
    div.textContent = texto ?? "";
    return div.innerHTML;
}
