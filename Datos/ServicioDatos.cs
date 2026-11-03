using System.Collections.Generic;
using Dominio;
using System.Data.SqlClient;
using System.Configuration;

namespace Datos
{
   public class ServicioDatos
    {
        public List<Servicio> Listar()
        {
            List<Servicio> servicios = new List<Servicio>();
            using(SqlConnection conexion = new SqlConnection()) // configura dónde y cómo conectarse.
            {
                ConnectionStringSettings configuracion = ConfigurationManager.ConnectionStrings["BarberiaDB"];
                if (configuracion == null)
                    throw new ConfigurationErrorsException("Falta la conexión BarberiaDB en Web.config.");

                conexion.ConnectionString = configuracion.ConnectionString;

                conexion.Open();
                using (SqlCommand comando = new SqlCommand())
                {
                    comando.Connection = conexion;

                    comando.CommandText =
                        "SELECT Id, Nombre, Precio, DuracionMinutos FROM Servicios";  // r la consulta que enviaremos a SQL Server
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while(lector.Read()){
                            Servicio servicio = new Servicio();  // crea el obj serviciio

                            servicio.Id = (int)lector["Id"];
                            servicio.Nombre = (string)lector["Nombre"];
                            servicio.Precio = (decimal)lector["Precio"];
                            servicio.DuracionMinutos = (int)lector["DuracionMinutos"];

                            servicios.Add(servicio); // aca pone el obj en la lista
                        }
                    }


                }

            }
            return servicios;

        }

    }
}
