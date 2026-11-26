using System;
using System.Collections.Generic;
using System.Web.UI;
using Datos;
using Dominio;
using System.IO;
using System.Web;

namespace Aplicación_de_turnos_Barberia
{
    public partial class GestionBarberos : Seguridad.PaginaAdministracion
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
            btnReactivar.Visible = false;

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
            string rutaFoto = barbero.FotoUrl ?? "";

            if (rutaFoto.StartsWith("~/Imagenes/Barberos/",
                StringComparison.OrdinalIgnoreCase)
                && System.IO.File.Exists(Server.MapPath(rutaFoto)))
            {
                hfFotoActual.Value = rutaFoto;
                imgFotoActual.ImageUrl = ResolveUrl(rutaFoto);
            }
            else
            {
                hfFotoActual.Value = "";
                imgFotoActual.ImageUrl =
                    "~/Imagenes/Barberos/sin-foto.png";
            }

            imgFotoActual.ImageUrl = string.IsNullOrWhiteSpace(barbero.FotoUrl)
                ? "~/Imagenes/Barberos/sin-foto.png"
                : barbero.FotoUrl;

            lblTituloFormulario.Text = "Modificación de Barbero";

            btnAlta.Visible = false;
            btnModificacion.Visible = true;
            btnBaja.Visible = barbero.Activo;
            btnReactivar.Visible = !barbero.Activo;
            pnlListado.Visible = false;
            pnlFormulario.Visible = true;
        }

        // Nuevo Barbero
        protected void btnAlta_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            Barbero barbero;
            if (!IntentarLeerFormulario(out barbero))
                return;
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

            Barbero barbero;
            if (!IntentarLeerFormulario(out barbero))
                return;
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
        // ALTA LÓGICA: reactiva un barbero dado de baja
        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            int id;

            if (!int.TryParse(hfIdBarbero.Value, out id))
            {
                MostrarMensaje("Primero seleccioná un barbero.");
                return;
            }

            BarberoDatos datos = new BarberoDatos();

            if (datos.Reactivar(id))
            {
                MostrarListado();
                MostrarMensaje("Barbero dado de alta correctamente.");
            }
            else
            {
                MostrarMensaje("El barbero no existe o ya está activo.");
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

        
        // Convierte los errores de carga en avisos sin interrumpir la página.
        private bool IntentarLeerFormulario(out Barbero barbero)
        {
            barbero = null;
            try
            {
                barbero = ObtenerDatosFormulario();
                return true;
            }
            catch (ArgumentException ex)
            {
                MostrarMensaje(ex.Message);
            }
            catch (IOException ex)
            {
                Trace.Warn("FotoBarbero", "No se pudo guardar la imagen.", ex);
                MostrarMensaje("No se pudo guardar la foto. Volvé a seleccionarla e intentá nuevamente.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Trace.Warn("FotoBarbero", "Sin permiso para guardar imágenes.", ex);
                MostrarMensaje("No se pudo guardar la foto por un problema de permisos.");
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
                FotoUrl = GuardarFoto(hfFotoActual.Value)
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
            hfFotoActual.Value = "";
            imgFotoActual.ImageUrl = ResolveUrl("~/Imagenes/Barberos/sin-foto.png");
            imgFotoActual.Visible = true;
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

        private string GuardarFoto(string fotoActual)
        {
            if (!fuFoto.HasFile)
                return fotoActual;

            const int maxBytes = 15 * 1024 * 1024;

            if (fuFoto.PostedFile.ContentLength > maxBytes ||
                fuFoto.PostedFile.ContentLength == 0)
                throw new ArgumentException("La foto debe pesar entre 1 byte y 15 MB.");

            string extension = Path.GetExtension(fuFoto.FileName)
                .ToLowerInvariant();

            if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png")
                throw new ArgumentException("Solo se permiten imágenes JPG o PNG.");

            byte[] firma = new byte[8];

            Stream archivo = fuFoto.PostedFile.InputStream;
            long posicion = archivo.Position;
            try
            {
                if (archivo.Read(firma, 0, firma.Length) < firma.Length)
                    throw new ArgumentException("El archivo no es una imagen válida.");
            }

            finally
            {
                archivo.Position = posicion;
            }

            bool jpeg = firma[0] == 0xFF &&
                        firma[1] == 0xD8 &&
                        firma[2] == 0xFF;

            bool png = firma[0] == 0x89 &&
                       firma[1] == 0x50 &&
                       firma[2] == 0x4E &&
                       firma[3] == 0x47 &&
                       firma[4] == 0x0D &&
                       firma[5] == 0x0A &&
                       firma[6] == 0x1A &&
                       firma[7] == 0x0A;

            if ((!jpeg && !png) ||
                (extension == ".png" && !png) ||
                (extension != ".png" && !jpeg))
                throw new ArgumentException("El archivo no es una imagen JPG o PNG válida.");

            string carpeta = Server.MapPath("~/Imagenes/Barberos/");
            Directory.CreateDirectory(carpeta);

            string nombre = Guid.NewGuid().ToString("N") + extension;
            string ruta = Path.Combine(carpeta, nombre);

            fuFoto.SaveAs(ruta);

            return "~/Imagenes/Barberos/" + nombre;
        }

        protected string ObtenerFotoListado(object fotoUrl)
        {
            string ruta = Convert.ToString(fotoUrl);
            string predeterminada = "~/Imagenes/Barberos/sin-foto.png";

            if (string.IsNullOrWhiteSpace(ruta))
                return ResolveUrl(predeterminada);

            if (ruta.StartsWith("~/Imagenes/Barberos/",
                StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    if (File.Exists(Server.MapPath(ruta)))
                        return ResolveUrl(ruta);
                }
                catch
                {
                    return ResolveUrl(predeterminada);
                }
            }

            return ResolveUrl(predeterminada);
        }

    }
}
