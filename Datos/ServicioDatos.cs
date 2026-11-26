using Datos.Infraestructura;
using Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Datos
{
    public class ServicioDatos
    {
        //Mantengo este método para compatibilidad con el código existente, aunque es redundante con ListarTodos().
        public List<Servicio> Listar()
        {
            return ListarTodos();
        }

        public List<Servicio> ListarActivos()
        {
            return ConsultarServicios(true);
        }

        public List<Servicio> ListarTodos()
        {
            return ConsultarServicios(false);
        }

        // Comparte la consulta y la lectura entre los dos listados.
        private List<Servicio> ConsultarServicios(bool soloActivos)
        {
            // La consulta que enviaremos a SQL Server.
            const string sql = @"
                SELECT Id, Nombre, Precio, DuracionMinutos, Activo
                FROM dbo.Servicios
                WHERE @SoloActivos = 0 OR Activo = 1
                ORDER BY Nombre, Id;";

            List<Servicio> servicios = new List<Servicio>();

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@SoloActivos", SqlDbType.Bit).Value = soloActivos;
                conexion.Open();

                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Servicio servicio = LeerServicio(lector); // crea el objeto Servicio.
                        servicios.Add(servicio); // Acá pone el objeto en la lista.
                    }
                }
            }

            return servicios;
        }

        // Busca un servicio por su Id.
        public Servicio BuscarPorId(int id)
        {
            const string sql = @"
                SELECT Id, Nombre, Precio, DuracionMinutos, Activo
                FROM dbo.Servicios
                WHERE Id = @Id;";

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                conexion.Open();

                using (var lector = comando.ExecuteReader())
                    return lector.Read() ? LeerServicio(lector) : null;
            }
        }

        // ALTA: registra un servicio.
        public bool Alta(Servicio servicio)
        {
            ValidarServicio(servicio);

            const string sql = @"
                INSERT INTO dbo.Servicios (Nombre, Precio, DuracionMinutos, Activo)
                VALUES (@Nombre, @Precio, @DuracionMinutos, 1);";

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                AgregarParametrosServicio(comando, servicio);
                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }

        // MODIFICACION: actualiza los datos de un servicio.
        public bool Modificacion(Servicio servicio)
        {
            ValidarServicio(servicio);

            const string sql = @"
                UPDATE dbo.Servicios
                SET Nombre = @Nombre, Precio = @Precio,
                    DuracionMinutos = @DuracionMinutos
                WHERE Id = @Id;";

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                AgregarParametrosServicio(comando, servicio);
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = servicio.Id;
                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }

        // Baja lógica: desactiva un servicio.
        public bool Baja(int id)
        {
            return CambiarEstado(id, false);
        }

        public bool Reactivar(int id)
        {
            return CambiarEstado(id, true);
        }

        // Borra únicamente servicios inactivos que nunca fueron usados en turnos.
        // La condición se comprueba en el mismo DELETE, no solo en la pantalla.
        public bool EliminarDefinitivamente(int id)
        {
            const string sql = @"
                DELETE FROM dbo.Servicios
                WHERE Id = @Id AND Activo = 0
                  AND NOT EXISTS (
                      SELECT 1 FROM dbo.Turnos WHERE ServicioId = @Id
                  );";

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }

        // Conserva el registro y sus relaciones con los turnos históricos.
        private bool CambiarEstado(int id, bool activo)
        {
            const string sql = @"
                UPDATE dbo.Servicios
                SET Activo = @Activo
                WHERE Id = @Id AND Activo <> @Activo;";

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = activo;
                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }

        // Comprueba si existe un servicio con el mismo nombre.
        // Al modificar, excluye la propia ficha; también revisa los inactivos.
        public bool ExisteServicio(string nombre, int idExcluir = 0)
        {
            const string sql = @"
                SELECT COUNT(*) FROM dbo.Servicios
                WHERE LTRIM(RTRIM(Nombre)) = @Nombre AND Id <> @IdExcluir;";

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre.Trim();
                comando.Parameters.Add("@IdExcluir", SqlDbType.Int).Value = idExcluir;
                conexion.Open();
                return (int)comando.ExecuteScalar() > 0;
            }
        }

        // Aplica las mismas reglas aunque se invoque Datos desde otra pantalla.
        private void ValidarServicio(Servicio servicio)
        {
            string error = ReglasServicio.Validar(servicio);
            if (!string.IsNullOrEmpty(error))
                throw new ArgumentException(error, nameof(servicio));
        }

        // Parámetros compartidos por alta y modificación.
        private void AgregarParametrosServicio(SqlCommand comando, Servicio servicio)
        {
            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = servicio.Nombre.Trim();
            var precio = comando.Parameters.Add("@Precio", SqlDbType.Decimal);
            precio.Precision = 10;
            precio.Scale = 2;
            precio.Value = servicio.Precio;
            comando.Parameters.Add("@DuracionMinutos", SqlDbType.Int).Value = servicio.DuracionMinutos;
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
