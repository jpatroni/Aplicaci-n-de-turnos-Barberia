using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using System.Globalization;
using Datos;
using System.Data.SqlClient;


namespace Aplicación_de_turnos_Barberia
{
    public partial class AgendaRecepcion : System.Web.UI.Page
    {
        // aca vamsoa   ver si el usuario tien permiso para entrar en la agenda
        private bool TieneAcceso()
        {
            if(!(Session["PerfilUsuario"] is PerfilUsuario))
            {
                return false;
            }

            // aca vamso a recuperar el perfil del login

            PerfilUsuario perfil = (PerfilUsuario)Session["PerfilUsuario"];

            // y aca la restriccion para q entre solo recepcion y administracion

            return perfil == PerfilUsuario.Recepcionista
               || perfil == PerfilUsuario.Administrador;

        }


        protected override void OnPreInit(EventArgs e)
        {
           if(!TieneAcceso())
            {
                // aca ni el usuarin  no inicio sesion lo mandamos al login 
                if (!(Session["PerfilUsuario"]is PerfilUsuario))
                {
                    Response.Redirect("~/Ingresar.aspx");
                    return;
                }

                // ahora si inicia sesion pero no es el perfil con permisos 

                throw new HttpException(403, "No tenés permiso para acceder a la agenda.");


            }
            base.OnPreInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                calAgenda.SelectedDate = DateTime.Today;
            }


        }


        protected void btnVerAgenda_Click(object sender,EventArgs e)
        {
            DateTime fecha = calAgenda.SelectedDate;

            lblFechaSeleccionada.Text = "Turnos del " + fecha.ToString("dd/MM/yyyy");

            CargarTurnos();


        }


        protected void btnBuscarTurno_Click(object sender, EventArgs e)
        {
            CargarTurnos(txtBuscarTurno.Text);
        }

        protected void dgvTurnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Recupera el Id de la fila que seleccionaste.
            int turnoId = (int)dgvTurnos.SelectedDataKey.Value;

            // Lo muestra para comprobar que elegimos el turno correcto.
            lblTurnoSeleccionado.Text = "Seleccionaste el turno número " + turnoId;

            // usamos el dia de la agenda como fecha para reprogramar

            txtNuevaFecha.Text = calAgenda.SelectedDate.ToString("yyyy-MM-dd");

            CargarHorariosDeEjemplo();

