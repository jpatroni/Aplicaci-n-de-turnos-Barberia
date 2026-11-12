using System;
using System.Web;
using System.Web.UI;
using Dominio;

namespace Aplicación_de_turnos_Barberia.Seguridad
{
    public class PaginaAdministracion : Page
    {
        // Verifica el permiso antes de consultar datos o ejecutar botones.
        protected override void OnPreInit(EventArgs e)
        {
            if (!(Session["PerfilUsuario"] is PerfilUsuario perfil)
                || perfil != PerfilUsuario.Administrador)
            {
                throw new HttpException(403, "Esta pantalla es solo para Administración.");
            }

            base.OnPreInit(e);
        }
    }
}
