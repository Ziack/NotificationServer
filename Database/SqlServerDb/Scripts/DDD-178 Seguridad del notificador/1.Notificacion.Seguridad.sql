PRINT N'Dropping [Notificacion].[DF_Applicaciones_dt_Creado]...';


GO
ALTER TABLE [Notificacion].[Aplicaciones] DROP CONSTRAINT [DF_Applicaciones_dt_Creado];


GO
PRINT N'Dropping [Notificacion].[DF_Applicaciones_dt_Modificado]...';


GO
ALTER TABLE [Notificacion].[Aplicaciones] DROP CONSTRAINT [DF_Applicaciones_dt_Modificado];


GO
PRINT N'Dropping [Notificacion].[FK_ServiciosPorAplicacion_Aplicacion]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] DROP CONSTRAINT [FK_ServiciosPorAplicacion_Aplicacion];


GO
PRINT N'Starting rebuilding table [Notificacion].[Aplicaciones]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Notificacion].[tmp_ms_xx_Aplicaciones] (
    [id_Aplicacion]  INT                IDENTITY (1, 1) NOT NULL,
    [ds_Titulo]      VARCHAR (50)       NOT NULL,
    [ds_Descripcion] VARCHAR (200)      NOT NULL,
    [id_Creado]      BIGINT             NULL,
    [ds_Token]       NVARCHAR (MAX)     NULL,
    [dt_Creado]      DATETIMEOFFSET (7) CONSTRAINT [DF_Applicaciones_dt_Creado] DEFAULT (sysdatetimeoffset()) NULL,
    [id_Modificado]  BIGINT             NULL,
    [dt_Modificado]  DATETIMEOFFSET (7) CONSTRAINT [DF_Applicaciones_dt_Modificado] DEFAULT (sysdatetimeoffset()) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Applications] PRIMARY KEY CLUSTERED ([id_Aplicacion] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Notificacion].[Aplicaciones])
    BEGIN
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_Aplicaciones] ON;
        INSERT INTO [Notificacion].[tmp_ms_xx_Aplicaciones] ([id_Aplicacion], [ds_Titulo], [ds_Descripcion], [id_Creado], [dt_Creado], [id_Modificado], [dt_Modificado])
        SELECT   [id_Aplicacion],
                 [ds_Titulo],
                 [ds_Descripcion],
                 [id_Creado],
                 [dt_Creado],
                 [id_Modificado],
                 [dt_Modificado]
        FROM     [Notificacion].[Aplicaciones]
        ORDER BY [id_Aplicacion] ASC;
        SET IDENTITY_INSERT [Notificacion].[tmp_ms_xx_Aplicaciones] OFF;
    END

DROP TABLE [Notificacion].[Aplicaciones];

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_Aplicaciones]', N'Aplicaciones';

EXECUTE sp_rename N'[Notificacion].[tmp_ms_xx_constraint_PK_Applications]', N'PK_Applications', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Notificacion].[TipoNotificacion]...';


GO
CREATE TABLE [Notificacion].[TipoNotificacion] (
    [id_TipoNotificacion] INT                IDENTITY (1, 1) NOT NULL,
    [cd_TipoNotificacion] VARCHAR (200)      NOT NULL,
    [nm_Plantilla]        VARCHAR (200)      NOT NULL,
    [nm_Servicio]         VARCHAR (50)       NOT NULL,
    [id_Creado]           BIGINT             NOT NULL,
    [dt_Creado]           DATETIMEOFFSET (7) NOT NULL,
    [id_Modificado]       BIGINT             NOT NULL,
    [dt_Modificado]       DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_TipoNotificacion] PRIMARY KEY CLUSTERED ([id_TipoNotificacion] ASC)
);


GO
PRINT N'Creating [Notificacion].[Usuarios]...';


GO
CREATE TABLE [Notificacion].[Usuarios] (
    [usuario_id]    BIGINT             IDENTITY (1, 1) NOT NULL,
    [nombre_cd]     VARCHAR (50)       NOT NULL,
    [clave_cd]      NVARCHAR (200)     NOT NULL,
    [uid_cd]        UNIQUEIDENTIFIER   NOT NULL,
    [id_Creado]     BIGINT             NOT NULL,
    [dt_Creado]     DATETIMEOFFSET (7) NOT NULL,
    [id_Modificado] BIGINT             NOT NULL,
    [dt_Modificado] DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([usuario_id] DESC),
    CONSTRAINT [UQ_Usuarios_nombre_cd] UNIQUE NONCLUSTERED ([nombre_cd] ASC),
    CONSTRAINT [UQ_Usuarios_uid] UNIQUE NONCLUSTERED ([uid_cd] ASC)
);


