using System;
using Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                dgvClientes.DataSource = new[]
                {
                    new Cliente
                    {  Id = 7,
                        Nombre = "Ana",
            Apellido = "Ejemplo",
            Telefono = "0000-0000",
            Email = "ana@example.com"
                    }
                };
                dgvClientes.DataBind();
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
            if (PuedeAccederRecepcion(perfil))
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