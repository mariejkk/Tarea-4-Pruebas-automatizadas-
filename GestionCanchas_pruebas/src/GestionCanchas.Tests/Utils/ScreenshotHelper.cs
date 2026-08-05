using NUnit.Framework;
using OpenQA.Selenium;

namespace GestionCanchas.Tests.Utils
{
    public static class ScreenshotHelper
    {
        public static string Capturar(IWebDriver driver, string nombrePrueba)
        {
            var carpeta = Path.Combine(TestContext.CurrentContext.TestDirectory, "Reportes", "Screenshots");
            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Sanitizar(nombrePrueba)}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(rutaCompleta);

            return rutaCompleta;
        }

        private static string Sanitizar(string texto)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                texto = texto.Replace(c, '_');
            }
            return texto;
        }
    }
}
