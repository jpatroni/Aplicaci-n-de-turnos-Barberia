using System;

namespace Dominio
{
    // Reglas del servicio, sin controles web ni consultas SQL.
    public static class ReglasServicio
    {
        public const int DuracionEstimadaMinutos = 30;

        public static string Validar(Servicio servicio)
        {
            if (servicio == null)
                return "No se recibieron los datos del servicio.";

            if (string.IsNullOrWhiteSpace(servicio.Nombre)
                || servicio.Nombre.Trim().Length > 100)
                return "Ingresá un nombre de entre 1 y 100 caracteres.";

            if (servicio.Precio <= 0 || servicio.Precio > 99999999.99m)
                return "El precio debe ser mayor a cero y no superar 99999999,99.";

            if (decimal.Round(servicio.Precio, 2) != servicio.Precio)
                return "El precio puede tener como máximo dos decimales.";

            if (servicio.DuracionMinutos != DuracionEstimadaMinutos)
                return "Los servicios se organizan en bloques de 30 minutos.";

            return string.Empty;
        }
    }
}
