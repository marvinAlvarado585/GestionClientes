CREATE TABLE [dbo].[Clientes]
(
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [Nombre]         NVARCHAR (100)      NOT NULL,
    [Apellido]       NVARCHAR (100)      NULL,
    [Identificacion] NVARCHAR (30)       NULL,
    [Email]          NVARCHAR (120)      NULL,
    [Telefono]       NVARCHAR (30)       NULL,
    [Direccion]      NVARCHAR (250)      NULL,
    [FechaRegistro]  DATETIME            NOT NULL CONSTRAINT [DF_Clientes_Fecha] DEFAULT (GETDATE()),
    CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED ([Id] ASC)
);
