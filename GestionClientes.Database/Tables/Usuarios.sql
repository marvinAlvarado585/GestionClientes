CREATE TABLE [dbo].[Usuarios]
(
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [NombreUsuario] NVARCHAR (50)       NOT NULL,
    [PasswordHash]  NVARCHAR (256)      NOT NULL,
    [PasswordSalt]  NVARCHAR (256)      NOT NULL,
    [FechaCreacion] DATETIME            NOT NULL CONSTRAINT [DF_Usuarios_Fecha] DEFAULT (GETDATE()),
    [Activo]        BIT                 NOT NULL CONSTRAINT [DF_Usuarios_Activo] DEFAULT ((1)),
    CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Usuarios_Nombre] UNIQUE NONCLUSTERED ([NombreUsuario] ASC)
);
