using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Cliente
    {
        public int Id { get; set; } // una propiedad permite consultar o asignar un dato de un objeto
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;



    }
}
