using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Turno
    {
        public int Id { get; set; }  // pra identificar cda reserva.
        public DateTime FechaHora { get; set; }


        /// relaciones 
        public Cliente Cliente { get; set; } = null!; /// aca a clase Turno tiene una propiedad llamada Cliente, que puede guardar una referencia a un objeto de la clase Cliente

        public Servicio Servicio { get; set; } = null!;

        public Barbero Barbero { get; set; } = null!;
        public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente; // Cada turno tiene un estado, elegido de la lista EstadoTurno, y comienza en Pendiente



    }
}
