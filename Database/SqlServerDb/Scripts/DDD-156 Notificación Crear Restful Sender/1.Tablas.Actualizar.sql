DECLARE @Comando_Eliminar_Constraints NVARCHAR(MAX) = N''

;WITH cte_constraint_objects AS (
	SELECT 
		o.type,
		o.name,
		SCHEMA_NAME(o.schema_id) AS 'Schema',
		OBJECT_NAME(o.parent_object_id) AS 'Table'
	FROM sys.objects o
	WHERE type_desc LIKE '%CONSTRAINT'
) 
SELECT
	/*o.type,
	o.name, 
	o.[Schema],
	o.[Table],
	ISNULL(u.COLUMN_NAME, f.col_name) AS columnName,*/
	@Comando_Eliminar_Constraints = @Comando_Eliminar_Constraints  + N'ALTER TABLE ['+ o.[Schema] + N'].[' + o.[Table] + N'] DROP CONSTRAINT [' + o.name + N'];
'
FROM cte_constraint_objects o
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE u ON o.name = u.CONSTRAINT_NAME
LEFT JOIN (
	select c.name, col.name as 'col_name' from sys.default_constraints c
    inner join sys.columns col on col.default_object_id = c.object_id
    inner join sys.objects o  on o.object_id = c.parent_object_id
    inner join sys.schemas s on s.schema_id = o.schema_id
) f ON o.name = f.name
WHERE o.[Schema] = 'Notificacion'
AND o.[Table] IN ('EventosNotificaciones','Notificaciones', 'ServiciosPorAplicacion', 'Adjuntos', 'Plantillas','ServiciosPorAplicacion')
AND o.TYPE IN ('D', 'F')
AND ISNULL(u.COLUMN_NAME, f.col_name) IN ('dt_Creado', 'dt_Modificado','cd_Servicio', 'cd_Applicacion','uid_notificacion')
--ORDER BY [type]

PRINT 'Executing: 
' + @Comando_Eliminar_Constraints

EXEC sp_executesql @Comando_Eliminar_Constraints

/*
ALTER TABLE [Notificacion].[EventosNotificaciones] DROP CONSTRAINT [DF__EventosNo__dt_Cr__3B01A16B];
ALTER TABLE [Notificacion].[EventosNotificaciones] DROP CONSTRAINT [DF__EventosNo__dt_Mo__3BF5C5A4];
ALTER TABLE [Notificacion].[Notificaciones] DROP CONSTRAINT [DF__Notificac__uid_n__3CE9E9DD];
ALTER TABLE [Notificacion].[Notificaciones] DROP CONSTRAINT [DF__Notificac__dt_Cr__3DDE0E16];
ALTER TABLE [Notificacion].[Notificaciones] DROP CONSTRAINT [DF__Notificac__dt_Mo__3ED2324F];
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [DF_ServiciosPorAplicacion_dt_Creado];
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [DF_ServiciosPorAplicacion_dt_Modificado];
ALTER TABLE [Notificacion].[Adjuntos] DROP CONSTRAINT [DF__Adjuntos__dt_Cre__391958F9];
ALTER TABLE [Notificacion].[Adjuntos] DROP CONSTRAINT [DF__Adjuntos__dt_Mod__3A0D7D32];
ALTER TABLE [Notificacion].[Plantillas] DROP CONSTRAINT [DF__Plantilla__dt_Cr__3FC65688];
ALTER TABLE [Notificacion].[Plantillas] DROP CONSTRAINT [DF__Plantilla__dt_Mo__40BA7AC1];
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [FK_ServiciosPorAplicacion_Aplicacion];
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [FK_ServiciosPorAplicacion_Servicios];
*/



GO

