using AventStack.ExtentReports;
using GestionCanchas.Tests.Utils;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace GestionCanchas.Tests
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IWebDriver Driver { get; private set; } = null!;
        protected ExtentTest Reporte { get; private set; } = null!;

        [SetUp]
        public void ConfigurarPrueba()
        {
            Driver = DriverFactory.CrearChromeDriver();
            Reporte = ReportManager.Instancia.CreateTest(TestContext.CurrentContext.Test.FullName);
        }

        [TearDown]
        public void FinalizarPrueba()
        {
            var resultado = TestContext.CurrentContext.Result.Outcome.Status;
            var nombrePrueba = TestContext.CurrentContext.Test.Name;

            try
            {
                if (resultado == TestStatus.Failed)
                {
                    var rutaCaptura = ScreenshotHelper.Capturar(Driver, nombrePrueba);
                    Reporte.Fail("La prueba falló.");
                    Reporte.Fail(TestContext.CurrentContext.Result.Message ?? string.Empty);
                    Reporte.AddScreenCaptureFromPath(rutaCaptura);
                }
                else
                {
                    var rutaCaptura = ScreenshotHelper.Capturar(Driver, nombrePrueba);
                    Reporte.Pass("La prueba pasó correctamente.");
                    Reporte.AddScreenCaptureFromPath(rutaCaptura);
                }
            }
            finally
            {
                Driver.Quit();
                Driver.Dispose();
            }
        }

        [OneTimeTearDown]
        public void FinalizarSuite()
        {
            ReportManager.Flush();
        }
    }
}
