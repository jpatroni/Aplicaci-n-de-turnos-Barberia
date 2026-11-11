using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Aplicación_de_turnos_Barberia.Seguridad
{
    public static class Contrasenas
    {
        private const int Iteraciones = 600000;

        // Genera el texto que después guardaremos en PasswordHash.
        public static string GenerarHash(string contrasena)
        {
            if (string.IsNullOrEmpty(contrasena))
            {
                throw new ArgumentException(
                    "La contraseña no puede estar vacía.", nameof(contrasena));
            }

            // Creamos un valor aleatorio para que dos contraseñas iguales
            // no tengan el mismo resultado.
            byte[] salt = new byte[16];

            using (var generador = RandomNumberGenerator.Create())
            {
                generador.GetBytes(salt);
            }

            // Acá .NET hace el cálculo con la contraseña, el salt y las iteraciones.
            byte[] hash = CalcularHash(contrasena, salt, Iteraciones);

            // Armamos el texto que después vamos a guardar.
            return $"pbkdf2-sha256${Iteraciones}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }







        // comprueba si la contraseña ingresada coincide con el hash

        public static bool Verificar(string contrasena, string hashGuardado)

        {
            if (string.IsNullOrEmpty(contrasena) || string.IsNullOrEmpty(hashGuardado))
            {
                return false;
            }

            // Separamos el hash en sus partes.

            string[] partes = hashGuardado.Split('$');


            if (partes.Length != 4 || partes[0] != "pbkdf2-sha256" || !int.TryParse(partes[1], out int iteraciones) || iteraciones != Iteraciones)
            {
                return false;
            }

            // Recupera el salt y el hash del texto guardado.
            byte[] salt;
            byte[] hashEsperado;

            try
            {
                //Agarrá el texto de la posición 2, convertilo a bytes y guardalo en salt
                salt = Convert.FromBase64String(partes[2]);
                hashEsperado = Convert.FromBase64String(partes[3]);
            }
            catch (FormatException)
            {
                // Si el texto guardado no tiene un formato válido, no se verifica.
                return false;
            }
            if (salt.Length != 16 || hashEsperado.Length != 32)
            {
                return false;
            }

            byte[] hashCalculado = CalcularHash(contrasena, salt, iteraciones);
            int diferencias = 0;
            for (int i = 0; i < hashEsperado.Length; i++)
            {
                diferencias |= hashEsperado[i] ^ hashCalculado[i];
            }

            return diferencias == 0;


        }

        // Hace el cálculo de PBKDF2 con los datos que recibe.
        private static byte[] CalcularHash(string contrasena, byte[] salt, int iteraciones)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                contrasena, salt, iteraciones, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32);
            }





        }
    }
}