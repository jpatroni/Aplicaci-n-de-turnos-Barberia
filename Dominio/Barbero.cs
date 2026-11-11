using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Barbero
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
       
    }
}
