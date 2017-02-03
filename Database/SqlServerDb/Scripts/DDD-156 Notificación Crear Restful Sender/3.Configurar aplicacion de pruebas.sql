DECLARE @id_aplicacion INT,
		@id_servicio INT

PRINT 'Creando la aplicación TripleD...'
IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.Aplicaciones a WHERE a.ds_Titulo = 'TripleD') BEGIN
	INSERT INTO Notificacion.Aplicaciones (
		[ds_Titulo]      ,
		[ds_Descripcion] ,
		[id_Creado]      ,
		[id_Modificado]  
	) VALUES (
		'TripleD',
		'Este es el repositorio de los ciudadanos',
		1,
		1
	)
	PRINT '[Correcto] Aplicación creada con éxito.'
END ELSE BEGIN
	PRINT '[Correcto] La aplicación ya existe.'
END

SELECT TOP 1 @id_aplicacion = a.id_aplicacion
FROM Notificacion.Aplicaciones a
WHERE a.ds_Titulo = 'TripleD'

PRINT 'Creando la servicio Rest.Gateway.Documentos...'
IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.Servicios s WHERE s.nm_Nombre = 'Rest.Gateway.Documentos') BEGIN
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
		'Rest.Gateway.Documentos',
		'Envío de documentos hacia Gateway usando el Sender RESTful',
		'',
		0,
		'',
		'',
		'{ "SenderType": "NotificationServer.Senders.Rest.RestSender, NotificationServer.Senders.Rest" }',
		1,
		1
	)
	PRINT '[Correcto] Servicio creado con éxito.'
END ELSE BEGIN
	PRINT '[Correcto] El servicio ya existe.'
END

SELECT TOP 1 @id_servicio = s.id_Servicio
FROM Notificacion.Servicios s
WHERE s.nm_Nombre = 'Rest.Gateway.Documentos'

PRINT 'Configurando el servicio Rest.Gateway.Documentos para la aplicación TripleD...'

DECLARE @configuracion VARCHAR(MAX) = '{
    "Host": "PONER URL EN QA DEL WS DE GATEWAY",
    "Resource": "/Documentos/Crear",
    "Method": "POST",
    "Headers": {
        "Accept": "application/json",
        "X-Auth-APPLICATION": "PONER TOKEN EN QA PARA ENVIAR DOCUMENTOS A GATEWAY"
    }
}'

IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.ServiciosPorAplicacion s WHERE s.cd_Applicacion = @id_aplicacion AND s.cd_Servicio = @id_servicio) BEGIN
	INSERT INTO Notificacion.ServiciosPorAplicacion (
		[id_ServiciosPorAplicacion],
		[cd_Applicacion]  ,
		[cd_Servicio]     ,         
		[ds_Configuracion],         
		--				  
		[id_Creado]       ,
		[id_Modificado] 
	) VALUES (
		2,
		@id_aplicacion,
		@id_servicio,
		@configuracion,
		1,
		1
	)
END ELSE BEGIN
	UPDATE Notificacion.ServiciosPorAplicacion
	SET 
		[ds_Configuracion] = @configuracion,
		[id_Modificado] = 1,
		dt_Modificado = dbo.fn_GetLocalDate()
	WHERE 
		[cd_Applicacion] = @id_aplicacion
		AND [cd_Servicio]    = @id_servicio
END

PRINT '[Correcto] Configuración aplicada con éxito.'

PRINT 'Creando plantilla de pruebas...'

DECLARE @plantilla VARCHAR(MAX) = '@model NotificationServer.Core.Senders.NotificationData
@using System.Linq

@functions {
    object Prop(string key){
        return Model.Properties.FirstOrDefault(p => p.Key == key).Value;
    }
}

{
  "IdDocumento": {
    "EspacioDeNombresDelContrato": "@Prop("EspacioDeNombresDelContrato")",
    "NumeroDocumento": "@Prop("numero_cd")",
    "FechaDocumento":  "@Prop("fecha_documento_dt")",
    "TipoDocumento":   "@Prop("TipoDocumento")",
    "IdentificadorSucursal": "0",
    "IdentificadorProceso": "0"
  },
  "TipoDocumento": "@Prop("TipoDocumento")",
  "MetaDatos": {
    "Etiquetas": [],
    "CamposDeBusqueda": [
        @foreach(var kv in Model.Properties){
           @: { "Clave": "@kv.Key", "Valor": "@kv.Value" },
        }
        { "Clave": "____", "Valor": "" }
    ]
  },
  "iGui_Toke": "0"
}'

IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.Plantillas p WHERE p.nm_nombre = 'TripleD/SendDocument') BEGIN
	INSERT INTO Notificacion.Plantillas(
		[nm_nombre]    ,
		[ds_contenido] ,
		-- 			   ,
		[id_Creado]    ,
		[id_Modificado]
	) VALUES (
		'TripleD/SendDocument',
		@plantilla,
		1,
		1
	)
END ELSE BEGIN
	UPDATE Notificacion.Plantillas
	SET 
		ds_contenido = @plantilla,
		[id_Modificado] = 1,
		dt_Modificado = dbo.fn_GetLocalDate()
	WHERE 
		[nm_nombre] = 'TripleD/SendDocument'
END

PRINT '[Correcto] Plantilla configurada con éxito.'
PRINT 'LISTO.'