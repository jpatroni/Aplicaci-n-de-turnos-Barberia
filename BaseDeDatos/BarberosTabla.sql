
USE BarberiaDB;
GO

-- Crear tabla de barberos
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

-- Cargar barberos de prueba sin duplicarlos
INSERT INTO dbo.Barberos
    (Nombre, Apellido, Especialidad, Descripcion, FotoUrl, Activo)
SELECT
    Datos.Nombre,
    Datos.Apellido,
    Datos.Especialidad,
    Datos.Descripcion,
    Datos.FotoUrl,
    Datos.Activo
FROM (VALUES

    (N'Mateo', N'Silva',
     N'Degradados y texturados',
     N'Cortes actuales con transiciones prolijas y atención a cada detalle.',
     N'https://randomuser.me/api/portraits/men/11.jpg', 1),

    (N'Lucas', N'Benítez',
     N'Cortes clásicos',
     N'Estilos tradicionales con terminaciones cuidadas y un toque personal.',
     N'https://randomuser.me/api/portraits/men/52.jpg', 1),

    (N'Santiago', N'Rossi',
     N'Barba y perfilado',
     N'Diseño y cuidado de barba para acompañar tus facciones y completar tu estilo.',
     N'https://randomuser.me/api/portraits/men/32.jpg', 1),

    (N'Joaquín', N'Vega',
     N'Corte personalizado',
     N'Asesoramiento para encontrar un corte cómodo que se adapte a tu día a día.',
     N'https://randomuser.me/api/portraits/men/75.jpg', 1),

    (N'Thiago', N'Fernández',
     N'Fade y diseños',
     N'Especialista en degradados modernos, diseños y líneas de precisión.',
     N'https://randomuser.me/api/portraits/men/22.jpg', 1),

    (N'Nicolás', N'Herrera',
     N'Corte con tijera',
     N'Cortes a tijera, estilos naturales y acabados profesionales.',
     N'https://randomuser.me/api/portraits/men/46.jpg', 1),

    (N'Franco', N'Martínez',
     N'Estilo urbano',
     N'Cortes modernos, crop, mullet y tendencias urbanas.',
     N'https://randomuser.me/api/portraits/men/60.jpg', 1),

    (N'Valentín', N'Castro',
     N'Barbería integral',
     N'Cortes masculinos, arreglo de barba y asesoramiento de imagen.',
     N'https://randomuser.me/api/portraits/men/81.jpg', 1)

) AS Datos
(
    Nombre,
    Apellido,
    Especialidad,
    Descripcion,
    FotoUrl,
    Activo
)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Barberos AS B
    WHERE B.Nombre = Datos.Nombre
      AND B.Apellido = Datos.Apellido
);
GO

-- Ver los barberos cargados
SELECT * FROM dbo.Barberos;
GO
