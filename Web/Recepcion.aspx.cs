using System;
using Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.SqlClient;

namespace Aplicación_de_turnos_Barberia
{
    public partial class Recepcion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!VerificarAcceso())
            {
                return;
            }


            if (!IsPostBack)
            {
                CargarClientes();

            }


        }

        private void CargarClientes(string texto = "")
        {
            ClienteDatos datos = new ClienteDatos();
            dgvClientes.DataSource = datos.Buscar(texto);
            dgvClientes.DataBind();
        }

        // Busca clientes con el texto ingresado y actualiza la grilla.
        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if (!VerificarAcceso())
            {
                return;
            }

            lblMensajeRecepcion.Text = string.Empty;
            pnlEdision.Visible = false;
            dgvClientes.SelectedIndex = -1;

            try
            {
                CargarClientes(txtBuscarCliente.Text);
            }
            catch (SqlException)
            {
                lblMensajeRecepcion.Text = "No pudimos buscar clientes. Intentá de nuevo.";
            }
        }

        // Lee el Id de la ficha seleccionada al tocar Editar.
        protected void dgvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!VerificarAcceso())
            {
                return;
            }

            int clienteId = (int)dgvClientes.SelectedDataKey.Value;


            lblMensajeRecepcion.Text = string.Empty;
            pnlEdision.Visible = false;

            try
            {
                // Busca en SQL Server la ficha de la fila seleccionada.
                Cliente cliente = new ClienteDatos().BuscarPorId(clienteId);

                if (cliente == null)
                {
                    lblMensajeRecepcion.Text = "No se encontró la ficha del cliente.";
                    return;
                }

                MostrarEdicion(cliente);
            }
            catch (SqlException)
            {
                lblMensajeRecepcion.Text = "No pudimos cargar la ficha. Intentá de nuevo.";
            }

        }




        // Llena los campos con los datos del cliente y muestra el panel.
        private void MostrarEdicion(Cliente cliente)
        {
            txtEditarNombre.Text = cliente.Nombre;
            txtEditarApellido.Text = cliente.Apellido;
            txtEditarTelefono.Text = cliente.Telefono;
            txtEditarEmail.Text = cliente.Email;
            btnCambiarEstado.Visible = EsAdministrador();
            btnCambiarEstado.Text = cliente.Activo ? "Desactivar cliente" : "Reactivar cliente";
            ViewState["ClienteActivo"] = cliente.Activo;
            pnlEdision.Visible = true;
        }

        // Guarda los cambios de la ficha seleccionada.
        protected void btnGuardarEdicion_Click(object sender, EventArgs e)
        {
            if (!VerificarAcceso())
            {
                return;
            }

            if (!pnlEdision.Visible || dgvClientes.SelectedDataKey == null)
            {
                lblMensajeRecepcion.Text = "Primero seleccioná un cliente para editar.";
                return;
            }

            int clienteId = (int)dgvClientes.SelectedDataKey.Value;
            Cliente cliente = LeerDatosEdicion(clienteId);

            if (string.IsNullOrWhiteSpace(cliente.Nombre)
                || string.IsNullOrWhiteSpace(cliente.Apellido)
                || string.IsNullOrWhiteSpace(cliente.Telefono)
                || string.IsNullOrWhiteSpace(cliente.Email))
            {
                lblMensajeRecepcion.Text = "Completá todos los campos.";
                return;
            }

            try
            {
                if (!new ClienteDatos().Actualizar(cliente))
                {
                    lblMensajeRecepcion.Text = "No se encontró la ficha para actualizar.";
                    return;
                }
            }
            catch (SqlException)
            {
                lblMensajeRecepcion.Text = "No pudimos guardar los cambios. Intentá de nuevo.";
                return;
            }

            CerrarEdicion();
            CargarClientes(txtBuscarCliente.Text);
            lblMensajeRecepcion.Text = "Cambios guardados.";
        }

        // Cierra la edición sin guardar.
        protected void btnCancelarEdicion_Click(object sender, EventArgs e)
        {
            if (VerificarAcceso())
            {
                CerrarEdicion();
            }
        }

        // Solo Administración puede cambiar el estado de la ficha.
        protected void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            if (!EsAdministrador())
            {
                throw new HttpException(403, "Solo Administración puede cambiar el estado.");
            }

            if (!pnlEdision.Visible || dgvClientes.SelectedDataKey == null
                || !(ViewState["ClienteActivo"] is bool activo))
            {
                lblMensajeRecepcion.Text = "Primero seleccioná un cliente.";
                return;
            }

            int id = (int)dgvClientes.SelectedDataKey.Value;
            try
            {
                if (!new ClienteDatos().CambiarEstado(id, !activo))
                {
                    lblMensajeRecepcion.Text = "No se encontró la ficha del cliente.";
                    return;
                }
            }
            catch (SqlException)
            {
                lblMensajeRecepcion.Text = "No pudimos cambiar el estado del cliente.";
                return;
            }

            CerrarEdicion();
            CargarClientes(txtBuscarCliente.Text);
            lblMensajeRecepcion.Text = activo ? "Cliente desactivado." : "Cliente reactivado.";
        }

        private void CerrarEdicion()
        {
            pnlEdision.Visible = false;
            dgvClientes.SelectedIndex = -1;
            ViewState.Remove("ClienteActivo");
            lblMensajeRecepcion.Text = string.Empty;
        }

        private bool EsAdministrador()
        {
            return Session["PerfilUsuario"] is PerfilUsuario perfil
                && perfil == PerfilUsuario.Administrador;
        }

        // Arma la ficha con lo que la recepcionista escribió en el panel.
        private Cliente LeerDatosEdicion(int clienteId)
        {
            return new Cliente
            {
                Id = clienteId,
                Nombre = txtEditarNombre.Text.Trim(),
                Apellido = txtEditarApellido.Text.Trim(),
                Telefono = txtEditarTelefono.Text.Trim(),
                Email = txtEditarEmail.Text.Trim()
            };
        }



        // revisa la sesion antes de la grilla
        private bool VerificarAcceso()
        {
            if (!(Session["PerfilUsuario"] is PerfilUsuario perfil))
            {
                Response.Redirect("~/Ingresar.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return false;
            }
            if (!PuedeAccederRecepcion(perfil))
            {
                throw new System.Web.HttpException(
                    403, "No tiene permisos para para entrar a Recepción.");

            }
            return true;
        }


        // aca vamos a ver si recepcionista tiene permisos

        private bool PuedeAccederRecepcion(PerfilUsuario perfil)
        {
            return perfil == PerfilUsuario.Recepcionista
                        
            || perfil == PerfilUsuario.Administrador;
        }


    }

}
