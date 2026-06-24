CREATE PROCEDURE [dbo].[usp_Clientes_Actualizar]
    @Id             INT,
    @Nombre         NVARCHAR(100),
    @Apellido       NVARCHAR(100),
    @Identificacion NVARCHAR(30),
    @Email          NVARCHAR(120),
    @Telefono       NVARCHAR(30),
    @Direccion      NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Clientes
    SET Nombre = @Nombre, Apellido = @Apellido, Identificacion = @Identificacion,
        Email = @Email, Telefono = @Telefono, Direccion = @Direccion
    WHERE Id = @Id;
END
