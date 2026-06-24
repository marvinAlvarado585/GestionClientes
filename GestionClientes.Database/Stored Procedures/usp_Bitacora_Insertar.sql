CREATE PROCEDURE [dbo].[usp_Bitacora_Insertar]
    @Accion        NVARCHAR(20),
    @ClienteId     INT,
    @NombreUsuario NVARCHAR(50),
    @Detalle       NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Bitacora (Accion, ClienteId, NombreUsuario, Detalle)
    VALUES (@Accion, @ClienteId, @NombreUsuario, @Detalle);
END
