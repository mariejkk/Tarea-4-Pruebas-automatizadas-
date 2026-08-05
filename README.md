# Gestor de Canchas Deportivas

## Descripción del proyecto

**Gestor de Canchas Deportivas** es una aplicación web para administrar canchas y reservas deportivas, desarrollada como proyecto base para la **Tarea 4 de Pruebas Automatizadas**.
El backend está desarrollado como una **API REST en ASP.NET Core**, siguiendo los principios de **Clean Architecture** e implementando autenticación mediante **JWT**.
El frontend fue construido con **HTML, CSS y JavaScript puro**, consumiendo directamente la API.
Sobre esta base se implementaron **15 pruebas automatizadas** utilizando **Selenium WebDriver en C# (NUnit)**, aplicando el patrón **Page Object Model (POM)**.

---

# Cómo correr las pruebas

## 1. Levantar la aplicación

Abre **dos terminales** y deja ambas aplicaciones ejecutándose.

### Backend (API)

```powershell
cd src/GestionCanchas.API
dotnet run --launch-profile https
```

### Frontend (Web)

```powershell
cd src/GestionCanchas.Web/WebApplication1
dotnet run --launch-profile https
```

Una vez iniciadas ambas aplicaciones, verifica que el frontend cargue correctamente abriendo en el navegador:

```text
https://localhost:7143/login.html
```

---

## 2. Ejecutar las pruebas automatizadas

Desde la carpeta:

```text
src/GestionCanchas.Tests
```

Ejecuta:

```powershell
dotnet test
```

O desde **Visual Studio: Prueba → Explorador de pruebas → Ejecutar todas**

> **Nota:** Chrome se abrirá y cerrará automáticamente una vez por cada prueba (15 en total). Este comportamiento es normal.

---

## 3. Revisar el reporte

Al finalizar la ejecución, abre el siguiente archivo:

```text
bin/Debug/net9.0/Reportes/ReporteEjecucion.html
```

Allí encontrarás:

- Estado de cada prueba (Exitosa o Fallida)
- Tiempo de ejecución
- Captura de pantalla de cada prueba

---

# Credenciales utilizadas en las pruebas

| Usuario | Contraseña |
|----------|------------|
| admin | Admin123! |








