using Datos;
using Dominio;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Aplicación_de_turnos_Barberia
{
    public partial class GestionServicios : Seguridad.PaginaAdministracion
    {
        private static readonly CultureInfo CulturaPrecio = CultureInfo.GetCultureInfo("es-AR");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LimpiarFormulario();
                EjecutarAccion(CargarServicios);
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            MostrarMensaje(string.Empty);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            MostrarMensaje(string.Empty);
        }

        protected void dgvServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvServicios.SelectedDataKey == null)
                return;

            int id = Convert.ToInt32(dgvServicios.SelectedDataKey.Value);
            EjecutarAccion(() => CargarEdicion(id));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Servicio servicio;
            string error = LeerFormulario(out servicio);

            if (!string.IsNullOrEmpty(error))
            {
                MostrarMensaje(error, true);
                return;
            }

            EjecutarAccion(() => GuardarServicio(servicio));
        }

        protected void dgvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool activar = e.CommandName == "ReactivarServicio";
            bool eliminar = e.CommandName == "EliminarServicio";
            if (!activar && !eliminar && e.CommandName != "DesactivarServicio")
                return;

            int id;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id) || id <= 0)
            {
                MostrarMensaje("No se pudo identificar el servicio. Actualizá el listado.", true);
                return;
            }

            if (eliminar)
                EjecutarAccion(() => EliminarServicio(id));
            else
                EjecutarAccion(() => CambiarEstadoServicio(id, activar));
        }

        // Carga todos los servicios: Administración también necesita ver los inactivos.
        private void CargarServicios()
        {
            dgvServicios.DataSource = new ServicioDatos().ListarTodos();
            dgvServicios.DataBind();
        }

        // Lee la ficha desde SQL; no toma los valores mostrados por la grilla.
        private void CargarEdicion(int id)
        {
            Servicio servicio = new ServicioDatos().BuscarPorId(id);
            if (servicio == null)
            {
                LimpiarFormulario();
                MostrarMensaje("El servicio ya no existe. Actualizá el listado.", true);
                return;
            }

            hfIdServicio.Value = servicio.Id.ToString(CultureInfo.InvariantCulture);
            txtNombre.Text = servicio.Nombre;
            txtPrecio.Text = servicio.Precio.ToString("0.##", CulturaPrecio);
            txtDuracion.Text = ReglasServicio.DuracionEstimadaMinutos.ToString();
            btnGuardar.Text = "Guardar cambios";
            MostrarMensaje("Editando: " + servicio.Nombre + ".");
        }

        // Convierte los textos del formulario y delega las reglas en Dominio.
        private string LeerFormulario(out Servicio servicio)
        {
            servicio = null;
            int id;
            decimal precio;

            if (!int.TryParse(hfIdServicio.Value, out id) || id < 0)
                return "Cancelá la edición y seleccioná nuevamente el servicio.";

            if (!decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.AllowDecimalPoint,
                CulturaPrecio, out precio))
                return "Ingresá el precio sin separador de miles: por ejemplo, 10000 o 10000,50.";

            servicio = new Servicio
            {
                Id = id,
                Nombre = txtNombre.Text.Trim(),
                Precio = precio,
                DuracionMinutos = ReglasServicio.DuracionEstimadaMinutos,
                Activo = true
            };

            return ReglasServicio.Validar(servicio);
        }

        // Decide entre alta y modificación; las consultas quedan en ServicioDatos.
        private void GuardarServicio(Servicio servicio)
        {
            ServicioDatos datos = new ServicioDatos();

            if (datos.ExisteServicio(servicio.Nombre, servicio.Id))
            {
                MostrarMensaje("Ya existe otro servicio con ese nombre. Si está inactivo, podés reactivarlo.", true);
                return;
            }

            bool nuevo = servicio.Id == 0;
            bool guardado = nuevo ? datos.Alta(servicio) : datos.Modificacion(servicio);

            if (!guardado)
            {
                MostrarMensaje("No se encontró el servicio. Actualizá el listado antes de volver a intentar.", true);
                return;
            }

            LimpiarFormulario();
            CargarServicios();
            MostrarMensaje(nuevo ? "Servicio agregado correctamente." : "Cambios guardados correctamente.");
        }

        // Da de baja o reactiva sin borrar el servicio ni sus turnos.
        private void CambiarEstadoServicio(int id, bool activar)
        {
            ServicioDatos datos = new ServicioDatos();
            bool cambiado = activar ? datos.Reactivar(id) : datos.Baja(id);

            LimpiarFormulario();
            CargarServicios();

            MostrarMensaje(cambiado
                ? (activar ? "Servicio reactivado correctamente." : "Servicio dado de baja correctamente.")
                : "El servicio no existe o su estado ya había cambiado.", !cambiado);
        }

        // Solicita el borrado; Datos protege los servicios que tienen historial.
        private void EliminarServicio(int id)
        {
            bool eliminado = new ServicioDatos().EliminarDefinitivamente(id);

            LimpiarFormulario();
            CargarServicios();
            MostrarMensaje(eliminado
                ? "Servicio eliminado definitivamente."
                : "No se eliminó el servicio. Debe estar dado de baja y no tener turnos asociados; también puede haber sido eliminado por otra persona.", !eliminado);
        }

        // Prepara una nueva alta y elimina cualquier selección anterior.
        private void LimpiarFormulario()
        {
            hfIdServicio.Value = "0";
            txtNombre.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtDuracion.Text = ReglasServicio.DuracionEstimadaMinutos.ToString();
            dgvServicios.SelectedIndex = -1;
            btnGuardar.Text = "Guardar servicio";
        }

        // Comparte el manejo de errores de acceso a Datos entre los eventos.
        private void EjecutarAccion(Action accion)
        {
            try
            {
                accion();
            }
            catch (SqlException ex)
            {
                Trace.Warn("Servicios", "Falló una operación SQL.", ex);
                MostrarMensaje("No pudimos completar la operación con la base. Actualizá el listado para verificar el resultado antes de volver a intentar.", true);
            }
            catch (ConfigurationErrorsException ex)
            {
                Trace.Warn("Servicios", "Falta configurar la conexión.", ex);
                MostrarMensaje("No se pudo conectar con la base. Revisá la configuración BarberiaDB.", true);
            }
            catch (ArgumentException ex)
            {
                Trace.Warn("Servicios", "Datos de servicio no válidos.", ex);
                MostrarMensaje("Revisá el nombre, el precio y la duración del servicio.", true);
            }
        }

        // Muestra texto seguro y distingue errores de confirmaciones.
        private void MostrarMensaje(string mensaje, bool esError = false)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);
            lblMensaje.Visible = !string.IsNullOrEmpty(mensaje);
            lblMensaje.CssClass = esError
                ? "alert alert-danger d-block"
                : "alert alert-success d-block";
        }
    }
}
