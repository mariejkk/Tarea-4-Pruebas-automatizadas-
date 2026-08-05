using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GestionCanchas.Tests.PageObjects
{
    public class ReservasPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private static readonly By BotonNueva = By.Id("btnNuevaReserva");
        private static readonly By Tabla = By.Id("tablaReservas");
        private static readonly By CampoCancha = By.Id("CanchaId");
        private static readonly By CampoCliente = By.Id("NombreCliente");
        private static readonly By CampoFecha = By.Id("FechaReserva");
        private static readonly By CampoHoraInicio = By.Id("HoraInicio");
        private static readonly By CampoHoraFin = By.Id("HoraFin");
        private static readonly By BotonGuardar = By.Id("btnGuardarReserva");
        private static readonly By CajaErrores = By.Id("erroresReserva");
        private static readonly By CampoBuscar = By.Id("buscarReservas");
        private static readonly By ModalReserva = By.Id("modalReserva");
        private static readonly By ModalConfirmarEliminar = By.Id("modalConfirmarEliminarReserva");
        private static readonly By BotonConfirmarEliminar = By.Id("btnConfirmarEliminarReserva");
        private static readonly By BotonCancelarEliminar = By.Id("btnCancelarEliminarReserva");

        public ReservasPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void Ir()
        {
            _driver.Navigate().GoToUrl(Config.BaseUrl + "reservas.html");
            _wait.Until(d => d.FindElement(Tabla).Displayed);
        }

        public void AbrirModalNuevaReserva()
        {
            _wait.Until(d => d.FindElement(BotonNueva)).Click();
            _wait.Until(d => d.FindElement(ModalReserva).Displayed);
        }

        public void SeleccionarPrimeraCanchaDisponible()
        {
            var select = new SelectElement(_driver.FindElement(CampoCancha));
            var opciones = select.Options.Where(o => !string.IsNullOrEmpty(o.GetAttribute("value"))).ToList();
            if (opciones.Count == 0)
                throw new InvalidOperationException("No hay canchas disponibles para reservar. Crea una cancha primero.");
            select.SelectByValue(opciones[0].GetAttribute("value"));
        }

        public void LlenarFormulario(string cliente, string fechaIso, string horaInicio, string horaFin)
        {
            var campoCliente = _driver.FindElement(CampoCliente);
            campoCliente.Clear();
            campoCliente.SendKeys(cliente);

            EstablecerCampoFecha(CampoFecha, fechaIso);
            EstablecerCampoHora(CampoHoraInicio, horaInicio);
            EstablecerCampoHora(CampoHoraFin, horaFin);
        }


        private void EstablecerCampoFecha(By locator, string valorIso)
        {
            var el = _driver.FindElement(locator);
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('change'));", el, valorIso);
        }

        private void EstablecerCampoHora(By locator, string valorHHmm)
        {
            var el = _driver.FindElement(locator);
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('change'));", el, valorHHmm);
        }

        public void Guardar() => _driver.FindElement(BotonGuardar).Click();

        public void Buscar(string texto)
        {
            var campo = _wait.Until(d => d.FindElement(CampoBuscar));
            campo.Clear();
            campo.SendKeys(texto);
        }

        public bool ModalSigueAbierto()
        {
            return _driver.FindElement(ModalReserva).GetAttribute("class")!.Contains("abierto");
        }

        public string ObtenerErrores()
        {
            _wait.Until(d => !string.IsNullOrEmpty(d.FindElement(CajaErrores).Text));
            return _driver.FindElement(CajaErrores).Text;
        }

        public void EsperarFilaConTexto(string texto)
        {
            _wait.Until(d => d.FindElements(By.XPath($"//table[@id='tablaReservas']//tr[contains(., '{texto}')]")).Count > 0);
        }

        public bool ExisteFilaConTexto(string texto)
        {
            return _driver.FindElements(By.XPath($"//table[@id='tablaReservas']//tr[contains(., '{texto}')]")).Count > 0;
        }

        private IWebElement ObtenerFilaPorTexto(string texto)
        {
            EsperarFilaConTexto(texto);
            return _driver.FindElement(By.XPath($"//table[@id='tablaReservas']//tr[contains(., '{texto}')]"));
        }

        public void ClickEliminarPorCliente(string cliente)
        {
            var fila = ObtenerFilaPorTexto(cliente);
            fila.FindElement(By.CssSelector("button[id^='eliminar-reserva-']")).Click();
            _wait.Until(d => d.FindElement(ModalConfirmarEliminar).GetAttribute("class")!.Contains("abierto"));
        }

        public void ConfirmarEliminar() => _driver.FindElement(BotonConfirmarEliminar).Click();

        public void CancelarEliminar() => _driver.FindElement(BotonCancelarEliminar).Click();

        public void EsperarQueDesaparezcaFilaConTexto(string texto)
        {
            _wait.Until(d => d.FindElements(By.XPath($"//table[@id='tablaReservas']//tr[contains(., '{texto}')]")).Count == 0);
        }
    }
}
