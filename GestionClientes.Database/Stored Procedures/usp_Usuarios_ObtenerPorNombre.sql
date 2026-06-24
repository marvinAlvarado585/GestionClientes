CREATE PROCEDURE [dbo].[usp_Usuarios_ObtenerPorNombre]
    @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, NombreUsuario, PasswordHash, PasswordSalt, FechaCreacion, Activo
    FROM dbo.Usuarios
    WHERE NombreUsuario = @NombreUsuario AND Activo = 1;
END
