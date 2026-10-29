using System;
using System.Security.Cryptography.X509Certificates;
using Dominio;

namespace Dominio
{
    public class Servicio
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int DuracionMinutos { get; set; }
    }
}

