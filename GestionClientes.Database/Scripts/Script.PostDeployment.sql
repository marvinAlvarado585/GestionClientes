/*
 Post-Deployment Script
 Datos semilla que se ejecutan después de crear/actualizar el esquema.
 Usuario inicial: admin / Admin123 (hash PBKDF2-SHA256 + salt, igual que SeguridadHelper.vb).
 Se usan guardas IF NOT EXISTS para que sea idempotente (no duplica al re-publicar).
*/

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = 'admin')
BEGIN
    INSERT INTO dbo.Usuarios (NombreUsuario, PasswordHash, PasswordSalt)
    VALUES ('admin',
            'YxSQruF3vau1vZeCxE4/v/DF6BzXKEWZAtWmkTGom04=',
            'x82DmHneAj58iTmPnnHLKw==');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Clientes)
BEGIN
    INSERT INTO dbo.Clientes (Nombre, Apellido, Identificacion, Email, Telefono, Direccion)
    VALUES
        (N'María',  N'González', N'01234567-8', N'maria.gonzalez@correo.com', N'7777-1111', N'San Salvador'),
        (N'Carlos', N'Martínez', N'09876543-2', N'carlos.martinez@correo.com', N'7777-2222', N'Santa Ana');
END
GO
