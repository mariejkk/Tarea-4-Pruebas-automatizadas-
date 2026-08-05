
function initDataTable(container) {
    const table = container.querySelector("table");
    if (!table) return;

    if (container._dataTableTeardown) {
        container._dataTableTeardown();
    }

    const tbody = table.querySelector("tbody");
    const searchInput = container.querySelector('[data-role="search"]');
    const infoEl = container.querySelector('[data-role="info"]');
    const prevBtn = container.querySelector('[data-role="prev"]');
    const nextBtn = container.querySelector('[data-role="next"]');
    const pageNumbersEl = container.querySelector('[data-role="page-numbers"]');
    const pageSize = parseInt(container.getAttribute("data-page-size") || "10", 10);

    const allRows = Array.prototype.slice.call(tbody.querySelectorAll("tr"));
    let filteredRows = allRows.slice();
    let currentPage = 1;
    let sortState = { column: null, asc: true };

    function applySearch() {
        const term = ((searchInput && searchInput.value) || "").trim().toLowerCase();
        filteredRows = term === ""
            ? allRows.slice()
            : allRows.filter(row => row.textContent.toLowerCase().indexOf(term) !== -1);
        currentPage = 1;
        render();
    }

    function applySort(colIndex, type) {
        const asc = sortState.column === colIndex ? !sortState.asc : true;
        sortState = { column: colIndex, asc };

        filteredRows.sort((a, b) => {
            const cellA = a.children[colIndex] ? a.children[colIndex].textContent.trim() : "";
            const cellB = b.children[colIndex] ? b.children[colIndex].textContent.trim() : "";
            let comparison;
            if (type === "number") {
                comparison = parseFloat(cellA.replace(/[^0-9.\-]/g, "")) - parseFloat(cellB.replace(/[^0-9.\-]/g, ""));
            } else {
                comparison = cellA.localeCompare(cellB, "es", { sensitivity: "base" });
            }
            return asc ? comparison : -comparison;
        });
        render();
    }

    function render() {
        allRows.forEach(row => row.remove());

        const totalPages = Math.max(1, Math.ceil(filteredRows.length / pageSize));
        currentPage = Math.min(currentPage, totalPages);
        const start = (currentPage - 1) * pageSize;
        const pageRows = filteredRows.slice(start, start + pageSize);
        pageRows.forEach(row => tbody.appendChild(row));

        if (infoEl) {
            infoEl.textContent = filteredRows.length === 0
                ? "No se encontraron registros"
                : `Mostrando ${pageRows.length} de ${filteredRows.length} registros`;
        }

        if (pageNumbersEl) {
            pageNumbersEl.innerHTML = "";
            for (let p = 1; p <= totalPages; p++) {
                const btn = document.createElement("button");
                btn.type = "button";
                btn.textContent = p;
                btn.className = "page-number" + (p === currentPage ? " active" : "");
                btn.addEventListener("click", () => { currentPage = p; render(); });
                pageNumbersEl.appendChild(btn);
            }
        }

        if (prevBtn) prevBtn.disabled = currentPage === 1;
        if (nextBtn) nextBtn.disabled = currentPage === totalPages;
    }

    const manejadorBusqueda = () => applySearch();
    const manejadorPrev = () => { if (currentPage > 1) { currentPage--; render(); } };
    const manejadorNext = () => { currentPage++; render(); };

    if (searchInput) searchInput.addEventListener("input", manejadorBusqueda);
    if (prevBtn) prevBtn.addEventListener("click", manejadorPrev);
    if (nextBtn) nextBtn.addEventListener("click", manejadorNext);

    const encabezadosOrdenables = [];
    Array.prototype.slice.call(table.querySelectorAll("thead th")).forEach((th, index) => {
        if (!th.hasAttribute("data-sort")) return;
        const manejador = () => applySort(index, th.getAttribute("data-sort"));
        th.addEventListener("click", manejador);
        encabezadosOrdenables.push({ th, manejador });
    });

    container._dataTableTeardown = function () {
        if (searchInput) searchInput.removeEventListener("input", manejadorBusqueda);
        if (prevBtn) prevBtn.removeEventListener("click", manejadorPrev);
        if (nextBtn) nextBtn.removeEventListener("click", manejadorNext);
        encabezadosOrdenables.forEach(({ th, manejador }) => th.removeEventListener("click", manejador));
    };

    render();
}