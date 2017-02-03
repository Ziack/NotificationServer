DECLARE @IdApp INT
DECLARE @IdService INT
DECLARE @Settings VARCHAR(MAX)

SELECT TOP 1 @IdApp=id_Aplicacion FROM  [Notificacion].[Aplicaciones] WHERE [ds_Titulo] = 'TripleD'
SELECT TOP 1 @IdService=id_Servicio FROM  [Notificacion].[Servicios] WHERE [nm_Nombre] = 'DnnCoreMessaging'

SET @Settings = '{ 
		"Host": "http://tripled-test.azurewebsites.net/", 
		"Resource":"/DesktopModules/InternalServices/API/MessagingService/Create"  
	}'

INSERT INTO [Notificacion].[ServiciosPorAplicacion]
           ([id_Applicacion]
           ,[id_Servicio]
           ,[nm_nombre]
           ,[ds_Configuracion]
           ,[id_Creado]
           ,[id_Modificado])
     VALUES
           (@IdApp
           ,@IdService
           ,'DnnCoreMessaging.TripleD'
           ,@Settings
           ,1
           ,1)

INSERT INTO [Notificacion].[TipoNotificacion]
           ([cd_TipoNotificacion]
           ,[nm_Plantilla]
           ,[nm_Servicio]
           ,[id_Creado]
           ,[id_Modificado])
     VALUES
		   ('TripleD_Mail_NuevoDocumento'
           ,'TripleD/Mail/NuevoDocumento'
           ,'DnnCoreMessaging'
           ,1
           ,1),
		   ('TripleD_Mail_NotificarAutorizacionExplicita'
           ,'TripleD/Mail/NotificarAutorizacionExplicita'
           ,'DnnCoreMessaging'
           ,1
           ,1),
		   ('TripleD_Mail_RevocacionAutorizacionExplicita'
           ,'TripleD/Mail/RevocacionAutorizacionExplicita'
           ,'DnnCoreMessaging'
           ,1
           ,1)