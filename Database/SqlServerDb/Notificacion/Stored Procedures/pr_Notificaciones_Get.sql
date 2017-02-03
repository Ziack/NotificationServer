CREATE PROCEDURE Notificacion.[pr_Notificaciones_Get]
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

	SELECT
		t.[nm_tag]
	FROM Notificacion.Tags t
	WHERE uid_notificacion = @uid_Notificacion
END
