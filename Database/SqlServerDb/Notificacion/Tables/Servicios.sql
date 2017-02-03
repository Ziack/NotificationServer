CREATE TABLE [Notificacion].[Servicios] (
    [id_Servicio]      INT                IDENTITY (1, 1) NOT NULL,
    [nm_Nombre]        VARCHAR (50)       NOT NULL,
    [ds_Descripcion]   VARCHAR (200)      NULL,
    [ds_Host]          VARCHAR (500)      NOT NULL,
    [nm_Puerto]        INT                NOT NULL,
    [ds_Usuario]       VARCHAR (50)       NOT NULL,
    [ds_Password]      VARCHAR (50)       NOT NULL,
    [ds_Configuracion] VARCHAR (MAX)      NULL,
    --
	[id_Creado]        BIGINT             NULL,
    [dt_Creado]        DATETIMEOFFSET (7) CONSTRAINT [DF_Servicios_dt_Creado] DEFAULT (sysdatetimeoffset()) NULL,
    [id_Modificado]    BIGINT             NULL,
    [dt_Modificado]    DATETIMEOFFSET (7) CONSTRAINT [DF_Servicios_dt_Modificado] DEFAULT (sysdatetimeoffset()) NULL,
    CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([id_Servicio] ASC)
);

