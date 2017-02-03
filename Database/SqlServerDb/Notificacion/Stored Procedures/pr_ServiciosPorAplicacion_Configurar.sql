/******************************************************************************
** Descripción: Configura una instancia de servicio para una palicación
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-22	Jeysson Guevara     Creación del procedimiento
*******************************************************************************/
CREATE PROCEDURE [Notificacion].[pr_ServiciosPorAplicacion_Configurar]
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
