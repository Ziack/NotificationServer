declare @idaplicacion int 
declare @idServicio int 
declare @configuracion varchar(max) = '{
		"SenderType": "NotificationServer.Senders.Twitter.TwitterSender, NotificationServer.Senders.Twitter"
	}'

declare @configuracionAplicacion varchar(max) = '{
	"ConsumerKey": "67Pgy0LxGXALUGXEwPdQm8Q3g",
	"ConsumerSecret": "jHr0K4zCzhkVxfOUxMCpZgh3BJZsgngUvfL6VRoLHVJvMJXOeC",
	"AccessToken": "4898165668-6JM11oJT3JOQV9iYE9liLZtbJBPzOs4rpIKuJ5a",
	"AccessTokenSecret": "8FWg5lXCFGeQ1VH56KOUil0Czb57f5jSy5BH71CFUkGUM"
}'

select @idaplicacion = id_Aplicacion
from [Notificacion].[Aplicaciones]
WHERE ds_Titulo = 'TripleD'

IF(EXISTS(SELECT 1 from [Notificacion].[Servicios] where nm_Nombre = 'Twitter'))
	BEGIN
		UPDATE [Notificacion].[Servicios]
		SET ds_Configuracion = @configuracion
		where nm_Nombre = 'Twitter'
		SELECT @idServicio = id_Servicio FROM [Notificacion].[Servicios] where nm_Nombre = 'Twitter'
	END 
ELSE
BEGIN
	INSERT INTO [Notificacion].[Servicios] (ds_Configuracion,nm_Nombre,ds_Host,nm_Puerto,ds_Password,ds_Usuario) VALUES(@configuracion,'Twitter','',0,'','')
	set @idServicio = SCOPE_IDENTITY()
END 

IF(EXISTS(SELECT 1 from [Notificacion].[ServiciosPorAplicacion] where id_Applicacion = @idaplicacion and id_Servicio = @idServicio))
	BEGIN
		UPDATE [Notificacion].[ServiciosPorAplicacion]
		SET ds_Configuracion = @configuracionAplicacion,
		nm_nombre = 'TWitter'
		where id_Applicacion = @idaplicacion and id_Servicio = @idServicio
	END 
ELSE
BEGIN
	INSERT INTO [Notificacion].[ServiciosPorAplicacion] (ds_Configuracion,nm_Nombre,id_Applicacion,id_Servicio) VALUES(@configuracionAplicacion,'Twitter',@idaplicacion,@idServicio)
END