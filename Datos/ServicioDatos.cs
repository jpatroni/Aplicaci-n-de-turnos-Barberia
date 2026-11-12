using Dominio;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Datos
{
   public class ServicioDatos
    {

        //Mantengo este método para compatibilidad con el código existente, aunque es redundante con ListarTodos().
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
                        "SELECT Id, Nombre, Precio, DuracionMinutos, Activo FROM Servicios";  // r la consulta que enviaremos a SQL Server
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while(lector.Read()){
                            Servicio servicio = new Servicio();  // crea el obj serviciio

                            servicio.Id = (int)lector["Id"];
                            servicio.Nombre = (string)lector["Nombre"];
                            servicio.Precio = (decimal)lector["Precio"];
                            servicio.DuracionMinutos = (int)lector["DuracionMinutos"];
                            servicio.Activo = (bool)lector["Activo"];

                            servicios.Add(servicio); // aca pone el obj en la lista
                        }
                    }


                }

            }
            return servicios;

        }

            public List<Servicio> ListarActivos()
        {
            List<Servicio> servicios = new List<Servicio>();

            using (SqlConnection conexion = new SqlConnection(ObtenerCadenaConexion()))
            {
                conexion.Open();

                using (SqlCommand comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = @"
                SELECT Id, Nombre, Precio, DuracionMinutos, Activo
                FROM Servicios
                WHERE Activo = 1";

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            servicios.Add(LeerServicio(lector));
                        }
                    }
                }
            }

            return servicios;
        }

        public List<Servicio> ListarTodos()
        {
            List<Servicio> servicios = new List<Servicio>();

            using (SqlConnection conexion = new SqlConnection(ObtenerCadenaConexion()))
            {
                conexion.Open();

                using (SqlCommand comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = @"
                SELECT Id, Nombre, Precio, DuracionMinutos, Activo
                FROM Servicios";

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            servicios.Add(LeerServicio(lector));
                        }
                    }
                }
            }

            return servicios;
        }


        // Busca un servicio por su Id.
        public Servicio BuscarPorId(int id)
        {
            const string sql = @"
                SELECT Id, Nombre, Precio, DuracionMinutos
                FROM dbo.Servicios
                WHERE Id = @Id;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                conexion.Open();

                using (var lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return LeerServicio(lector);
                    }

                    return null;
                }
            }
        }

        // ALTA: registra un servicio.
        public bool Alta(Servicio servicio)
        {
            const string sql = @"
                INSERT INTO dbo.Servicios
                    (Nombre, Precio, DuracionMinutos)
                VALUES
                    (@Nombre, @Precio, @DuracionMinutos);";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = servicio.Nombre;

                var parametroPrecio = comando.Parameters.Add("@Precio", SqlDbType.Decimal);
                parametroPrecio.Precision = 10;
                parametroPrecio.Scale = 2;
                parametroPrecio.Value = servicio.Precio;

                comando.Parameters.Add("@DuracionMinutos", SqlDbType.Int).Value = servicio.DuracionMinutos;

                conexion.Open();

                return comando.ExecuteNonQuery() == 1;
            }
        }

        // MODIFICACION: actualiza los datos de un servicio.
        public bool Modificacion(Servicio servicio)
        {
            const string sql = @"
                UPDATE dbo.Servicios
                SET Nombre = @Nombre,
                    Precio = @Precio,
                    DuracionMinutos = @DuracionMinutos
                WHERE Id = @Id;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = servicio.Nombre;

                var parametroPrecio = comando.Parameters.Add("@Precio", SqlDbType.Decimal);
                parametroPrecio.Precision = 10;
                parametroPrecio.Scale = 2;
                parametroPrecio.Value = servicio.Precio;

                comando.Parameters.Add("@DuracionMinutos", SqlDbType.Int).Value = servicio.DuracionMinutos;
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = servicio.Id;

                conexion.Open();

                return comando.ExecuteNonQuery() == 1;
            }
        }

        // Baja lógica: desactiva un servicio.
        public bool Baja(int id)
        {
            const string sql = @"
        UPDATE dbo.Servicios
        SET Activo = 0
        WHERE Id = @Id AND Activo = 1;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                conexion.Open();

                return comando.ExecuteNonQuery() == 1;
            }
        }

        public bool Reactivar(int id)
        {
            const string sql = @"
        UPDATE dbo.Servicios
        SET Activo = 1
        WHERE Id = @Id AND Activo = 0;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                conexion.Open();

                return comando.ExecuteNonQuery() == 1;
            }
        }

        // Comprueba si existe un servicio con el mismo nombre.
        public bool ExisteServicio(string nombre)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.Servicios
                WHERE Nombre = @Nombre;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;

                conexion.Open();

                return (int)comando.ExecuteScalar() > 0;
            }
        }

        // Obtiene la conexión configurada en Web.config.
        private string ObtenerCadenaConexion()
        {
            var configuracion =
                ConfigurationManager.ConnectionStrings["BarberiaDB"];

            if (configuracion == null)
            {
                throw new ConfigurationErrorsException(
                    "Falta configurar la conexión BarberiaDB.");
            }

            return configuracion.ConnectionString;
        }

        // Convierte una fila SQL en un objeto Servicio.
        private Servicio LeerServicio(SqlDataReader lector)
        {
            return new Servicio
            {
                Id = (int)lector["Id"],
                Nombre = (string)lector["Nombre"],
                Precio = (decimal)lector["Precio"],
                DuracionMinutos = (int)lector["DuracionMinutos"],
                Activo = (bool)lector["Activo"]
            };
        }
    }
}

