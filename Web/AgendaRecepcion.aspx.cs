using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;

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

            MostrarTurnosDeEjemplo();


        }


        protected void btnBuscarTurno_Click(object sender, EventArgs e)
        {
            MostrarTurnosDeEjemplo(txtBuscarTurno.Text);
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

        protected void btnRevisarReprogramacion_Click(object sender, EventArgs e)
        {
            lblMensajeReprogramacion.Text =
                " Nueva Fecha:" + txtNuevaFecha.Text
                + "-Horario:" + ddlNuevoHorario.SelectedValue;
            

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


    }
}
