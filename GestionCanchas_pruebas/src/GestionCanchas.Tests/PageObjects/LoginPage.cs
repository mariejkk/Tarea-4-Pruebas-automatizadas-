using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GestionCanchas.Tests.PageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private static readonly By CampoUsuario = By.Id("NombreUsuario");
        private static readonly By CampoPassword = By.Id("Password");
        private static readonly By BotonEntrar = By.Id("btnLogin");
        private static readonly By MensajeError = By.Id("errorLogin");

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public void Ir()
        {
            _driver.Navigate().GoToUrl(Config.BaseUrl + "login.html");
            _wait.Until(d => d.FindElement(CampoUsuario).Displayed);
        }

        public void EscribirUsuario(string usuario)
        {
            var campo = _driver.FindElement(CampoUsuario);
            campo.Clear();
            campo.SendKeys(usuario);
        }

        public void EscribirPassword(string password)
        {
            var campo = _driver.FindElement(CampoPassword);
            campo.Clear();
            campo.SendKeys(password);
        }

        public void ClickEntrar() => _driver.FindElement(BotonEntrar).Click();

        public void IniciarSesion(string usuario, string password)
        {
            EscribirUsuario(usuario);
            EscribirPassword(password);
            ClickEntrar();
        }

        public bool EsperarRedireccionADashboard()
        {
            try
            {
                _wait.Until(d => d.Url.Contains("canchas.html"));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string ObtenerMensajeError()
        {
            _wait.Until(d => d.FindElement(MensajeError).Displayed);
            return _driver.FindElement(MensajeError).Text;
        }

        public string ObtenerValidacionNativaUsuario()
        {
            return _driver.FindElement(CampoUsuario).GetAttribute("validationMessage") ?? string.Empty;
        }

        public bool SigueEnLogin() => _driver.Url.Contains("login.html");
    }
}
