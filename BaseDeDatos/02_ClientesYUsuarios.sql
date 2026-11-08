-- Ejecutar en SQL Server Management Studio después de BarberiaDB.sql.
-- Si las tablas ya existen, no se modifican ni se cargan datos.
USE BarberiaDB;
GO

IF OBJECT_ID(N'dbo.Clientes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Apellido NVARCHAR(100) NOT NULL,
        Telefono NVARCHAR(30) NOT NULL,
        Email NVARCHAR(254) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EmailAcceso NVARCHAR(254) NOT NULL UNIQUE,
        -- 1 = Cliente, 2 = Administrador, 3 = Recepcionista.
        Perfil INT NOT NULL CHECK (Perfil IN (1, 2, 3)),
        ClienteId INT NULL REFERENCES dbo.Clientes(Id),
        PasswordHash NVARCHAR(255) NOT NULL,

        CONSTRAINT CK_Usuarios_PerfilCliente CHECK
        (
            (Perfil = 1 AND ClienteId IS NOT NULL)
            OR
            (Perfil IN (2, 3) AND ClienteId IS NULL)
        )
    );
END;
GO
