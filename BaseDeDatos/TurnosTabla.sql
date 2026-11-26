
USE BarberiaDB;
GO

IF OBJECT_ID('dbo.Turnos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Turnos
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,

        ClienteId INT NOT NULL,
        BarberoId INT NOT NULL,
        ServicioId INT NOT NULL,

        FechaHora DATETIME2 NOT NULL,

        Estado NVARCHAR(20) NOT NULL
            DEFAULT 'Confirmado',

        CONSTRAINT FK_Turnos_Clientes
            FOREIGN KEY (ClienteId)
            REFERENCES dbo.Clientes(Id),

        CONSTRAINT FK_Turnos_Barberos
            FOREIGN KEY (BarberoId)
            REFERENCES dbo.Barberos(Id),

        CONSTRAINT FK_Turnos_Servicios
            FOREIGN KEY (ServicioId)
            REFERENCES dbo.Servicios(Id),

        CONSTRAINT CK_Turnos_Estado
            CHECK (Estado IN
            (
                'Confirmado',
                'Cancelado',
                'Atendido',
                'NoAsistio'
            ))
    );

    PRINT 'Tabla Turnos creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Turnos ya existe. No se modifico.';
END;
GO

SELECT
    t.Id,
    c.Nombre + ' ' + c.Apellido AS Cliente,
    b.Nombre + ' ' + b.Apellido AS Barbero,
    s.Nombre AS Servicio,
    t.FechaHora,
    t.Estado
FROM dbo.Turnos t
INNER JOIN dbo.Clientes c
    ON t.ClienteId = c.Id
INNER JOIN dbo.Barberos b
    ON t.BarberoId = b.Id
INNER JOIN dbo.Servicios s
    ON t.ServicioId = s.Id
ORDER BY t.FechaHora;
GO
