CREATE TABLE [Notificacion].[TipoNotificacion] (
    [id_TipoNotificacion] INT                IDENTITY (1, 1) NOT NULL,
    [cd_TipoNotificacion] VARCHAR (200)      NOT NULL,
    [nm_Plantilla]        VARCHAR (200)      NOT NULL,
    [nm_Servicio]         VARCHAR (50)       NOT NULL,
    [id_Creado]           BIGINT             NOT NULL,
    [dt_Creado]           DATETIMEOFFSET (7) CONSTRAINT [DF_TipoNotificacion_dt_Creado] DEFAULT (sysdatetimeoffset()) NOT NULL,
    [id_Modificado]       BIGINT             NOT NULL,
    [dt_Modificado]       DATETIMEOFFSET (7) CONSTRAINT [DF_TipoNotificacion_dt_Modificado] DEFAULT (sysdatetimeoffset()) NOT NULL,
    CONSTRAINT [PK_TipoNotificacion] PRIMARY KEY CLUSTERED ([id_TipoNotificacion] ASC)
);

