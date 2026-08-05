using GestionCanchas.Tests.PageObjects;
using NUnit.Framework;

namespace GestionCanchas.Tests.Tests
{
    [TestFixture]
    public class ReservaCrudTests : TestBase
    {
        private ReservasPage IniciarSesionYNavegarAReservas()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Ir();
            loginPage.IniciarSesion(Config.UsuarioValido, Config.PasswordValido);
            loginPage.EsperarRedireccionADashboard();

            var reservasPage = new ReservasPage(Driver);
            reservasPage.Ir(); 
            return reservasPage;
        }

        [Test]
        [Category("CaminoFeliz")]
        public void CrearReserva_CaminoFeliz_ApareceEnTabla()
        {
            var reservasPage = IniciarSesionYNavegarAReservas();
            var cliente = "Cliente Selenium " + DateTime.Now.Ticks;
            var manana = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

            reservasPage.AbrirModalNuevaReserva();
            reservasPage.SeleccionarPrimeraCanchaDisponible();
            reservasPage.LlenarFormulario(cliente, manana, "10:00", "11:00");
            reservasPage.Guardar();

            reservasPage.EsperarFilaConTexto(cliente);
            Assert.That(reservasPage.ExisteFilaConTexto(cliente), Is.True,
                "La reserva creada debe aparecer en la tabla.");
        }

        [Test]
        [Category("PruebaNegativa")]
        public void CrearReserva_PruebaNegativa_HoraFinAntesDeHoraInicio_MuestraErrorDelBackend()
        {
            var reservasPage = IniciarSesionYNavegarAReservas();
            var cliente = "Cliente Negativo " + DateTime.Now.Ticks;
            var manana = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

            reservasPage.AbrirModalNuevaReserva();
            reservasPage.SeleccionarPrimeraCanchaDisponible();

            reservasPage.LlenarFormulario(cliente, manana, "11:00", "10:00");
            reservasPage.Guardar();

            var errores = reservasPage.ObtenerErrores();
            Assert.That(errores, Does.Contain("hora de fin").IgnoreCase.Or.Not.Empty,
                "El backend debe rechazar una reserva con la hora de fin antes que la de inicio.");
        }

        [Test]
        [Category("PruebaLimite")]
        public void CrearReserva_PruebaLimite_FechaHoy_Guarda()
        {
            var reservasPage = IniciarSesionYNavegarAReservas();
            var cliente = "Cliente Limite Fecha " + DateTime.Now.Ticks;
            var hoy = DateTime.Today.ToString("yyyy-MM-dd");

            reservasPage.AbrirModalNuevaReserva();
            reservasPage.SeleccionarPrimeraCanchaDisponible();
            reservasPage.LlenarFormulario(cliente, hoy, "14:00", "15:00");
            reservasPage.Guardar();

            reservasPage.EsperarFilaConTexto(cliente);
            Assert.That(reservasPage.ExisteFilaConTexto(cliente), Is.True,
                "Una reserva con fecha de hoy (límite inferior permitido) debe guardarse.");
        }

        [Test]
        [Category("CaminoFeliz")]
        public void EliminarReserva_CaminoFeliz_DesapareceDeTabla()
        {
            var reservasPage = IniciarSesionYNavegarAReservas();
            var cliente = "Cliente A Eliminar " + DateTime.Now.Ticks;
            var manana = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

            reservasPage.AbrirModalNuevaReserva();
            reservasPage.SeleccionarPrimeraCanchaDisponible();
            reservasPage.LlenarFormulario(cliente, manana, "09:00", "10:00");
            reservasPage.Guardar();
            reservasPage.EsperarFilaConTexto(cliente);

            reservasPage.ClickEliminarPorCliente(cliente);
            reservasPage.ConfirmarEliminar();

            Assert.That(EsperarQueDesaparezca(reservasPage, cliente), Is.True,
                "Tras confirmar la eliminación, la reserva no debe seguir en la tabla.");
        }

        [Test]
        [Category("PruebaNegativa")]
        public void EliminarReserva_PruebaNegativa_CancelarConfirmacion_PermaneceEnTabla()
        {
            var reservasPage = IniciarSesionYNavegarAReservas();
            var cliente = "Cliente No Eliminar " + DateTime.Now.Ticks;
            var manana = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

            reservasPage.AbrirModalNuevaReserva();
            reservasPage.SeleccionarPrimeraCanchaDisponible();
            reservasPage.LlenarFormulario(cliente, manana, "16:00", "17:00");
            reservasPage.Guardar();
            reservasPage.EsperarFilaConTexto(cliente);

            reservasPage.ClickEliminarPorCliente(cliente);
            reservasPage.CancelarEliminar();

            Assert.That(reservasPage.ExisteFilaConTexto(cliente), Is.True,
                "Si se cancela la confirmación, la reserva debe seguir en la tabla.");
        }

        [Test]
        [Category("PruebaLimite")]
        public void EliminarReserva_PruebaLimite_BuscarDespuesDeEliminar_NoApareceEnResultados()
        {
            var reservasPage = IniciarSesionYNavegarAReservas();
            var cliente = "Cliente Limite Busqueda " + DateTime.Now.Ticks;
            var manana = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

            reservasPage.AbrirModalNuevaReserva();
            reservasPage.SeleccionarPrimeraCanchaDisponible();
            reservasPage.LlenarFormulario(cliente, manana, "12:00", "13:00");
            reservasPage.Guardar();
            reservasPage.EsperarFilaConTexto(cliente);

            reservasPage.ClickEliminarPorCliente(cliente);
            reservasPage.ConfirmarEliminar();
            EsperarQueDesaparezca(reservasPage, cliente);

            reservasPage.Buscar(cliente);

            Assert.That(reservasPage.ExisteFilaConTexto(cliente), Is.False,
                "El cliente eliminado no debe aparecer al buscarlo después.");
        }

        private static bool EsperarQueDesaparezca(ReservasPage reservasPage, string texto)
        {
            var limite = DateTime.Now.AddSeconds(10);
            while (DateTime.Now < limite)
            {
                if (!reservasPage.ExisteFilaConTexto(texto)) return true;
                Thread.Sleep(300);
            }
            return !reservasPage.ExisteFilaConTexto(texto);
        }
    }
}