GO
PRINT N'Altering [Notificacion]...';
PRINT N'Starting rebuilding table [Notificacion].[EventosNotificaciones]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Notificacion].[tmp_ms_xx_EventosNotificaciones] (
    [id_estadoNotificacion] BIGINT           IDENTITY (1, 1) NOT NULL,
    [uid_Notificacion]      UNIQUEIDENTIFIER NOT NULL,
    [cd_NombreEstado]       VARCHAR (50)     NOT NULL,
    [ds_DescripcionEstado]  VARCHAR (500)    NULL,
    [cd_NombreServicio]     VARCHAR (50)     NULL,
    [ds_Error]              VARCHAR (MAX)    NULL,
    [id_Creado]             INT              NOT NULL,
    [dt_Creado]             DATETIME2 (7)    CONSTRAINT [DF_EventosNotificaciones_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()) NOT NULL,
    [id_Modificado]         INT              NOT NULL,
    [dt_Modificado]         DATETIME2 (7)    CONSTRAINT [DF_EventosNotificaciones_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()) NOT NULL,
    PRIMARY KEY CLUSTERED ([id_estadoNotificacion] DESC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Notificacion].[EventosNotificaciones])
    BEGIN
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_EventosNotificaciones] ON;
        INSERT INTO [Notificacion].[tmp_ms_xx_EventosNotificaciones] ([id_estadoNotificacion], [uid_Notificacion], [cd_NombreEstado], [cd_NombreServicio], [ds_Error], [id_Creado], [dt_Creado], [id_Modificado], [dt_Modificado])
        SELECT   [id_estadoNotificacion],
                 [uid_Notificacion],
                 [cd_NombreEstado],
                 [cd_NombreServicio],
                 [ds_Error],
                 [id_Creado],
                 [dt_Creado],
                 [id_Modificado],
                 [dt_Modificado]
        FROM     [Notificacion].[EventosNotificaciones]
        ORDER BY [id_estadoNotificacion] DESC;
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_EventosNotificaciones] OFF;
    END

DROP TABLE [Notificacion].[EventosNotificaciones];

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_EventosNotificaciones]', N'EventosNotificaciones';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [Notificacion].[Notificaciones]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Notificacion].[tmp_ms_xx_Notificaciones] (
    [id_notificacion]      BIGINT           IDENTITY (1, 1) NOT NULL,
    [uid_notificacion]     UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL,
    [nm_nombre_aplicacion] VARCHAR (50)     NULL,
    [nm_from]              VARCHAR (60)     NOT NULL,
    [ds_subject]           VARCHAR (200)    NULL,
    [ds_json_to]           VARCHAR (MAX)    NULL,
    [ds_json_propiedades]  VARCHAR (MAX)    NULL,
    [id_Creado]            INT              NOT NULL,
    [dt_Creado]            DATETIME2 (7)    CONSTRAINT [DF_Notificaciones_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()) NOT NULL,
    [id_Modificado]        INT              NOT NULL,
    [dt_Modificado]        DATETIME2 (7)    CONSTRAINT [DF_Notificaciones_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_UQ_Notificaciones_uid_notificacion] UNIQUE NONCLUSTERED ([uid_notificacion] ASC),
    PRIMARY KEY CLUSTERED ([id_notificacion] DESC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Notificacion].[Notificaciones])
    BEGIN
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_Notificaciones] ON;
        INSERT INTO [Notificacion].[tmp_ms_xx_Notificaciones] ([id_notificacion], [uid_notificacion], [nm_from], [ds_subject], [ds_json_to], [ds_json_propiedades], [id_Creado], [dt_Creado], [id_Modificado], [dt_Modificado])
        SELECT   [id_notificacion],
                 [uid_notificacion],
                 [nm_from],
                 [ds_subject],
                 [ds_json_to],
                 [ds_json_propiedades],
                 [id_Creado],
                 [dt_Creado],
                 [id_Modificado],
                 [dt_Modificado]
        FROM     [Notificacion].[Notificaciones]
        ORDER BY [id_notificacion] DESC;
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_Notificaciones] OFF;
    END

