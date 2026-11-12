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
                SELECT Id, EmailAcceso, Perfil, ClienteId, PasswordHash
                FROM dbo.Usuarios
                WHERE EmailAcceso = @Email;";

            using (var conexion = ConexionDatos.CrearConexion())
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Email", SqlDbType.NVarChar, 254).Value = email;
                conexion.Open();

                using (var lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                    {
                        return null;
                    }

                    return LeerUsuario(lector);
                } // Acá se cierra el lector.
            }
        } // Acá termina BuscarPorEmail.

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
    } // Acá termina la clase UsuarioDatos.
}