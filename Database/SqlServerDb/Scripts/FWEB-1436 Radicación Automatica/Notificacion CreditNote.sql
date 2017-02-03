DECLARE @id_aplicacion INT,
		@id_servicio INT,
		@nm_aplicacion VARCHAR(100) = 'Plcolab',
		@ds_aplicacion VARCHAR(100) = 'Plataforma de colaboracion',
		@nm_servicio   VARCHAR(100) = 'REST',
		@ds_servicio   VARCHAR(100) = 'Llamados a WebServices REST.',
		@json_config_servicio VARCHAR(MAX) = '{    "SenderType": "NotificationServer.Senders.Rest.RestSender, NotificationServer.Senders.Rest" }',
		@nm_servicio_aplicacion VARCHAR(100) = 'PlColab.Rest.CreditNote',
		@json_configuracion_servicio_aplicacion VARCHAR(MAX) = '{
    "Host": "http://plcolabrepext-test.azurewebsites.net",
    "Resource": "/Inbox/NC",
    "Method": "POST",
    "Headers": {
        "Accept": "application/json"
    }
}',
		@nm_plantilla VARCHAR(200) = 'PLColab_Rest_Inbox',
		@ds_Plantilla VARCHAR(200) = 'Notificar documentos inbox',
		@ds_Contenido VARCHAR(MAX) = '@model NotificationServer.Core.Senders.NotificationData
@using System
@using System.Text
@using System.Linq
@using Newtonsoft.Json.Linq

@{
    var doc = Model.Properties;
    var documento_xml_Base64 = (Model.Properties["documento_xml_Base64"] as String) ?? String.Empty;
	var xml = Encoding.UTF8.GetString(Convert.FromBase64String(documento_xml_Base64));
}
@xml'



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


IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.ServiciosPorAplicacion s WHERE s.id_Applicacion = @id_aplicacion AND s.id_Servicio = @id_servicio and nm_nombre = @nm_servicio_aplicacion) BEGIN
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
		and nm_nombre = @nm_servicio_aplicacion
	PRINT '[Correcto] Servicio por aplicacion actualizado.'
END


PRINT 'Configurando la planilla ' + @nm_plantilla + '...'

IF NOT EXISTS(SELECT 1 FROM Notificacion.Plantillas WHERE nm_nombre = @nm_plantilla)
BEGIN
	INSERT INTO Notificacion.Plantillas (
		[nm_nombre],
		[ds_Descripcion],
		[ds_contenido],
		[id_Creado],
		[id_Modificado]
	)
	VALUES (
		@nm_plantilla,
		@ds_Plantilla,
		@ds_Contenido,
		1,
		1
	)
	PRINT '[Correcto] Servicio creado con éxito.'
END ELSE BEGIN
	UPDATE Notificacion.Plantillas
	SET [nm_nombre] = @nm_plantilla,
		[ds_Descripcion] = @ds_Plantilla,
		[ds_contenido] = @ds_Contenido
	WHERE nm_nombre = @nm_plantilla
	PRINT '[Correcto] Servicio actualizado con éxito.'
END

PRINT '[Correcto] Configuración aplicada con éxito.'

PRINT 'LISTO.'