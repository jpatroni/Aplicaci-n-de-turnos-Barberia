-- Ejecutar después de 02_ClientesYUsuarios.sql.
USE BarberiaDB;
GO

IF COL_LENGTH(N'dbo.Clientes', N'Activo') IS NULL
BEGIN
    ALTER TABLE dbo.Clientes
        ADD Activo BIT NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT (1);
END;
GO
