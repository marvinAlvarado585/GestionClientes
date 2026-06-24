CREATE PROCEDURE [dbo].[usp_Usuarios_Listar]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, NombreUsuario, FechaCreacion, Activo
    FROM dbo.Usuarios
    ORDER BY Id;
END
