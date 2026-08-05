using GestionCanchas.Tests.PageObjects;
using NUnit.Framework;

namespace GestionCanchas.Tests.Tests
{
 
    [TestFixture]
    public class LoginTests : TestBase
    {
        [Test]
        [Category("CaminoFeliz")]
        public void Login_CaminoFeliz_CredencialesValidas_RedirigeADashboard()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Ir();

            loginPage.IniciarSesion(Config.UsuarioValido, Config.PasswordValido);

            Assert.That(loginPage.EsperarRedireccionADashboard(), Is.True,
                "Con credenciales válidas, el sistema debería redirigir a canchas.html.");
        }

        [Test]
        [Category("PruebaNegativa")]
        public void Login_PruebaNegativa_PasswordIncorrecta_MuestraMensajeError()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Ir();

            loginPage.IniciarSesion(Config.UsuarioValido, "ContraseñaIncorrecta123");

            var mensaje = loginPage.ObtenerMensajeError();

            Assert.That(mensaje, Is.Not.Empty, "Debe mostrarse un mensaje de error visible.");
            Assert.That(loginPage.SigueEnLogin(), Is.True, "No debe redirigir con credenciales inválidas.");
        }

        [Test]
        [Category("PruebaLimite")]
        public void Login_PruebaLimite_CamposVacios_NoEnviaFormulario()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Ir();

            loginPage.ClickEntrar();

            var validacionNativa = loginPage.ObtenerValidacionNativaUsuario();

            Assert.That(validacionNativa, Is.Not.Empty,
                "El campo de usuario vacío debe disparar la validación nativa del navegador.");
            Assert.That(loginPage.SigueEnLogin(), Is.True, "No debe salir de login.html con campos vacíos.");
        }
    }
}
