using Datos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Aplicación_de_turnos_Barberia
{
    public partial class ReservarTurno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar que haya un cliente en sesión.
                if (Session["Cliente"] == null)
                {
                    Response.Redirect("~/Ingresar.aspx");
                    return;
                }

                // Cargar barberos y servicios.
                CargarBarberos();
                CargarServicios();

                // Recuperar el servicio seleccionado desde Servicios.aspx.
                if (Session["IdServicio"] != null)
                {
                    int idServicio = (int)Session["IdServicio"];

                    ListItem item = ddlServicio.Items.FindByValue(
                        idServicio.ToString()
                    );

                    if (item != null)
                    {
                        ddlServicio.SelectedValue = idServicio.ToString();

                        // Bloquear la selección del servicio.
                        ddlServicio.Enabled = false;
                    }

                    // Evitar reutilizar la selección en futuros ingresos.
                    Session.Remove("IdServicio");
                }

                // Habilitar el botón de confirmar.
                btnConfirmarTurno.Enabled = true;
            }
        }

        private void CargarBarberos()
        {
            BarberoDatos datos = new BarberoDatos();
            var lista = datos.Listar()
                .Where(bar => bar.Activo)
                .Select(bar => new
                {
                    bar.Id,
                    NombreCompleto = bar.Nombre + "" + bar.Apellido

                })
              .ToList();
            ddlBarbero.DataSource = lista;
            ddlBarbero.DataTextField = "NombreCompleto";
            ddlBarbero.DataValueField = "Id";
            ddlBarbero.DataBind();
            ddlBarbero.Items.Insert(0,
                new ListItem("Selecciona un Barbero", ""));

        }
        private void CargarServicios()
        {
            ServicioDatos datos = new ServicioDatos();
            // Obtenemos la lista de servicios activos.
            var lista = datos.ListarActivos();

            ddlServicio.DataSource = lista;
            ddlServicio.DataTextField = "Nombre";
            ddlServicio.DataValueField = "Id";
            ddlServicio.DataBind();

            ddlServicio.Items.Insert(0,
                new ListItem("Seleccioná un servicio", ""));
        }


        protected void btnConfirmarTurno_Click(object sender, EventArgs e)
        {
            // Verificar que haya un cliente en sesión.
            if (Session["Cliente"] == null)
            {
                Response.Redirect("~/Ingresar.aspx");
                return;
            }

            Cliente cliente = (Cliente)Session["Cliente"];

            // Obtener los datos seleccionados.
            int barberoId = Convert.ToInt32(ddlBarbero.SelectedValue);
            int servicioId = Convert.ToInt32(ddlServicio.SelectedValue);

            DateTime fecha = DateTime.Parse(txtFecha.Text);
            TimeSpan horario = TimeSpan.Parse(ddlHorario.SelectedValue);

            DateTime fechaHora = fecha.Date.Add(horario);

            // Guardar el turno.
            TurnoDatos datos = new TurnoDatos();

            try
            {
                bool guardado = datos.GuardarTurno(
                    cliente.Id,
                    barberoId,
                    servicioId,
                    fechaHora
                );

                if (guardado)
                    lblMensaje.Text = "¡Turno reservado correctamente!";
                else
                    lblMensaje.Text = "No se pudo reservar el turno. Verifique la fecha, el horario y la disponibilidad del barbero.";
            }
            catch (Exception)
            {
                lblMensaje.Text = "Ocurrió un error al intentar reservar el turno.";
            }
        }
    }
}