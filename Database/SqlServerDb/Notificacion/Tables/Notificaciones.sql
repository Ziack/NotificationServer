CREATE TABLE [Notificacion].[Notificaciones]
(
	[id_notificacion] BIGINT NOT NULL  IDENTITY(1, 1), 
    [uid_notificacion] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    
	[nm_nombre_aplicacion] VARCHAR(50) NULL, 
	
    [nm_from] VARCHAR(60) NOT NULL, 
    [ds_subject] VARCHAR(200) NULL, 
    [ds_json_to] VARCHAR(MAX) NULL, 
	
    [ds_json_propiedades] VARCHAR(MAX) NULL, 

    [id_Creado] INT NOT NULL, 
    [dt_Creado] DATETIME2 NOT NULL CONSTRAINT [DF_Notificaciones_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()),
    [id_Modificado] INT NOT NULL, 
    [dt_Modificado] DATETIME2 NOT NULL CONSTRAINT [DF_Notificaciones_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()),
    CONSTRAINT [UQ_Notificaciones_uid_notificacion] UNIQUE (uid_notificacion), 
    PRIMARY KEY ([id_notificacion] DESC)
)
