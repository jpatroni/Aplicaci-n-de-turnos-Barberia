using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
namespace Aplicación_de_turnos_Barberia
{
    public partial class Servicios : System.Web.UI.Page
    {


        /// recorrido : Elegir servicio → evento del Repeater → SeleccionarServicio → Reserva

        // Elige la foto de presentación; los datos del servicio siguen viniendo de SQL.
        protected string ObtenerImagenServicio(string nombre)
        {
            if (string.Equals(nombre.Trim(), "Corte de pelo",
                StringComparison.OrdinalIgnoreCase))
            {
                return "~/Imagenes/corte-pelo.jpg";
            }

            return string.Empty;
        }

        // Consulta los servicios activos y carga los controles de la página.
        private void CargarServicios()
        {
            ServicioDatos datos = new ServicioDatos();
            var servicios = datos.ListarActivos();

            rptServicios.DataSource = servicios;
            rptServicios.DataBind();

            // Conservamos temporalmente la tabla mientras armamos las tarjetas.
            dgvServicios.DataSource = servicios;
            dgvServicios.DataBind();
        }

        // Guarda el servicio elegido y abre la pantalla de reserva.
        private void SeleccionarServicio(object argumento)
        {
            int idServicio;

            if (!int.TryParse(Convert.ToString(argumento), out idServicio))
            {
                return;
            }

            Session["IdServicio"] = idServicio;

            Response.Redirect("~/ReservarTurno.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Recibe la acción del botón de una tarjeta.
        protected void rptServicios_ItemCommand(
            object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarServicio")
            {
                SeleccionarServicio(e.CommandArgument);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarServicios();
            }
        }

        // Evento que se dispara cuando se hace clic en el botón "Seleccionar" de un servicio en el GridView.
        protected void dgvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarServicio")
            {
                int idServicio;

                if (!int.TryParse(e.CommandArgument.ToString(), out idServicio))
                {
                    return;
                }

                // Guardamos el ID del servicio seleccionado.
                Session["IdServicio"] = idServicio;

                // Vamos a la página de reserva.
                Response.Redirect("~/ReservarTurno.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

    }
}
