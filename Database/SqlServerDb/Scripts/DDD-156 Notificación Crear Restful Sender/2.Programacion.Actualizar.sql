PRINT N'Altering [Notificacion].[pr_EventosNotificaciones_Save]...';


GO
ALTER PROCEDURE Notificacion.[pr_EventosNotificaciones_Save]
	@uid_Notificacion      UNIQUEIDENTIFIER,
	@cd_NombreEstado       VARCHAR(50)     ,
	@ds_DescripcionEstado   VARCHAR(500)   ,
	@cd_NombreServicio     VARCHAR(50)     ,
	@ds_Error              VARCHAR(MAX)    ,
	@id_Usuario            INT             
AS BEGIN
	INSERT INTO Notificacion.EventosNotificaciones (
		uid_Notificacion    ,
		cd_NombreEstado     ,
		ds_DescripcionEstado,
		cd_NombreServicio   ,
		ds_Error            ,
		id_Creado,
		id_Modificado
	) VALUES (
		@uid_Notificacion    , 
		@cd_NombreEstado     , 
		@ds_DescripcionEstado,
		@cd_NombreServicio   , 
		@ds_Error            , 
		@id_Usuario          ,
		@id_Usuario
	)
END
GO
PRINT N'Altering [Notificacion].[pr_Notificaciones_Get]...';


GO
ALTER PROCEDURE Notificacion.[pr_Notificaciones_Get]
	@uid_Notificacion UNIQUEIDENTIFIER
AS BEGIN
	SELECT TOP 1
		[id_notificacion]     ,
		[uid_notificacion]    ,
		[nm_nombre_aplicacion]  ,
		[nm_from]             ,
		[ds_subject]          ,
		[ds_json_to]          ,
		[ds_json_propiedades]
	FROM Notificacion.Notificaciones n
	WHERE uid_notificacion = @uid_Notificacion

	SELECT 
		a.[nm_nombre],
		a.[bin_datos]
	FROM Notificacion.Adjuntos a
	WHERE uid_notificacion = @uid_Notificacion
END
GO
PRINT N'Altering [Notificacion].[pr_Notificaciones_Save]...';


GO
ALTER PROCEDURE [Notificacion].pr_Notificaciones_Save
	@nm_nombre_aplicacion VARCHAR(50)             ,
    @nm_from              VARCHAR(60)             ,
    @ds_subject           VARCHAR(200)            ,
    @ds_json_to           VARCHAR(MAX)            ,
    @ds_json_propiedades  VARCHAR(MAX)            ,
    @id_Usuario           INT
AS BEGIN
	
	DECLARE @uid_notificacion UNIQUEIDENTIFIER = NEWID();

	INSERT INTO [Notificacion].Notificaciones
	(
		uid_notificacion    ,
		nm_nombre_aplicacion,
		nm_from             ,
		ds_subject          ,
		ds_json_to          ,
		ds_json_propiedades ,
		id_Creado,
		id_Modificado
	)VALUES(
		@uid_notificacion    ,
		@nm_nombre_aplicacion,
		@nm_from             ,
		@ds_subject          ,
		@ds_json_to          ,
		@ds_json_propiedades ,
		@id_Usuario          ,
		@id_Usuario
	)
		
	SELECT @uid_notificacion
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
AS
BEGIN
	SET NOCOUNT ON;

    SELECT id_Aplicacion, ds_Titulo, ds_Descripcion
	FROM Notificacion.Aplicaciones
	ORDER BY ds_Titulo
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
    @id_Creado BIGINT
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
           ,@id_Creado
           ,SYSDATETIMEOFFSET()
           ,@id_Creado
           ,SYSDATETIMEOFFSET())
END
GO
PRINT N'Refreshing [Notificacion].[pr_NotificacionesConfiguracion_List]...';


GO
EXECUTE sp_refreshsqlmodule N'[Notificacion].[pr_NotificacionesConfiguracion_List]';