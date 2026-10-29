using System.Collections.Generic;
using Dominio;
using System.Data.SqlClient;

namespace Datos
{
    public class ServicioDatos
    {
        public List<Servicio> Listar()
        {
            List<Servicio> servicios = new List<Servicio>();
            using (SqlConnection conexion = new SqlConnection()) // configura dónde y cómo conectarse.
            {
                conexion.ConnectionString =
    conexion.ConnectionString =
    @"Server=.\SQLEXPRESS;Database=BarberiaDB;Integrated Security=True;";

                conexion.Open();
                using (SqlCommand comando = new SqlCommand())
                {
                    comando.Connection = conexion;

                    comando.CommandText =
                        "SELECT Id, Nombre, Precio, DuracionMinutos FROM Servicios";  // r la consulta que enviaremos a SQL Server
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
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
