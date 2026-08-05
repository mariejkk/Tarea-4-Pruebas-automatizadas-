
async function apiFetch(path, options = {}) {
    const token = localStorage.getItem("token");

    const headers = Object.assign(
        { "Accept": "application/json" },
        options.body ? { "Content-Type": "application/json" } : {},
        token ? { "Authorization": "Bearer " + token } : {},
        options.headers || {}
    );

    let response;
    try {
        response = await fetch(API_BASE_URL + path, Object.assign({}, options, { headers }));
    } catch (networkError) {
        throw { errores: ["No se pudo conectar con el servidor. Verifica que la API esté corriendo."] };
    }

    if (response.status === 401) {
        localStorage.removeItem("token");
        localStorage.removeItem("usuario");

        if (!window.location.pathname.endsWith("login.html")) {
            window.location.href = "login.html";
        }

        throw { errores: ["Credenciales incorrectas o sesión expirada."] };
    }

    if (response.status === 204) {
        return null; 
    }

    let data = null;
    const text = await response.text();
    if (text) {
        try { data = JSON.parse(text); } catch { data = null; }
    }

    if (!response.ok) {
        
        let errores = [];
        if (data?.errores) {
            errores = data.errores;
        } else if (data?.errors) {
            errores = Object.values(data.errors).flat();
        } else {
            errores = [`Error inesperado (código ${response.status}).`];
        }
        throw { errores, status: response.status };
    }

    return data;
}

const api = {
    get: (path) => apiFetch(path, { method: "GET" }),
    post: (path, body) => apiFetch(path, { method: "POST", body: JSON.stringify(body) }),
    put: (path, body) => apiFetch(path, { method: "PUT", body: JSON.stringify(body) }),
    patch: (path) => apiFetch(path, { method: "PATCH" }),
    delete: (path) => apiFetch(path, { method: "DELETE" })
};
