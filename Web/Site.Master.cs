using System;
using Dominio;
using System.Web.UI;

namespace Aplicación_de_turnos_Barberia
{
    public partial class Site : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // El login guarda el perfil en la sesión cuando el ingreso es correcto.
            bool sesionIniciada = Session["PerfilUsuario"] is PerfilUsuario;

            menuIngresar.Visible = !sesionIniciada;
            menuRegistrarse.Visible = !sesionIniciada;
            menuCerrarSesion.Visible = sesionIniciada;
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Borra los datos de la sesión y la termina.
            Session.Clear();
            Session.Abandon();

            // Vuelve a Inicio después de cerrar la sesión.
            Response.Redirect("~/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}