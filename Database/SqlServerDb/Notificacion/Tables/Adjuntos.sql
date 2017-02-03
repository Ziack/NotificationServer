CREATE TABLE [Notificacion].[Adjuntos]
(
	[id_adjunto] BIGINT NOT NULL  IDENTITY(1, 1), 
    [uid_notificacion] UNIQUEIDENTIFIER NOT NULL, 
    [nm_nombre] VARCHAR(300) NULL, 
    [bin_datos] VARBINARY(MAX), 
	-- 
    [id_Creado] INT NOT NULL, 
    [dt_Creado] DATETIME2 NOT NULL CONSTRAINT [DF_Adjuntos_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()),
    [id_Modificado] INT NOT NULL, 
    [dt_Modificado] DATETIME2 NOT NULL CONSTRAINT [DF_Adjuntos_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()),
    PRIMARY KEY ([id_adjunto] DESC)
)