DROP TABLE [Notificacion].[Notificaciones];

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_Notificaciones]', N'Notificaciones';

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_constraint_UQ_Notificaciones_uid_notificacion]', N'UQ_Notificaciones_uid_notificacion', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [Notificacion].[ServiciosPorAplicacion]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Notificacion].[tmp_ms_xx_ServiciosPorAplicacion] (
    [id_ServiciosPorAplicacion] INT                NOT NULL,
    [cd_Applicacion]            INT                NOT NULL,
    [cd_Servicio]               INT                NOT NULL,
    [ds_Configuracion]          VARCHAR (MAX)      NULL,
    [id_Creado]                 BIGINT             NULL,
    [dt_Creado]                 DATETIMEOFFSET (7) CONSTRAINT [DF_ServiciosPorAplicacion_dt_Creado] DEFAULT (sysdatetimeoffset()) NULL,
    [id_Modificado]             BIGINT             NULL,
    [dt_Modificado]             DATETIMEOFFSET (7) CONSTRAINT [DF_ServiciosPorAplicacion_dt_Modificado] DEFAULT (sysdatetimeoffset()) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_ServicesPerApplication] PRIMARY KEY CLUSTERED ([id_ServiciosPorAplicacion] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Notificacion].[ServiciosPorAplicacion])
    BEGIN
        INSERT INTO [Notificacion].[tmp_ms_xx_ServiciosPorAplicacion] ([id_ServiciosPorAplicacion], [cd_Applicacion], [cd_Servicio], [ds_Configuracion], [id_Creado], [dt_Creado], [id_Modificado], [dt_Modificado])
        SELECT   [id_ServiciosPorAplicacion],
                 [cd_Applicacion],
                 [cd_Servicio],
                 [ds_Configuracion],
                 [id_Creado],
                 [dt_Creado],
                 [id_Modificado],
                 [dt_Modificado]
        FROM     [Notificacion].[ServiciosPorAplicacion]
        ORDER BY [id_ServiciosPorAplicacion] ASC;
    END

DROP TABLE [Notificacion].[ServiciosPorAplicacion];

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_ServiciosPorAplicacion]', N'ServiciosPorAplicacion';

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_constraint_PK_ServicesPerApplication]', N'PK_ServicesPerApplication', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Notificacion].[DF_Adjuntos_dt_creado]...';


GO
ALTER TABLE [Notificacion].[Adjuntos]
    ADD CONSTRAINT [DF_Adjuntos_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()) FOR [dt_Creado];


GO
PRINT N'Creating [Notificacion].[DF_Adjuntos_dt_modificado]...';


GO
ALTER TABLE [Notificacion].[Adjuntos]
    ADD CONSTRAINT [DF_Adjuntos_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()) FOR [dt_Modificado];


GO
PRINT N'Creating [Notificacion].[DF_Plantilla_dt_creado]...';


GO
ALTER TABLE [Notificacion].[Plantillas]
    ADD CONSTRAINT [DF_Plantilla_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()) FOR [dt_Creado];


GO
PRINT N'Creating [Notificacion].[DF_Plantilla_dt_modificado]...';


GO
ALTER TABLE [Notificacion].[Plantillas]
    ADD CONSTRAINT [DF_Plantilla_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()) FOR [dt_Modificado];


GO
PRINT N'Creating [Notificacion].[FK_ServiciosPorAplicacion_Aplicacion]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] WITH NOCHECK
    ADD CONSTRAINT [FK_ServiciosPorAplicacion_Aplicacion] FOREIGN KEY ([cd_Applicacion]) REFERENCES [Notificacion].[Aplicaciones] ([id_Aplicacion]);


GO
PRINT N'Creating [Notificacion].[FK_ServiciosPorAplicacion_Servicios]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] WITH NOCHECK
    ADD CONSTRAINT [FK_ServiciosPorAplicacion_Servicios] FOREIGN KEY ([cd_Servicio]) REFERENCES [Notificacion].[Servicios] ([id_Servicio]);


GO