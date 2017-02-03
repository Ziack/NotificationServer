CREATE PROCEDURE Notificacion.[pr_Plantillas_Get]
	@nm_nombre VARCHAR(200)
AS BEGIN
	SELECT TOP 1 
		p.ds_contenido
	FROM Notificacion.Plantillas p
	WHERE p.nm_nombre = @nm_nombre
END
