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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ServicioDatos datos = new ServicioDatos();
                dgvServicios.DataSource = datos.Listar();
                dgvServicios.DataBind();
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