GO
PRINT N'Creating [Notificacion].[DF_TipoNotificacion_dt_Creado]...';


GO
ALTER TABLE [Notificacion].[TipoNotificacion]
    ADD CONSTRAINT [DF_TipoNotificacion_dt_Creado] DEFAULT (sysdatetimeoffset()) FOR [dt_Creado];


GO
PRINT N'Creating [Notificacion].[DF_TipoNotificacion_dt_Modificado]...';


GO
ALTER TABLE [Notificacion].[TipoNotificacion]
    ADD CONSTRAINT [DF_TipoNotificacion_dt_Modificado] DEFAULT (sysdatetimeoffset()) FOR [dt_Modificado];


GO
PRINT N'Creating [Notificacion].[DF_Notificacion_Usuarios_dt_Creado]...';


GO
ALTER TABLE [Notificacion].[Usuarios]
    ADD CONSTRAINT [DF_Notificacion_Usuarios_dt_Creado] DEFAULT (sysdatetimeoffset()) FOR [dt_Creado];


GO
PRINT N'Creating [Notificacion].[DF_Notificacion_Usuarios_dt_Modificado]...';


GO
ALTER TABLE [Notificacion].[Usuarios]
    ADD CONSTRAINT [DF_Notificacion_Usuarios_dt_Modificado] DEFAULT (sysdatetimeoffset()) FOR [dt_Modificado];


GO
PRINT N'Creating [Notificacion].[FK_ServiciosPorAplicacion_Aplicacion]...';


GO
ALTER TABLE [Notificacion].[ServiciosPorAplicacion] WITH NOCHECK
    ADD CONSTRAINT [FK_ServiciosPorAplicacion_Aplicacion] FOREIGN KEY ([id_Applicacion]) REFERENCES [Notificacion].[Aplicaciones] ([id_Aplicacion]);


GO
PRINT N'Altering [Notificacion].[pr_ServiciosPorAplicacion_Configurar]...';


GO
/******************************************************************************
** Descripción: Configura una instancia de servicio para una palicación
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-22	Jeysson Guevara     Creación del procedimiento
*******************************************************************************/
ALTER PROCEDURE [Notificacion].[pr_ServiciosPorAplicacion_Configurar]
	@nm_nombreAplicacion VARCHAR(50),
	@nm_nombreServicio VARCHAR(50),
	@nm_nombreServicioParaLaAplicacion VARCHAR(200),
	@ds_Configuracion VARCHAR(MAX),
	@id_Usuario INT
AS BEGIN
	DECLARE 
		@id_Aplicacion INT,
		@id_Servicio INT

	SELECT TOP 1 @id_Aplicacion = a.id_Aplicacion FROM Notificacion.Aplicaciones a WHERE a.ds_Titulo = @nm_nombreAplicacion
	SELECT TOP 1 @id_Servicio = s.id_Servicio FROM Notificacion.Servicios s WHERE s.nm_Nombre = @nm_nombreServicio

	IF NOT EXISTS (
		SELECT TOP 1 1 
		FROM Notificacion.ServiciosPorAplicacion s 
		WHERE 
			s.nm_nombre = @nm_nombreServicioParaLaAplicacion 
			AND s.id_Applicacion = @id_Aplicacion
	) BEGIN
		INSERT INTO Notificacion.ServiciosPorAplicacion (
			id_Applicacion,
			id_Servicio,
			nm_nombre,
			ds_Configuracion,
			id_Creado,
			id_Modificado
		) VALUES (
			@id_Aplicacion,
			@id_Servicio,
			@nm_nombreServicioParaLaAplicacion,
			@ds_Configuracion,
			@id_Usuario,
			@id_Usuario
		)
	END ELSE BEGIN 
		UPDATE Notificacion.ServiciosPorAplicacion SET
			id_Servicio = @id_Servicio,
			ds_Configuracion = @ds_Configuracion,
			id_Modificado = @id_Usuario,
			dt_Modificado = dbo.fn_GetLocalDate()
		WHERE 
			nm_nombre = @nm_nombreServicioParaLaAplicacion 
			AND id_Applicacion = @id_Aplicacion
	END
END
GO
PRINT N'Altering [Notificacion].[pr_Aplicacion_Select]...';


GO
/******************************************************************************
** Descripción: Selecciona las aplicaciones registradas
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-13	César Aguirre		Creación del procedimiento
*******************************************************************************/
ALTER PROCEDURE [Notificacion].[pr_Aplicacion_Select]
	@iInt_numPagina INT = NULL
	,@iInt_tamPagina INT = NULL
