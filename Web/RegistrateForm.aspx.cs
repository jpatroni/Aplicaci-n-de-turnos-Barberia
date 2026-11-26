using Aplicación_de_turnos_Barberia.Seguridad;
using Datos;
using Dominio;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Aplicación_de_turnos_Barberia
{
    public partial class RegistrateForm : System.Web.UI.Page
    {
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            lblErrorRegistro.Text = string.Empty;
            Page.Validate("Registro");
            if (!Page.IsValid)
                return;

            Cliente cliente = LeerCliente();
            string error = ValidarFormulario(cliente);
            if (!string.IsNullOrEmpty(error))
            {
                MostrarError(error);
                return;
            }

            IntentarRegistrar(cliente);
        }

        private Cliente LeerCliente()
        {
            return new Cliente
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Email = txtEmail.Text.Trim()
            };
        }

        private string ValidarFormulario(Cliente cliente)
        {
            string error = ReglasRegistro.ValidarCliente(cliente);
            return !string.IsNullOrEmpty(error)
                ? error
                : ReglasRegistro.ValidarContrasena(txtPassword.Text);
        }

        // Maneja errores técnicos y conserva el formulario para corregirlo.
        private void IntentarRegistrar(Cliente cliente)
        {
            try
            {
                GuardarRegistro(cliente);
            }
            catch (SqlException ex)
            {
                Trace.Warn("Registro", "Falló el registro de la cuenta.", ex);
                MostrarError(ex.Number == 2601 || ex.Number == 2627
                    ? "Ese correo ya está registrado. Iniciá sesión o consultá a Recepción."
                    : "No pudimos completar el registro. Intentá nuevamente.");
            }
            catch (ConfigurationErrorsException ex)
            {
                Trace.Warn("Registro", "Falta configurar BarberiaDB.", ex);
                MostrarError("No se pudo conectar con la base. Revisá la configuración BarberiaDB.");
            }
            catch (ArgumentException ex)
            {
                Trace.Warn("Registro", "Datos de registro no válidos.", ex);
                MostrarError("Revisá los datos y la contraseña antes de volver a registrar.");
            }
        }

        private void GuardarRegistro(Cliente cliente)
        {
            //// Prepara el hash antes de guardar la cuenta.
            string passwordHash = Contrasenas.GenerarHash(txtPassword.Text);

            // Registrar ficha y cuenta de usuario.
            new RegistroDatos().RegistrarCliente(cliente, passwordHash);

            // Limpiar los campos.
            LimpiarCampos();
            Response.Redirect("~/RegistroExitoso.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }

        private void MostrarError(string mensaje)
        {
            lblErrorRegistro.Text = Server.HtmlEncode(mensaje);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
