  PRINT 'Ponerle nombres adecuados a los ServiciosPorAplicacion'
  UPDATE sa
  SET sa.nm_nombre = s.nm_Nombre
  FROM Notificacion.ServiciosPorAplicacion sa
  INNER JOIN Notificacion.Servicios s ON s.id_Servicio = sa.id_Servicio
  

  PRINT 'Hacer que todos los servicios por aplicación de tipo REST apunten a un solo servicio REST'
  DECLARE @id_Primer_Rest INT
  SELECT TOP 1 @id_Primer_Rest = id_Servicio FROM Notificacion.Servicios WHERE nm_Nombre LIKE 'Rest.%'

  UPDATE Notificacion.ServiciosPorAplicacion
  SET id_Servicio = @id_Primer_Rest
  WHERE nm_nombre LIKE 'Rest.%'

  PRINT 'Borrar los servicios REST repetidos'
  DELETE Notificacion.Servicios
  WHERE nm_nombre LIKE 'Rest.%' AND id_Servicio <> @id_Primer_Rest

  PRINT 'Renombrar el único servicio REST que quede'
  UPDATE Notificacion.Servicios
  SET nm_Nombre = 'REST'
  WHERE id_Servicio = @id_Primer_Rest