            // Muestra el panel para elegir la nueva fecha y horario.
            pnlReprogramacion.Visible = true;

        }

        public string ValidarReprogramacion(out DateTime nuevaFechaHora)
        {
            nuevaFechaHora = DateTime.MinValue;
            if(dgvTurnos.SelectedDataKey == null)
            {
                return "Seleccioná un turno";
            }

            if(!TieneHorarioSeleccionado())
            {
                return "Seleccioná un horario";
            }

            DateTime fecha;

            if(!EsFechaValida(out fecha))
            {
                return "Ingresá una fecha valida";

            }
            if(!EsDiaDeAtencion(fecha))
            {
                return "La barbería atiende de martes a sábado.";
            }
            nuevaFechaHora = ObtenerNuevaFechaHora(fecha);
            if(!EsFechaHoraFutura(nuevaFechaHora))
            {
                return "Elegí una fecha y un horario futuros.";
            }
            return string.Empty;

        }

        protected void btnCancelarEdicion_Click(object sender, EventArgs e)
        {
            pnlReprogramacion.Visible = false;
            dgvTurnos.SelectedIndex = -1;

            lblTurnoSeleccionado.Text = string.Empty;
            lblMensajeReprogramacion.Text = string.Empty;
        }



        private void GuardarReprogramacion(DateTime nuevaFechaHora)
        {
            int turnoId = (int)dgvTurnos.SelectedDataKey.Value;

            try
            {
                TurnoDatos datos = new TurnoDatos();
                bool guardado = datos.ReprogramarTurno(
                    turnoId, nuevaFechaHora);

                lblMensajeReprogramacion.Text = guardado
                    ///si guardado es true, usa el primer mensaje; si es false, usa el segundo.

                    ? "El turno se reprogramó correctamente"
                    : "No se pudo reprogramar. El turno puede no estar disponible o el horario no ser válido.";
                // Actualiza la agenda únicamente si se guardó el cambio.
                if (guardado)
                {
                    MostrarAgenda(txtBuscarTurno.Text);
                }

            }

            catch (SqlException)
            {     
                lblMensajeReprogramacion.Text = "Ocurrió un error al guardar. Volvé a consultar la agenda para verificar el turno.";
            }

        }


        protected void btnRevisarReprogramacion_Click(object sender, EventArgs e)
        {
            DateTime nuevaFechaHora;

            // Revisa los datos y obtiene el primer error, si hay alguno.
            string error = ValidarReprogramacion(out nuevaFechaHora);

            if (!string.IsNullOrEmpty(error))
            {
                lblMensajeReprogramacion.Text = error;
                return;
            }

            // Si no hubo errores, muestra la fecha y hora elegidas.
            GuardarReprogramacion(nuevaFechaHora);
        }


        // carga horarios de ejemplo 
        // Carga horarios de ejemplo cada media hora.
        private void CargarHorariosDeEjemplo()
        {
            ddlNuevoHorario.Items.Clear();
            ddlNuevoHorario.Items.Add(
                new ListItem("Seleccioná un horario", ""));

            DateTime hora = DateTime.Today.AddHours(9);
            DateTime cierre = DateTime.Today.AddHours(21);

            while (hora < cierre)
            {
                string textoHora = hora.ToString("HH:mm");

                ddlNuevoHorario.Items.Add(
                    new ListItem(textoHora, textoHora));

                hora = hora.AddMinutes(30);
            }
        }

        // Intenta cargar la agenda y muestra un aviso si falla SQL.
        private void MostrarAgenda(string texto = "")
        {
            lblMensajeAgenda.Text = string.Empty;

            try
            {
                CargarTurnos(texto);
            }
            catch (SqlException)
            {
                // Limpia los resultados anteriores si la consulta falla.
                dgvTurnos.DataSource = null;
                dgvTurnos.DataBind();

                lblMensajeAgenda.Text =
                    "No pudimos cargar los turnos. Intentá nuevamente.";
            }
        }


        //CONSULTA LOS TURNOS Y LOS MUETSRA EN LA GRILLA 

        private void CargarTurnos(string texto = "")
        {
            DateTime fecha = calAgenda.SelectedDate;
            TurnoDatos datos = new TurnoDatos();
            List<Turno> turnos = datos.ListarTurnosPorFecha(fecha);

            dgvTurnos.DataSource = turnos
                .Where(turno =>
                (turno.Cliente.Nombre + "" + turno.Cliente.Apellido)
                .IndexOf(texto.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(turno => new

            {
                Id = turno.Id,
                Hora = turno.FechaHora.ToString("HH:mm"),
                Cliente = turno.Cliente.Nombre + " " + turno.Cliente.Apellido,
                Barbero = turno.Barbero.Nombre + " " + turno.Barbero.Apellido,
                Servicio  = turno.Servicio.Nombre,
                Estado = turno.Estado.ToString()


            }).ToList();
            dgvTurnos.DataBind();
        }


        // Muestra un turno ficticio para probar la grilla.
        // Muestra turnos ficticios y permite filtrar por cliente.


        private void MostrarTurnosDeEjemplo(string texto = "")
        {
            var turnos = new[]
            {
        new
        {
            Id = 1,
            Hora = "09:00",
            Cliente = "Ana Pérez",
            Barbero = "Martín",
            Servicio = "Corte de pelo",
            Estado = "Confirmado"
        },
        new
        {
            Id = 2,
            Hora = "09:30",
            Cliente = "Lucas Gómez",
            Barbero = "Martín",
            Servicio = "Arreglo de barba",
            Estado = "Confirmado"
        },
        new
        {
            Id = 3,
            Hora = "10:00",
            Cliente = "Juan Pérez",
            Barbero = "Martín",
            Servicio = "Corte de pelo",
            Estado = "Confirmado"
        }
    };

            string filtro = texto.Trim();

            dgvTurnos.DataSource = turnos.Where(turno =>
                turno.Cliente.IndexOf(
                    filtro, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            dgvTurnos.DataBind();
        }


 private DateTime ObtenerNuevaFechaHora(DateTime fecha)
        {
            DateTime horario = DateTime.ParseExact(ddlNuevoHorario.SelectedValue, "HH:mm", CultureInfo.InvariantCulture);
            return fecha.Date.Add(horario.TimeOfDay);
        }



        // comprobacion de fecha hora 

        private bool EsFechaHoraFutura(DateTime fechaHora)
        {
            return fechaHora > DateTime.Now;
        }



        // cpomprobacion de dia laborable 

        private bool EsDiaDeAtencion(DateTime fecha)

        {
            return fecha.DayOfWeek != DayOfWeek.Sunday
            && fecha.DayOfWeek != DayOfWeek.Monday;
        }



        // Comprueba si se eligió un horario.
        private bool TieneHorarioSeleccionado()
        {
            return !string.IsNullOrEmpty(ddlNuevoHorario.SelectedValue);
        }

        // comproivacion de la fecha 

        private bool EsFechaValida(out DateTime fecha)
        {
           

            return DateTime.TryParseExact(
                txtNuevaFecha.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fecha);
        }
    }
}
