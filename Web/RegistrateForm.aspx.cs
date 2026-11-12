using Datos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aplicación_de_turnos_Barberia.Seguridad;

namespace Aplicación_de_turnos_Barberia
{
    public partial class RegistrateForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegistrar_Click(object sender, EventArgs e)

        {
            try
            {
                Cliente cliente = new Cliente();

                cliente.Nombre = txtNombre.Text.Trim();
                cliente.Apellido = txtApellido.Text.Trim();
                cliente.Telefono = txtTelefono.Text.Trim();
                cliente.Email = txtEmail.Text.Trim();

                //// Prepara el hash antes de guardar la cuenta.
                string passwordHash = Contrasenas.GenerarHash(txtPassword.Text);

                // Registrar ficha y cuenta de usuario.
                RegistroDatos registro = new RegistroDatos();

                int clienteId = registro.RegistrarCliente(cliente, passwordHash);

                // Limpiar los campos.
                txtNombre.Text = string.Empty;
                txtApellido.Text = string.Empty;
                txtTelefono.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtPassword.Text = string.Empty;

                Response.Redirect("~/RegistroExitoso.aspx");
            }
            catch (Exception ex)
            {
                Session["Error"] = ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }

}