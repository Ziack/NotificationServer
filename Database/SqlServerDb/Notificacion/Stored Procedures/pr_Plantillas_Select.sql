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