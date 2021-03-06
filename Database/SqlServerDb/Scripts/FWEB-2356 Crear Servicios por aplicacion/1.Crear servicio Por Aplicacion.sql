   
  DECLARE @PP_URL VARCHAR(max) = 'http://corepp-test.azurewebsites.net'---URL DE CORE api 
  DECLARE @PP_Aplicacion VARCHAR(max) = 'plcolab19'--Nombre de la aplicaicon en Core PP
  DECLARE @PP_TokenContratoPadre VARCHAR(max) = '11111111-2222-3333-4444-555555555555'--Token del contraro padre en Core PP
  DECLARE @Application_id INT = 1
  DECLARE @Servicio_id INT = 10
  DECLARE @Creador_id int = 1
  DECLARE @Servicio_nm VARCHAR(200) = 'PLColab.Rest.Registration.CORE.PP'
  DECLARE @Servicio_Config VARCHAR(MAX) ='{
 	"Host": "'+@PP_URL+'",
 	"Resource": "/api/Contratos",
 	"Method": "POST",
 	"ContentType": "application/json",
 	"Headers": {
 		"Accept": "application/json",
		"Aplicacion":"'+@PP_Aplicacion+'",
		"TokenContratoPadre":"'+@PP_TokenContratoPadre+'"
 	}
 }'

 IF(EXISTS (SELECT 1 FROM [Notificacion].[ServiciosPorAplicacion] WHERE id_Applicacion = @Application_id AND id_Servicio = @Servicio_id AND nm_nombre = @Servicio_nm))
	DELETE FROM [Notificacion].[ServiciosPorAplicacion] WHERE [id_Applicacion] = @Application_id AND [id_Servicio] = @Servicio_id AND [nm_nombre] = @Servicio_nm

  INSERT INTO [Notificacion].[ServiciosPorAplicacion]([id_Applicacion],[id_Servicio],[nm_nombre],[ds_Configuracion],[id_Creado],[id_Modificado])
  VALUES(@Application_id,@Servicio_id,@Servicio_nm,@Servicio_Config,@Creador_id,@Creador_id)


  