protegerPagina();

let reservaAEliminarId = null;
let canchasDisponiblesCache = [];

document.addEventListener("DOMContentLoaded", async function () {
    await cargarCanchasParaSelect();
    await cargarReservas();

    document.getElementById("btnNuevaReserva").addEventListener("click", () => abrirModalReserva(null));
    document.getElementById("btnCancelarModalReserva").addEventListener("click", cerrarModalReserva);
    document.getElementById("formReserva").addEventListener("submit", guardarReserva);

    document.getElementById("btnCancelarEliminarReserva").addEventListener("click", cerrarModalEliminar);
    document.getElementById("btnConfirmarEliminarReserva").addEventListener("click", confirmarEliminarReserva);
});

async function cargarCanchasParaSelect() {
    const select = document.getElementById("CanchaId");
    try {
        const canchas = await api.get("/canchas");
        canchasDisponiblesCache = canchas.filter(c => c.disponible);
        canchasDisponiblesCache.forEach(c => {
            const opt = document.createElement("option");
            opt.value = c.id;
            opt.textContent = c.nombre;
            select.appendChild(opt);
        });
    } catch (err) {
        
    }
}

async function cargarReservas() {
    const tbody = document.getElementById("tbodyReservas");
    tbody.innerHTML = "";

    let reservas = [];
    try {
        reservas = await api.get("/reservas");
    } catch (err) {
        tbody.innerHTML = `<tr><td colspan="6">${(err.errores && err.errores[0]) || "No se pudieron cargar las reservas."}</td></tr>`;
        return;
    }

    reservas.forEach(r => tbody.appendChild(crearFilaReserva(r)));
    initDataTable(document.getElementById("contenedorTablaReservas"));
}

function crearFilaReserva(r) {
    const tr = document.createElement("tr");
    tr.id = "fila-reserva-" + r.id;

    const fecha = new Date(r.fechaReserva).toLocaleDateString("es-DO", { timeZone: "UTC" });

    tr.innerHTML = `
        <td>
            <div class="action-buttons">
                <button type="button" id="editar-reserva-${r.id}" class="icon-btn icon-btn-edit" title="Editar">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 20h9"/><path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4Z"/></svg>
                </button>
                <button type="button" id="eliminar-reserva-${r.id}" class="icon-btn icon-btn-delete" title="Eliminar">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"/><path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6"/><path d="M10 11v6"/><path d="M14 11v6"/></svg>
                </button>
            </div>
        </td>
        <td>${escapeHtml(r.nombreCancha || "")}</td>
        <td>${escapeHtml(r.nombreCliente)}</td>
        <td>${fecha}</td>
        <td>${formatearHora(r.horaInicio)}</td>
        <td>${formatearHora(r.horaFin)}</td>
    `;

    tr.querySelector("#editar-reserva-" + r.id).addEventListener("click", () => abrirModalReserva(r));
    tr.querySelector("#eliminar-reserva-" + r.id).addEventListener("click", () => abrirModalEliminar(r));

    return tr;
}

function formatearHora(valor) {
    
    return (valor || "").toString().substring(0, 5);
}

function abrirModalReserva(reserva) {
    const esEdicion = !!reserva;
    document.getElementById("tituloModalReserva").textContent = esEdicion ? "Editar Reserva" : "Nueva Reserva";
    document.getElementById("erroresReserva").style.display = "none";
    document.getElementById("erroresReserva").innerHTML = "";

    document.getElementById("ReservaId").value = esEdicion ? reserva.id : 0;
    document.getElementById("CanchaId").value = esEdicion ? reserva.canchaId : "";
    document.getElementById("NombreCliente").value = esEdicion ? reserva.nombreCliente : "";
    document.getElementById("FechaReserva").value = esEdicion ? reserva.fechaReserva.substring(0, 10) : "";
    document.getElementById("HoraInicio").value = esEdicion ? formatearHora(reserva.horaInicio) : "";
    document.getElementById("HoraFin").value = esEdicion ? formatearHora(reserva.horaFin) : "";

    document.getElementById("modalReserva").classList.add("abierto");
}

function cerrarModalReserva() {
    document.getElementById("modalReserva").classList.remove("abierto");
}

async function guardarReserva(event) {
    event.preventDefault();

    const id = parseInt(document.getElementById("ReservaId").value, 10);
    const dto = {
        Id: id,
        CanchaId: parseInt(document.getElementById("CanchaId").value, 10),
        NombreCliente: document.getElementById("NombreCliente").value.trim(),
        FechaReserva: document.getElementById("FechaReserva").value,
        HoraInicio: document.getElementById("HoraInicio").value,
        HoraFin: document.getElementById("HoraFin").value
    };

    const btn = document.getElementById("btnGuardarReserva");
    btn.disabled = true;

    try {
        if (id > 0) {
            await api.put(`/reservas/${id}`, dto);
        } else {
            await api.post("/reservas", dto);
        }
        cerrarModalReserva();
        await cargarReservas();
    } catch (err) {
        mostrarErrores("erroresReserva", err.errores);
    } finally {
        btn.disabled = false;
    }
}

function abrirModalEliminar(reserva) {
    reservaAEliminarId = reserva.id;
    document.getElementById("clienteReservaAEliminar").textContent = reserva.nombreCliente;
    document.getElementById("modalConfirmarEliminarReserva").classList.add("abierto");
}

function cerrarModalEliminar() {
    reservaAEliminarId = null;
    document.getElementById("modalConfirmarEliminarReserva").classList.remove("abierto");
}

async function confirmarEliminarReserva() {
    if (!reservaAEliminarId) return;
    try {
        await api.delete(`/reservas/${reservaAEliminarId}`);
        cerrarModalEliminar();
        await cargarReservas();
    } catch (err) {
        alert((err.errores && err.errores[0]) || "No se pudo eliminar la reserva.");
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
