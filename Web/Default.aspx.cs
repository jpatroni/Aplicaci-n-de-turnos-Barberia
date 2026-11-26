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

        protected string ObtenerFotoLocal(object fotoUrl)
        {
            string ruta = Convert.ToString(fotoUrl);

            string predeterminada =
                "~/Imagenes/Barberos/sin-foto.png";

            
            if (string.IsNullOrWhiteSpace(ruta))
                return ResolveUrl(predeterminada);

            
            if (!ruta.StartsWith("~/Imagenes/Barberos/",
                StringComparison.OrdinalIgnoreCase))
                return ResolveUrl(predeterminada);

            try
            {
                
                if (System.IO.File.Exists(Server.MapPath(ruta)))
                    return ResolveUrl(ruta);
            }
            catch
            {
                
            }

            return ResolveUrl(predeterminada);
        }

    }
}
