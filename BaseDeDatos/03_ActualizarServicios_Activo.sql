USE BarberiaDB;
GO

-- Agrega la columna Activo solamente si todavía no existe.
IF COL_LENGTH('dbo.Servicios', 'Activo') IS NULL
BEGIN
    ALTER TABLE dbo.Servicios
    ADD Activo BIT NOT NULL
        CONSTRAINT DF_Servicios_Activo DEFAULT 1;
END;
GO

-- Comprobamos el resultado.
SELECT Id, Nombre, Precio, DuracionMinutos, Activo
FROM dbo.Servicios;
GO