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
CREATE PROCEDURE [Notificacion].[pr_ServiciosPorAplicacion_Save]
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