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
