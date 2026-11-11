
USE BarberiaDB;
GO

-- Crea la tabla de barberos
IF OBJECT_ID('dbo.Barberos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Barberos
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Apellido NVARCHAR(100) NOT NULL,
        Especialidad NVARCHAR(100) NULL,
        Descripcion NVARCHAR(MAX) NULL,
        FotoUrl NVARCHAR(500) NULL,
        Activo BIT NOT NULL DEFAULT 1
    );
END;
GO
