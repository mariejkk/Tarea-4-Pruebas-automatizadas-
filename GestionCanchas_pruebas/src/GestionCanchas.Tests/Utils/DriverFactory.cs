using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace GestionCanchas.Tests.Utils
{
    public static class DriverFactory
    {
    
        public static IWebDriver CrearChromeDriver()
        {
            var opciones = new ChromeOptions();
            opciones.AddArgument("--start-maximized");
            opciones.AddArgument("--ignore-certificate-errors"); 
            return new ChromeDriver(opciones);
        }
    }
}
