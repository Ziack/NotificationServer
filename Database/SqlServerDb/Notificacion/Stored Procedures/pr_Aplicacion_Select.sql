/******************************************************************************
** Descripción: Selecciona las aplicaciones registradas
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-13	César Aguirre		Creación del procedimiento
*******************************************************************************/
CREATE PROCEDURE [Notificacion].[pr_Aplicacion_Select]
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