AS
BEGIN
	SET NOCOUNT ON;	

	IF (@iInt_numPagina IS NULL AND @iInt_tamPagina IS NULL)
	BEGIN
		SELECT 
			id_Aplicacion, 
			ds_Titulo, 
			ds_Descripcion, 
			num_TotalFilas = COUNT(1) OVER(),
			ds_Token
		FROM Notificacion.Aplicaciones
		ORDER BY ds_Titulo
	END
	ELSE
	BEGIN
		SELECT 
			id_Aplicacion, 
			ds_Titulo, 
			ds_Descripcion, 
			num_TotalFilas = COUNT(1) OVER(),
			ds_Token
		FROM Notificacion.Aplicaciones
		ORDER BY dt_Creado DESC
		OFFSET (@iInt_numPagina-1)*ISNULL(@iInt_tamPagina, 1) ROWS 
		FETCH NEXT ISNULL(@iInt_tamPagina, @@ROWCOUNT) ROWS ONLY 
	END

END
GO
PRINT N'Altering [Notificacion].[pr_NotificacionesConfiguracion_List]...';


GO
/******************************************************************************
** Descripción: Selecciona las configuraciones de los servicios dados disponibles 
** para una aplicación
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-22   Jeysson Guevara     Buscar por nombre del ServicioPorAplicacion en vez de por el nombre del servicio.
*******************************************************************************/
ALTER PROCEDURE [Notificacion].[pr_NotificacionesConfiguracion_List]
	@ds_nombreAplicacion VARCHAR(50),
	@ds_Servicios VARCHAR(500)
AS BEGIN

	DECLARE	@id_aplicacion AS INT

	SELECT @id_aplicacion = a.id_Aplicacion
	FROM Notificacion.Aplicaciones a
	WHERE a.ds_Titulo = @ds_nombreAplicacion

	SELECT 
		sa.id_servicio		   ,
		sa.nm_Nombre           ,
		s.[ds_Host]            ,
		s.[nm_Puerto]          ,
		s.[ds_Usuario]         ,
		s.[ds_Password]        ,
		s.[ds_Configuracion] AS  ds_ConfiguracionBase, 
		sa.[ds_Configuracion] AS ds_ConfiguracionSobreescrita
	FROM dbo.fn_SplitString(@ds_Servicios, ',')
	JOIN Notificacion.ServiciosPorAplicacion sa ON sa.nm_Nombre = valor
	INNER JOIN Notificacion.Servicios s ON s.id_Servicio = sa.id_Servicio
	WHERE sa.id_Applicacion = @id_aplicacion
END
GO
PRINT N'Altering [Notificacion].[pr_Servicio_Save]...';


GO
/******************************************************************************
** Descripción: Servicio Save.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-08	César Aguirre		Creación del procedimiento
*******************************************************************************/
ALTER PROCEDURE [Notificacion].[pr_Servicio_Save]
	@nm_Nombre VARCHAR(50),
	@ds_Descripcion VARCHAR(200),
    @ds_Host VARCHAR(500),
    @nm_Puerto INT,
    @ds_Usuario VARCHAR(50),
    @ds_Password VARCHAR(50),
    @id_Creado BIGINT,
	@ds_Configuracion VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1
				FROM [Notificacion].[Servicios]
				WHERE nm_Nombre = @nm_Nombre)
	BEGIN
		DECLARE @id_ErrorNumber VARCHAR(10);
		SET @id_ErrorNumber = 'M50002';
		RAISERROR (@id_ErrorNumber, 16, 2)
		RETURN
	END
	INSERT INTO [Notificacion].[Servicios]
           ([nm_Nombre]
           ,[ds_Descripcion]
           ,[ds_Host]
           ,[nm_Puerto]
           ,[ds_Usuario]
           ,[ds_Password]
		   ,[ds_Configuracion]
           ,[id_Creado]
           ,[dt_Creado]
           ,[id_Modificado]
           ,[dt_Modificado])
     VALUES
           (@nm_Nombre
           ,@ds_Descripcion
           ,@ds_Host
           ,@nm_Puerto
           ,@ds_Usuario
           ,@ds_Password
		   ,@ds_Configuracion
           ,@id_Creado
           ,SYSDATETIMEOFFSET()
           ,@id_Creado
           ,SYSDATETIMEOFFSET())
END
GO
PRINT N'Altering [Notificacion].[pr_Servicio_Select]...';


