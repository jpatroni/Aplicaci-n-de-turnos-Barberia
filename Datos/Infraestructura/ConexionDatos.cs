using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Datos.Infraestructura
{
    internal  static class ConexionDatos
    {
 // buscador en web config el texto de conexion a la base de datos

        private static string ObtenerCadenaDeConexion()
        {
            var configuracion = ConfigurationManager.ConnectionStrings["BarberiaDB"];
            if (configuracion == null )
            {
                throw new ConfigurationErrorsException("falta configurar la conexion BarberiaDB en el archivo Web.config");

            }

            return configuracion.ConnectionString;


        }

        internal static SqlConnection CrearConexion()
        {
            return new SqlConnection(ObtenerCadenaDeConexion());

        }


    }
}
