using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dominio;

namespace Datos
{
    public class ClienteDatos
    {
        public List<Cliente> Buscar(string texto)
        {
            // Acá guardo en memoria los clientes que leo de SQL Server.
            List<Cliente> clientes = new List<Cliente>();

            const string sql = @"
                SELECT Id, Nombre, Apellido, Telefono, Email
                FROM dbo.Clientes
                WHERE @Texto = N''
                   OR CHARINDEX(@Texto, Nombre) > 0
                   OR CHARINDEX(@Texto, Apellido) > 0
                   OR CHARINDEX(@Texto, Telefono) > 0
                ORDER BY Apellido, Nombre, Id;";

            // Busco la conexión en Web.config usando el método compartido.
            // Así me ahorro escribir la búsqueda de la configuración cada vez.
            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            {
                conexion.Open();

                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.Add("@Texto", SqlDbType.NVarChar, 100).Value =
                        (texto ?? string.Empty).Trim();

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Cliente cliente = LeerCliente(lector);
                            clientes.Add(cliente);
                        }
                    }
                }
            }

            return clientes;
        }

        public List<Cliente> Listar()
        {
            return Buscar(string.Empty);
        }

        // Busca una ficha de cliente por su identificador.
        public Cliente BuscarPorId(int id)
        {
            const string sql = @"
                SELECT Id, Nombre, Apellido, Telefono, Email
                FROM dbo.Clientes
                WHERE Id = @Id;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            {
                conexion.Open();

                using (var comando = new SqlCommand(sql, conexion))
                {
                    // Si recibimos el número 7, @Id toma el valor 7.
                    comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return LeerCliente(lector);
                        }

                        // No existe una ficha con ese Id.
                        return null;
                    }
                }
            }
        }

        // Actualiza los datos de contacto de una ficha existente.
        public bool Actualizar(Cliente cliente)
        {
            const string sql = @"
                UPDATE dbo.Clientes
                SET Nombre = @Nombre,
                    Apellido = @Apellido,
                    Telefono = @Telefono,
                    Email = @Email
                WHERE Id = @Id;";

            using (var conexion = new SqlConnection(ObtenerCadenaConexion()))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;
                comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 30).Value = cliente.Telefono;
                comando.Parameters.Add("@Email", SqlDbType.NVarChar, 254).Value = cliente.Email;
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = cliente.Id;

                conexion.Open();

                // Si se modificó una fila, se guardaron los cambios de esa ficha.
                return comando.ExecuteNonQuery() == 1;
            }
        }

        // Lee la cadena de conexión configurada para la barbería.
        private string ObtenerCadenaConexion()
        {
            var configuracion = ConfigurationManager.ConnectionStrings["BarberiaDB"];

            if (configuracion == null)
            {
                throw new ConfigurationErrorsException(
                    "Falta configurar la conexión BarberiaDB.");
            }

            return configuracion.ConnectionString;
        }

        // Lee los datos de la fila actual y los guarda en un objeto Cliente.
        private Cliente LeerCliente(SqlDataReader lector)
        {
            return new Cliente
            {
                Id = (int)lector["Id"],
                Nombre = (string)lector["Nombre"],
                Apellido = (string)lector["Apellido"],
                Telefono = (string)lector["Telefono"],
                Email = (string)lector["Email"]
            };
        }
    }
}