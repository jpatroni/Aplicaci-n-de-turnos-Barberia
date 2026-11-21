using System.Data;
using System.Data.SqlClient;
using Datos.Infraestructura;
using Dominio;

namespace Datos
{
    public class UsuarioDatos
    {
        public Usuario BuscarPorEmail(string email)
        {
            const string sql = @"
                SELECT
                    u.Id,
                    u.EmailAcceso,
                    u.Perfil,
                    u.ClienteId,
                    u.PasswordHash
                FROM dbo.Usuarios AS u
                LEFT JOIN dbo.Clientes AS c ON c.Id = u.ClienteId
                WHERE u.EmailAcceso = @Email
                  AND (u.ClienteId IS NULL OR c.Activo = 1);";

            Usuario usuario = null;

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Email", SqlDbType.NVarChar, 254).Value = email;

                conexion.Open();

                using (var lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        usuario = LeerUsuario(lector);
                    }
                }
            }

            // Si el Usuario es un Cliente buscamos sus datos utilizando ClienteDatos.
            if (usuario != null && usuario.ClienteId.HasValue)
            {
                ClienteDatos clienteDatos = new ClienteDatos();

                usuario.Cliente = clienteDatos.BuscarPorId(
                    usuario.ClienteId.Value
                );
            }

            return usuario;
        }

        // Convierte la fila encontrada en un objeto Usuario.
        private Usuario LeerUsuario(SqlDataReader lector)
        {
            return new Usuario
            {
                Id = (int)lector["Id"],
                EmailAcceso = (string)lector["EmailAcceso"],
                Perfil = (PerfilUsuario)(int)lector["Perfil"],
                ClienteId = lector["ClienteId"] == System.DBNull.Value
                    ? (int?)null
                    : (int)lector["ClienteId"],
                PasswordHash = (string)lector["PasswordHash"]
            };
        }
    }
}