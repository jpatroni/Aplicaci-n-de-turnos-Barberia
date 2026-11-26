using Dominio;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Datos.Infraestructura;

namespace Datos
{
    // La clase que va a guardar el registro en SQL Server.
    public class RegistroDatos
    {
        // Organiza el registro completo.
        public int RegistrarCliente(Cliente cliente, string passwordHash)
        {
            ValidarRegistro(cliente, passwordHash);

            // Busca en Web.config dónde está la base de datos mediante Infraestructura.
            using (var conexion = ConexionDatos.CrearConexion())
            {
                conexion.Open();

                // La ficha y la cuenta van a compartir esta transacción.
                using (var transaccion = conexion.BeginTransaction())
                {
                    int clienteId = InsertarCliente(conexion, transaccion, cliente);
                    // aca guardo al cuenta y ademas la vinculo con al ficha recien creada
                    InsertarUsuarioCliente(conexion, transaccion,clienteId,cliente.Email,passwordHash);


                    transaccion.Commit();
                    return clienteId;

                }
            }
        }

        // Protege también las llamadas que no vengan del formulario web.
        private void ValidarRegistro(Cliente cliente, string passwordHash)
        {
            string error = ReglasRegistro.ValidarCliente(cliente);
            if (!string.IsNullOrEmpty(error))
                throw new ArgumentException(error, nameof(cliente));

            if (string.IsNullOrWhiteSpace(passwordHash) || passwordHash.Length > 255)
                throw new ArgumentException("La contraseña no se pudo preparar.", nameof(passwordHash));
        }

        // Hace una tarea puntual: guardar la ficha.
        private int InsertarCliente(
            SqlConnection conexion,
            SqlTransaction transaccion,
            Cliente cliente)
        {
            const string sqlCliente = @"
                INSERT INTO dbo.Clientes (Nombre, Apellido, Telefono, Email)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Apellido, @Telefono, @Email);";

            using (var comando = new SqlCommand(sqlCliente, conexion, transaccion))
            {
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;
                comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 30).Value = cliente.Telefono;
                comando.Parameters.Add("@Email", SqlDbType.NVarChar, 254).Value = cliente.Email;

                // Ejecuta el INSERT y devuelve el Id que generó SQL Server.
                return (int)comando.ExecuteScalar();
            }
        }

        // Guarda la cuenta y la vincula con la ficha mediante clienteId.
        private void InsertarUsuarioCliente(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int clienteId,
            string emailAcceso,
            string passwordHash)
        {
            const string sqlUsuario = @"
                INSERT INTO dbo.Usuarios (EmailAcceso, Perfil, ClienteId, PasswordHash)
                VALUES (@EmailAcceso, @Perfil, @ClienteId, @PasswordHash);";

            using (var comando = new SqlCommand(sqlUsuario, conexion, transaccion))
            {
                comando.Parameters.Add("@EmailAcceso", SqlDbType.NVarChar, 254).Value = emailAcceso;

                // El registro público siempre crea cuentas con perfil Cliente.
                comando.Parameters.Add("@Perfil", SqlDbType.Int).Value = (int)PerfilUsuario.Cliente;

                comando.Parameters.Add("@ClienteId", SqlDbType.Int).Value = clienteId;
                comando.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = passwordHash;

                // Ejecuta el INSERT. Acá no necesitamos recuperar el Id de la cuenta.
                comando.ExecuteNonQuery();
            }
        }
    }
}