GO
/******************************************************************************
** Descripción: Selecciona los servicios registrados
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-13	César Aguirre		Creación del procedimiento
*******************************************************************************/
ALTER PROCEDURE [Notificacion].[pr_Servicio_Select]
	@iInt_numPagina INT = NULL,
	@iInt_tamPagina INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (@iInt_numPagina IS NULL AND @iInt_tamPagina IS NULL)
	BEGIN
		SELECT id_Servicio, nm_Nombre, ds_Descripcion, ds_Host, nm_Puerto, ds_Usuario, ds_Password, num_TotalFilas = COUNT(1) OVER()
		FROM Notificacion.Servicios
		ORDER BY nm_Nombre
	END
	ELSE
	BEGIN
		SELECT id_Servicio, nm_Nombre, ds_Descripcion, ds_Host, nm_Puerto, ds_Usuario, ds_Password, num_TotalFilas = COUNT(1) OVER()
		FROM Notificacion.Servicios
		ORDER BY dt_Creado DESC
		OFFSET (@iInt_numPagina-1)*ISNULL(@iInt_tamPagina, 1) ROWS 
		FETCH NEXT ISNULL(@iInt_tamPagina, @@ROWCOUNT) ROWS ONLY 
	END
END
GO
PRINT N'Creating [Notificacion].[pr_Aplicacion_SetToken]...';


GO
CREATE PROCEDURE Notificacion.[pr_Aplicacion_SetToken]
	@ds_Titulo      VARCHAR (50), 
	@ds_Token       NVARCHAR(MAX)
AS BEGIN
	UPDATE Notificacion.Aplicaciones
	SET ds_Token = @ds_Token,
		id_Modificado = 1,
		dt_Modificado = dbo.fn_GetLocalDate()
	WHERE ds_Titulo = @ds_Titulo

END
GO
PRINT N'Creating [Notificacion].[pr_TipoNotificacion_Get]...';


GO
/******************************************************************************
** Descripción: 
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-02-17	Cesar Aguirre		Creación del procedimiento
*******************************************************************************/
CREATE PROCEDURE [Notificacion].[pr_TipoNotificacion_Get]
	@cd_TipoNotificacion VARCHAR(200)
AS
BEGIN
	SELECT id_TipoNotificacion, cd_TipoNotificacion, nm_Plantilla, nm_Servicio
	FROM Notificacion.TipoNotificacion
	WHERE (cd_TipoNotificacion = @cd_TipoNotificacion)
END
GO
PRINT N'Creating [Notificacion].[pr_Usuarios_CambiarClave]...';


GO
CREATE PROCEDURE Notificacion.[pr_Usuarios_CambiarClave]
	@nombre_cd  VARCHAR(50),
	@clave_cd   NVARCHAR(200)
AS BEGIN

	UPDATE Notificacion.Usuarios
	SET clave_cd = @clave_cd
	WHERE nombre_cd = @nombre_cd

END
GO
PRINT N'Creating [Notificacion].[pr_Usuarios_Get]...';


GO
CREATE PROCEDURE Notificacion.[pr_Usuarios_Get]
	@nombre_cd  VARCHAR(50),
	@clave_cd   NVARCHAR(200),
	@uid_cd     UNIQUEIDENTIFIER
AS BEGIN
	
	IF @nombre_cd IS NULL
		SELECT TOP 1 
			u.nombre_cd,
			u.clave_cd,
			u.uid_cd,
			u.usuario_id
		FROM Notificacion.Usuarios u
		WHERE u.uid_cd = @uid_cd
	ELSE IF @clave_cd IS NULL
		SELECT TOP 1 
			u.nombre_cd,
			u.clave_cd,
			u.uid_cd,
			u.usuario_id
		FROM Notificacion.Usuarios u
		WHERE u.nombre_cd = @nombre_cd
	ELSE 
		SELECT TOP 1 
			u.nombre_cd,
			u.clave_cd,
			u.uid_cd,
			u.usuario_id
		FROM Notificacion.Usuarios u
		WHERE 
			u.nombre_cd = @nombre_cd
			AND u.clave_cd = @clave_cd

END
GO
PRINT N'Creating [Notificacion].[pr_Usuarios_Insert]...';


GO
CREATE PROCEDURE Notificacion.[pr_Usuarios_Insert] 
    @nombre_cd  VARCHAR(50),
    @clave_cd   NVARCHAR(200),
    @uid_cd     UNIQUEIDENTIFIER,
	@id_UsuarioCreador BIGINT
AS BEGIN
	INSERT INTO Notificacion.Usuarios (
		nombre_cd,
		clave_cd ,
		uid_cd   ,
		id_Creado,
		id_Modificado
	) VALUES (
		@nombre_cd,
		@clave_cd,
		@uid_cd,
		@id_UsuarioCreador,
		@id_UsuarioCreador
	)
END
GO
PRINT N'Refreshing [Notificacion].[pr_Aplicacion_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[Notificacion].[pr_Aplicacion_Save]';


GO
PRINT N'Refreshing [Notificacion].[pr_ServiciosPorAplicacion_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[Notificacion].[pr_ServiciosPorAplicacion_Select]';


GO