using Dominio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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
            DateTime cierre = fecha.Date.AddHours(20);

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
                        if (duracion <= 0 ||
                            fechaHora.TimeOfDay < TimeSpan.FromHours(9) ||
                            fin.Date != fechaHora.Date ||
                            fin.TimeOfDay > TimeSpan.FromHours(20) ||
                            fechaHora.Minute % 30 != 0 ||
                            fechaHora.Second != 0)
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

        public bool ReprogramarTurno(int turnoId, DateTime nuevaFechaHora)
        {
            if (nuevaFechaHora <= DateTime.Now)
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
                        string sqlTurno = @"
                    SELECT t.BarberoId, s.DuracionMinutos
                    FROM dbo.Turnos t WITH (UPDLOCK, HOLDLOCK)
                    INNER JOIN dbo.Servicios s
                        ON t.ServicioId = s.Id
                    WHERE t.Id = @TurnoId
                      AND t.Estado = 'Confirmado'
                      AND t.FechaHora > GETDATE()";

                        int barberoId;
                        int duracion;

                        using (SqlCommand comando =
                            new SqlCommand(sqlTurno, conexion, transaccion))
                        {
                            comando.Parameters.Add("@TurnoId", SqlDbType.Int)
                                .Value = turnoId;

                            using (SqlDataReader lector = comando.ExecuteReader())
                            {
                                if (!lector.Read())
                                {
                                    transaccion.Rollback();
                                    return false;
                                }

                                barberoId = (int)lector["BarberoId"];
                                duracion = (int)lector["DuracionMinutos"];
                            }
                        }

                        DateTime fin = nuevaFechaHora.AddMinutes(duracion);

                        if (duracion <= 0 ||
                            nuevaFechaHora.TimeOfDay < TimeSpan.FromHours(9) ||
                            fin.Date != nuevaFechaHora.Date ||
                            fin.TimeOfDay > TimeSpan.FromHours(20) ||
                            nuevaFechaHora.Minute % 30 != 0 ||
                            nuevaFechaHora.Second != 0 ||
                            nuevaFechaHora.Millisecond != 0)
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
                      AND t.Id <> @TurnoId
                      AND t.Estado = 'Confirmado'
                      AND t.FechaHora < @Fin
                      AND DATEADD(MINUTE, s.DuracionMinutos,
                          t.FechaHora) > @Inicio";

                        using (SqlCommand comando =
                            new SqlCommand(sqlOcupado, conexion, transaccion))
                        {
                            comando.Parameters.Add("@BarberoId", SqlDbType.Int)
                                .Value = barberoId;
                            comando.Parameters.Add("@TurnoId", SqlDbType.Int)
                                .Value = turnoId;
                            comando.Parameters.Add("@Inicio", SqlDbType.DateTime2)
                                .Value = nuevaFechaHora;
                            comando.Parameters.Add("@Fin", SqlDbType.DateTime2)
                                .Value = fin;

                            if ((int)comando.ExecuteScalar() > 0)
                            {
                                transaccion.Rollback();
                                return false;
                            }
                        }

                        string sqlActualizar = @"
                    UPDATE dbo.Turnos
                    SET FechaHora = @NuevaFechaHora
                    WHERE Id = @TurnoId
                      AND Estado = 'Confirmado'";

                        using (SqlCommand comando =
                            new SqlCommand(sqlActualizar, conexion, transaccion))
                        {
                            comando.Parameters.Add("@TurnoId", SqlDbType.Int)
                                .Value = turnoId;
                            comando.Parameters.Add("@NuevaFechaHora", SqlDbType.DateTime2)
                                .Value = nuevaFechaHora;

                            if (comando.ExecuteNonQuery() != 1)
                            {
                                transaccion.Rollback();
                                return false;
                            }
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