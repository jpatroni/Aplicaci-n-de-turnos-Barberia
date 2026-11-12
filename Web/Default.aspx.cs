using System;
using System.Linq;
using Datos;

namespace Aplicación_de_turnos_Barberia
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarBarberos();
            }
        }

        private void CargarBarberos()
        {
            BarberoDatos datos = new BarberoDatos();

            var lista = datos.Listar();

            rptBarberos.DataSource = lista
                .Where(b => b.Activo)
                .ToList();

            rptBarberos.DataBind();
        }
    }
}
