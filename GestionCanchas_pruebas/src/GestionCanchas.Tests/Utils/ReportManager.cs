using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace GestionCanchas.Tests.Utils
{

    public static class ReportManager
    {
        private static ExtentReports? _extent;
        private static readonly object _candado = new();

        public static ExtentReports Instancia
        {
            get
            {
                if (_extent == null)
                {
                    lock (_candado)
                    {
                        if (_extent == null)
                        {
                            var carpeta = Path.Combine(TestContext.CurrentContext.TestDirectory, "Reportes");
                            Directory.CreateDirectory(carpeta);

                            var ruta = Path.Combine(carpeta, "ReporteEjecucion.html");
                            var reporter = new ExtentSparkReporter(ruta)
                            {
                                Config = { DocumentTitle = "Gestor de Canchas - Reporte de Pruebas Automatizadas" }
                            };

                            _extent = new ExtentReports();
                            _extent.AttachReporter(reporter);
                        }
                    }
                }
                return _extent;
            }
        }

        public static void Flush() => _extent?.Flush();
    }
}
