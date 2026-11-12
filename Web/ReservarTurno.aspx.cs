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
                CargarBarberos();
                CargarServicios();
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

            var lista = datos.Listar();

            ddlServicio.DataSource = lista;
            ddlServicio.DataTextField = "Nombre";
            ddlServicio.DataValueField = "Id";
            ddlServicio.DataBind();

            ddlServicio.Items.Insert(0,
                new ListItem("Seleccioná un servicio", ""));
        }
    }
}