DECLARE @id_aplicacion INT,
		@id_servicio INT,
		@nm_aplicacion VARCHAR(100) = 'Plcolab',
		@ds_aplicacion VARCHAR(100) = 'Plataforma de colaboracion',
		@nm_servicio   VARCHAR(100) = 'Mail',
		@ds_servicio   VARCHAR(100) = 'Envio masivo de correos',
		@json_config_servicio VARCHAR(MAX) = ' { "SenderType": "NotificationServer.Senders.Mail.Senders.SmtpMailSender, NotificationServer.Senders.Mail" }',
		@nm_servicio_aplicacion VARCHAR(100) = 'Plcolab.Mail.Amazon',
		@json_configuracion_servicio_aplicacion VARCHAR(MAX) = '{
	"SenderType": "NotificationServer.Senders.Amazon.SESMailSenser, NotificationServer.Senders.Amazon",
	"Host": "us-east-1",
	"Usuario": "AKIAIY35X5IHHZOF2F2A",
	"Password": "P+PrycBzvHykBJo5hndxH3D6Ns2SF0VQIkEk1RsW",
	"From": "pruebasdesarrolloamazon@facture.co"
}'


PRINT 'Creando la aplicación ' + @nm_aplicacion + '...'
IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.Aplicaciones a WHERE a.ds_Titulo = @nm_aplicacion) BEGIN
	INSERT INTO Notificacion.Aplicaciones (
		[ds_Titulo]      ,
		[ds_Descripcion] ,
		[id_Creado]      ,
		[id_Modificado]  
	) VALUES (
		@nm_aplicacion,
		@ds_aplicacion,
		1,
		1
	)
	PRINT '[Correcto] Aplicación creada con éxito.'
END ELSE BEGIN
	PRINT '[Correcto] La aplicación ya existe.'
END

SELECT TOP 1 @id_aplicacion = a.id_aplicacion
FROM Notificacion.Aplicaciones a
WHERE a.ds_Titulo = @nm_aplicacion

PRINT 'Creando el servicio ' + @nm_servicio + '...'
IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.Servicios s WHERE s.nm_Nombre = @nm_servicio) BEGIN
	INSERT INTO Notificacion.Servicios(
		[nm_Nombre]       ,
		[ds_Descripcion]  ,
		[ds_Host]         ,
		[nm_Puerto]       ,
		[ds_Usuario]      ,
		[ds_Password]     ,
		[ds_Configuracion],
		--				  
		[id_Creado]       ,
		[id_Modificado]
	) VALUES (
		@nm_servicio,
		@ds_servicio,
		'',
		0,
		'',
		'',
		@json_config_servicio,
		1,
		1
	)
	PRINT '[Correcto] Servicio creado con éxito.'
END ELSE BEGIN
	PRINT '[Correcto] El servicio ya existe.'
END

SELECT TOP 1 @id_servicio = s.id_Servicio
FROM Notificacion.Servicios s
WHERE s.nm_Nombre = @nm_servicio

PRINT 'Configurando el servicio ' + @nm_servicio + ' para la aplicación ' + @nm_aplicacion + '...'


IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.ServiciosPorAplicacion s WHERE s.id_Applicacion = @id_aplicacion AND s.id_Servicio = @id_servicio) BEGIN
	INSERT INTO Notificacion.ServiciosPorAplicacion (
		[nm_nombre],
		[id_Applicacion]  ,
		[id_Servicio]     ,         
		[ds_Configuracion],         
		--				  
		[id_Creado]       ,
		[id_Modificado] 
	) VALUES (
		@nm_servicio_aplicacion,
		@id_aplicacion,
		@id_servicio,
		@json_configuracion_servicio_aplicacion,
		1,
		1
	)
END ELSE BEGIN
	UPDATE Notificacion.ServiciosPorAplicacion
	SET 
		[ds_Configuracion] = @json_configuracion_servicio_aplicacion,
		[id_Modificado] = 1,
		dt_Modificado = dbo.fn_GetLocalDate()
	WHERE 
		[id_Applicacion] = @id_aplicacion
		AND [id_Servicio]    = @id_servicio
END

PRINT '[Correcto] Configuración aplicada con éxito.'

PRINT 'LISTO.'