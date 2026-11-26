using System;

namespace Dominio
{
    // Reglas compartidas por disponibilidad, reserva y reprogramación.
    public static class ReglasTurno
    {
        public static bool EsDiaDeAtencion(DateTime fecha)
        {
            return fecha.DayOfWeek != DayOfWeek.Sunday
                && fecha.DayOfWeek != DayOfWeek.Monday;
        }

        public static string ValidarFecha(DateTime fecha)
        {
            if (fecha.Date < DateTime.Today)
                return "Elegí una fecha de hoy en adelante.";

            if (!EsDiaDeAtencion(fecha))
                return "La barbería atiende de martes a sábado.";

            return string.Empty;
        }

        public static bool EsHorarioValido(DateTime inicio, DateTime fin, int duracion)
        {
            return duracion > 0
                && EsDiaDeAtencion(inicio)
                && inicio.TimeOfDay >= TimeSpan.FromHours(9)
                && fin.Date == inicio.Date
                && fin.TimeOfDay <= TimeSpan.FromHours(21)
                && inicio.Minute % 30 == 0
                && inicio.Second == 0
                && inicio.Millisecond == 0;
        }
    }
}
