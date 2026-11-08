using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario // recorda a clase que representa la cuenta con la que alguien inicia sesión
    {
        public int Id { get; set; }
        public PerfilUsuario Perfil { get; set; }
        public int? ClienteId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;

        public string EmailAcceso { get; set; } = string.Empty;
    }
}
