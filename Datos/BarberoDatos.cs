using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dominio;

namespace Datos
{
    public class BarberoDatos
    {
        // Obtiene todos los barberos de SQL Server.
        public List<Barbero> Listar()
        {
            List<Barbero> barberos = new List<Barbero>();

            const string sql = @"
                SELECT Id, Nombre, Apellido, Especialidad,
                       Descripcion, FotoUrl, Activo
                FROM dbo.Barberos
                ORDER BY Apellido, Nombre, Id;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            {
                conexion.Open();

                using (var comando = new SqlCommand(sql, conexion))
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Barbero barbero = LeerBarbero(lector);
                        barberos.Add(barbero);
                    }
                }
            }

            return barberos;
        }

        // Busca un barbero por su Id.
        public Barbero BuscarPorId(int id)
        {
            const string sql = @"
                SELECT Id, Nombre, Apellido, Especialidad,
                       Descripcion, FotoUrl, Activo
                FROM dbo.Barberos
                WHERE Id = @Id;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            {
                conexion.Open();

                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return LeerBarbero(lector);
                        }

                        return null;
                    }
                }
            }
        }

        // ALTA: registra un barbero.
        public bool Alta(Barbero barbero)
        {
            const string sql = @"
                INSERT INTO dbo.Barberos
                    (Nombre, Apellido, Especialidad,
                     Descripcion, FotoUrl, Activo)
                VALUES
                    (@Nombre, @Apellido, @Especialidad,
                     @Descripcion, @FotoUrl, 1);";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = barbero.Nombre;
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = barbero.Apellido;
                comando.Parameters.Add("@Especialidad", SqlDbType.NVarChar, 100).Value = (object)barbero.Especialidad ?? "";
                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, -1).Value = (object)barbero.Descripcion ?? "";
                comando.Parameters.Add("@FotoUrl", SqlDbType.NVarChar, 500).Value = (object)barbero.FotoUrl ?? "";

                conexion.Open();

                return comando.ExecuteNonQuery() == 1;
            }
        }

        // MODIFICACION: cambia los datos de un barbero.
        public bool Modificacion(Barbero barbero)
        {
            const string sql = @"
                UPDATE dbo.Barberos
                SET Nombre = @Nombre,
                    Apellido = @Apellido,
                    Especialidad = @Especialidad,
                    Descripcion = @Descripcion,
                    FotoUrl = @FotoUrl
                WHERE Id = @Id;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = barbero.Nombre;
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = barbero.Apellido;
                comando.Parameters.Add("@Especialidad", SqlDbType.NVarChar, 100).Value = (object)barbero.Especialidad ?? "";
                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, -1).Value = (object)barbero.Descripcion ?? "";
                comando.Parameters.Add("@FotoUrl", SqlDbType.NVarChar, 500).Value = (object)barbero.FotoUrl ?? "";
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = barbero.Id;

                conexion.Open();

                return comando.ExecuteNonQuery() == 1;
            }
        }

        // BAJA: desactiva un barbero sin eliminarlo.
        public bool Baja(int id)
        {
            const string sql = @"
                UPDATE dbo.Barberos
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

        // Comprueba si existe un barbero con el mismo nombre y apellido.
        public bool ExisteBarbero(string nombre, string apellido)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.Barberos
                WHERE Nombre = @Nombre
                  AND Apellido = @Apellido;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = apellido;

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

        // Convierte una fila de SQL Server en un objeto Barbero.
        private Barbero LeerBarbero(SqlDataReader lector)
        {
            return new Barbero
            {
                Id = (int)lector["Id"],
                Nombre = (string)lector["Nombre"],
                Apellido = (string)lector["Apellido"],
                Especialidad = lector["Especialidad"] == DBNull.Value
                    ? "" : (string)lector["Especialidad"],
                Descripcion = lector["Descripcion"] == DBNull.Value
                    ? "" : (string)lector["Descripcion"],
                FotoUrl = lector["FotoUrl"] == DBNull.Value
                    ? "" : (string)lector["FotoUrl"],
                Activo = (bool)lector["Activo"]
            };
        }

        public bool Reactivar(int id)
        {
            const string sql = @"
        UPDATE dbo.Barberos
        SET Activo = 1
        WHERE Id = @Id AND Activo = 0";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                conexion.Open();

                return comando.ExecuteNonQuery() == 1;
            }
        }
    }
}
