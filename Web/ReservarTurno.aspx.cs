using Datos;
using Dominio;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace Aplicación_de_turnos_Barberia
{
    public partial class ReservarTurno : System.Web.UI.Page
    {
        // Verifica el cliente en cada solicitud, también al enviar un formulario.
        protected override void OnPreInit(EventArgs e)
        {
            if (!(Session["Cliente"] is Cliente cliente) || cliente.Id <= 0
                || !(Session["PerfilUsuario"] is PerfilUsuario perfil)
                || perfil != PerfilUsuario.Cliente)
            {
                Response.Redirect("~/Ingresar.aspx");
                return;
            }

            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtFecha.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
            if (!IsPostBack)
            {
                ReiniciarHorarios();
                EjecutarAccion(PrepararFormulario);
                MostrarConfirmacionPendiente();
            }
        }

        // Los cambios de servicio, barbero o fecha invalidan el horario anterior.
        protected void SeleccionReserva_Changed(object sender, EventArgs e)
        {
            EjecutarAccion(CargarDisponibilidad);
        }

        protected void btnConsultarHorarios_Click(object sender, EventArgs e)
        {
            EjecutarAccion(CargarDisponibilidad);
        }

        protected void btnConfirmarTurno_Click(object sender, EventArgs e)
        {
            Page.Validate("Reserva");
            if (!Page.IsValid)
                return;

            Turno solicitud;
            string error = LeerSolicitud(out solicitud);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarMensaje(error, true);
                return;
            }

            EjecutarAccion(() => ConfirmarReserva(solicitud));
        }

        // Cargar los barberos y servicios en los dropdownlists.
        private void PrepararFormulario()
        {
            CargarBarberos();
            CargarServicios();
            AplicarPreseleccion();
        }

        private void CargarBarberos()
        {
            var lista = new BarberoDatos().Listar()
                .Where(bar => bar.Activo)
                .Select(bar => new
                {
                    bar.Id,
                    NombreCompleto = bar.Nombre + " " + bar.Apellido
                }).ToList();

            ddlBarbero.DataSource = lista;
            ddlBarbero.DataTextField = "NombreCompleto";
            ddlBarbero.DataValueField = "Id";
            ddlBarbero.DataBind();
            ddlBarbero.Items.Insert(0, new ListItem("Seleccioná un barbero", ""));
        }

        private void CargarServicios()
        {
            // Obtenemos la lista de servicios activos.
            ddlServicio.DataSource = new ServicioDatos().ListarActivos();
            ddlServicio.DataTextField = "Nombre";
            ddlServicio.DataValueField = "Id";
            ddlServicio.DataBind();
            ddlServicio.Items.Insert(0, new ListItem("Seleccioná un servicio", ""));
        }

        // Si hay un servicio seleccionado en la página de servicios, lo seleccionamos.
        private void AplicarPreseleccion()
        {
            string id = Convert.ToString(Session["IdServicio"], CultureInfo.InvariantCulture);
            // Evitar reutilizar la selección en futuros ingresos, como en el último cambio de Cris.
            Session.Remove("IdServicio");
            ListItem item = ddlServicio.Items.FindByValue(id);
            if (string.IsNullOrEmpty(id) || item == null)
            {
                // Puede pasar si el servicio fue eliminado o dado de baja.
                Session.Remove("IdServicio");
                return;
            }

            ddlServicio.SelectedValue = id;
            // Bloquear la selección del servicio que vino desde el catálogo.
            ddlServicio.Enabled = false;
        }

        // Solo prepara el estado visual; no consulta ni guarda en SQL.
        private void ReiniciarHorarios()
        {
            ddlHorario.Items.Clear();
            ddlHorario.Items.Add(new ListItem("Seleccioná un horario disponible", ""));
            ddlHorario.Enabled = false;
            btnConfirmarTurno.Enabled = false;
        }

        // Lee la selección sin conversiones que lancen errores por campos vacíos.
        private string LeerSeleccion(out int barberoId, out int servicioId, out DateTime fecha)
        {
            barberoId = 0;
            servicioId = 0;
            fecha = DateTime.MinValue;

            if (!int.TryParse(ddlServicio.SelectedValue, out servicioId) || servicioId <= 0)
                return "Seleccioná un servicio.";

            if (!int.TryParse(ddlBarbero.SelectedValue, out barberoId) || barberoId <= 0)
                return "Seleccioná un barbero.";

            if (!DateTime.TryParseExact(txtFecha.Text, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                return "Ingresá una fecha válida.";

            return ReglasTurno.ValidarFecha(fecha);
        }

        // Comprueba que las opciones sigan activas después de haber cargado la página.
        private Servicio ObtenerServicioDisponible(int servicioId, int barberoId)
        {
            Servicio servicio = new ServicioDatos().BuscarPorId(servicioId);
            Barbero barbero = new BarberoDatos().BuscarPorId(barberoId);

            if (servicio == null || !servicio.Activo || barbero == null || !barbero.Activo)
                return null;

            return servicio;
        }

        // Consulta disponibilidad real; no genera una lista fija de horarios.
        private void CargarDisponibilidad()
        {
            ReiniciarHorarios();
            int barberoId, servicioId;
            DateTime fecha;
            string error = LeerSeleccion(out barberoId, out servicioId, out fecha);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarMensaje(error);
                return;
            }

            Servicio servicio = ObtenerServicioDisponible(servicioId, barberoId);
            if (servicio == null)
            {
                MostrarMensaje("El servicio o el barbero ya no están disponibles. Volvé a elegir desde Servicios.", true);
                return;
            }

            var horarios = new TurnoDatos().ObtenerHorariosDisponibles(
                barberoId, fecha, servicio.DuracionMinutos);
            MostrarHorarios(horarios);
        }

        private void MostrarHorarios(System.Collections.Generic.List<DateTime> horarios)
        {
            foreach (DateTime horario in horarios)
            {
                string valor = horario.ToString("HH:mm", CultureInfo.InvariantCulture);
                ddlHorario.Items.Add(new ListItem(valor, valor));
            }

            bool hayHorarios = horarios.Count > 0;
            ddlHorario.Enabled = hayHorarios;
            btnConfirmarTurno.Enabled = hayHorarios;
            MostrarMensaje(hayHorarios
                ? "Elegí uno de los horarios disponibles para confirmar."
                : "No hay horarios disponibles para esa selección. Probá otro día o barbero.");
        }

        // Obtener los datos seleccionados y formar una solicitud de reserva.
        private string LeerSolicitud(out Turno solicitud)
        {
            solicitud = null;
            int barberoId, servicioId;
            DateTime fecha, fechaHora;
            string error = LeerSeleccion(out barberoId, out servicioId, out fecha);
            if (!string.IsNullOrEmpty(error))
                return error;

            string texto = fecha.ToString("yyyy-MM-dd") + " " + ddlHorario.SelectedValue;
            if (!DateTime.TryParseExact(texto, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaHora))
                return "Seleccioná un horario disponible.";

            if (fechaHora <= DateTime.Now)
                return "Ese horario ya pasó. Consultá nuevamente la disponibilidad.";

            solicitud = new Turno
            {
                Cliente = (Cliente)Session["Cliente"],
                Barbero = new Barbero { Id = barberoId },
                Servicio = new Servicio { Id = servicioId },
                FechaHora = fechaHora
            };
            return string.Empty;
        }

        // Guardar el turno: Datos vuelve a comprobar el horario dentro de su transacción.
        private void ConfirmarReserva(Turno solicitud)
        {
            bool guardado = new TurnoDatos().GuardarTurno(
                solicitud.Cliente.Id, solicitud.Barbero.Id,
                solicitud.Servicio.Id, solicitud.FechaHora);

            if (!guardado)
            {
                CargarDisponibilidad();
                MostrarMensaje("No se pudo reservar: el horario o alguna de las opciones ya no están disponibles. Revisá la selección y elegí otro horario.", true);
                return;
            }

            // Redirige a un GET para que actualizar el navegador no repita el envío.
            Session["FechaReservaConfirmada"] = solicitud.FechaHora;
            Session["IdServicio"] = solicitud.Servicio.Id;
            Response.Redirect("~/ReservarTurno.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void MostrarConfirmacionPendiente()
        {
            if (!(Session["FechaReservaConfirmada"] is DateTime fecha))
                return;

            Session.Remove("FechaReservaConfirmada");
            MostrarMensaje("¡Turno confirmado para el " + fecha.ToString("dd/MM/yyyy")
                + " a las " + fecha.ToString("HH:mm") + "!");
        }

        // Maneja errores técnicos, sin usarlos para validar campos del formulario.
        private void EjecutarAccion(Action accion)
        {
            try
            {
                accion();
            }
            catch (SqlException ex)
            {
                Trace.Warn("Reserva", "Falló una operación de turnos.", ex);
                ReiniciarHorarios();
                MostrarMensaje("No pudimos completar la operación con la base. Verificá el turno con Recepción antes de volver a confirmar.", true);
            }
            catch (ConfigurationErrorsException ex)
            {
                Trace.Warn("Reserva", "Falta configurar BarberiaDB.", ex);
                ReiniciarHorarios();
                MostrarMensaje("No se pudo conectar con la base. Revisá la configuración BarberiaDB.", true);
            }
            catch (ArgumentException ex)
            {
                Trace.Warn("Reserva", "Selección no válida.", ex);
                ReiniciarHorarios();
                MostrarMensaje("La selección no es válida. Volvé a consultar los horarios.", true);
            }
        }

        private void MostrarMensaje(string mensaje, bool esError = false)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);
            lblMensaje.CssClass = esError
                ? "reservar-mensaje reservar-error"
                : "reservar-mensaje";
        }
    }
}
