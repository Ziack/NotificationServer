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
CREATE PROCEDURE [Notificacion].[pr_ServiciosPorAplicacion_Select]
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