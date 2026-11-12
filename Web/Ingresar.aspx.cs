using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Dominio;
using Aplicación_de_turnos_Barberia.Seguridad;


namespace Aplicación_de_turnos_Barberia
{
    public partial class Ingresar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        
        { string email = txtEmail.Text.Trim();
            UsuarioDatos datos = new UsuarioDatos();
            Usuario usuario = datos.BuscarPorEmail(email);

            // Si no existe la cuenta o la contraseña no coincide, frenamos

            if (usuario == null || !Contrasenas.Verificar(txtPassword.Text,usuario.PasswordHash))
            {
                lblErrorIngreso.Text = "El correo o la contraseña no son correctos";
                return;

            }

            Session["PerfilUsuario"] = usuario.Perfil;
            // Recepción entra directo a su pantalla de trabajo.
            string destino = "~/Default.aspx";

            if (usuario.Perfil == PerfilUsuario.Recepcionista)
            {
                destino = "~/Recepcion.aspx";
            }

            Response.Redirect(destino, false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }
}