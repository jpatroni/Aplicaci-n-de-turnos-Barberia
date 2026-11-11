
using System;
using System.Collections.Generic;
using System.Web.UI;
using Datos;
using Dominio;

namespace Aplicación_de_turnos_Barberia
{
    public partial class GestionBarberos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MostrarListado();
            }
        }

        // Muestra los barberos registrados de DB
        private void CargarBarberos()
        {
            BarberoDatos datos = new BarberoDatos();

            dgvBarberos.DataSource = datos.Listar();
            dgvBarberos.DataBind();
        }

        
        private void MostrarListado()
        {
            pnlListado.Visible = true;
            pnlFormulario.Visible = false;

            LimpiarCampos();
            CargarBarberos();
        }

        // Abre el form para registrar un barbero
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();

            lblTituloFormulario.Text = "Alta de Barbero";

            btnAlta.Visible = true;
            btnModificacion.Visible = false;
            btnBaja.Visible = false;

            pnlListado.Visible = false;
            pnlFormulario.Visible = true;
        }

        // Selecciona un barbero para modificarlo
        protected void dgvBarberos_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            int id = Convert.ToInt32(
                dgvBarberos.SelectedDataKey.Value);

            BarberoDatos datos = new BarberoDatos();
            Barbero barbero = datos.BuscarPorId(id);

            if (barbero == null)
            {
                MostrarMensaje("No se encontró el barbero.");
                return;
            }

            // Carga los datos en el form
            hfIdBarbero.Value = barbero.Id.ToString();
            txtNombre.Text = barbero.Nombre;
            txtApellido.Text = barbero.Apellido;
            txtEspecialidad.Text = barbero.Especialidad;
            txtDescripcion.Text = barbero.Descripcion;
            txtFotoUrl.Text = barbero.FotoUrl;

            lblTituloFormulario.Text = "Modificación de Barbero";

            btnAlta.Visible = false;
            btnModificacion.Visible = true;
            btnBaja.Visible = barbero.Activo;

            pnlListado.Visible = false;
            pnlFormulario.Visible = true;
        }

        // Nuevo Barbero
        protected void btnAlta_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            Barbero barbero = ObtenerDatosFormulario();
            BarberoDatos datos = new BarberoDatos();

            if (ExisteDuplicado(barbero.Nombre,
                                barbero.Apellido, 0))
            {
                MostrarMensaje(
                    "Ya existe un barbero con ese nombre y apellido.");
                return;
            }

            if (datos.Alta(barbero))
            {
                MostrarListado();
                MostrarMensaje("Barbero registrado correctamente.");
            }
            else
            {
                MostrarMensaje("No se pudo registrar el barbero.");
            }
        }

        // Modificara
        protected void btnModificacion_Click(object sender, EventArgs e)
        {
            int id;

            if (!int.TryParse(hfIdBarbero.Value, out id))
            {
                MostrarMensaje("Primero seleccioná un barbero.");
                return;
            }

            if (!ValidarCampos())
                return;

            Barbero barbero = ObtenerDatosFormulario();
            barbero.Id = id;

            BarberoDatos datos = new BarberoDatos();

            // Validacion duplicado otro barbero
            if (ExisteDuplicado(barbero.Nombre,
                                barbero.Apellido, id))
            {
                MostrarMensaje(
                    "Ya existe otro barbero con ese nombre y apellido.");
                return;
            }

            if (datos.Modificacion(barbero))
            {
                MostrarListado();
                MostrarMensaje("Barbero modificado correctamente.");
            }
            else
            {
                MostrarMensaje("No se encontró el barbero.");
            }
        }

        // BAJA: desactiva un barbero sin eliminarlo de DB
        protected void btnBaja_Click(object sender, EventArgs e)
        {
            int id;

            if (!int.TryParse(hfIdBarbero.Value, out id))
            {
                MostrarMensaje("Primero seleccioná un barbero.");
                return;
            }

            BarberoDatos datos = new BarberoDatos();

            if (datos.Baja(id))
            {
                MostrarListado();
                MostrarMensaje("Barbero dado de baja correctamente.");
            }
            else
            {
                MostrarMensaje(
                    "El barbero no existe o ya está inactivo.");
            }
        }

        // Comprueba si otro barbero esta duplicado nombre +apellido
        private bool ExisteDuplicado(
            string nombre, string apellido, int idActual)
        {
            BarberoDatos datos = new BarberoDatos();
            List<Barbero> barberos = datos.Listar();

            foreach (Barbero barbero in barberos)
            {
                if (barbero.Id != idActual &&
                    string.Equals(barbero.Nombre.Trim(),
                        nombre.Trim(),
                        StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(barbero.Apellido.Trim(),
                        apellido.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        
        private Barbero ObtenerDatosFormulario()
        {
            return new Barbero
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Especialidad = txtEspecialidad.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                FotoUrl = txtFotoUrl.Text.Trim()
            };
        }

        
        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MostrarMensaje("Ingresá el nombre y apellido.");
                return false;
            }

            return true;
        }

        // Limpia los campos del form
        private void LimpiarCampos()
        {
            hfIdBarbero.Value = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEspecialidad.Text = "";
            txtDescripcion.Text = "";
            txtFotoUrl.Text = "";
            dgvBarberos.SelectedIndex = -1;
        }

      
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            MostrarListado();
        }

        
        private void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);
            lblMensaje.CssClass =
                "d-block alert alert-info mb-3";
        }
    }
}
