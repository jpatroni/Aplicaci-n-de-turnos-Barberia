using Aplicación_de_turnos_Barberia.Seguridad;
using Datos;
using Dominio;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Aplicación_de_turnos_Barberia
{
    public partial class Ingresar : System.Web.UI.Page
    {
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            lblErrorIngreso.Text = string.Empty;
            if (!ReglasRegistro.EsEmailValido(txtEmail.Text.Trim())
                || string.IsNullOrEmpty(txtPassword.Text))
            {
                lblErrorIngreso.Text = "Ingresá un correo válido y tu contraseña.";
                return;
            }

            IntentarIngresar();
        }

        private void IntentarIngresar()
        {
            try
            {
                Autenticar();
            }
            catch (SqlException ex)
            {
                Trace.Warn("Ingreso", "Falló la consulta de la cuenta.", ex);
                lblErrorIngreso.Text = "No pudimos verificar la cuenta. Intentá nuevamente.";
            }
            catch (ConfigurationErrorsException ex)
            {
                Trace.Warn("Ingreso", "Falta configurar BarberiaDB.", ex);
                lblErrorIngreso.Text = "No se pudo conectar con la base. Revisá la configuración BarberiaDB.";
            }
        }

        private void Autenticar()
        {
            Usuario usuario = new UsuarioDatos().BuscarPorEmail(txtEmail.Text.Trim());

            // Si no existe la cuenta o la contraseña no coincide, frenamos.
            if (usuario == null || !Contrasenas.Verificar(txtPassword.Text, usuario.PasswordHash)
                || (usuario.Perfil == PerfilUsuario.Cliente && usuario.Cliente == null))
            {
                lblErrorIngreso.Text = "El correo o la contraseña no son correctos.";
                return;
            }

            CrearSesion(usuario);
            Response.Redirect(ObtenerDestino(usuario.Perfil), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        // Evita conservar un cliente de un ingreso anterior con otro perfil.
        private void CrearSesion(Usuario usuario)
        {
            Session.Remove("Cliente");
            Session.Remove("ClienteId");
            Session["PerfilUsuario"] = usuario.Perfil;

            if (usuario.Perfil == PerfilUsuario.Cliente)
            {
                Session["Cliente"] = usuario.Cliente;
                Session["ClienteId"] = usuario.Cliente.Id;
            }
        }

        private string ObtenerDestino(PerfilUsuario perfil)
        {
            // Recepción entra directo a su pantalla de trabajo.
            if (perfil == PerfilUsuario.Recepcionista)
                return "~/Recepcion.aspx";

            if (perfil == PerfilUsuario.Administrador)
                return "~/Administracion.aspx";

            return "~/Servicios.aspx";
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
