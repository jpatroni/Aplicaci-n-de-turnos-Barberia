using Dominio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Datos.Infraestructura;

namespace Datos
{
    public class TurnoDatos
    {
        
        public List<DateTime> ListarHorariosOcupados(int barberoId, DateTime fecha)
        {
            List<DateTime> horarios = new List<DateTime>();

            string cadena = ConfigurationManager
                .ConnectionStrings["BarberiaDB"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                string consulta = @"
                    SELECT FechaHora
                    FROM dbo.Turnos
                    WHERE BarberoId = @BarberoId
                    AND FechaHora >= @Inicio
                    AND FechaHora < @Fin
                    AND Estado IN ('Confirmado')
                    ORDER BY FechaHora";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@BarberoId", SqlDbType.Int).Value = barberoId;
                    comando.Parameters.Add("@Inicio", SqlDbType.DateTime2).Value = fecha.Date;
                    comando.Parameters.Add("@Fin", SqlDbType.DateTime2).Value = fecha.Date.AddDays(1);

                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            horarios.Add((DateTime)lector["FechaHora"]);
                        }
                    }
                }
            }
            return horarios;
        }
        public List<DateTime> ObtenerHorariosDisponibles(
            int barberoId, DateTime fecha, int duracionMinutos)
        {
            List<DateTime> disponibles = new List<DateTime>();

            if (duracionMinutos <= 0)
                throw new ArgumentException("La duración debe ser mayor a cero.");

            DateTime apertura = fecha.Date.AddHours(9);
            DateTime cierre = fecha.Date.AddHours(21);


            // Comprueba si el servicio puede entrar en la jornada de ese día.
            DateTime primerFin = apertura.AddMinutes(duracionMinutos);

            if (!EsHorarioValido(apertura, primerFin, duracionMinutos))
            {
                return disponibles;
            }

            List<Tuple<DateTime, int>> ocupados =
                new List<Tuple<DateTime, int>>();

            string cadena = ConfigurationManager
                .ConnectionStrings["BarberiaDB"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                string consulta = @"
            SELECT t.FechaHora, s.DuracionMinutos
            FROM dbo.Turnos t
            INNER JOIN dbo.Servicios s ON t.ServicioId = s.Id
            WHERE t.BarberoId = @BarberoId
              AND t.FechaHora >= @Inicio
              AND t.FechaHora < @Fin
              AND t.Estado IN ('Confirmado')";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@BarberoId", SqlDbType.Int).Value = barberoId;
                    comando.Parameters.Add("@Inicio", SqlDbType.DateTime2).Value = fecha.Date;
                    comando.Parameters.Add("@Fin", SqlDbType.DateTime2).Value = fecha.Date.AddDays(1);

                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            ocupados.Add(Tuple.Create(
                                (DateTime)lector["FechaHora"],
                                (int)lector["DuracionMinutos"]));
                        }
                    }
                }
            }
          
            for (DateTime hora = apertura;
                 hora.AddMinutes(duracionMinutos) <= cierre;
                 hora = hora.AddMinutes(30))
            {
               
                if (hora <= DateTime.Now)
                    continue;

                DateTime finNuevoTurno = hora.AddMinutes(duracionMinutos);
                bool libre = true;

                foreach (var ocupado in ocupados)
                {
                    DateTime inicioOcupado = ocupado.Item1;
                    DateTime finOcupado =
                        inicioOcupado.AddMinutes(ocupado.Item2);

                    
                    if (hora < finOcupado &&
                        finNuevoTurno > inicioOcupado)
                    {
                        libre = false;
                        break;
                    }
                }

                if (libre)
                    disponibles.Add(hora);
            }

            return disponibles;
        }


        public bool GuardarTurno(int clienteId, int barberoId,
            int servicioId, DateTime fechaHora)
        {
            if (fechaHora <= DateTime.Now)
                return false;

            string cadena = ConfigurationManager
                .ConnectionStrings["BarberiaDB"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                conexion.Open();

                using (SqlTransaction transaccion =
                    conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // duracion del servicio
                        string sqlDuracion = @"
                    SELECT DuracionMinutos
                    FROM dbo.Servicios
                    WHERE Id = @ServicioId AND Activo = 1";

                        int duracion;

                        using (SqlCommand comando =
                            new SqlCommand(sqlDuracion, conexion, transaccion))
                        {
                            comando.Parameters.Add("@ServicioId", SqlDbType.Int)
                                .Value = servicioId;

                            object resultado = comando.ExecuteScalar();

                            if (resultado == null)
                            {
                                transaccion.Rollback();
                                return false;
                            }

                            duracion = Convert.ToInt32(resultado);
                        }

                        DateTime fin = fechaHora.AddMinutes(duracion);

                        // horario de apertura y cierre
                        if (!EsHorarioValido(fechaHora, fin, duracion))
                        {
                            transaccion.Rollback();
                            return false;
                        }

                       
                        string sqlOcupado = @"
                    SELECT COUNT(*)
                    FROM dbo.Turnos t WITH (UPDLOCK, HOLDLOCK)
                    INNER JOIN dbo.Servicios s
                        ON t.ServicioId = s.Id
                    WHERE t.BarberoId = @BarberoId
                      AND t.Estado IN ('Confirmado')
                      AND t.FechaHora < @Fin
                      AND DATEADD(MINUTE, s.DuracionMinutos, t.FechaHora)
                          > @Inicio";

                        using (SqlCommand comando =
                            new SqlCommand(sqlOcupado, conexion, transaccion))
                        {
                            comando.Parameters.Add("@BarberoId", SqlDbType.Int)
                                .Value = barberoId;
                            comando.Parameters.Add("@Inicio", SqlDbType.DateTime2)
                                .Value = fechaHora;
                            comando.Parameters.Add("@Fin", SqlDbType.DateTime2)
                                .Value = fin;

                            if ((int)comando.ExecuteScalar() > 0)
                            {
                                transaccion.Rollback();
                                return false;
                            }
                        }
                                                
                        string sqlInsertar = @"
                    INSERT INTO dbo.Turnos
                        (ClienteId, BarberoId, ServicioId, FechaHora, Estado)
                    VALUES
                        (@ClienteId, @BarberoId, @ServicioId,
                         @FechaHora, 'Confirmado')";

                        using (SqlCommand comando =
                            new SqlCommand(sqlInsertar, conexion, transaccion))
                        {
                            comando.Parameters.Add("@ClienteId", SqlDbType.Int)
                                .Value = clienteId;
                            comando.Parameters.Add("@BarberoId", SqlDbType.Int)
                                .Value = barberoId;
                            comando.Parameters.Add("@ServicioId", SqlDbType.Int)
                                .Value = servicioId;
                            comando.Parameters.Add("@FechaHora", SqlDbType.DateTime2)
                                .Value = fechaHora;

                            comando.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                        return true;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<Turno> ListarTurnosPorFecha(DateTime fecha)
        {
            List<Turno> turnos = new List<Turno>();

            string cadena = ConfigurationManager
                .ConnectionStrings["BarberiaDB"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                string consulta = @"
            SELECT
                t.Id,
                t.FechaHora,
                t.Estado,
                c.Id AS ClienteId,
                c.Nombre AS ClienteNombre,
                c.Apellido AS ClienteApellido,
                b.Id AS BarberoId,
                b.Nombre AS BarberoNombre,
                b.Apellido AS BarberoApellido,
                s.Id AS ServicioId,
                s.Nombre AS ServicioNombre,
                s.DuracionMinutos
            FROM dbo.Turnos t
            INNER JOIN dbo.Clientes c ON t.ClienteId = c.Id
            INNER JOIN dbo.Barberos b ON t.BarberoId = b.Id
            INNER JOIN dbo.Servicios s ON t.ServicioId = s.Id
            WHERE t.FechaHora >= @Inicio
              AND t.FechaHora < @Fin
            ORDER BY t.FechaHora";

                using (SqlCommand comando =
                    new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@Inicio", SqlDbType.DateTime2)
                        .Value = fecha.Date;

                    comando.Parameters.Add("@Fin", SqlDbType.DateTime2)
                        .Value = fecha.Date.AddDays(1);

                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Turno turno = new Turno();

                            turno.Id = (int)lector["Id"];
                            turno.FechaHora = (DateTime)lector["FechaHora"];

                            turno.Cliente = new Cliente
                            {
                                Id = (int)lector["ClienteId"],
                                Nombre = lector["ClienteNombre"].ToString(),
                                Apellido = lector["ClienteApellido"].ToString()
                            };

                            turno.Barbero = new Barbero
                            {
                                Id = (int)lector["BarberoId"],
                                Nombre = lector["BarberoNombre"].ToString(),
                                Apellido = lector["BarberoApellido"].ToString()
                            };

                            turno.Servicio = new Servicio
                            {
                                Id = (int)lector["ServicioId"],
                                Nombre = lector["ServicioNombre"].ToString(),
                                DuracionMinutos =
                                    (int)lector["DuracionMinutos"]
                            };

                            turno.Estado = (EstadoTurno)Enum.Parse(
                                typeof(EstadoTurno),
                                lector["Estado"].ToString());

                            turnos.Add(turno);
                        }
                    }
                }
            }

            return turnos;
        }

        public bool CancelarTurno(int turnoId)
        {
            string cadena = ConfigurationManager
                .ConnectionStrings["BarberiaDB"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                string consulta = @"
            UPDATE dbo.Turnos
            SET Estado = 'Cancelado'
            WHERE Id = @TurnoId
              AND Estado = 'Confirmado'
              AND FechaHora > GETDATE()";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@TurnoId", SqlDbType.Int).Value = turnoId;

                    conexion.Open();

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }



        // la busqueda de los datos necesarios para reprogramar una reserva.
        private bool BuscarTurnoParaReprogramar(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int turnoId,
            out int barberoId,
            out int duracion)
        {
            barberoId = 0;
            duracion = 0;

            const string sql = @"
        SELECT t.BarberoId, s.DuracionMinutos
        FROM dbo.Turnos t WITH (UPDLOCK, HOLDLOCK)
        INNER JOIN dbo.Servicios s ON t.ServicioId = s.Id
        WHERE t.Id = @TurnoId
          AND t.Estado = 'Confirmado'
          AND t.FechaHora > GETDATE();";

            using (var comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.Add("@TurnoId", SqlDbType.Int).Value = turnoId;

                using (var lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                    {
                        return false;
                    }

                    barberoId = (int)lector["BarberoId"];
                    duracion = (int)lector["DuracionMinutos"];

                    return true;
                }
            }
        }


        // duraciones y eso
        private bool EsHorarioValido(
            DateTime inicio, DateTime fin, int duracion)
        {
            return duracion > 0

                    && inicio.DayOfWeek != DayOfWeek.Sunday
    && inicio.DayOfWeek != DayOfWeek.Monday
                && inicio.TimeOfDay >= TimeSpan.FromHours(9)
                && fin.Date == inicio.Date
                && fin.TimeOfDay <= TimeSpan.FromHours(21)
                && inicio.Minute % 30 == 0
                && inicio.Second == 0
                && inicio.Millisecond == 0;
        }


        // para saber si el barbero esta ocupado 

        // Comprueba si otra reserva ocupa parte del horario solicitado.
        private bool ExisteSuperposicion(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int turnoId,
            int barberoId,
            DateTime inicio,
            DateTime fin)
        {
            const string sql = @"
        SELECT COUNT(*)
        FROM dbo.Turnos t WITH (UPDLOCK, HOLDLOCK)
        INNER JOIN dbo.Servicios s ON t.ServicioId = s.Id
        WHERE t.BarberoId = @BarberoId
          AND t.Id <> @TurnoId
          AND t.Estado = 'Confirmado'
          AND t.FechaHora < @Fin
          AND DATEADD(MINUTE, s.DuracionMinutos, t.FechaHora) > @Inicio;";

            using (var comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.Add("@BarberoId", SqlDbType.Int).Value = barberoId;
                comando.Parameters.Add("@TurnoId", SqlDbType.Int).Value = turnoId;
                comando.Parameters.Add("@Inicio", SqlDbType.DateTime2).Value = inicio;
                comando.Parameters.Add("@Fin", SqlDbType.DateTime2).Value = fin;

                return (int)comando.ExecuteScalar() > 0;
            }
        }


        // Actualiza la fecha y hora de una reserva confirmada.
        private bool ActualizarFechaHora(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int turnoId,
            DateTime nuevaFechaHora)
        {
            const string sql = @"
        UPDATE dbo.Turnos
        SET FechaHora = @NuevaFechaHora
        WHERE Id = @TurnoId
          AND Estado = 'Confirmado';";

            using (var comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.Add("@TurnoId", SqlDbType.Int).Value = turnoId;
                comando.Parameters.Add("@NuevaFechaHora", SqlDbType.DateTime2)
                    .Value = nuevaFechaHora;

                return comando.ExecuteNonQuery() == 1;
            }
        }



        /// metodo principal 
        /// Buscar el turno.
      //  Validar el horario.
       // Comprobar disponibilidad.
       // Actualizar la fecha.
//Confirmar la transacción si todo salió bien.
        public bool ReprogramarTurno(int turnoId, DateTime nuevaFechaHora)
        {
            if (nuevaFechaHora <= DateTime.Now)
                return false;

            using (SqlConnection conexion = ConexionDatos.CrearConexion())
            {
                conexion.Open();

                using (SqlTransaction transaccion =
                    conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {

                        int barberoId;
                        int duracion;

                        // Busca los datos del turno y cierra el lector antes de regresar.
                        bool encontrado = BuscarTurnoParaReprogramar(
                            conexion, transaccion, turnoId,
                            out barberoId, out duracion);

                        if (!encontrado)
                        {
                            transaccion.Rollback();
                            return false;
                        }


                        DateTime fin = nuevaFechaHora.AddMinutes(duracion);
                        if (!EsHorarioValido(nuevaFechaHora, fin, duracion))
                        {
                            transaccion.Rollback();
                            return false;
                        }
                        // Si el barbero está ocupado, conserva la reserva original.
                        if (ExisteSuperposicion(
                            conexion, transaccion, turnoId,
                            barberoId, nuevaFechaHora, fin))
                        {
                            transaccion.Rollback();
                            return false;
                        }
                        // Intenta actualizar la reserva dentro de la misma transacción.
                        if (!ActualizarFechaHora(
                            conexion, transaccion, turnoId, nuevaFechaHora))
                        {
                            transaccion.Rollback();
                            return false;
                        }
                        transaccion.Commit();
                        return true;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
              
            }
        }

    }

}