CREATE PROCEDURE [dbo].[usp_Usuarios_ExisteNombre]
    @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(1) FROM dbo.Usuarios WHERE NombreUsuario = @NombreUsuario;
END
