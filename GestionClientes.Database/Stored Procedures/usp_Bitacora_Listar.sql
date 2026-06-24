CREATE PROCEDURE [dbo].[usp_Bitacora_Listar]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Accion, ClienteId, NombreUsuario, FechaHora, Detalle
    FROM dbo.Bitacora
    ORDER BY FechaHora DESC, Id DESC;
END
