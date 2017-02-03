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
ALTER PROCEDURE Notificacion.[pr_NotificacionesConfiguracion_List]
	@ds_nombreAplicacion VARCHAR(50),
	@ds_Servicios VARCHAR(500)
AS BEGIN

	DECLARE @tblServicios AS TABLE(
		id_servicio        INT,
		nm_Nombre          varchar(50),
		[ds_Host]          VARCHAR (500),
		[nm_Puerto]        INT          ,
		[ds_Usuario]       VARCHAR (50) ,
		[ds_Password]      VARCHAR (50) ,
		[ds_Configuracion] VARCHAR (MAX)
	);
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
PRINT N'Altering [Notificacion].[pr_ServiciosPorAplicacion_Save]...';


GO
/******************************************************************************
** Descripción: Registra el listado de servicios a una aplicacion
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-13	César Aguirre		Creación del procedimiento
** 2016-01-22	Jeysson Guevara     Declaración de obsolescencia del procedimiento.
*******************************************************************************/
/*[Obsolete("Hubo cambios drásticos en la forma como se manejan los ServiciosPorAplicacion y este procedimiento no puede lidiar con dichos cambios")]*/
ALTER PROCEDURE [Notificacion].[pr_ServiciosPorAplicacion_Save]
	@id_Aplicacion INT,
	@ds_ServiciosId VARCHAR(MAX),
	@id_Creado BIGINT
AS
BEGIN
	SET NOCOUNT ON;


DECLARE @ServiciosTemp TABLE(id_Aplicacion INT, id_Servicio INT);

INSERT INTO @ServiciosTemp (id_Aplicacion,id_Servicio)
SELECT @id_Aplicacion, * FROM fn_SplitString(@ds_ServiciosId, ',')

MERGE INTO Notificacion.ServiciosPorAplicacion AS T
USING @ServiciosTemp AS S
ON T.id_Applicacion = S.id_Aplicacion AND T.id_Servicio = S.id_Servicio
WHEN NOT MATCHED BY Source AND T.id_Applicacion = @id_Aplicacion THEN
DELETE
WHEN NOT MATCHED BY Target THEN
INSERT (id_Applicacion,id_Servicio,ds_Configuracion,id_Creado,dt_Creado,id_Modificado,dt_Modificado)
	   VALUES (@id_Aplicacion,S.id_Servicio,'{}',@id_Creado,SYSDATETIMEOFFSET(),@id_Creado,SYSDATETIMEOFFSET());
END
GO
PRINT N'Altering [Notificacion].[pr_ServiciosPorAplicacion_Select]...';


GO
/******************************************************************************
** Descripción: Selecciona los servicios registrados por las aplicaciones
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-13	César Aguirre		Creación del procedimiento
** 2016-01-22   Jeysson Guevara     Obtención de los nuevos campos de la tabla ServiciosPorAplicacion.
*******************************************************************************/
ALTER PROCEDURE [Notificacion].[pr_ServiciosPorAplicacion_Select]
	@id_Aplicacion INT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		s.id_Servicio, 
		s.nm_Nombre as nm_nombreServicio, 
		s.ds_Descripcion,
		sa.id_ServiciosPorAplicacion,
		sa.id_Applicacion as cd_Applicacion,
		sa.nm_nombre
	FROM Notificacion.Aplicaciones a
		INNER JOIN Notificacion.ServiciosPorAplicacion sa ON a.id_Aplicacion = sa.id_Applicacion
		INNER JOIN Notificacion.Servicios s ON sa.id_Servicio = s.id_Servicio
	WHERE 
		a.id_Aplicacion = @id_Aplicacion
	ORDER BY s.nm_Nombre
END
GO
PRINT N'Creating [Notificacion].[pr_Plantillas_Delete]...';


GO
/******************************************************************************
** Descripción: Consulta de Planillas.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-14	María Zabaleta		Creación del procedimiento
*******************************************************************************/

CREATE PROCEDURE [Notificacion].[pr_Plantillas_Delete]
	 @id_plantilla INT = NULL

AS

BEGIN
	DECLARE @id_ErrorNumber INT

	BEGIN TRY
		SET @id_ErrorNumber = 51093;

		DELETE FROM [Notificacion].[Plantillas]
		WHERE id_plantilla = @id_plantilla;
	END TRY

	BEGIN CATCH
		IF ( @id_ErrorNumber = 51093 )
		BEGIN
			RAISERROR (@id_ErrorNumber, 16, 2)
			RETURN
		END
	END CATCH
END
GO
PRINT N'Creating [Notificacion].[pr_Plantillas_Save]...';


