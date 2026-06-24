/* ============================================================================
   Prueba Técnica FUNDAMICRO - Gestión de Clientes
   Script de creación de base de datos, tablas y usuario administrador semilla.

   Motor:    SQL Server 2022
   Ejecutar: en SSMS (abrir y presionar F5) o por línea de comandos:
             sqlcmd -S localhost -U sa -P Admin123 -i script_basedatos.sql

   Tablas:   Usuarios, Clientes, Bitacora
   ============================================================================ */

/* ---------------------------------------------------------------------------
   1. Crear la base de datos (si no existe)
   --------------------------------------------------------------------------- */
IF DB_ID('GestionClientesDB') IS NULL
BEGIN
    CREATE DATABASE GestionClientesDB;
END
GO

USE GestionClientesDB;
GO

/* ---------------------------------------------------------------------------
   2. Tabla Usuarios
      Almacena las credenciales. La contraseña NUNCA se guarda en texto plano:
      se guarda el hash (PBKDF2/SHA256) y el salt, ambos en Base64.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        Id              INT IDENTITY(1,1)   NOT NULL,
        NombreUsuario   NVARCHAR(50)        NOT NULL,
        PasswordHash    NVARCHAR(256)       NOT NULL,   -- hash PBKDF2 en Base64
        PasswordSalt    NVARCHAR(256)       NOT NULL,   -- salt en Base64
        FechaCreacion   DATETIME            NOT NULL CONSTRAINT DF_Usuarios_Fecha DEFAULT (GETDATE()),
        Activo          BIT                 NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
        CONSTRAINT PK_Usuarios PRIMARY KEY (Id),
        CONSTRAINT UQ_Usuarios_Nombre UNIQUE (NombreUsuario)
    );
END
GO

/* ---------------------------------------------------------------------------
   3. Tabla Clientes
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        Id              INT IDENTITY(1,1)   NOT NULL,
        Nombre          NVARCHAR(100)       NOT NULL,
        Apellido        NVARCHAR(100)       NULL,
        Identificacion  NVARCHAR(30)        NULL,       -- DUI / cédula / documento
        Email           NVARCHAR(120)       NULL,
        Telefono        NVARCHAR(30)        NULL,
        Direccion       NVARCHAR(250)       NULL,
        FechaRegistro   DATETIME            NOT NULL CONSTRAINT DF_Clientes_Fecha DEFAULT (GETDATE()),
        CONSTRAINT PK_Clientes PRIMARY KEY (Id)
    );
END
GO

/* ---------------------------------------------------------------------------
   4. Tabla Bitacora
      Registra cada cambio realizado sobre los clientes.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Bitacora', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bitacora
    (
        Id              INT IDENTITY(1,1)   NOT NULL,
        Accion          NVARCHAR(20)        NOT NULL,   -- AGREGAR / EDITAR / ELIMINAR
        ClienteId       INT                 NULL,       -- Id del cliente afectado
        NombreUsuario   NVARCHAR(50)        NOT NULL,   -- usuario que hizo el cambio
        FechaHora       DATETIME            NOT NULL CONSTRAINT DF_Bitacora_Fecha DEFAULT (GETDATE()),
        Detalle         NVARCHAR(500)       NULL,       -- descripción opcional del cambio
        CONSTRAINT PK_Bitacora PRIMARY KEY (Id)
    );
END
GO

/* ---------------------------------------------------------------------------
   5. Usuario administrador semilla
      Usuario: admin   Contraseña: Admin123
      El hash y el salt fueron generados con PBKDF2 (SHA256, 100000 iteraciones),
      exactamente el mismo algoritmo que usa SeguridadHelper.vb en la aplicación.
   --------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = 'admin')
BEGIN
    INSERT INTO dbo.Usuarios (NombreUsuario, PasswordHash, PasswordSalt)
    VALUES
    (
        'admin',
        'YxSQruF3vau1vZeCxE4/v/DF6BzXKEWZAtWmkTGom04=',  -- hash de "Admin123"
        'x82DmHneAj58iTmPnnHLKw=='                        -- salt
    );
END
GO

/* ---------------------------------------------------------------------------
   6. (Opcional) Datos de ejemplo de clientes
   --------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Clientes)
BEGIN
    INSERT INTO dbo.Clientes (Nombre, Apellido, Identificacion, Email, Telefono, Direccion)
    VALUES
        (N'María',  N'González', N'01234567-8', N'maria.gonzalez@correo.com', N'7777-1111', N'San Salvador'),
        (N'Carlos', N'Martínez', N'09876543-2', N'carlos.martinez@correo.com', N'7777-2222', N'Santa Ana');
END
GO

/* ============================================================================
   7. PROCEDIMIENTOS ALMACENADOS
      Toda la lógica SQL vive aquí (no incrustada en la aplicación).
      Se usa CREATE OR ALTER para que el script sea re-ejecutable.
      Todos reciben parámetros -> protegidos contra inyección SQL.
   ============================================================================ */

/* ----------------------------- Clientes ----------------------------- */
GO
CREATE OR ALTER PROCEDURE dbo.usp_Clientes_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Apellido, Identificacion, Email, Telefono, Direccion, FechaRegistro
    FROM dbo.Clientes
    ORDER BY Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Clientes_ObtenerPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Apellido, Identificacion, Email, Telefono, Direccion, FechaRegistro
    FROM dbo.Clientes
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Clientes_Insertar
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
GO

CREATE OR ALTER PROCEDURE dbo.usp_Clientes_Actualizar
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
GO

CREATE OR ALTER PROCEDURE dbo.usp_Clientes_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Clientes WHERE Id = @Id;
END
GO

/* ----------------------------- Usuarios ----------------------------- */
CREATE OR ALTER PROCEDURE dbo.usp_Usuarios_ObtenerPorNombre
    @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, NombreUsuario, PasswordHash, PasswordSalt, FechaCreacion, Activo
    FROM dbo.Usuarios
    WHERE NombreUsuario = @NombreUsuario AND Activo = 1;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Usuarios_ExisteNombre
    @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(1) FROM dbo.Usuarios WHERE NombreUsuario = @NombreUsuario;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Usuarios_Insertar
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
GO

CREATE OR ALTER PROCEDURE dbo.usp_Usuarios_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, NombreUsuario, FechaCreacion, Activo
    FROM dbo.Usuarios
    ORDER BY Id;
END
GO

/* ----------------------------- Bitácora ----------------------------- */
CREATE OR ALTER PROCEDURE dbo.usp_Bitacora_Insertar
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
GO

CREATE OR ALTER PROCEDURE dbo.usp_Bitacora_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Accion, ClienteId, NombreUsuario, FechaHora, Detalle
    FROM dbo.Bitacora
    ORDER BY FechaHora DESC, Id DESC;
END
GO

PRINT 'Base de datos GestionClientesDB lista. Usuario: admin / Contraseña: Admin123';
GO
