INSERT INTO [Notificacion].[Servicios]
           ([nm_Nombre]
           ,[ds_Descripcion]
           ,[ds_Host]
           ,[nm_Puerto]
           ,[ds_Usuario]
           ,[ds_Password]
           ,[ds_Configuracion]
           ,[id_Creado]
           ,[dt_Creado]
           ,[id_Modificado]
           ,[dt_Modificado])
     VALUES
           ('DnnCoreMessaging'
           ,'Envio de notificaciones a la bandeja de entrada'
           ,''
           ,80
           ,''
           ,''
           ,'{ "SenderType": "NotificationServer.Senders.DnnCoreMessaging.DnnCoreMessagingSender, NotificationServer.Senders.DnnCoreMessaging" }'
           ,1
           ,1)