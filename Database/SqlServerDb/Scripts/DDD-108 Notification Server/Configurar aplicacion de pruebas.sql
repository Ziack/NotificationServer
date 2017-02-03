

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
	PRINT 'Aplicación creada con éxito.'
END ELSE BEGIN
	PRINT 'La aplicación ya existe.'
END

SELECT TOP 1 @id_aplicacion = a.id_aplicacion
FROM Notificacion.Aplicaciones a
WHERE a.ds_Titulo = 'TripleD'

PRINT 'Creando la servicio Mail...'
IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.Servicios s WHERE s.nm_Nombre = 'Mail') BEGIN
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
		'Mail',
		'Envio masivo de correos',
		'',
		0,
		'',
		'',
		'{ "SenderType": "NotificationServer.Senders.Mail.Senders.SmtpMailSender, NotificationServer.Senders.Mail" }',
		1,
		1
	)
	PRINT 'Servicio creado con éxito.'
END ELSE BEGIN
	PRINT 'El servicio ya existe.'
END

SELECT TOP 1 @id_servicio = s.id_Servicio
FROM Notificacion.Servicios s
WHERE s.nm_Nombre = 'Mail'

PRINT 'Configurando el servicio Mail para la aplicación TripleD...'

DECLARE @configuracion VARCHAR(MAX) = '{
	"Host": "192.168.221.95",
	"Port": 25,
	"Usuario": "catchall@fakedomain.com",
	"Password": ""
}'

IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.ServiciosPorAplicacion s WHERE s.cd_Applicacion = @id_aplicacion AND s.cd_Servicio = @id_servicio) BEGIN
	INSERT INTO Notificacion.ServiciosPorAplicacion (
		[cd_Applicacion]  ,
		[cd_Servicio]     ,         
		[ds_Configuracion],         
		--				  
		[id_Creado]       ,
		[id_Modificado] 
	) VALUES (
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

PRINT 'Configuración aplicada con éxito.'

PRINT 'Creando plantilla de pruebas...'

DECLARE @plantilla VARCHAR(MAX) = '@model NotificationServer.Core.Senders.NotificationData

<div>
    <h1>Hola 
        @foreach(var destination in Model.To){
            <span>@destination</span><span>, </span>
        }
    </h1>
    <p>
        Este es un mensaje de <strong>@Model.From</strong> y tiene las siguientes propiedades:
    </p>
    <table style="width: 100%">
        <thead>
            <tr>
                <th>Nombre</th>
                <th>Valor</th>
            </tr>
        </thead>
        <tbody>
            @foreach(var kv in Model.Properties){
                <tr>
                    <td>@kv.Key</td>
                    <td>@kv.Value</td>
                </tr>
            }
        </tbody>
    </table>
</div>
'

IF NOT EXISTS(SELECT TOP 1 1 FROM Notificacion.Plantillas p WHERE p.nm_nombre = 'TripleD/Mail/Prueba') BEGIN
	INSERT INTO Notificacion.Plantillas(
		[nm_nombre]    ,
		[ds_contenido] ,
		-- 			   ,
		[id_Creado]    ,
		[id_Modificado]
	) VALUES (
		'TripleD/Mail/Prueba',
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
		[nm_nombre] = 'TripleD/Mail/Prueba'
END

PRINT 'Plantilla configurada con éxito.'
PRINT 'LISTO.'