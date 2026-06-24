CREATE PROCEDURE [dbo].[usp_Clientes_ObtenerPorId]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Apellido, Identificacion, Email, Telefono, Direccion, FechaRegistro
    FROM dbo.Clientes
    WHERE Id = @Id;
END
