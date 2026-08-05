using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GestionCanchas.Tests.PageObjects
{
    public class CanchasPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private static readonly By BotonNueva = By.Id("btnNuevaCancha");
        private static readonly By Tabla = By.Id("tablaCanchas");
        private static readonly By CampoNombre = By.Id("Nombre");
        private static readonly By CampoDeporte = By.Id("TipoDeporte");
        private static readonly By CampoUbicacion = By.Id("Ubicacion");
        private static readonly By CampoPrecio = By.Id("PrecioPorHora");
        private static readonly By CheckDisponible = By.Id("Disponible");
        private static readonly By BotonGuardar = By.Id("btnGuardarCancha");
        private static readonly By CajaErrores = By.Id("erroresCancha");
        private static readonly By CampoBuscar = By.Id("buscarCanchas");
        private static readonly By ModalCancha = By.Id("modalCancha");
        private static readonly By ModalConfirmarEliminar = By.Id("modalConfirmarEliminar");
        private static readonly By BotonConfirmarEliminar = By.Id("btnConfirmarEliminarCancha");
        private static readonly By BotonCancelarEliminar = By.Id("btnCancelarEliminarCancha");
        private static readonly By InfoTabla = By.Id("infoCanchas");

        public CanchasPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void Ir()
        {
            _driver.Navigate().GoToUrl(Config.BaseUrl + "canchas.html");
            _wait.Until(d => d.FindElement(Tabla).Displayed);
        }

        public void AbrirModalNuevaCancha()
        {
            _wait.Until(d => d.FindElement(BotonNueva)).Click();
            _wait.Until(d => d.FindElement(ModalCancha).GetAttribute("class")!.Contains("abierto"));
        }

        public void LlenarFormulario(string nombre, string deporte, string ubicacion, string precio, bool disponible = true)
        {
            var campoNombre = _driver.FindElement(CampoNombre);
            campoNombre.Clear();
            campoNombre.SendKeys(nombre);

            if (!string.IsNullOrEmpty(deporte))
            {
                new SelectElement(_driver.FindElement(CampoDeporte)).SelectByValue(deporte);
            }

            var campoUbicacion = _driver.FindElement(CampoUbicacion);
            campoUbicacion.Clear();
            campoUbicacion.SendKeys(ubicacion);

            var campoPrecio = _driver.FindElement(CampoPrecio);
            campoPrecio.Clear();
            if (!string.IsNullOrEmpty(precio))
            {
                campoPrecio.SendKeys(precio);
            }

            var checkbox = _driver.FindElement(CheckDisponible);
            if (checkbox.Selected != disponible) checkbox.Click();
        }

        public void Guardar() => _driver.FindElement(BotonGuardar).Click();

        public void Buscar(string texto)
        {
            var campo = _wait.Until(d => d.FindElement(CampoBuscar));
            campo.Clear();
            campo.SendKeys(texto);
        }

        public string ObtenerTextoInfoTabla() => _driver.FindElement(InfoTabla).Text;

        public bool ModalSigueAbierto()
        {
            return _driver.FindElement(ModalCancha).GetAttribute("class")!.Contains("abierto");
        }

        public string ObtenerErrores()
        {
            _wait.Until(d => !string.IsNullOrEmpty(d.FindElement(CajaErrores).Text));
            return _driver.FindElement(CajaErrores).Text;
        }

        public void EsperarFilaConTexto(string texto)
        {
            _wait.Until(d => d.FindElements(By.XPath($"//table[@id='tablaCanchas']//tr[contains(., '{texto}')]")).Count > 0);
        }

        public void EsperarQueDesaparezcaFilaConTexto(string texto)
        {
            _wait.Until(d => d.FindElements(By.XPath($"//table[@id='tablaCanchas']//tr[contains(., '{texto}')]")).Count == 0);
        }

        public bool ExisteFilaConTexto(string texto)
        {
            return _driver.FindElements(By.XPath($"//table[@id='tablaCanchas']//tr[contains(., '{texto}')]")).Count > 0;
        }

        private IWebElement ObtenerFilaPorTexto(string texto)
        {
            EsperarFilaConTexto(texto);
            return _driver.FindElement(By.XPath($"//table[@id='tablaCanchas']//tr[contains(., '{texto}')]"));
        }

        public void ClickEditarPorNombre(string nombre)
        {
            var fila = ObtenerFilaPorTexto(nombre);
            fila.FindElement(By.CssSelector("button[id^='editar-']")).Click();
            _wait.Until(d => d.FindElement(ModalCancha).GetAttribute("class")!.Contains("abierto"));
        }

        public void ClickEliminarPorNombre(string nombre)
        {
            var fila = ObtenerFilaPorTexto(nombre);
            fila.FindElement(By.CssSelector("button[id^='eliminar-']")).Click();
            _wait.Until(d => d.FindElement(ModalConfirmarEliminar).GetAttribute("class")!.Contains("abierto"));
        }

        public void ClickAlternarDisponibilidadPorNombre(string nombre)
        {
            var fila = ObtenerFilaPorTexto(nombre);
            fila.FindElement(By.CssSelector("button[id^='disponibilidad-']")).Click();
        }

        public void ConfirmarEliminar() => _driver.FindElement(BotonConfirmarEliminar).Click();

        public void CancelarEliminar() => _driver.FindElement(BotonCancelarEliminar).Click();
    }
}
