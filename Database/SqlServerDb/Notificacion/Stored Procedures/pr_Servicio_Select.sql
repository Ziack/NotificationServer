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
		ORDER BY dt_Creado DESC
		OFFSET (@iInt_numPagina-1)*ISNULL(@iInt_tamPagina, 1) ROWS 
		FETCH NEXT ISNULL(@iInt_tamPagina, @@ROWCOUNT) ROWS ONLY 
	END
END