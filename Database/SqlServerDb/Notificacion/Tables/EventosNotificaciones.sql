CREATE TABLE Notificacion.[EventosNotificaciones]
(
	id_estadoNotificacion BIGINT NOT NULL  IDENTITY, 
	[uid_Notificacion] UNIQUEIDENTIFIER NOT NULL, 
	[cd_NombreEstado] VARCHAR(50) NOT NULL,
	[ds_DescripcionEstado] VARCHAR(500) NULL,
	[cd_NombreServicio] VARCHAR(50) NULL, 
	[ds_Error] VARCHAR(MAX) NULL, 
	--
	[id_Creado] INT NOT NULL, 
	[dt_Creado] DATETIME2 NOT NULL CONSTRAINT [DF_EventosNotificaciones_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()),
    [id_Modificado] INT NOT NULL, 
    [dt_Modificado] DATETIME2 NOT NULL CONSTRAINT [DF_EventosNotificaciones_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()),
	PRIMARY KEY ([id_estadoNotificacion] DESC)
)
