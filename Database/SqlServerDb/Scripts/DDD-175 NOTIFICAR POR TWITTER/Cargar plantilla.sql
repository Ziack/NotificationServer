IF(exists(select 1 from [Notificacion].[Plantillas] WHERE nm_nombre='TripleD/Twiter/NuevoDocumento'))
	BEGIN
		UPDATE [Notificacion].[Plantillas]
		SET [ds_contenido] =  N'@model NotificationServer.Core.Senders.NotificationData
Le ha llegado un nuevo documento @Model.Properties["tipo_documento"] con número @Model.Properties["numero_documento"]'
		WHERE nm_nombre='TripleD/Twitter/NuevoDocumento'
	END 
ELSE
BEGIN
	INSERT [Notificacion].[Plantillas] ( [nm_nombre], [ds_Descripcion], [ds_contenido], [id_Creado], [dt_Creado], [id_Modificado], [dt_Modificado]) VALUES ( N'TripleD/Twitter/NuevoDocumento', N'notificar a twiter nuevo documento', N'@model NotificationServer.Core.Senders.NotificationData
Le ha llegado un nuevo documento @Model.Properties["tipo_documento"] con número @Model.Properties["numero_documento"]', 1, CAST(N'2012-02-09 00:00:00.0000000' AS DateTime2), 1, CAST(N'2016-02-09 00:00:00.0000000' AS DateTime2))
END 
