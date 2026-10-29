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
            ServicioDatos datos = new ServicioDatos();
    dgvServicios.DataSource = datos.Listar();
    dgvServicios.DataBind();
        }
    }
}