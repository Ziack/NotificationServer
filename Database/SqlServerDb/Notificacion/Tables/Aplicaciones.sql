CREATE TABLE [Notificacion].[Aplicaciones] (
    [id_Aplicacion]  INT                IDENTITY (1, 1) NOT NULL,
    [ds_Titulo]      VARCHAR (50)       NOT NULL,
    [ds_Descripcion] VARCHAR (200)      NOT NULL,
    [id_Creado]      BIGINT             NULL,
    [ds_Token]       NVARCHAR(MAX) NULL, 
    [dt_Creado]      DATETIMEOFFSET (7) CONSTRAINT [DF_Applicaciones_dt_Creado] DEFAULT (sysdatetimeoffset()) NULL,
    [id_Modificado]  BIGINT             NULL,
    [dt_Modificado]  DATETIMEOFFSET (7) CONSTRAINT [DF_Applicaciones_dt_Modificado] DEFAULT (sysdatetimeoffset()) NULL,
    CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([id_Aplicacion] ASC)
);

