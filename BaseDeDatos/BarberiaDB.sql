-- Ejecutar en SQL Server Management Studio. No elimina datos existentes.
USE master;
GO
IF DB_ID(N'BarberiaDB') IS NULL
    EXEC(N'CREATE DATABASE BarberiaDB');
GO
USE BarberiaDB;
GO
IF OBJECT_ID(N'dbo.Servicios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Servicios
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Precio DECIMAL(10,2) NOT NULL,
        DuracionMinutos INT NOT NULL
    );
END;
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Servicios)
BEGIN
    INSERT INTO dbo.Servicios (Nombre, Precio, DuracionMinutos)
    VALUES (N'Corte de pelo', 10000, 30),
           (N'Arreglo de barba', 6000, 20),
           (N'Corte y barba', 14000, 45);
END;
GO
SELECT Id, Nombre, Precio, DuracionMinutos FROM dbo.Servicios;
