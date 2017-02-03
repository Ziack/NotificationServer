ALTER TABLE [Notificacion].[Plantillas] DROP CONSTRAINT [DF_Plantilla_dt_creado];


GO
PRINT N'Dropping [Notificacion].[DF_Plantilla_dt_modificado]...';


GO
ALTER TABLE [Notificacion].[Plantillas] DROP CONSTRAINT [DF_Plantilla_dt_modificado];


GO
PRINT N'Dropping [Notificacion].[DF_ServiciosPorAplicacion_dt_Creado]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [DF_ServiciosPorAplicacion_dt_Creado];


GO
PRINT N'Dropping [Notificacion].[DF_ServiciosPorAplicacion_dt_Modificado]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [DF_ServiciosPorAplicacion_dt_Modificado];


GO
PRINT N'Dropping [Notificacion].[FK_ServiciosPorAplicacion_Aplicacion]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [FK_ServiciosPorAplicacion_Aplicacion];


GO
PRINT N'Dropping [Notificacion].[FK_ServiciosPorAplicacion_Servicios]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [FK_ServiciosPorAplicacion_Servicios];

GO
PRINT N'Starting rebuilding table [Notificacion].[Plantillas]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Notificacion].[tmp_ms_xx_Plantillas] (
    [id_plantilla]   INT           IDENTITY (1, 1) NOT NULL,
    [nm_nombre]      VARCHAR (200) NOT NULL,
    [ds_Descripcion] VARCHAR (200) NULL,
    [ds_contenido]   VARCHAR (MAX) NOT NULL,
    [id_Creado]      INT           NOT NULL,
    [dt_Creado]      DATETIME2 (7) CONSTRAINT [DF_Plantilla_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()) NOT NULL,
    [id_Modificado]  INT           NOT NULL,
    [dt_Modificado]  DATETIME2 (7) CONSTRAINT [DF_Plantilla_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()) NOT NULL,
    PRIMARY KEY CLUSTERED ([id_plantilla] ASC),
    CONSTRAINT [tmp_ms_xx_constraint_UQ_Plantillas_nm_nombre] UNIQUE NONCLUSTERED ([nm_nombre] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Notificacion].[Plantillas])
    BEGIN
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_Plantillas] ON;
        INSERT INTO [Notificacion].[tmp_ms_xx_Plantillas] ([id_plantilla], [nm_nombre], [ds_contenido], [id_Creado], [dt_Creado], [id_Modificado], [dt_Modificado])
        SELECT   [id_plantilla],
                 [nm_nombre],
                 [ds_contenido],
                 [id_Creado],
                 [dt_Creado],
                 [id_Modificado],
                 [dt_Modificado]
        FROM     [Notificacion].[Plantillas]
        ORDER BY [id_plantilla] ASC;
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_Plantillas] OFF;
    END

DROP TABLE [Notificacion].[Plantillas];

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_Plantillas]', N'Plantillas';

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_constraint_UQ_Plantillas_nm_nombre]', N'UQ_Plantillas_nm_nombre', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [Notificacion].[ServiciosPorAplicacion]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Notificacion].[tmp_ms_xx_ServiciosPorAplicacion] (
    [id_ServiciosPorAplicacion] INT                IDENTITY (1, 1) NOT NULL,
    [id_Applicacion]            INT                NOT NULL,
    [id_Servicio]               INT                NOT NULL,
    [nm_nombre]                 VARCHAR (200)      NOT NULL CONSTRAINT [DF_TEMP_ServiciosPorAplicacion_nm_nombre] DEFAULT(NEWID()),
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
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_ServiciosPorAplicacion] ON;
        INSERT INTO [Notificacion].[tmp_ms_xx_ServiciosPorAplicacion] ([id_ServiciosPorAplicacion], [id_Applicacion], [id_Servicio], [ds_Configuracion], [id_Creado], [dt_Creado], [id_Modificado], [dt_Modificado])
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
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_ServiciosPorAplicacion] OFF;
    END

DROP TABLE [Notificacion].[ServiciosPorAplicacion];

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_ServiciosPorAplicacion]', N'ServiciosPorAplicacion';

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_constraint_PK_ServicesPerApplication]', N'PK_ServicesPerApplication', N'OBJECT';

ALTER TABLE Notificacion.ServiciosPorAplicacion DROP CONSTRAINT [DF_TEMP_ServiciosPorAplicacion_nm_nombre]

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Notificacion].[FK_ServiciosPorAplicacion_Aplicacion]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] WITH NOCHECK
    ADD CONSTRAINT [FK_ServiciosPorAplicacion_Aplicacion] FOREIGN KEY ([id_Applicacion]) REFERENCES [Notificacion].[Aplicaciones] ([id_Aplicacion]);


GO
PRINT N'Creating [Notificacion].[FK_ServiciosPorAplicacion_Servicios]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] WITH NOCHECK
    ADD CONSTRAINT [FK_ServiciosPorAplicacion_Servicios] FOREIGN KEY ([id_Servicio]) REFERENCES [Notificacion].[Servicios] ([id_Servicio]);


GO
PRINT N'Update complete.';
GO