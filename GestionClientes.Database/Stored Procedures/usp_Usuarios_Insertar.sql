CREATE PROCEDURE [dbo].[usp_Usuarios_Insertar]
    @NombreUsuario NVARCHAR(50),
    @PasswordHash  NVARCHAR(256),
    @PasswordSalt  NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Usuarios (NombreUsuario, PasswordHash, PasswordSalt)
    VALUES (@NombreUsuario, @PasswordHash, @PasswordSalt);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
