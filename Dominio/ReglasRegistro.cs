using System;
using System.Text.RegularExpressions;

namespace Dominio
{
    // Validaciones de los datos de una cuenta pública de cliente.
    public static class ReglasRegistro
    {
        public static bool EsEmailValido(string email)
        {
            return !string.IsNullOrWhiteSpace(email)
                && email.Length <= 254
                && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static string ValidarCliente(Cliente cliente)
        {
            if (cliente == null)
                return "Completá los datos del cliente.";

            if (string.IsNullOrWhiteSpace(cliente.Nombre) || cliente.Nombre.Length > 100)
                return "Ingresá un nombre de hasta 100 caracteres.";

            if (string.IsNullOrWhiteSpace(cliente.Apellido) || cliente.Apellido.Length > 100)
                return "Ingresá un apellido de hasta 100 caracteres.";

            if (!EsTelefonoValido(cliente.Telefono))
                return "Ingresá un teléfono válido, con al menos 6 dígitos y hasta 30 caracteres.";

            if (!EsEmailValido(cliente.Email))
                return "Ingresá un correo electrónico válido de hasta 254 caracteres.";

            return string.Empty;
        }

        public static string ValidarContrasena(string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena)
                || contrasena.Length < 8 || contrasena.Length > 128)
                return "La contraseña debe tener entre 8 y 128 caracteres.";

            return string.Empty;
        }

        private static bool EsTelefonoValido(string telefono)
        {
            return !string.IsNullOrWhiteSpace(telefono)
                && telefono.Length <= 30
                && Regex.IsMatch(telefono, @"^\+?[0-9\s().-]+$")
                && Regex.Replace(telefono, @"\D", "").Length >= 6;
        }
    }
}
