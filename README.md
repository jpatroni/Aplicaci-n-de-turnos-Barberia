# Aplicación de turnos de barbería

Trabajo práctico integrador. ASP.NET Web Forms y .NET Framework 4.8.

## Organización

- `Dominio`: Cliente, Barbero, Servicio, Turno y EstadoTurno.
- `Datos`: consulta SQL Server mediante ServicioDatos.Listar(). Referencia a Dominio.
- `Web`: pantallas de los compañeros. Referencia a Datos y Dominio.
- `BaseDeDatos/BarberiaDB.sql`: creación de base, tabla Servicios y ejemplos.

Las clases se mantienen en sus proyectos; no se copian dentro de Web.

## Ejecutar

1. Instalar en Visual Studio la carga de trabajo ASP.NET y desarrollo web y las herramientas de destino de .NET Framework 4.8. Usar Visual Studio 2022 o posterior con soporte para C# 10. Tener SQL Server disponible.
2. Ejecutar `BaseDeDatos/BarberiaDB.sql` en Management Studio. No borra registros y solo agrega ejemplos cuando la tabla está vacía.
3. Abrir `Barberia.sln` con Visual Studio Community.
4. En `Web/Web.config`, ajustar `Server` dentro de la conexión `BarberiaDB` al servidor de cada equipo. `Integrated Security=True` usa la cuenta de Windows que ejecuta la aplicación.
5. Establecer `Web` como proyecto de inicio, compilar y ejecutar. Visitar `Servicios.aspx`.

La base de datos no viaja con Git: cada integrante debe prepararla con el script.
No se versionan `.vs`, `bin`, `obj` ni paquetes restaurables.

## Alcance actual del primer hito

La pantalla Servicios llama a Datos, que lee SQL Server y devuelve objetos Servicio para la grilla.
Inicio contiene la bienvenida y accesos. Reservar turno contiene servicio, barbero, fecha y horario de ejemplo, con confirmación deshabilitada y enlace de regreso.
No se implementaron reservas ni ABM. El modelo deberá seguir revisándose contra la propuesta.

## Verificación de la integración

Se compiló la solución con MSBuild de Visual Studio y se ejecutó en IIS Express.
Inicio, Servicios y Reservar turno respondieron HTTP 200. La grilla de Servicios
mostró el registro existente de SQL Server: Corte de pelo, 10000,00, 30 minutos.
No se ejecutó el script SQL sobre la base existente ni se modificaron sus registros.

## Procedencia

Las pantallas se incorporaron desde la rama `origin/auxiliar`, commit `26fd1b0`, conservando sus archivos de presentación. Se retiraron las copias de Dominio y AccesoDatos del proyecto web para utilizar los proyectos compartidos.

## Trabajo del equipo

La rama compartida es `auxiliar`. Guardar y confirmar el trabajo propio antes de cambiar de rama o integrar cambios. Obtener las ramas remotas, cambiar a `auxiliar` y hacer Pull. Abrir siempre `Barberia.sln` de la raíz; la solución anterior del proyecto web ya no se utiliza.

Antes de Push, compilar y revisar los archivos incluidos. No subir cambios personales en el nombre del servidor ni archivos temporales. Si Pull informa conflictos, resolverlos antes de continuar; no descartar el trabajo de otro integrante.
