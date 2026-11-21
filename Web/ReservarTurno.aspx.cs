using Datos;
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

                //No puede acceder a esta página si no hay un cliente en sesión.
                if (Session["Cliente"] == null)
                {
                    Response.Redirect("~/Ingresar.aspx");
                    return;
                }

                //Cargar los barberos y servicios en los dropdownlists.
                CargarBarberos();
                CargarServicios();
                if (Session["IdServicio"] != null)
                {   // Si hay un servicio seleccionado en la página de servicios, lo seleccionamos en el dropdownlist.
                    int idServicio = (int)Session["IdServicio"];
                    // Seleccionamos el servicio en el dropdownlist.
                    ListItem item = ddlServicio.Items.FindByValue(
                        idServicio.ToString()
                    );

                    if (item != null)
                    {
                        ddlServicio.SelectedValue = idServicio.ToString();

                        // Bloqueamos la selección.
                        ddlServicio.Enabled = false;
                    }
                    else
                    {   // Si no encontramos el servicio, lo quitamos de la sesión.
                        // Puede pasar si el servicio fue eliminado de la base de datos.
                        Session.Remove("IdServicio");
                    }

                    // Habilitar el botón de confirmar turno solo si hay un cliente en sesión.
                    btnConfirmarTurno.Enabled = Session["Cliente"] != null;
                }
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


        }
    }
}