GO
/******************************************************************************
** Descripción: Consulta de Planillas.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-14	María Zabaleta		Creación del procedimiento
*******************************************************************************/

CREATE PROCEDURE [Notificacion].[pr_Plantillas_Save]
	 @id_plantilla INT = NULL
    ,@nm_nombre VARCHAR(200) = NULL
	,@ds_descripcion VARCHAR(200) = NULL
	,@ds_contenido VARCHAR(MAX) = NULL
	,@dt_Creado DATETIME2(7) = NULL
	,@id_Creado INT = NULL
	,@dt_Modificado DATETIME2(7) = NULL
	,@id_Modificado INT = NULL

AS

BEGIN
	IF (@id_plantilla <> 0)
	BEGIN
		UPDATE [Notificacion].[Plantillas]
		SET	nm_nombre = @nm_nombre
			,ds_descripcion = @ds_descripcion
			,ds_contenido = @ds_contenido
			,dt_Modificado = @dt_Modificado
			,id_Modificado = @id_Modificado
		WHERE id_plantilla = @id_plantilla;
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT 1
				FROM [Notificacion].[Plantillas]
				WHERE nm_Nombre = @nm_Nombre)
		BEGIN
			DECLARE @id_ErrorNumber VARCHAR(10);
			SET @id_ErrorNumber = 'M50003';
			RAISERROR (@id_ErrorNumber, 16, 2)
			RETURN
		END

		INSERT INTO [Notificacion].[Plantillas](
			nm_nombre
			,ds_descripcion
			,ds_contenido
			,dt_Creado
			,id_Creado
			,dt_Modificado
			,id_Modificado
		) 
		VALUES( 
			@nm_nombre
			,@ds_descripcion
			,@ds_contenido
			,@dt_Creado
			,@id_Creado
			,@dt_Modificado
			,@id_Modificado
		)
	END
END
GO
PRINT N'Creating [Notificacion].[pr_Plantillas_Select]...';


GO
/******************************************************************************
** Descripción: Consulta de Planillas.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-14	María Zabaleta		Creación del procedimiento
*******************************************************************************/

CREATE PROCEDURE [Notificacion].[pr_Plantillas_Select]
	 @id_Plantilla INT = NULL
	,@iInt_numPagina INT = NULL
	,@iInt_tamPagina INT = NULL
AS

BEGIN	
	
	IF (@iInt_numPagina IS NULL)
	BEGIN
		SET @iInt_numPagina = 1
	END

	IF (@iInt_tamPagina IS NULL)
	BEGIN
		SET @iInt_tamPagina = 10
	END

	SELECT
	     P.id_plantilla as id_plantilla 
		,P.nm_nombre as nm_nombre
		,P.ds_descripcion as ds_descripcion
		,P.ds_contenido as ds_contenido
		,P.dt_Creado as dt_Creado
		,P.id_Creado as id_Creado
		,P.dt_Modificado as dt_Modificado
		,P.id_Modificado as id_Modificado
		,num_TotalFilas = COUNT(1) OVER()
	FROM [Notificacion].[Plantillas] as P
	WHERE (@id_Plantilla IS NULL OR @id_Plantilla = P.id_plantilla)		
	ORDER BY P.dt_Creado DESC
	OFFSET (@iInt_numPagina-1)*ISNULL(@iInt_tamPagina, 1) ROWS 
	FETCH NEXT ISNULL(@iInt_tamPagina, @@ROWCOUNT) ROWS ONLY 

END 
RETURN
GO
PRINT N'Creating [Notificacion].[pr_Servicio_Select]...';


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
CREATE PROCEDURE [Notificacion].[pr_Servicio_Select]
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
		ORDER BY nm_Nombre
		OFFSET (@iInt_numPagina-1)*ISNULL(@iInt_tamPagina, 1) ROWS 
		FETCH NEXT ISNULL(@iInt_tamPagina, @@ROWCOUNT) ROWS ONLY 
	END
END
GO
PRINT N'Creating [Notificacion].[pr_ServiciosPorAplicacion_Configurar]...';


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
CREATE PROCEDURE Notificacion.[pr_ServiciosPorAplicacion_Configurar]
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
			s.nm_nombre = @nm_nombreAplicacion 
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
			nm_nombre = @nm_nombreAplicacion 
			AND id_Applicacion = @id_Aplicacion
	END
END
GO
PRINT N'Refreshing [Notificacion].[pr_Plantillas_Get]...';


GO
EXECUTE sp_refreshsqlmodule N'[Notificacion].[pr_Plantillas_Get]';


PRINT N'Update complete.';
GO
