CREATE PROCEDURE [dbo].[usp_Clientes_Eliminar]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Clientes WHERE Id = @Id;
END
