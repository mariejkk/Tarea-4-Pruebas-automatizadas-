using GestionCanchas.Tests.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;

namespace GestionCanchas.Tests.Tests
{
    
    [TestFixture]
    public class CanchaCrudTests : TestBase
    {
        private CanchasPage IniciarSesionYNavegarACanchas()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Ir();
            loginPage.IniciarSesion(Config.UsuarioValido, Config.PasswordValido);
            loginPage.EsperarRedireccionADashboard();

            return new CanchasPage(Driver);
        }


        [Test]
        [Category("CaminoFeliz")]
        public void CrearCancha_CaminoFeliz_ApareceEnTabla()
        {
            var canchasPage = IniciarSesionYNavegarACanchas();
            var nombre = "Cancha Selenium " + DateTime.Now.Ticks;

            canchasPage.AbrirModalNuevaCancha();
            canchasPage.LlenarFormulario(nombre, "Futbol", "Zona de pruebas", "50.00");
            canchasPage.Guardar();

            canchasPage.EsperarFilaConTexto(nombre);
            Assert.That(canchasPage.ExisteFilaConTexto(nombre), Is.True,
                "La cancha creada debe aparecer en la tabla.");
        }

        [Test]
        [Category("PruebaNegativa")]
        public void CrearCancha_PruebaNegativa_NombreVacio_NoGuarda()
        {
            var canchasPage = IniciarSesionYNavegarACanchas();

            canchasPage.AbrirModalNuevaCancha();
            canchasPage.LlenarFormulario("", "Futbol", "Zona de pruebas", "50.00");
            canchasPage.Guardar();

            Assert.That(canchasPage.ModalSigueAbierto(), Is.True,
                "Con el nombre vacío, el formulario no debe enviarse y el modal debe permanecer abierto.");
        }

        [Test]
        [Category("PruebaLimite")]
        public void CrearCancha_PruebaLimite_PrecioMaximoPermitido_Guarda()
        {
            var canchasPage = IniciarSesionYNavegarACanchas();
            var nombre = "Cancha Limite Max " + DateTime.Now.Ticks;

            canchasPage.AbrirModalNuevaCancha();
            canchasPage.LlenarFormulario(nombre, "Baloncesto", "Zona de pruebas", "10000");
            canchasPage.Guardar();

            canchasPage.EsperarFilaConTexto(nombre);
            Assert.That(canchasPage.ExisteFilaConTexto(nombre), Is.True,
                "El precio en el límite superior permitido (10000) debe guardarse correctamente.");
        }


        [Test]
        [Category("CaminoFeliz")]
        public void EditarCancha_CaminoFeliz_CambiosSeReflejanEnTabla()
        {
            var canchasPage = IniciarSesionYNavegarACanchas();
            var nombreOriginal = "Cancha Editar " + DateTime.Now.Ticks;
            var nombreEditado = nombreOriginal + " (Editada)";

            canchasPage.AbrirModalNuevaCancha();
            canchasPage.LlenarFormulario(nombreOriginal, "Tenis", "Zona original", "30.00");
            canchasPage.Guardar();
            canchasPage.EsperarFilaConTexto(nombreOriginal);

            canchasPage.ClickEditarPorNombre(nombreOriginal);
            canchasPage.LlenarFormulario(nombreEditado, "Tenis", "Zona actualizada", "35.00");
            canchasPage.Guardar();

            canchasPage.EsperarFilaConTexto(nombreEditado);
            Assert.That(canchasPage.ExisteFilaConTexto(nombreEditado), Is.True,
                "El nombre actualizado debe reflejarse en la tabla.");
        }

        [Test]
        [Category("PruebaNegativa")]
        public void EditarCancha_PruebaNegativa_PrecioVacio_NoGuarda()
        {
            var canchasPage = IniciarSesionYNavegarACanchas();
            var nombre = "Cancha Editar Negativo " + DateTime.Now.Ticks;

            canchasPage.AbrirModalNuevaCancha();
            canchasPage.LlenarFormulario(nombre, "Voleibol", "Zona de pruebas", "20.00");
            canchasPage.Guardar();
            canchasPage.EsperarFilaConTexto(nombre);

            canchasPage.ClickEditarPorNombre(nombre);
            canchasPage.LlenarFormulario(nombre, "Voleibol", "Zona de pruebas", "");
            canchasPage.Guardar();

            Assert.That(canchasPage.ModalSigueAbierto(), Is.True,
                "Con el precio vacío, el formulario no debe enviarse.");
        }

        [Test]
        [Category("PruebaLimite")]
        public void EditarCancha_PruebaLimite_PrecioMenorAlMinimo_MuestraErrorDelBackend()
        {
            var canchasPage = IniciarSesionYNavegarACanchas();
            var nombre = "Cancha Editar Limite " + DateTime.Now.Ticks;

            canchasPage.AbrirModalNuevaCancha();
            canchasPage.LlenarFormulario(nombre, "Beisbol", "Zona de pruebas", "15.00");
            canchasPage.Guardar();
            canchasPage.EsperarFilaConTexto(nombre);

            canchasPage.ClickEditarPorNombre(nombre);

            var inputPrecio = Driver.FindElement(By.Id("PrecioPorHora"));
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].value = '0';", inputPrecio);

            canchasPage.Guardar();

            var errores = canchasPage.ObtenerErrores();
            Assert.That(errores, Is.Not.Empty,
                "Un precio de 0 está fuera del rango permitido y el backend debe rechazarlo.");
        }
    }
}
