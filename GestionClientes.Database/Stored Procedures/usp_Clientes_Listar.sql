CREATE PROCEDURE [dbo].[usp_Clientes_Listar]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Apellido, Identificacion, Email, Telefono, Direccion, FechaRegistro
    FROM dbo.Clientes
    ORDER BY Id;
END
