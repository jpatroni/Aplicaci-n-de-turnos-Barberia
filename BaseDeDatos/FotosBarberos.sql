
USE BarberiaDB;
GO

UPDATE dbo.Barberos
SET FotoUrl = '~/Imagenes/Barberos/Lucas_Benitez.jpg'
WHERE Nombre = 'Lucas' AND Apellido = 'Benítez';

UPDATE dbo.Barberos
SET FotoUrl = '~/Imagenes/Barberos/Valentin_Castro.jpg'
WHERE Nombre = 'Valentín' AND Apellido = 'Castro';

UPDATE dbo.Barberos
SET FotoUrl = '~/Imagenes/Barberos/Thiago_Fernandez.jpg'
WHERE Nombre = 'Thiago' AND Apellido = 'Fernández';

UPDATE dbo.Barberos
SET FotoUrl = '~/Imagenes/Barberos/Nicolas_Herrera.jpg'
WHERE Nombre = 'Nicolás' AND Apellido = 'Herrera';

UPDATE dbo.Barberos
SET FotoUrl = '~/Imagenes/Barberos/Franco_Martinez.jpg'
WHERE Nombre = 'Franco' AND Apellido = 'Martínez';

UPDATE dbo.Barberos
SET FotoUrl = '~/Imagenes/Barberos/Santiago_Rossi.jpg'
WHERE Nombre = 'Santiago' AND Apellido = 'Rossi';

UPDATE dbo.Barberos
SET FotoUrl = '~/Imagenes/Barberos/Mateo_Silva.jpg'
WHERE Nombre = 'Mateo' AND Apellido = 'Silva';

UPDATE dbo.Barberos
SET FotoUrl = '~/Imagenes/Barberos/Joaquin_Vega.jpg'
WHERE Nombre = 'Joaquín' AND Apellido = 'Vega';

-- Verificar las rutas guardadas
SELECT Nombre, Apellido, FotoUrl
FROM dbo.Barberos
ORDER BY Nombre;
GO
