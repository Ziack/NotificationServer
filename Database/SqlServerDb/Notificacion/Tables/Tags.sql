CREATE TABLE [Notificacion].[Tags]
(
	[id_tag] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [uid_notificacion] UNIQUEIDENTIFIER NOT NULL, 
    [nm_tag] VARCHAR(100) NOT NULL, 
    [id_creado] INT NOT NULL, 
    [dt_creado] DATETIME2 NOT NULL DEFAULT ([dbo].[fn_GetLocalDate]()), 
    [id_modificado] INT NOT NULL, 
    [dt_modificado] DATETIME2 NOT NULL DEFAULT ([dbo].[fn_GetLocalDate]())
)
