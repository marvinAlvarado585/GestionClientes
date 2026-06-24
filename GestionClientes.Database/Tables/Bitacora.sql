CREATE TABLE [dbo].[Bitacora]
(
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [Accion]        NVARCHAR (20)       NOT NULL,
    [ClienteId]     INT                 NULL,
    [NombreUsuario] NVARCHAR (50)       NOT NULL,
    [FechaHora]     DATETIME            NOT NULL CONSTRAINT [DF_Bitacora_Fecha] DEFAULT (GETDATE()),
    [Detalle]       NVARCHAR (500)      NULL,
    CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED ([Id] ASC)
);
