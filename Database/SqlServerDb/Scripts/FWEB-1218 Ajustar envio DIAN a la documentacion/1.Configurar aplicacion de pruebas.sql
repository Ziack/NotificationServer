DECLARE @id_aplicacion INT,
		@id_servicio INT,
		@nm_aplicacion VARCHAR(100) = 'Plcolab',
		@nm_servicio   VARCHAR(100) = 'WcfDian',
		@nm_servicio_aplicacion VARCHAR(100) = 'PLColab.WCF.Dian',
		@json_configuracion_servicio_aplicacion VARCHAR(MAX) = '{
	"Url": "http://facturaelectronica-dian.getsandbox.com/B2BIntegrationEngine/FacturaElectronica",
	"Username": "af93b748-3747-41a1-9432-cb956183086a",
	"Password": "f882b83fc0a5579fb57d34ad6660ad38921a59e7d1611121e5b0a6f1f7faf969",
	"ContentType": "application/soap+xml; charset=utf-8",
	"Method": "POST",
	"InterceptorTypes": [
		"NotificationServer.Interceptors.CoreInterceptors.ReplyInterceptor, NotificationServer.Interceptors.CoreInterceptors"
	],
	"ReplyToNotificationCommand": {
		"ApplicationName": "Plcolab",
		"Destinations": [
			{ "Service": "PLColab.Rest.Respuesta", "TemplateName": "PLColab_Respuesta" }
		],
		"From": "inbox_DIAN@fakedomain.com",
		"Subject": "EMISION",
		"ReplyTo": [],
		"Properties": []
	}
}'


SELECT TOP 1 @id_aplicacion = a.id_aplicacion
FROM Notificacion.Aplicaciones a
WHERE a.ds_Titulo = @nm_aplicacion

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