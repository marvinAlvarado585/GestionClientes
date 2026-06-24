CREATE PROCEDURE [dbo].[usp_Clientes_Insertar]
    @Nombre         NVARCHAR(100),
    @Apellido       NVARCHAR(100),
    @Identificacion NVARCHAR(30),
    @Email          NVARCHAR(120),
    @Telefono       NVARCHAR(30),
    @Direccion      NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Clientes (Nombre, Apellido, Identificacion, Email, Telefono, Direccion)
    VALUES (@Nombre, @Apellido, @Identificacion, @Email, @Telefono, @Direccion);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
